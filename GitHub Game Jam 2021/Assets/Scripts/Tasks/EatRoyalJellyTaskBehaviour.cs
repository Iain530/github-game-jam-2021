using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EatRoyalJellyTaskBehaviour : TaskBehaviour, IPointerDownHandler
{

    private int jellyEatenCount = 0;
    private int jellyCount = 21;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        
        if (Input.GetKeyDown("e")) {
            reset();
        }
        
        if (jellyEatenCount >= jellyCount) {
            completeTask();
            hideUI();
        }
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject clickTarget = eventData.pointerCurrentRaycast.gameObject;
        if (clickTarget.tag == "TaskInteractable") {
            clickTarget.SetActive(false);
            jellyEatenCount++;
        }
    }
    
    private void reset() {
        jellyEatenCount = 0;
    }
}
