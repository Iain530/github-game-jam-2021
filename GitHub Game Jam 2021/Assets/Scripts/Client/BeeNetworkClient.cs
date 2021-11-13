using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using NativeWebSocket;
using System.Linq;


public class BeeNetworkClient : MonoBehaviour {
    private static BeeNetworkClient _instance;
    public static BeeNetworkClient Instance { get { return _instance; } }

    public string websocketUrl = "ws://localhost:2567";
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

    void Start() {
        ConnectToWebSocket();
    }

    // Start is called before the first frame update
    async void ConnectToWebSocket() {
        websocket = new WebSocket(websocketUrl);

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
            GameStateManager.Instance.UpdateStateFromJson(message);    
        };

        // Keep sending messages at every 0.3s
        InvokeRepeating("SendWebSocketMessage", 0.0f, 0.3f);

        // waiting for messages
        await websocket.Connect();
    }

    void Update() {
    #if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
    #endif
    }

    public void SendCurrentPosition(Vector2 position) {
        int playerId = GameStateManager.Instance.CurrentPlayerId;
        PlayerPositionMessage message = new PlayerPositionMessage(playerId, position);
        SendJsonMessage(message);
    }

    public void SendAiBeePositions(List<GameObject> bees) {
        AiBeesPositionMessage message = new AiBeesPositionMessage();
        message.beePositions = bees.Select(bee => {
            // TODO: get ai bee ids
            int id = 0;
            return new BeePosition(id, bee.transform.position);
        }).ToList();

        SendJsonMessage(message);
    }

    async void SendJsonMessage(object serializableObj) {
        if (websocket.State == WebSocketState.Open) {
            string json = JsonUtility.ToJson(serializableObj);
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
