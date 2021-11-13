using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class GameState {
    public string gameId;
    public int messageTime;
    public bool gameStarted;
    public Dictionary<string, Bee> bees = new Dictionary<string, Bee>();
    public List<Task> tasks = new List<Task>();
    public List<Player> players = new List<Player>();

    public Vector2? GetBeePosition(string id) {
        Bee bee;
        bees.TryGetValue(id, out bee);
        return bee?.position;
    }
}


[Serializable]
public class Bee {
    public string id;
    public string name;
    public Vector2 position;
}


[Serializable]
public class Task {
    public string id;
    public bool complete;
}


[Serializable]
public class Player {
    public string id;
    public Bee bee;
    public int currentTaskIndex;
    public bool isQueenBee;
}
