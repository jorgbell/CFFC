using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : Weapon
{
    Collider col;
    Animator anim;

    private void Start()
    {
        col = GetComponentInChildren<Collider>(true);
        col.gameObject.SetActive(false);
        anim = GetComponent<Animator>();
    }

    public override void Attack()
    {
        if (!onCooldown)
        {
            onCooldown = true;
            col.gameObject.SetActive(true);
            anim.SetTrigger("attack");
        }
    }

    void turnOffCollider()
    {
        col.gameObject.SetActive(false);
    }

    protected override void AttackAnim()
    {
    }

    protected override void ResetAttackAnim()
    {
        onCooldown = false;
    }

}
