using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour {

    private static GameStateManager _instance;
    public static GameStateManager Instance { get { return _instance; } }

    private GameState _state;
    private int _currentPlayerId;

    public GameState state { get { return _state; } }
    public int CurrentPlayerId { get { return _currentPlayerId; } }

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


    public void SetCurrentPlayerId(int id) {
        Debug.Log("Current player id set: " + id);
        _currentPlayerId = id;
    }

    public string GetStateAsJson() {
        return JsonUtility.ToJson(state);
    }

    public void UpdateStateFromJson(string jsonState) {
        _state = JsonUtility.FromJson<GameState>(jsonState);
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
