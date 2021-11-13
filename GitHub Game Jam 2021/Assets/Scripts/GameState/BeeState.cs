using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeState : MonoBehaviour {

    private string _id;
    public string Id { get { return _id; } }

    private string _hatName;
    public string HatName { get { return _hatName; } }
    private bool _isOtherPlayer;
    public bool IsOtherPlayer { get { return _isOtherPlayer; } }
    private bool _isCurrentPlayer;
    public bool IsCurrentPlayer { get { return _isCurrentPlayer; } }

    private bool updatePositionFromServer;

    GameStateManager stateManager;

    void Start() {
        stateManager = GameStateManager.Instance;
        stateManager.GameStateUpdated += UpdatePosition;
    }

    public void Initialize(Bee bee) {
        _isCurrentPlayer = GameStateManager.Instance.IsCurrentPlayer(bee.id);
        _isOtherPlayer = bee.isPlayer && !_isCurrentPlayer;

        _id = bee.id;
        _hatName = bee.hatName;

        if (IsCurrentPlayer) {
            updatePositionFromServer = false;
        } else if (IsOtherPlayer) {
            updatePositionFromServer = true;
        } else {
            updatePositionFromServer = !GameStateManager.Instance.IsRoomOwner;
        }
    }

    public void UpdatePosition() {
        if (updatePositionFromServer) {
            Vector2? position = stateManager.state.GetBeePosition(Id);
            if (position.HasValue) {
                gameObject.transform.position = position.Value;
            }
        }
    }

    public Vector2 GetPosition() {
        return gameObject.transform.position;
    }

    void OnDestroy() {
        stateManager.GameStateUpdated -= UpdatePosition;    
    }
}
