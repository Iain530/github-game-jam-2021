using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskBehaviour : MonoBehaviour
{

    private Transform canvas;
    
    private bool playerPresent;
    private bool uiVisible;

    // Start is called before the first frame update
    void Start()
    {
        canvas = this.gameObject.transform.GetChild(0);
        hideUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            if (uiVisible) {
                hideUI();
            } else if (playerPresent) {
                showUI();
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
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
    }
}
