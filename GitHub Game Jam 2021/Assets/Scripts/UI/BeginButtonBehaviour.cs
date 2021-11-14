using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginButtonBehaviour : MonoBehaviour {
    GameStateManager stateManager;

    void Start() {
        stateManager = GameStateManager.Instance;

        if (!stateManager.IsRoomOwner) {
            this.enabled = false;
        }
    }

    public void OnBeginButtonClick() {
        BeeNetworkClient.Instance.BeginGame();
    }
}
