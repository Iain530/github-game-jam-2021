using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using NativeWebSocket;
using System.Linq;


public class BeeNetworkClient : MonoBehaviour {
    private static BeeNetworkClient _instance;
    public static BeeNetworkClient Instance { get { return _instance; } }

    int websocketPort = 8999;

    string serverHostname = "192.168.0.48";
    string joinPath = "/join";
    string createPath = "/create";


    string BuildWebsocketURL() {
        return "ws://" + serverHostname + ":" + websocketPort;
    }

    WebSocket websocket;

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
        ConnectToWebSocket();
    }

    public void JoinGame(string gameCode) {
        StartCoroutine(MakeJoinGameRequest(gameCode));
    }

    public void CreateGame() {
        StartCoroutine(MakeCreateGameRequest());
    }


    string BuildJoinURL(string gameCode) {
        return serverHostname + joinPath + "/" + gameCode;
    }
    string BuildCreateUrl() { return serverHostname + createPath; }


    IEnumerator MakeJoinGameRequest(string gameCode) {
        UnityWebRequest www = UnityWebRequest.Get(BuildJoinURL(gameCode));
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success) {
            ServerConnectResponse resp = JsonUtility.FromJson<ServerConnectResponse>(www.downloadHandler.text);
            GameStateManager.Instance.JoinGame(resp.gameId, resp.playerId, resp.gameCode, false);
            ConnectToWebSocket();
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
            ConnectToWebSocket();
        } else {
            Debug.LogError(www.error);
        }
    }

    async void ConnectToWebSocket() {
        websocket = new WebSocket(BuildWebsocketURL());

        websocket.OnOpen += () => {
            Debug.Log("Connection open!");
        };

        websocket.OnError += (e) => {
            Debug.Log("Error! " + e);
        };

        websocket.OnClose += (e) => {
            Debug.Log("Connection closed!");
        };

        websocket.OnMessage += (bytes) => {
            Debug.Log("OnMessage!");
            Debug.Log(bytes);

            // getting the message as a string
            var message = Encoding.UTF8.GetString(bytes);
            Debug.Log("OnMessage! " + message);

            // Update the game state from the server
            // GameStateManager.Instance.UpdateStateFromJson(message);    
        };

        // waiting for messages
        await websocket.Connect();
    }

    void Update() {
    #if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
    #endif
    }

    public void SendCurrentPosition(Vector2 position) {
        PlayerPositionMessage message = new PlayerPositionMessage(position);
        SendJsonMessage(message);
    }

    public void SendAiBeePositions(List<GameObject> bees) {
        var message = new AiBeesPositionMessage(
            bees.Select(bee => {
                // TODO: get ai bee ids
                int id = 0;
                return new BeePosition(id, bee.transform.position);
            }).ToList()
        );

        SendJsonMessage(message);
    }

    async void SendJsonMessage(object message) {
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
