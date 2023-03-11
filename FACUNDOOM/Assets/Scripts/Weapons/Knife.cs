using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : Weapon
{
    Collider col;
    private void Start()
    {
        col = GetComponentInChildren<Collider>(true);
        col.gameObject.SetActive(false);
    }

    public override void Attack()
    {
        if (!onCooldown)
        {
            col.gameObject.SetActive(true);
            StopAllCoroutines();
            StartCoroutine("CooldownCoroutine");
        }
    }

    protected override void AttackAnim()
    {
    }

    protected override void ResetAttackAnim()
    {
        col.gameObject.SetActive(false);
    }

}
