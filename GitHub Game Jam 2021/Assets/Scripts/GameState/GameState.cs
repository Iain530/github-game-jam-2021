using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class GameState {
    public int gameId;
    public List<Bee> bees = new List<Bee>();
    public List<Task> tasks = new List<Task>();
    public List<Player> players = new List<Player>();
}


[Serializable]
public class Bee {
    public int id;
    public string name;
    public Vector2 position;
    public bool isPlayer;
}


[Serializable]
public class Task {
    public int id;
    public bool complete;
}


[Serializable]
public class Player {
    public int id;
    public bool isQueenBee;
}
