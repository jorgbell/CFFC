using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCloseAndAttack : MonoBehaviour
{
    private enum State { RUNNING, PREPARING, DASHING, RESTING }
    State currentState = State.RUNNING;

    [Range(0, 10)]
    public float moveSpeed = 2;

    [SerializeField]
    public float dashRange = 2;

    [SerializeField]
    public float dashForce = 4;

    public float animationSpeed = 0.5f;

    [SerializeField]
    public float waitTime = 0.5f, restTime = 1.0f, dashDistance = 3.0f;


    Transform player;
    Rigidbody rb;

    float prepareStartTime = 0;
    float restStartTime = 0;
    float animationChangeTime = 0;
    Vector3 dashStartPos;

    // Start is called before the first frame update
    void Start()
    {
        player = RoundManager.instance.getPlayer().transform;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 toPlayer = new Vector3(player.position.x - transform.position.x, 0f, player.position.z - transform.position.z);
        rb.rotation = Quaternion.LookRotation(toPlayer, Vector3.up);

        switch (currentState)
        {
            case (State.RUNNING):
                Run();

                if (Time.time > animationChangeTime + animationSpeed)
                {
                    animationChangeTime = Time.time;
                    GetComponent<Enemy>().ChangeAnimation();
                }

                if (toPlayer.magnitude < dashRange)
                {
                    rb.velocity = Vector3.zero;
                    currentState = State.PREPARING;
                    GetComponent<Enemy>().ResetAnimation();
                    prepareStartTime = Time.time;
                }
                break;
            case (State.PREPARING):
                if (Time.time > prepareStartTime + waitTime)
                {
                    Vector3 dash = toPlayer.normalized;
                    rb.velocity = dash * dashForce;
                    currentState = State.DASHING;
                    GetComponent<DamageOnCollision>().Activate();
                    GetComponent<Enemy>().ChangeAnimation();
                    //Cambia de sprite
                }
                break;
            case (State.DASHING):
                if ((dashStartPos - transform.position).magnitude > dashDistance)
                {
                    currentState = State.RESTING;
                    GetComponent<DamageOnCollision>().Deactivate();
                    restStartTime = Time.time;
                }
                break;
            case (State.RESTING):
                if (Time.time > restStartTime + restTime)
                {
                    rb.velocity = Vector3.zero;
                    currentState = State.RUNNING;
                }
                break;
            default:
                break;
        }
    }

    void Run()
    {
        //transform.LookAt(target, Vector3.up);
        Vector3 movement = new Vector3(player.position.x - transform.position.x, 0f, player.position.z - transform.position.z);
        //rb.rotation = Quaternion.LookRotation(movement, Vector3.up);
        //movement = Quaternion.FromToRotation(Vector3.forward, transform.forward) * movement.normalized * moveSpeed;

        movement = movement.normalized * moveSpeed;

        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
    }
}
