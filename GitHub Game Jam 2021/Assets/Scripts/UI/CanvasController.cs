using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public GameObject panelToHide;
    public GameObject panelToShow;
    public GameObject[] panelsList;
    public GameObject beginningPanel;

    // Start is called before the first frame update
    void Start()
    {
        if(panelsList.Length > 0 && beginningPanel != null)
        {
            foreach (GameObject panel in panelsList)
            {
                panel.SetActive(false);
            }
            beginningPanel.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchPanels()
    { 
        panelToHide.SetActive(false);
        panelToShow.SetActive(true);
    }
}
