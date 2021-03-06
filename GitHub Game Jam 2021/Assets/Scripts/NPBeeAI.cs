using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class NPBeeAI : MonoBehaviour
{
    public Transform target;
    public float speed = 1f;
    // How close to waypoint before moving to next one
    public float nextWayPointDistance = 3f;
    public Transform npbGFX;

    Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;

    private Seeker seeker;
    private Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }
    
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    
    void FixedUpdate()
    {
        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2) path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        
        // rb.AddForce(force);
        rb.velocity= direction * speed;
        
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWayPointDistance)
        {
            currentWaypoint++;
        }
        
        // GOING RIGHT
        if (force.x >= 0.01f)
        {
            npbGFX.localScale = new Vector3(-1f, 1f, 1f);
        }   
        // GOING LEFT 
        else if (force.x <= -0.01f)
        {
            npbGFX.localScale = new Vector3(1f, 1f, 1f);
        }
    }
    
}
