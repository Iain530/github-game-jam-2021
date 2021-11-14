using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyInBoxBehaviour : MonoBehaviour
{

    private GameObject inBox;

    public void OnTriggerEnter2D(Collider2D other)
    {
    	if (other.gameObject.tag == "TaskInteractable") {
    	    inBox = gameObject;
    	}
    }
    
    public void OnTriggerExit2D(Collider2D other)
    {
    	if (other.gameObject.tag == "TaskInteractable") {
    	    inBox = null;
    	}
    }
    
    public GameObject getInBox() {
        return inBox;
    }
}
