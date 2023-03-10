using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float spawnPosDelta = 0.6f;
    public int enemyType = 0;
    public ColorType colorType = ColorType.red;
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        if (colorType == ColorType.red)
            GetComponent<MeshRenderer>().material.color = Color.red;
        else GetComponent<MeshRenderer>().material.color = Color.blue;
        //InvokeRepeating("ChangeColor", 2, 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeColor()
    {
        List<ColorType> colors = new List<ColorType>();
        for (int i = 0; i < ((int)ColorType.lastColor); i++)
        {
            if (i != ((int)colorType))
                
                colors.Add(((ColorType)i));
        }
        colorType = colors[Random.Range(0, ((int)ColorType.lastColor) - 1)];

        if (colorType == ColorType.red)
            GetComponent<MeshRenderer>().material.color = Color.red;
        else GetComponent<MeshRenderer>().material.color = Color.blue;
    }

    public void SetColor(ColorType newColor)
    {
        colorType = newColor;
        if (colorType == ColorType.red)
            GetComponent<MeshRenderer>().material.color = Color.red;
        else GetComponent<MeshRenderer>().material.color = Color.blue;
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
        GameObject clone = Instantiate(gameObject, transform.parent);
        clone.GetComponent<Enemy>().colorType = colorType;
        transform.position += axis;
        clone.transform.position -= axis;
    }

    public void Death()
    {
        Destroy(gameObject);
    }

}
