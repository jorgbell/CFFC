using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Weapon
{
    public float bombVelocity;

    public GameObject player;

    public GameObject bombPrefab;
    public GameObject explosionPrefab;

    private void Start()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;

    }

    private void Update()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * 10, Color.magenta);
    }

    public override void Attack()
    {
        Debug.Log("chiquita bomba");

        GameObject bomb = Instantiate(bombPrefab, transform.position + transform.lossyScale.z * transform.forward, Quaternion.identity);
        bomb.GetComponent<BombProjectileBehaviour>().setExplosionSystem(explosionPrefab, explosionPrefab.GetComponentInChildren<ParticleSystem>());

        Rigidbody bombRB = bomb.GetComponent<Rigidbody>();

        bombRB.velocity = player.GetComponent<Rigidbody>().velocity + bombVelocity * transform.forward;
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
