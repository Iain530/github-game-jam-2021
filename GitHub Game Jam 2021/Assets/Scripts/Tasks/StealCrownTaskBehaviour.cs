using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StealCrownTaskBehaviour : TaskBehaviour, IPointerClickHandler
{

    private Image crown;
    private List<GameObject> laserSources;
    private int totalLaserCount;
    private int clickedLaserCount;

    // Start is called before the first frame update
    void Start()
    {
        crown = this.gameObject.transform.Find("Canvas").Find("Crown").GetComponent<Image>();
        
        laserSources = new List<GameObject>();
        foreach (Transform child in this.gameObject.transform.Find("Canvas").transform) {
            if (child.tag == "TaskInteractable") {
                laserSources.Add(child.gameObject);
            }
        }
        totalLaserCount = laserSources.Count;
        
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        
        if (Input.GetKeyDown("e")) {
            reset();
        }
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject clickTarget = eventData.pointerCurrentRaycast.gameObject;
        if (clickTarget.name == "Crown") {
            if (clickedLaserCount >= totalLaserCount) {
                completeTask();
                hideUI();
            } else {
                reset();
            }
        } else if (clickTarget.tag == "TaskInteractable") {
            clickTarget.transform.Find("Laser").gameObject.SetActive(false);
            clickedLaserCount++;
        } else {
            reset();
        }
    }
    
    private void reset() {
        clickedLaserCount = 0;
        if (!uiVisible) return;
        foreach (Transform child in canvas.transform) {
            if (child.tag == "TaskInteractable") {
                child.gameObject.SetActive(true);
                child.transform.Find("Laser").gameObject.SetActive(true);
            }
        }
    }
}
