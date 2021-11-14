using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class TaskManager : MonoBehaviour
{
    string taskPositionsParentName = "TaskManager";

    private List<Transform> tasks = new List<Transform>();
    private Transform targetTransform;
    private bool timerStarted;
    private NPBeeAI ai;
    private float aiAssignedSpeed;

    // Start is called before the first frame update
    void Start()
    {
        bool timerStarted = false;
        ai = GetComponent<NPBeeAI>();
        aiAssignedSpeed = ai.speed;
        InitialisePositions();
        ChooseNextTarget();
    }

    void InitialisePositions()
    {
        foreach (Transform child in GameObject.Find(taskPositionsParentName).transform)
        {
            tasks.Add(child);
        }
    }

    void ChooseNextTarget()
    {
        Debug.Log("Task.count: "+tasks.Count);
        Transform perspectiveTargetPos = tasks[Random.Range(0, tasks.Count)];
        while (targetTransform == perspectiveTargetPos)
        {
            perspectiveTargetPos = tasks[Random.Range(0, tasks.Count)];
        }
        targetTransform = perspectiveTargetPos;
        Debug.Log("New target: " + targetTransform.position);
        ai.target = targetTransform;
        // Reassign speed to chosen speed
        ai.speed = aiAssignedSpeed;
    }

    private IEnumerator Countdown()
    {
        float duration = Random.Range(5f, 11f); // Random time between 5-10s for a task
        float endDuration = 0f;
        timerStarted =  true;
        Debug.Log("Start task!");
        // AI bee stop moving whilst does task
        ai.speed = 0;
        while (endDuration < duration)
        {
            duration -= Time.deltaTime;
            yield return null; 
        }
        Debug.Log("Finished task!");
        timerStarted = false;
        ChooseNextTarget();
    }
    
    void FixedUpdate()
    {
        Vector3 NPBeePosition = GetComponent<Transform>().position;
        if (Vector2.Distance(NPBeePosition, targetTransform.position) < 0.05f && !timerStarted)
        {
            StartCoroutine(Countdown());
        }
    }
}
