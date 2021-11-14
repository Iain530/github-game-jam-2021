using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class JoinLobbyMessage {
    public string messageType = "JOIN_GAME_LOBBY";
    public string gameId;
    public string playerId;

    public JoinLobbyMessage(string gameId, string playerId) {
        this.gameId = gameId;
        this.playerId = playerId;
    }
}

[Serializable]
public class BeginGameMessage {
    public string messageType = "START_GAME";
    public string gameId;
    public string secretToken;
    public List<string> taskIds;
 
    public BeginGameMessage() {
        gameId = GameStateManager.Instance.CurrentGameID;
        secretToken = GameStateManager.Instance.SecretToken;
        taskIds = TaskManager.Instance.AvailableTaskIds();
    }
}


[Serializable]
public class PlayerPositionMessage {
    public string messageType = "PLAYER_POSITION_UPDATE";
    public string gameId;
    public string secretToken;
    public Vector2 position;

    public PlayerPositionMessage(Vector2 position) {
        gameId = GameStateManager.Instance.CurrentGameID;
        secretToken = GameStateManager.Instance.SecretToken;
        this.position = position;
    }
}


[Serializable]
public class TaskCompleteMessage {
    public string messageType = "TASK_COMPLETE";
    public string gameId;
    public string secretToken;
    public string taskId;

    public TaskCompleteMessage(string taskId) {
        gameId = GameStateManager.Instance.CurrentGameID;
        secretToken = GameStateManager.Instance.SecretToken;
        this.taskId = taskId;
    }
}


[Serializable]
public class AiBeesPositionMessage {
    public string messageType = "AI_POSITION_UPDATE";
    public string gameId;
    public string secretToken;
    public Dictionary<string, BeePosition> beePositions = new Dictionary<string, BeePosition>();

    public AiBeesPositionMessage(Dictionary<string, BeePosition> beePositions) {
        gameId = GameStateManager.Instance.CurrentGameID;
        secretToken = GameStateManager.Instance.SecretToken;
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
