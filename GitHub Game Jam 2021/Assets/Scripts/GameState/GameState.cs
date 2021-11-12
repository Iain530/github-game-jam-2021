using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class GameState {
    public int gameId;
    public Bee[] bees;
    public Task[] tasks;
    public Player[] players;
}


[Serializable]
public class Bee {
    public int id;
    public string name = "";
    public Vector2 position;
    public bool isPlayer;
}


[Serializable]
public class Task {
    public int id;
    public bool complete;
}


public class Player {
    public int id;
    public bool isQueenBee;
}
