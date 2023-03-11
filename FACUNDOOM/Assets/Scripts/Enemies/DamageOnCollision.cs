using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnCollision : MonoBehaviour
{
    [SerializeField]
    private int m_Damage = 1;

    [SerializeField]
    private float m_CooldownSeconds = 1;

    private float LastHitTime { get; set; }
    // Start is called before the first frame update

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
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
