using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public ColorType[] colors;
    [SerializeField]
    protected ColorComponent oPlayer = null;

    public enum ColorChangeType { ChangeAllColors, ChangeAllColorsFullRandom }
    [SerializeField]
    ColorChangeType colorChangeType = ColorChangeType.ChangeAllColors;

    [SerializeField]
    private float changeFrequency = 2.0f, firstChangeTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        colors = new ColorType[(int)ColorType.lastColor];
        for (int i = 0; i < ((int)ColorType.lastColor); i++)
        {
            colors[i] = (ColorType)i;
        }
        //InvokeRepeating(colorChangeType.ToString(), firstChangeTime, changeFrequency);

    }

    // Update is called once per frame
    void Update()
    {
    }

    void ChangeAllColors()
    {
        //Shuffle de tal forma que ningun elemento se quede donde este
        Utilities.ShuffleColorArray(ref colors);
        foreach (Enemy e in GetComponentsInChildren<Enemy>())
        {
            e.SetColor(colors[(int)e.enemyType]);
        }
    }

    void ChangeAllColorsFullRandom()
    {
        foreach (Enemy e in GetComponentsInChildren<Enemy>())
        {
            e.SetColor((ColorType)(Random.Range(0, colors.Length)));
        }
    }
}
