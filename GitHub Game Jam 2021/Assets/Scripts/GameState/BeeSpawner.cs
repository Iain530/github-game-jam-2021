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
        foreach (Player p in stateManager.state.players) {
            Bee bee = p.bee;
            if (!bees.ContainsKey(bee.id)) {
                GameObject instance;
                if (stateManager.IsCurrentPlayer(bee.id)) {
                    // Spawn player bee
                    instance = Instantiate(playerBeePrefab, transform);
                } else {
                    // spawn other bee
                    instance = Instantiate(otherBeePrefab, transform);
                }
                Debug.Log(JsonUtility.ToJson(bee));
                BeeState beeState = instance.GetComponent<BeeState>();
                beeState.Initialize(bee);
                bees.Add(bee.id, beeState);
                Debug.Log(bees.Count);
            }
        }        
    }

    void OnDestroy() {
        stateManager.GameStateUpdated -= OnGameStateUpdate;    
    }
}
