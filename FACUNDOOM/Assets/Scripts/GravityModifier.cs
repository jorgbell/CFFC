using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityModifier : MonoBehaviour
{
    public float gravityMultiplier;

    Rigidbody bombRB;

    // Start is called before the first frame update
    void Start()
    {
        bombRB = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        bombRB.AddForce(new Vector3(0f, Physics.gravity.y * gravityMultiplier - Physics.gravity.y, 0f), ForceMode.Acceleration);
    }
}
