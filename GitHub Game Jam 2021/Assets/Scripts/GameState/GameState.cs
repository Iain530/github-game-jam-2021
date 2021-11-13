using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class GameState {
    public string gameId;
    public int messageTime;
    public bool gameStarted;
    public List<Bee> bees = new List<Bee>();
    public List<Task> tasks = new List<Task>();
    public List<Player> players = new List<Player>();
}


[Serializable]
public class Bee {
    public string id;
    public string name;
    public Vector2 position;
    public bool isPlayer;
}


[Serializable]
public class Task {
    public string id;
    public bool complete;
}


[Serializable]
public class Player {
    public string id;
    public string beeId;
    public string currentTaskId;
    public bool isQueenBee;
}
