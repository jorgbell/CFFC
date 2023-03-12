using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushAndJump : MonoBehaviour
{
    private enum State { RUNNING, PREPARING, JUMPING, RESTING }
    State currentState = State.RUNNING;

    [Range(0, 10)]
    public float moveSpeed = 4;

    [SerializeField]
    public float jumpDistance = 4;

    [SerializeField]
    public float jumpForce = 8;

    [Range(0, 1)]
    public float jumpAngle = 0.6f;

    [SerializeField]
    public float waitTime = 0.5f, restTime = 0.5f;

    [SerializeField]
    private Enemy m_enemy;

    Transform player;
    Rigidbody rb;

    float prepareStartTime = 0;
    float restStartTime = 0;
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
                if (toPlayer.magnitude < jumpDistance)
                {
                    rb.velocity = Vector3.zero;
                    currentState = State.PREPARING;
                    prepareStartTime = Time.time;
                }
                break;
            case (State.PREPARING):
                if (Time.time > prepareStartTime + waitTime)
                {
                    Vector3 jump = toPlayer.normalized;
                    jump.y = jumpAngle;
                    rb.AddForce(jump * jumpForce, ForceMode.Impulse);
                    currentState = State.JUMPING;
                    GetComponent<DamageOnCollision>().Activate();
                }
                break;
            case (State.JUMPING):
                float floorDistance = 0;
                RaycastHit hit;
                if (Physics.Raycast(transform.position, Vector3.down, out hit)) floorDistance = Mathf.Abs(hit.point.y - transform.position.y);
                //Si vamos a cambiar de boxcollider a otra cosa hay que cambiar esto
                if (floorDistance < GetComponent<BoxCollider>().size.y / 2 + 0.1 && rb.velocity.y <= 0)
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
