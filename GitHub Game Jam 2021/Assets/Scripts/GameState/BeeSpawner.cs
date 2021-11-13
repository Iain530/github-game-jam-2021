using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeSpawner : MonoBehaviour {

    public GameObject playerBeePrefab;
    public GameObject otherBeePrefab;

    GameStateManager stateManager;
    Dictionary<string, BeeState> bees = new Dictionary<string, BeeState>();

    void Start() {
        stateManager = GameStateManager.Instance;
        stateManager.GameStateUpdated += OnGameStateUpdate;
        OnGameStateUpdate();
    }

    public void OnGameStateUpdate() {
        foreach (KeyValuePair<string, Bee> entry in stateManager.state.bees) {
            Bee bee = entry.Value;
            if (!bees.ContainsKey(bee.id)) {
                GameObject instance;
                if (stateManager.IsCurrentPlayer(bee.id)) {
                    // Spawn player bee
                    instance = Instantiate(playerBeePrefab, transform);
                } else {
                    // spawn other bee
                    instance = Instantiate(otherBeePrefab, transform);
                }
                BeeState beeState = instance.GetComponent<BeeState>();
                beeState.Initialize(bee);
                bees.Add(bee.id, beeState);
            }
        }        
    }

    void OnDestroy() {
        stateManager.GameStateUpdated -= OnGameStateUpdate;    
    }
}
