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

	public static Color[] m_colorList = { new Color(0.98f, 0.4f, 0.2f), new Color(0.45f,0.666f,0.145f), new Color(0.466f,0.8f,0.88f) };

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
		m_colorType = color;
		m_eColorChanged?.Invoke(color);
	}
}