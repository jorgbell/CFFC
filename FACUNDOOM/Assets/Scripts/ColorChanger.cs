using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public List<ColorType> colors = new List<ColorType>();

    public enum ColorChangeType { ChangeAllColors, ChangeAllColorsFullRandom }
    [SerializeField]
    ColorChangeType colorChangeType = ColorChangeType.ChangeAllColors;

    [SerializeField]
    private float changeFrequency = 2.0f, firstChangeTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < ((int)ColorType.lastColor); i++)
        {
            colors.Add(((ColorType)i));
        }
        InvokeRepeating(colorChangeType.ToString(), firstChangeTime, changeFrequency);

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

    void ChangeAllColorsFullRandom()
    {
        foreach (Enemy e in GetComponentsInChildren<Enemy>())
        {
            e.SetColor((ColorType)(Random.Range(0, colors.Count)));
        }
    }
}
