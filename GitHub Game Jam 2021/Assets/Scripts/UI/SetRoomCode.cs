using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetRoomCode : MonoBehaviour {
    void Start()
    {
        
    }

    void Update()
    {
        GameObject.Find("RoomName").GetComponent<Text>().text = GameStateManager.Instance.CurrentGameCode;
    }
}
