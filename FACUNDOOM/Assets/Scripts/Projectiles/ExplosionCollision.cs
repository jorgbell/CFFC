using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ExplosionCollision : MonoBehaviour
{
    protected ColorComponent m_colorComponent = null;

    ParticleSystem ps;
    public List<ParticleCollisionEvent> collisionEvents;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = ps.GetCollisionEvents(other, collisionEvents);

        Rigidbody rb = other.GetComponent<Rigidbody>();
        int i = 0;

        while (i < numCollisionEvents)
        {
            if (rb)
            {
                if (rb.TryGetComponent<Enemy>(out Enemy targetEnemy))
                {
                    targetEnemy.Hit(m_colorComponent.GetColor());
                }
            }

            i++;
        }
    }

    public void setColor(ColorComponent c) { m_colorComponent = c; }
}
