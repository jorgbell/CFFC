using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    [Range(0, 1000)]
    public float xSensitivity = 100f;

    [Range(0, 1000)]
    public float ySensitivity = 100f;

    public GameObject camera;

    float xRotation;
    float yRotation;

    // Start is called before the first frame update
    void Start()
    {
        xRotation = transform.rotation.y;
        yRotation = camera.transform.rotation.x;

        /*if (!Application.isEditor)*/
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * xSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * ySensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -89f, 89f);

        yRotation += mouseX;
       
        transform.SetPositionAndRotation(transform.position, Quaternion.Euler(new Vector3(0, yRotation, 0)));
        camera.transform.SetPositionAndRotation(camera.transform.position, Quaternion.Euler(new Vector3(xRotation, yRotation, 0)));

        Debug.Log("XRotation: " + xRotation + "YRotation: " + yRotation);
    }

    public Vector2 getRotations() { return new Vector2(xRotation, yRotation); }

    public void addRotationY(float r) { yRotation += r; }

    public void addRotationX(float r) { xRotation += r; }
}
