using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPlayerCounter : MonoBehaviour {

    Text counter;
    GameStateManager stateManager;

    void Start() {
        stateManager = GameStateManager.Instance;
        counter = GetComponent<Text>();
    }

    void Update() {
        counter.text = stateManager.state.players.Count.ToString();
    }
}
