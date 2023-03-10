using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Range(1, 10)]
    public float moveSpeed;

    [Range(1, 50)]
    public float jumpForce;

    public Rigidbody rb;

    [SerializeField]
    bool moving = false;

    [SerializeField]
    bool jumping = false;

    float floorDistance;
    int floorFrames = 0; // How many frames the player has touched the floor, to check if it should be able to jump

    void Update()
    {
        #region HorizontalCalculations
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
        #endregion

        #region JumpCalculations

        RaycastHit hit1;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit1, 100)) floorDistance = Mathf.Abs(hit1.point.y - transform.position.y);

        if (floorDistance < GetComponent<CapsuleCollider>().height / 2 + 0.1 && rb.velocity.y <= 0f)
        {
            floorFrames++;
            if (floorFrames > 5) jumping = false;
        }

        else floorFrames = 0;

        if (Input.GetKey(KeyCode.Space) && !jumping) 
        {
            if(rb.velocity.y <= 0f) rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            jumping = true;
        }


        #endregion
    }

    public bool isMoving() { return moving; }
}
