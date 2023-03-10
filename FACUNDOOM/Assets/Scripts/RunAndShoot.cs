using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAndShoot : MonoBehaviour
{
    enum State { RUNNING, SHOOTING }
    State currentState = State.RUNNING;

    [SerializeField]
    private float minDistance = 10, maxDistance = 15;

    [SerializeField]
    private float secondsOfMovement = 2, secondsOfShooting = 1;

    [SerializeField]
    private float moveSpeed = 2;
    [SerializeField]
    private float projectileSpeed = 3;

    private float startRunTime = 0, startShootTime = 0;

    //Quitar que esto sea public en cuanto el GameManager tenga un getPlayer
    public Transform player;

    public Transform projectilePrefab;

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookAtDir = new Vector3(player.position.x - transform.position.x, 0f, player.position.z - transform.position.z);
        rb.rotation = Quaternion.LookRotation(lookAtDir, Vector3.up);
        switch (currentState)
        {
            case (State.RUNNING):
                if (Time.time > secondsOfMovement + startRunTime)
                {
                    startShootTime = Time.time;
                    currentState = State.SHOOTING;
                    Shoot();
                }
                break;
            case (State.SHOOTING):
                if (Time.time > secondsOfShooting + startShootTime)
                {
                    startRunTime = Time.time;
                    currentState = State.RUNNING;
                    Run();
                }
                break;
            default:
                break;
        }
    }

    void Run()
    {
        
        Vector2 moveDir;
        Vector2 distance = new Vector2(player.position.x - transform.position.x, player.position.z - transform.position.z);
        //Si esta demasiado cerca
        if (distance.magnitude < minDistance)
        {
            //Se aleja del jugador con +- 45 grados de desvio
            moveDir = -distance + Vector2.Perpendicular(distance) * Random.Range(-1.0f, 1.0f);
        }
        //Si esta demasiado lejos
        else if (distance.magnitude > maxDistance)
        {
            //Se acerca al jugador con +- 45 grados de desvio
            moveDir = distance + Vector2.Perpendicular(distance) * Random.Range(-1.0f, 1.0f);
        }
        else
        {
            //Se mueve random
            moveDir = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        }
        moveDir = moveDir.normalized * moveSpeed;
        rb.velocity = new Vector3(moveDir.x, 0, moveDir.y);
        
    }

    void Shoot()
    {
        rb.velocity = Vector3.zero;
        //Instanciar proyectil
        //Esto se deberia mover al proyectil cuando este el gamemanager bien
        Rigidbody projectile = Instantiate(projectilePrefab).GetComponent<Rigidbody>();
        projectile.gameObject.SetActive(true);
        Vector3 movement = new Vector3(player.position.x - transform.position.x, 0f, player.position.z - transform.position.z);
        projectile.transform.position = transform.position + movement.normalized;

        projectile.velocity = movement.normalized * projectileSpeed;

        Debug.Log("DISPARO");
    }
}
