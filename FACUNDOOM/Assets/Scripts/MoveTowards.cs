using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowards : MonoBehaviour
{
    [Range(0, 10)]
    public float moveSpeed = 1;


    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.LookAt(target, Vector3.up);
        Transform target = RoundManager.instance.getPlayer().transform;
        Vector3 movement = new Vector3(target.position.x - transform.position.x, 0f, target.position.z - transform.position.z);
        rb.rotation = Quaternion.LookRotation(movement, Vector3.up);
        //movement = Quaternion.FromToRotation(Vector3.forward, transform.forward) * movement.normalized * moveSpeed;

        movement = movement.normalized * moveSpeed;

        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
    }
}
