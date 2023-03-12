using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    float lifeTime = 5f;

    [SerializeField]
    private int m_Damage = 1;

    float actualTime = 0;
    void Start()
    {
        //Get Player position del game manager y settea tu direccion para alla
        //De momento esta a pelo donde se instancia
    }

    private void OnTriggerEnter(Collider collision)
    {
        PlayerHealth playerHealth;
        if (collision.transform.TryGetComponent(out playerHealth))
        {
            playerHealth.TakeDamage(m_Damage);
            gameObject.SetActive(false);
            //GetComponent<Enemy>()?.Death();
        }
    }

    private void Update()
    {
        actualTime += Time.deltaTime;
        if (actualTime >= lifeTime)
            gameObject.SetActive(false);
    }
}
