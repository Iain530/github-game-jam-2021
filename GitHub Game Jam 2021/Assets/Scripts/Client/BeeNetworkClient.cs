using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using NativeWebSocket;
using System.Linq;


public class BeeNetworkClient : MonoBehaviour {
    private static BeeNetworkClient _instance;
    public static BeeNetworkClient Instance { get { return _instance; } }

    public string lobbySceneName = "BeginGameMenu";

    WebSocket websocket;
    int port = 8999;
    string serverHostname = "192.168.0.48";
    string joinPath = "/joinGame";
    string createPath = "/createGame";


    string BuildWebsocketURL() {
        return "ws://" + serverHostname + ":" + port;
    }


    private void Awake() {
        if (_instance != null && _instance != this) {
            // Destroy if already an instance
            Destroy(this.gameObject);
        } else {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start() {
    }

    public void JoinGame(string gameCode) {
        StartCoroutine(MakeJoinGameRequest(gameCode));
    }

    public void CreateGame() {
        StartCoroutine(MakeCreateGameRequest());
    }


    string BuildJoinURL(string gameCode) {
        return "http://" + serverHostname + ":" + port + joinPath + "?gameCode=" + gameCode;
    }
    string BuildCreateUrl() {
        return "http://" + serverHostname + ":" + port + createPath;
    }


    IEnumerator MakeJoinGameRequest(string gameCode) {
        UnityWebRequest www = UnityWebRequest.Get(BuildJoinURL(gameCode));
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success) {
            ServerConnectResponse resp = JsonUtility.FromJson<ServerConnectResponse>(www.downloadHandler.text);
            GameStateManager.Instance.JoinGame(resp.gameId, resp.playerId, resp.gameCode, false);
            ConnectToWebSocket(resp.gameId, resp.playerId);
        } else {
            Debug.LogError(www.error);
        }
    }

    IEnumerator MakeCreateGameRequest() {
        UnityWebRequest www = UnityWebRequest.Get(BuildCreateUrl());
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success) {
            ServerConnectResponse resp = JsonUtility.FromJson<ServerConnectResponse>(www.downloadHandler.text);
            GameStateManager.Instance.JoinGame(resp.gameId, resp.playerId, resp.gameCode, true);
            ConnectToWebSocket(resp.gameId, resp.playerId);
        } else {
            Debug.LogError(www.error);
        }
    }

    async void ConnectToWebSocket(string gameId, string playerId) {
        websocket = new WebSocket(BuildWebsocketURL());

        websocket.OnOpen += () => {
            Debug.Log("Connection open!");
            SendJsonMessage(new JoinLobbyMessage(gameId, playerId));
            SceneManager.LoadScene(lobbySceneName);
        };

        websocket.OnError += (e) => {
            Debug.Log("Error! " + e);
        };

        websocket.OnClose += (e) => {
            Debug.Log("Connection closed!");
        };

        websocket.OnMessage += (bytes) => {
            // getting the message as a string
            var message = Encoding.UTF8.GetString(bytes);
            Debug.Log("OnMessage! " + message);

            // Update the game state from the server
            GameStateManager.Instance.UpdateStateFromJson(message);    
        };

        // waiting for messages
        await websocket.Connect();
    }

    void Update() {
    #if !UNITY_WEBGL || UNITY_EDITOR
        if (websocket != null) {
            websocket.DispatchMessageQueue();
        }
    #endif
    }

    public void BeginGame() {
        if (GameStateManager.Instance.IsRoomOwner) {
            SendJsonMessage(new BeginGameMessage());
        }
    }

    public void SendCurrentPosition(Vector2 position) {
        PlayerPositionMessage message = new PlayerPositionMessage(position);
        SendJsonMessage(message);
    }

    public void SendAiBeePositions(List<BeeState> bees) {
        var message = new AiBeesPositionMessage(
            bees.Select(bee => new BeePosition(bee.Id, bee.GetPosition())).ToList()
        );
        SendJsonMessage(message);
    }


    public void SendTaskComplete(string id) {
        var message = new TaskCompleteMessage(id);
        SendJsonMessage(message);
    }

    async void SendJsonMessage(object message) {
        if (websocket == null)
            return;

        if (websocket.State == WebSocketState.Open) {
            string json = JsonUtility.ToJson(message);
            byte[] messageBytes = Encoding.UTF8.GetBytes(json);
            await websocket.Send(messageBytes);
        } else {
            Debug.LogError("Socket is closed! Cannot send message");
        }
    }

    private async void OnApplicationQuit() {
        await websocket.Close();
    }
}
