using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : Weapon
{
    Collider col;
    Animator anim;
    Transform initialTransform;

    private void Start()
    {
        col = GetComponentInChildren<Collider>(true);
        col.gameObject.SetActive(false);
        anim = GetComponent<Animator>();
        anim.keepAnimatorStateOnDisable = false;
        initialTransform = transform;
    }

    public override void Attack()
    {
        if (!onCooldown)
        {
            Debug.Log("navajazo");
            onCooldown = true;
            col.gameObject.SetActive(true);
            anim.SetTrigger("attack");
        }
    }

    private void OnDisable()
    {
        transform.localPosition = initialTransform.localPosition;
        transform.localRotation = initialTransform.localRotation;
        ResetAttackAnim();
        turnOffCollider();
    }
    //private void OnEnable()
    //{
    //    ResetAttackAnim();
    //    turnOffCollider();
    //}

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
