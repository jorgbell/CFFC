using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public float coolDown;
    [SerializeField]
    protected bool onCooldown = false;
    protected float elapsed = 0.0f;

    [SerializeField] 
    protected ColorComponent m_colorComponent = null;

    void Start()
    {
        m_colorComponent.OnColorChanged.AddListener(OnColorChanged);
    }

    virtual protected void OnColorChanged(ColorType colorType)
    {
        if (colorType == ColorType.red)
            GetComponent<MeshRenderer>().material.color = Color.red;
        else GetComponent<MeshRenderer>().material.color = Color.blue;
    }


    abstract public void Attack();
    protected IEnumerator CooldownCoroutine()
    {
        elapsed = 0.0f;

        while (elapsed < coolDown)
        {
            elapsed += Time.deltaTime;
            AttackAnim();
            yield return null;
        }

        ResetCooldown();
        onCooldown = false;
    }
    abstract protected void AttackAnim();
    abstract protected void ResetCooldown();
}
