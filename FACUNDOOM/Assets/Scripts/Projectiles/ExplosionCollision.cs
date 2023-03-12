using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ExplosionCollision : MonoBehaviour
{
    protected ColorComponent m_colorComponent = null;

    ParticleSystem ps;
    public List<ParticleCollisionEvent> collisionEvents;
    List<Rigidbody> collidedRBs = new List<Rigidbody>();

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    private void OnParticleSystemStopped()
    {
        collidedRBs = new List<Rigidbody>();
    }

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = ps.GetCollisionEvents(other, collisionEvents);

        Rigidbody rb = other.GetComponent<Rigidbody>();

        int i = 0;

        while (i < numCollisionEvents)
        {
            if(i > 2) 
            {
                int a = 2;
            }

            if (rb)
            {
                if (rb.TryGetComponent<Enemy>(out Enemy targetEnemy))
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
}
