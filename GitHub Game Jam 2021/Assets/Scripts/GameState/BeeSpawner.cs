using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BeeSpawner : MonoBehaviour {

    public GameObject playerBeePrefab;
    public GameObject otherBeePrefab;
    public GameObject aiBeePrefab;
    public GameObject queenBeePrefab;

    public CinemachineVirtualCamera camera;

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
        foreach (Bee bee in stateManager.state.aiBees) {
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
                    camera.Follow = instance.transform;
                } else {
                    // spawn other bee
                    instance = Instantiate(otherBeePrefab, transform);
                }
            } else if (stateManager.IsRoomOwner) {
                Debug.Log("Attempting to spawn AI bees");
                instance = Instantiate(aiBeePrefab, transform);
                // Debug.Break();
            } else {
                instance = Instantiate(otherBeePrefab, transform);
            }
            instance.transform.position = bee.position;
            BeeState beeState = instance.GetComponent<BeeState>();
            beeState.Initialize(bee);
            bees.Add(bee.id, beeState);
        }
    }

    void OnDestroy() {
        stateManager.GameStateUpdated -= OnGameStateUpdate;    
    }
}
