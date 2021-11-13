using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskBehaviour : MonoBehaviour
{

    private Transform canvas;
    private Text spaceCountText;
    
    private bool playerPresent;
    private bool uiVisible;
    private bool complete;
    
    private int spacePressCountTarget = 10;
    private int spacePressCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        canvas = this.gameObject.transform.GetChild(0);
        spaceCountText = GameObject.Find("Space Count").GetComponent<Text>();
        hideUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("e") && !complete)
        {
            if (uiVisible) {
                hideUI();
            } else if (playerPresent) {
                showUI();
            }
        }
        
        if (Input.GetKeyDown("space") && playerPresent) {
            spacePressCount++;
            if (spacePressCount >= spacePressCountTarget) {
            	complete = true;
            	hideUI();
            } else {
            	spaceCountText.text = spacePressCount + " / " + spacePressCountTarget;
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
    	if (complete) return;
    	if (other.gameObject.tag == "Player") {
    	    playerPresent = true;
    	}
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") {
    	    playerPresent = false;
    	    hideUI();
    	}
    }
    
    private void showUI() {
        foreach (Transform child in canvas.transform) {
            child.gameObject.SetActive(true);
        }
        uiVisible = true;
    }
    
    private void hideUI() {
        foreach (Transform child in canvas.transform) {
            child.gameObject.SetActive(false);
        }
        uiVisible = false;
        spacePressCount = 0;
        spaceCountText.text = spacePressCount + " / " + spacePressCountTarget;
    }
}
