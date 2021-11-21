using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StealBabyTaskBehaviour : TaskBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{

    private GameObject babyBeingDragged;
    private Image box;
    private List<GameObject> babyBees = new List<GameObject>();
    private List<Vector3> initialBabyBeePositions = new List<Vector3>();
    
    private int capturedBabiesCount;
    private int numberOfBabies = 9;

    // Start is called before the first frame update
    new void Start()
    {
        box = this.gameObject.transform.Find("Canvas").Find("Box").GetComponent<Image>();
        foreach (Transform child in this.gameObject.transform.Find("Canvas").transform) {
            if (child.tag == "TaskInteractable") {
                babyBees.Add(child.gameObject);
                initialBabyBeePositions.Add(child.transform.position);
            }
        }
        
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        
        if (Input.GetKeyDown("e")) {
            reset();
        }
        
        if (babyBeingDragged != null && box.GetComponent<BabyInBoxBehaviour>().getInBox() != null) {
            babyBeingDragged.SetActive(false);
            babyBeingDragged = null;
            capturedBabiesCount++;
        }
        
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
    
    private void reset() {
        capturedBabiesCount = 0;
        for (int i = 0; i < numberOfBabies; i++) {
            babyBees[i].transform.position = initialBabyBeePositions[i];
        }
    }
}
