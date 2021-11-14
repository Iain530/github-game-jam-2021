using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpotlightMovement : MonoBehaviour, IPointerClickHandler
{
    public Transform targetCamera;
    private Vector3 initalOffset;
    private Vector3 cameraPosition;

    void Start()
    {
        initalOffset = transform.position - targetCamera.position;
    }

    void Update()
    {
        cameraPosition = targetCamera.position + initalOffset;
        transform.position = cameraPosition;
    }

    public void OnMouseDown(PointerEventData eventData)
    {
        Debug.Log("Clicked");
        GameObject clickTarget = eventData.pointerCurrentRaycast.gameObject;
        Debug.Log(clickTarget.name);
        if (clickTarget.tag == "Player") {
            Debug.Log("Clicked on player");
            clickTarget.GetComponent<BeeState>().PlayerDeath();
        }
    }


    void FixedUpdate()
    {
        BeeNetworkClient.Instance.SendCurrentPosition(transform.position);
    }
}
