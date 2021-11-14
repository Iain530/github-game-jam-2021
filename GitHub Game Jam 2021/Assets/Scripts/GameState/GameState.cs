using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class GameStateUpdate {
    public GameState gameState;
    public bool success;
    public string secretToken;
    public string messageType;
}


[Serializable]
public class GameState {
    public string gameId;
    public string gameCode;
    public int lastUpdated;
    public bool gameStarted;
    public List<Bee> aiBees = new List<Bee>();
    public List<Task> tasks = new List<Task>();
    public List<Player> players = new List<Player>();

    public Vector2 GetBeePosition(string id) {
        Player player = players.Find(player => player.bee.id == id);
        if (player != null) {
            return player.bee.position;
        }
        Bee bee = aiBees.Find(aiBee => aiBee.id == id);    
        return bee.position;
    }

    public Task GetTaskWithId(string id) {
        foreach (Player p in players) {
            foreach (Task t in p.tasks) {
                if (t.id == id) {
                    return t;
                }
            }
        }
        return null;
    }
}


[Serializable]
public class Bee {
    public string id;
    public string hatName;
    public string name;
    public bool isPlayer;
    public Vector2 position;
}


[Serializable]
public class Task {
    public string id;  // for looking up task metadata
    public bool complete;
}

[Serializable]
public class Player {
    public string id;
    public Bee bee;
    public List<Task> tasks;
    public bool isQueenBee;

    public Task GetCurrentTask() {
        Task current = tasks.Find(task => !task.complete);
        return current;  // null if tasks complete
    }
}
