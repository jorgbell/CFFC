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
        resetCooldown();
        RoundManager.instance.eBombExploded.AddListener(resetCooldown);
    }

    private void Update()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * 10, Color.magenta);
    }

    private void OnEnable()
    {
        var main = GetComponentInChildren<ParticleSystem>().main;
        main.prewarm = true;
    }

    public override void Attack()
    {
        Debug.Log("chiquita bomba");

        if (!onCooldown) 
        {
            //var main = GetComponentInChildren<ParticleSystem>().main;
            GetComponentInChildren<ParticleSystem>().Stop();
            GetComponentInChildren<ParticleSystem>().Clear();

            explosionPrefab.GetComponentInChildren<ExplosionCollision>().setColor(m_colorComponent);

            GameObject bomb = Instantiate(bombPrefab, transform.position + transform.lossyScale.z * transform.forward, Quaternion.identity);
            bomb.GetComponent<BombProjectileBehaviour>().setExplosionSystem(explosionPrefab, explosionPrefab.GetComponentInChildren<ParticleSystem>());

            Rigidbody bombRB = bomb.GetComponent<Rigidbody>();

            bombRB.velocity = player.GetComponent<Rigidbody>().velocity / 2 + bombVelocity * transform.forward;

            onCooldown = true;
        }
    }

    protected override void AttackAnim()
    {
        throw new System.NotImplementedException();
    }

    protected override void ResetAttackAnim()
    {
        throw new System.NotImplementedException();
    }

    private void resetCooldown() 
    { 
        onCooldown = false;
        //var main = GetComponentInChildren<ParticleSystem>().main;
        GetComponentInChildren<ParticleSystem>().Play();
    }
}
