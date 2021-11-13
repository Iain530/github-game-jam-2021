using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public interface BeeNetworkMessage {
    string messageType { get; }
}


[Serializable]
public class PlayerPositionMessage : BeeNetworkMessage {
    public string messageType { get { return "PLAYER_POSITION_UPDATE"; } }
    public int playerId;
    public Vector2 position;

    public PlayerPositionMessage(int playerId, Vector2 position) {
        this.playerId = playerId;
        this.position = position;
    }
}


[Serializable]
public class TaskMessage : BeeNetworkMessage {
    public string messageType { get { return "TASK_UPDATE"; } }
    public int taskId;
    public string taskStatus;
}


[Serializable]
public class AiBeesPositionMessage : BeeNetworkMessage {
    public string messageType { get { return "AI_POSITION_UPDATE"; } }
    public List<BeePosition> bees = new List<BeePosition>();
}


[Serializable]
public class BeePosition {
    public Vector2 position;
    public int beeId;
}
