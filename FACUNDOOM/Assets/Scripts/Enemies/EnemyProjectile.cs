using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    float lifeTime = 5f;
    float actualTime = 0;
    void Start()
    {
        //Get Player position del game manager y settea tu direccion para alla
        //De momento esta a pelo donde se instancia
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Feo feo feo
        if (collision.gameObject.GetComponent<PlayerMovement>())
        {
            Debug.Log("Proyectil pega a jugador");
            gameObject.SetActive(false);
        }

    }

    private void Update()
    {
        actualTime += Time.deltaTime;
        if (actualTime >= lifeTime)
            gameObject.SetActive(false);
    }
}
