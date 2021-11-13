using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class PlayerPositionMessage {
    public string messageType = "PLAYER_POSITION_UPDATE";
    public string gameId;
    public string playerId;
    public Vector2 position;

    public PlayerPositionMessage(Vector2 position) {
        gameId = GameStateManager.Instance.CurrentGameID;
        playerId = GameStateManager.Instance.CurrentPlayerId;
        this.position = position;
    }
}


[Serializable]
public class TaskMessage {
    public string messageType = "TASK_UPDATE";
    public string gameId;
    public string playerId;
    public string taskId;
    public string taskStatus;
}


[Serializable]
public class AiBeesPositionMessage {
    public string messageType = "AI_POSITION_UPDATE";
    public string gameId;
    public string playerId;
    public List<BeePosition> beePositions = new List<BeePosition>();

    public AiBeesPositionMessage(List<BeePosition> beePositions) {
        gameId = GameStateManager.Instance.CurrentGameID;
        playerId = GameStateManager.Instance.CurrentPlayerId;
        this.beePositions = beePositions;
    }
}


[Serializable]
public class BeePosition {
    public string id;
    public Vector2 position;

    public BeePosition(string id, Vector2 position) {
        this.id = id;
        this.position = position;
    }
}


[Serializable]
public class ServerConnectResponse {
    public string gameId;
    public string gameCode;
    public string playerId;
}
