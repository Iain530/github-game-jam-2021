using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskBehaviour : MonoBehaviour
{

    protected Transform canvas;
    private TaskState taskState;
    
    protected bool playerPresent;
    private bool uiVisible;
    protected bool complete;

    // Start is called before the first frame update
    protected void Start()
    {
        taskState = GetComponent<TaskState>();
        canvas = this.gameObject.transform.GetChild(0);
        hideUI();
    }

    // Update is called once per frame
    protected void Update()
    {
        if (Input.GetKeyDown("e") && !complete)
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
    
    protected void OnTriggerExit2D(Collider2D other)
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
    
    protected void hideUI() {
        foreach (Transform child in canvas.transform) {
            child.gameObject.SetActive(false);
        }
        uiVisible = false;
    }
    
    public bool isComplete() {
        return complete;
    }
    
    public void setComplete() {
        complete = true;
    }
    
    protected void completeTask() {
    	setComplete();
        taskState.OnComplete();
    }
}
