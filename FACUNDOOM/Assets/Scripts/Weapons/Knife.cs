using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : Weapon
{
    private void Start()
    {
        GetComponent<MeshRenderer>().material.color = Color.green;

    }
    public override void Attack()
    {
        Debug.Log("navajazo");
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
