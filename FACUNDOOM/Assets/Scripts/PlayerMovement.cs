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

    public float floorDistance;
    public float floorTime = 0; // How much time the player has touched the floor, to check if it should be able to jump

    int timesJumped = 0;

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

        if (floorDistance < GetComponent<CapsuleCollider>().height / 2 + 0.1/* && rb.velocity.y <= 0f*/)
        {
            floorTime += Time.deltaTime;

            if (floorTime > 0.1f) jumping = false;
        }

        else floorTime = 0;

        if (Input.GetKey(KeyCode.Space) && !jumping) 
        {
            Debug.Log("Jumping: " + jumping + " Velocity: " + rb.velocity.y + " Floor Time: " + floorTime);

            floorTime = 0;
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            jumping = true;
            AudioManager.instance.Play("SFX_jump");

            timesJumped++;
        }

        #endregion
    }

    public bool isMoving() { return moving; }
}
