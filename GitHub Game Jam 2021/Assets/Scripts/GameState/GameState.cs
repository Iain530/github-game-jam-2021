using System;
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
    public int messageTime;
    public bool gameStarted;
    public List<Bee> bees = new List<Bee>();
    public List<Task> tasks = new List<Task>();
    public List<Player> players = new List<Player>();

    public Vector2? GetBeePosition(string id) {
        Bee bee = bees.Find(bee => bee.id == id);
        return bee.position;
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
    public int currentTaskIndex;
    public bool isQueenBee;
}
