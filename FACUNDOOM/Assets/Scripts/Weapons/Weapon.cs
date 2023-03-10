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
    protected ColorType colorType = ColorType.red;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !onCooldown)
        {
            Attack();
            StopAllCoroutines();
            StartCoroutine(CooldownCoroutine());
            onCooldown = true;
        }
    }

    abstract protected void Attack();
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
