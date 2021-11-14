using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StealBabyTaskBehaviour : TaskBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{

    private GameObject babyBeingDragged;
    private GameObject box;
    private int capturedBabiesCount;
    private int numberOfBabies = 7;

    // Start is called before the first frame update
    void Start()
    {
        crown = this.gameObject.transform.FindChild("Canvas").FindChild("Box").GetComponent<Image>();
        
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        
        //if (babyBeingDragged != null && babyBeingDragged.transform.rect.Overlaps(box.transform.rect)) {
          //  capturedBabiesCount++;
            //babyBeingDragged.SetActive(false);
        //}
        
        if (capturedBabiesCount >= numberOfBabies) {
            completeTask();
            hideUI();
        }
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject clickTarget = eventData.pointerCurrentRaycast.gameObject;
        if (clickTarget.tag == "TaskInteractable") {
            babyBeingDragged = clickTarget;
        }
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        babyBeingDragged = null;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if (babyBeingDragged != null) {
            babyBeingDragged.transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
    }
}
