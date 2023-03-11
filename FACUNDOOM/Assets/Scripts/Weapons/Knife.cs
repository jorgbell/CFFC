using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : Weapon
{
    [SerializeField]
    Camera camera;
    private Animator anim;
    bool isAttacking = false;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public override void Attack()
    {
        if (!onCooldown)
        {
            Debug.Log("attacking");
            onCooldown = true;
            StopAllCoroutines();
            StartCoroutine("CooldownCoroutine");
        }



    }

    protected override void AttackAnim()
    {
        if (!isAttacking)
        {
            anim.SetTrigger("attack");
            isAttacking = true;
        }
    }

    protected override void ResetAttackAnim()
    {
        isAttacking = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out Enemy targetEnemy) && onCooldown)
        {
            Debug.Log("navajazo");
            targetEnemy.Hit(m_colorComponent.GetColor());
        }
    }
}
