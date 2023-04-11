using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : Weapon
{
    Collider col;
    Animator anim;
    Vector3 initialPos;
    Quaternion initialRot;
    AnimatorClipInfo clipInfo;

    private void Start()
    {
        col = GetComponentInChildren<Collider>(true);
        col.gameObject.SetActive(false);
        anim = GetComponent<Animator>();
        anim.keepAnimatorStateOnDisable = false;
        initialPos = transform.localPosition;
        initialRot = transform.localRotation;
    }

    private void Update()
    {
        Debug.Log(anim.speed);
        anim.speed = 0.9f / coolDown;
    }

    public override void Attack()
    {
        if (!onCooldown)
        {
            Debug.Log("navajazo");
            onCooldown = true;
            col.gameObject.SetActive(true);
            anim.SetTrigger("attack");
            AudioManager.instance.Play("SFX_sword");
        }
    }

    private void OnDisable()
    {
        transform.localPosition = initialPos;
        transform.localRotation = initialRot;
        ResetCooldown();
        turnOffCollider();
    }

    void turnOffCollider()
    {
        col.gameObject.SetActive(false);
    }

    void turnOnCollider()
    {
        col.gameObject.SetActive(true);
    }

    protected override void AttackAnim()
    {
    }

    protected override void ResetCooldown()
    {
        onCooldown = false;
    }

}
