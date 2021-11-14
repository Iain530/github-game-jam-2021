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
    public string mainMenuScene = "NoirGameMenu";

    public string serverHostname = "192.168.0.48";
    public int port = 8999;
    public bool useHttps = false;
    string Scheme { get { return useHttps ? "https://" : "http://"; } }
    string joinPath = "/joinGame";
    string createPath = "/createGame";

    WebSocket websocket;

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

    public void JoinGame(string gameCode) {
        string url = BuildJoinURL(gameCode);
        StartCoroutine(MakeLobbyRequest(url, false));
    }

    public void CreateGame() {
        string url = BuildCreateUrl();
        StartCoroutine(MakeLobbyRequest(url, true));
    }

    string BuildJoinURL(string gameCode) {
        return Scheme + serverHostname + ":" + port + joinPath + "?gameCode=" + gameCode;
    }
    string BuildCreateUrl() {
        return Scheme + serverHostname + ":" + port + createPath;
    }

    IEnumerator MakeLobbyRequest(string url, bool isRoomCreator) {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success) {
            ServerConnectResponse resp = JsonUtility.FromJson<ServerConnectResponse>(www.downloadHandler.text);
            GameStateManager.Instance.JoinGame(resp.gameId, resp.playerId, resp.gameCode, isRoomCreator);
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
            ResetGame();
        };

        websocket.OnClose += (e) => {
            Debug.Log("Connection closed!");
            ResetGame();
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

    public void ResetGame() {
        if (websocket != null && websocket.State == WebSocketState.Open) {
            // TODO: send leave lobby message after it's merged in
            websocket.Close();
            return;  // websocket OnClose will re-call ResetGame()
        }
        GameStateManager.Instance.ResetState();
        SceneManager.LoadScene(mainMenuScene);
        websocket = null;
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

    public void SendUpdateBeeNameMessage(string beeId, string newName) {
        bool isPlayerBee = GameStateManager.Instance.state.GetBee(beeId).isPlayer;
        var message = new UpdateBeeNameMessage(beeId, newName, isPlayerBee);
        SendJsonMessage(message);
    }

    public void SendUpdateBeeHatMessage(string beeId, string newHat) {
        bool isPlayerBee = GameStateManager.Instance.state.GetBee(beeId).isPlayer;
        var message = new UpdateBeeHatMessage(beeId, newHat, isPlayerBee);
        SendJsonMessage(message);
    }

    public void SendKickPlayerMessage(string playerId) {
        SendJsonMessage(new KickPlayerMessage(playerId));
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
