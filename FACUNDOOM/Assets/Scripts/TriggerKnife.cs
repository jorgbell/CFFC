using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class TriggerKnife : MonoBehaviour
{
    ColorComponent m_colorComponent;
    private void Awake()
    {
        m_colorComponent = transform.parent.GetComponent<ColorComponent>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Enemy>(out Enemy targetEnemy))
        {
            Debug.Log("navajazo");
            targetEnemy.Hit(m_colorComponent.GetColor());
        }
    }
}
