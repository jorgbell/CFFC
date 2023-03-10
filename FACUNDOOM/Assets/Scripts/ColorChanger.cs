using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("ChangeAllColors", 0, 2);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void ChangeAllColors()
    {
        foreach(Enemy e in GetComponentsInChildren<Enemy>())
        {
            e.ChangeColor();
        }
    }
}
