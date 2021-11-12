using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour {

    private static GameStateManager _instance;
    public static GameStateManager Instance { get { return _instance; } }

    private GameState _state;
    public GameState state {
        get { return _state; }
        private set { _state = value; }
    }

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

    void LoadDefaultState() {
        _state = new GameState();
        _state.bees.Add(new Bee());
    }


    public string GetStateAsJson() {
        return JsonUtility.ToJson(state);
    }

    public void UpdateStateFromJson(string jsonState) {
        state = JsonUtility.FromJson<GameState>(jsonState);
    }

    IEnumerator LogGameStateEvery10Seconds() {
        while (true) {
            Debug.Log(GetStateAsJson());
            yield return new WaitForSeconds(10);
        }
    }
}
