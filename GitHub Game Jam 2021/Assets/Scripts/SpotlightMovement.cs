using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightMovement : MonoBehaviour
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

    void FixedUpdate()
    {
        BeeNetworkClient.Instance.SendCurrentPosition(transform.position);
    }
}
