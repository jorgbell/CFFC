using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class TriggerKnife : MonoBehaviour
{
    [SerializeField]
    bool onlyKillOne = true;
    ColorComponent m_colorComponent;
    int hitCount = 0;

    private void Awake()
    {
        m_colorComponent = transform.parent.GetComponent<ColorComponent>();
    }
    private void OnTriggerEnter(Collider other)
    {
        bool canKill;
        if ((onlyKillOne && hitCount == 0) || !onlyKillOne) canKill = true;
        else canKill = false;

        if (canKill && other.TryGetComponent<Enemy>(out Enemy targetEnemy))
        {
            Debug.Log("navajazo" + hitCount);
            hitCount++;
            targetEnemy.Hit(m_colorComponent.GetColor());
        }
    }

    private void OnEnable()
    {
        hitCount = 0;
    }



}
