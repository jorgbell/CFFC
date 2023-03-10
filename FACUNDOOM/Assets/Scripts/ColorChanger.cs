using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public List<ColorType> colors = new List<ColorType>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < ((int)ColorType.lastColor); i++)
        {
            colors.Add(((ColorType)i));
        }
        InvokeRepeating("ChangeAllColors", 0, 2);

    }

    // Update is called once per frame
    void Update()
    {
    }

    void ChangeAllColors()
    {
        //Shuffle de tal forma que ningun elemento se quede donde este
        for (int i = 0; i < colors.Count; i++)
        {
            int index = Random.Range(i, colors.Count);
            ColorType aux = colors[i];
            colors[i] = colors[index];
            colors[index] = aux;
        }
        foreach (Enemy e in GetComponentsInChildren<Enemy>())
        {
            e.SetColor(colors[(int)e.enemyType]);
        }
    }
}
