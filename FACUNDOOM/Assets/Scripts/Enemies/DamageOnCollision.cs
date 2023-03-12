using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnCollision : MonoBehaviour
{
    [SerializeField]
    private int m_Damage = 1;

    [SerializeField]
    private float m_CooldownSeconds = 1;

    private bool active = false;

    public void Activate()
    {
        active = true;
    }

    public void Deactivate()
    {
        active = false;
    }

    private float LastHitTime { get; set; }

    private void OnCollisionEnter(Collision collision)
    {
        if (active)
        {
            PlayerHealth playerHealth;
            if (Time.time > LastHitTime + m_CooldownSeconds && collision.transform.TryGetComponent(out playerHealth))
            {
                playerHealth.TakeDamage(m_Damage);
                LastHitTime = Time.time;
                //GetComponent<Enemy>()?.Death();
            }
        }
    }

}
