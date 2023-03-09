using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Range(1, 10)]
    public float moveSpeed;

    public Rigidbody rb;

    [SerializeField]
    bool moving = false;

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(moveX, 0f, moveZ);

        if (movement.magnitude > 0)
        {
            movement = Quaternion.FromToRotation(Vector3.forward, transform.forward) * movement.normalized * moveSpeed;
            moving = true;
        }

        else moving = false;

        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

        Debug.DrawLine(transform.position, transform.position + transform.forward, Color.red);
        //Debug.Log(transform.forward);

        //Debug.Log(moveX);
        //Debug.Log(moveZ);
    }

    public bool isMoving() { return moving; }
}
