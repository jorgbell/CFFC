using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Weapon
{
    [SerializeField]
    private float initialVel = 5.0f;
    [SerializeField]
    public GameObject camera;

    private float angle = 0.0f;
    private float H; //altura desde la que se lanza

    private void Start()
    {
        H = camera.GetComponent<Transform>().position.y;
    }

    protected override void Attack()
    {
        //shoot projectile
        //angle = camera.transform.forward;
    }

    protected override void AttackAnim()
    {
        throw new System.NotImplementedException();
    }

    protected override void ResetAttackAnim()
    {
        throw new System.NotImplementedException();
    }

}
