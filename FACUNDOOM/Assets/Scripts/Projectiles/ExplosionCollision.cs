using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ExplosionCollision : MonoBehaviour
{
    [SerializeField] bool checking = true;

    protected ColorComponent m_colorComponent = null;

    ParticleSystem ps;
    public List<ParticleCollisionEvent> collisionEvents;
    List<Rigidbody> collidedRBs = new List<Rigidbody>();

    float timeChecking = 0;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
        RoundManager.instance.eBombShot.AddListener(disableChecking);
        RoundManager.instance.eBombShot.AddListener(clearRBList);
    }

    private void Update()
    {
        if (checking)
        {
            timeChecking += Time.deltaTime;
            if (timeChecking > 0.2f)
            {
                disableChecking();
                clearRBList();
            }
        }
        else timeChecking = 0;
    }

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = ps.GetCollisionEvents(other, collisionEvents);

        Rigidbody rb = other.GetComponent<Rigidbody>();

        int i = 0;

        while (i < numCollisionEvents && checking)
        {
            if(i > 2) 
            {
                int a = 2;
            }

            if (rb)
            {
                if (rb.TryGetComponent<Enemy>(out Enemy targetEnemy) && targetEnemy.GetTimeAlive() > 0.2f)
                {
                    bool alreadyCollided = false;

                    int j = 0;

                    while (j < collidedRBs.Count && !alreadyCollided)
                    {
                        if (rb == collidedRBs[j]) alreadyCollided = true;
                        j++;
                    }

                    if (!alreadyCollided)
                    {
                        collidedRBs.Add(rb);
                        targetEnemy.Hit(m_colorComponent.GetColor());
                        Debug.Log("Hit" + i);
                    }
                }
            }

            i++;
        }
    }

    public void setColor(ColorComponent c) { m_colorComponent = c; }

    void clearRBList() { collidedRBs = new List<Rigidbody>(); }

    public void disableChecking() { checking = false; }
    public void enableChecking() { checking = true; }
}
