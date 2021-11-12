using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class GameState {
    public int gameId;
    public Bee[] bees;
    public Task[] tasks;
}


[Serializable]
public class Bee {
    public int id;
    public string name = "";
    public Vector2 position;
}


[Serializable]
public class Task {
    public int id;
    public bool complete;
}
