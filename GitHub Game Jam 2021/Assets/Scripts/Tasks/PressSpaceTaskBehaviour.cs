using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PressSpaceTaskBehaviour : TaskBehaviour
{

    private Text spaceCountText;

    private int spacePressCountTarget = 10;
    private int spacePressCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        spaceCountText = this.gameObject.transform.Find("Canvas").Find("Space Count").GetComponent<Text>();
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        
        if (complete) return;
        
        if (Input.GetKeyDown("e")) {
            reset();
        }
        
        if (Input.GetKeyDown("space") && playerPresent) {
            spacePressCount++;
            if (spacePressCount >= spacePressCountTarget) {
            	completeTask();
            	hideUI();
            } else {
            	spaceCountText.text = spacePressCount + " / " + spacePressCountTarget;
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        if (complete) return;
        reset();
    }
    
    private void reset() {
        spacePressCount = 0;
        spaceCountText.text = spacePressCount + " / " + spacePressCountTarget;
    }
}
