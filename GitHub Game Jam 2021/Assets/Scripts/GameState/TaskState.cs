using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskState : MonoBehaviour {

    private string _id;
    public string Id { get { return _id; } }

    GameStateManager stateManager;
    BeeNetworkClient networkClient;
    TaskBehaviour taskBehaviour;

    void Start() {
        stateManager = GameStateManager.Instance;
        stateManager.GameStateUpdated += OnGameStateUpdated;
        networkClient = BeeNetworkClient.Instance;
        taskBehaviour = GetComponent<TaskBehaviour>();
    }

    public void Initialize(string id) {
        _id = id;
    }

    public void OnGameStateUpdated() {
        Task task = stateManager.state.tasks.Find(t => t.id == _id);
        if (task.complete && !taskBehaviour.isComplete()) {
            taskBehaviour.setComplete();
        }
    }


    public void OnComplete() {
        networkClient.SendTaskComplete(Id);
    }


    public Vector2 GetPosition() {
        return gameObject.transform.position;
    }

    private void OnDestroy() {
        stateManager.GameStateUpdated -= OnGameStateUpdated;
    }
}
