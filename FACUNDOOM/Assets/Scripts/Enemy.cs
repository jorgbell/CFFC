using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float spawnPosDelta = 0.6f;
    private ColorType colorType = ColorType.red;
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit(ColorType hitColor)
    {
        if (hitColor == colorType)
        {
            Death();
        }
        else
        {
            Duplicate();
        }
    }

    public void Duplicate()
    {
        Vector2 dir = Vector2.Perpendicular(new Vector2(transform.position.x - player.position.x, transform.position.z - player.position.z));

        Vector3 axis = new Vector3(dir.x, 0, dir.y).normalized * spawnPosDelta;
        GameObject clone = Instantiate(gameObject);
        transform.position += axis;
        clone.transform.position -= axis;
    }

    public void Death()
    {
        Destroy(gameObject);
    }

}
