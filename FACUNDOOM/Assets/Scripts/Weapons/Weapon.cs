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
    protected ColorComponent m_colorComponent = null;

    void Start()
    {

    }

    protected void OnColorChanged(ColorType colorType)
    {
        if (colorType == ColorType.red)
            GetComponent<MeshRenderer>().material.color = Color.red;
        else GetComponent<MeshRenderer>().material.color = Color.blue;
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !onCooldown)
        {
            Attack();
            //StopAllCoroutines();
            //StartCoroutine(CooldownCoroutine());
            onCooldown = true;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            ColorComponent weaponColor;
            if(TryGetComponent(out weaponColor))
            {
                weaponColor.SetColor((ColorType)(((int)weaponColor.GetColor() + 1) % (int)ColorType.lastColor));
            }
        }
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

        ResetAttackAnim();
        onCooldown = false;
    }
    abstract protected void AttackAnim();
    abstract protected void ResetAttackAnim();
}
