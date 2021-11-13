using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeState : MonoBehaviour {

    private string _id;
    public string Id { get { return _id; } }

    private string _hatName;
    public string HatName { get { return _hatName; } }

    GameStateManager stateManager;

    void Start() {
        stateManager = GameStateManager.Instance;
        stateManager.GameStateUpdated += UpdatePosition;
    }

    public void Initialize(string id, string hatName) {
        _id = id;
        _hatName = hatName;
    }

    public void UpdatePosition() {
        Vector2? position = stateManager.state.GetBeePosition(Id);
        if (position.HasValue) {
            gameObject.transform.position = position.Value;
        }
    }

    public Vector2 GetPosition() {
        return gameObject.transform.position;
    }

    void OnDestroy() {
        stateManager.GameStateUpdated -= UpdatePosition;    
    }
}
