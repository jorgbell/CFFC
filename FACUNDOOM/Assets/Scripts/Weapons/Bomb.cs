using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Weapon
{
    private void Start()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;

    }
    public override void Attack()
    {
        Debug.Log("chiquita bomba");
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
