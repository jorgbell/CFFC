using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities
{
    public static void ShuffleColorArray(ref ColorType[] colors)
	{
		for (int i = 0; i < colors.Length; i++)
		{
			int index = Random.Range(i, colors.Length);
			ColorType aux = colors[i];
			colors[i] = colors[index];
			colors[index] = aux;
		}
	}
}
