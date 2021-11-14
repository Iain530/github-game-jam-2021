using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeSpawner : MonoBehaviour {

    public GameObject playerBeePrefab;
    public GameObject otherBeePrefab;
    public GameObject aiBeePrefab;
    public GameObject queenBeePrefab;

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
            SpawnBee(bee);
        }
    }

    void SpawnBee(Bee bee) {
        if (!bees.ContainsKey(bee.id)) {
            GameObject instance;
            if (bee.isPlayer) {
                if (stateManager.IsCurrentPlayer(bee.id)) {
                    if (stateManager.GetCurrentPlayer().isQueenBee) {
                        instance = Instantiate(queenBeePrefab, transform);
                    } else {
                        instance = Instantiate(playerBeePrefab, transform);
                    }
                    // Spawn player bee
                } else {
                    // spawn other bee
                    instance = Instantiate(otherBeePrefab, transform);
                }
            } else if (stateManager.IsRoomOwner) {
                instance = Instantiate(aiBeePrefab, transform);
            } else {
                instance = Instantiate(otherBeePrefab, transform);
            }
            BeeState beeState = instance.GetComponent<BeeState>();
            beeState.Initialize(bee);
            bees.Add(bee.id, beeState);
        }
    }

    void OnDestroy() {
        stateManager.GameStateUpdated -= OnGameStateUpdate;    
    }
}
