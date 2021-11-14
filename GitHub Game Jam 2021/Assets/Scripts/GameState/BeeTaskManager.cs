using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeeTaskManager : MonoBehaviour {
    private static BeeTaskManager _instance;
    public static BeeTaskManager Instance { get { return _instance; } }

    Dictionary<string, GameObject> _tasks = new Dictionary<string, GameObject>();
    public int taskCount;

    private void Awake() {
        if (_instance != null && _instance != this) {
            // Destroy if already an instance
            Destroy(this.gameObject);
        } else {
            _instance = this;
            AssignTaskIds();
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.name == "Main") {
            EnableTasks();
        }
    }

    void EnableTasks() {
        foreach (Player player in GameStateManager.Instance.state.players) {
            foreach (Task task in player.tasks) {
                GameObject taskInstance;
                _tasks.TryGetValue(task.id, out taskInstance);
                taskInstance.SetActive(true);
            }
        }
    }

    void AssignTaskIds() {
        List<string> taskIds = new List<string>();
        int index = 0;
        foreach (Transform child in transform) {
            child.gameObject.SetActive(false);
            TaskState state = child.GetComponent<TaskState>();
            string id = index.ToString();
            state.Initialize(id);
            _tasks.Add(id, child.gameObject);
            index++;
        }
    }

    public List<string> AvailableTaskIds() {
        return _tasks.Select(pair => pair.Key).ToList();
    }
}
