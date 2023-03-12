using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ColorChangedEvent : UnityEvent<ColorType>
{

}

public class ColorComponent : MonoBehaviour
{

	private ColorChangedEvent m_eColorChanged = null;
	[SerializeField]
	private ColorType m_colorType = ColorType.red;

	//red,purple,green
	public static Color[] m_colorList = { new Color(0.7019608f, 0.2470588f, 0.427451f), new Color(0.7019608f, 0.5764706f, 0.8823529f), new Color(0.7490196f, 0.854902f, 0.2196078f) };
	//color de arriba disminuido un 80% 
	static float percentajeCRT = 0.90f;
    public static Color[] m_colorListCRT = { new Color(
		(float)(m_colorList[0].r - (m_colorList[0].r*percentajeCRT)),
		(float)(m_colorList[0].g - (m_colorList[0].g*percentajeCRT)),
		(float)(m_colorList[0].b - (m_colorList[0].b*percentajeCRT))
		),
		new Color(
		(float)(m_colorList[1].r - (m_colorList[1].r*percentajeCRT)),
		(float)(m_colorList[1].g - (m_colorList[1].g*percentajeCRT)),
		(float)(m_colorList[1].b - (m_colorList[1].b*percentajeCRT))
		),
		new Color(
		(float)(m_colorList[2].r - (m_colorList[2].r*percentajeCRT)),
		(float)(m_colorList[2].g - (m_colorList[2].g*percentajeCRT)),
		(float)(m_colorList[2].b - (m_colorList[2].b*percentajeCRT))
		)
	};
    public ColorChangedEvent OnColorChanged
	{
		get
		{
			if (m_eColorChanged == null)
			{
				m_eColorChanged = new ColorChangedEvent();
			}
			return m_eColorChanged;
		}
	}


	public ColorType GetColor() { return m_colorType; }

	private void Start()
	{
		
	}
	public void SetColor(ColorType color)
	{
		Debug.Log("COMPONENT SETCOLOR");
		m_colorType = color;
		m_eColorChanged?.Invoke(color);
	}
}