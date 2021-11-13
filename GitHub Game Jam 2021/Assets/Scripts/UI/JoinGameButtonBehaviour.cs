using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinGameButtonBehaviour : MonoBehaviour {

    public InputField inputField;
    public Text validationMessage;

    string BAD_CODE_FORMAT = "Please enter a 4 letter code";
    string JOINING = "Joining game lobby...";

    public void OnClickJoinGame() {
        string gameCode = inputField.text.ToUpper().Trim();
        if (gameCode.Length != 4) {
            validationMessage.text = BAD_CODE_FORMAT;
            return;
        }

        validationMessage.text = JOINING;
        BeeNetworkClient.Instance.JoinGame(gameCode);
    }

    public void LoadJoinGameScene()
    { 
        Application.LoadLevel(2);
    }
}
