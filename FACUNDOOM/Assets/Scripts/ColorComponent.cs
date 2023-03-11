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
    private ColorType m_colorType;

    public ColorChangedEvent OnColorChanged { get
        {
            if (m_eColorChanged == null)
            {
                m_eColorChanged = new ColorChangedEvent();
            }
            return m_eColorChanged;} 
    }


    public ColorType GetColor() { return m_colorType; }

    private void Start()
    {

    }
    public void SetColor(ColorType color) { 
        m_colorType = color;
        m_eColorChanged?.Invoke(color);
    }
}