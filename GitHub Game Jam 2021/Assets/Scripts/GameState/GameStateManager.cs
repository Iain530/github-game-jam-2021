using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour {

    private static GameStateManager _instance;
    public static GameStateManager Instance { get { return _instance; } }

    private GameState _state;
    private string _currentPlayerId;
    private string _currentGameId;
    private string _currentGameCode;
    private bool _isRoomOwner;

    public GameState state { get { return _state; } }
    public string CurrentPlayerId { get { return _currentPlayerId; } }
    public string CurrentGameID { get { return _currentGameId; } }
    public string CurrentGameCode { get { return _currentGameCode; } }
    public bool IsRoomOwner { get { return _isRoomOwner; } }

    private void Awake() {
        if (_instance != null && _instance != this) {
            // Destroy if already an instance
            Destroy(this.gameObject);
        } else {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            LoadDefaultState();
            StartCoroutine(LogGameStateEvery10Seconds());
        }
    }


    public void JoinGame(string gameId, string playerId, string gameCode, bool isRoomOwner) {
        Debug.Log("player id: " + playerId + " game id: " + gameId + " game code: " + gameCode);
        _currentPlayerId = playerId;
        _currentGameId = gameId;
        _currentGameCode = gameCode;
        _isRoomOwner = isRoomOwner;
    }


    public string GetStateAsJson() {
        return JsonUtility.ToJson(state);
    }

    public void UpdateStateFromJson(string jsonState) {
        GameState newState = JsonUtility.FromJson<GameState>(jsonState);
        if (newState.messageTime >= _state.messageTime) {
            _state = newState;
        } else {
            Debug.LogWarning("Received old state from server, discarding");
        }
    }
    void LoadDefaultState() {
        _state = new GameState();
        _state.bees.Add(new Bee());
    }

    IEnumerator LogGameStateEvery10Seconds() {
        while (true) {
            Debug.Log(GetStateAsJson());
            yield return new WaitForSeconds(10);
        }
    }
}
