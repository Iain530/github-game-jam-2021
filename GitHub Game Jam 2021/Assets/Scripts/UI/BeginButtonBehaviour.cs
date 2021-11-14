using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginButtonBehaviour : MonoBehaviour {
    // Start is called before the first frame update
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
