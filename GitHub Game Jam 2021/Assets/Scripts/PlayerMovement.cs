using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float speed = 5.0f;
    private SpriteRenderer _renderer;
    private Rigidbody2D _rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
        _rigidBody.velocity = new Vector2(inputX * speed, inputY * speed);

        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            _renderer.flipX = false;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            _renderer.flipX = true;
        }

        // Vector3 movement = new Vector3(speed.x * inputX, speed.y * inputY, 0);

        // movement *= Time.deltaTime;

        // transform.Translate(movement);
        // _rigidBody.MovePosition(_rigidBody.position + speed * Time.fixedDeltaTime);
    }
}
