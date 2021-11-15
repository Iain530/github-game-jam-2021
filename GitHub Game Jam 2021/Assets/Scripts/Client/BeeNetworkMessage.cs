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
public class LeaveLobbyMessage {
    public string messageType = "LEAVE_GAME_LOBBY";
    public string gameId;
    public string secretToken;

    public LeaveLobbyMessage() {
        gameId = GameStateManager.Instance.CurrentGameID;
        secretToken = GameStateManager.Instance.SecretToken;
    }
}

[Serializable]
public class KickPlayerMessage {
    public string messageType = "KICK_PLAYER";
    public string gameId;
    public string secretToken;
    public string playerId;

    public KickPlayerMessage(string playerId) {
        gameId = GameStateManager.Instance.CurrentGameID;
        secretToken = GameStateManager.Instance.SecretToken;
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
        taskIds = BeeTaskManager.Instance.AvailableTaskIds();
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
public class AssignTaskMessage {
    public string messageType = "ASSIGN_TASK";
    public string gameId;
    public string secretToken;
    public string playerId;
    public string taskId;

    public AssignTaskMessage(string taskId, string playerId) {
        gameId = GameStateManager.Instance.CurrentGameID;
        secretToken = GameStateManager.Instance.SecretToken;
        this.taskId = taskId;
        this.playerId = playerId;
    } 
}


[Serializable]
public class AiBeesPositionMessage {
    public string messageType = "AI_POSITION_UPDATE";
    public string gameId;
    public string secretToken;
    public List<BeePosition> beePositions = new List<BeePosition>();

    public AiBeesPositionMessage(List<BeePosition> beePositions) {
        gameId = GameStateManager.Instance.CurrentGameID;
        secretToken = GameStateManager.Instance.SecretToken;
        this.beePositions = beePositions;
    }
}

[Serializable]
public class UpdateBeeNameMessage {
    public string messageType = "UPDATE_BEE_NAME";
    public string gameId;
    public string secretToken;
    public string beeId;
    public string name;
    public bool isPlayerBee;

    public UpdateBeeNameMessage(string beeId, string name, bool isPlayerBee) {
        gameId = GameStateManager.Instance.CurrentGameID;
        secretToken = GameStateManager.Instance.SecretToken;
        this.beeId = beeId;
        this.name = name;
        this.isPlayerBee = isPlayerBee;
    }
}

[Serializable]
public class UpdateBeeHatMessage {
    public string messageType = "UPDATE_BEE_HAT";
    public string gameId;
    public string secretToken;
    public string beeId;
    public string hat;
    public bool isPlayerBee;

    public UpdateBeeHatMessage(string beeId, string hat, bool isPlayerBee) {
        gameId = GameStateManager.Instance.CurrentGameID;
        secretToken = GameStateManager.Instance.SecretToken;
        this.beeId = beeId;
        this.hat = hat;
        this.isPlayerBee = isPlayerBee;
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
