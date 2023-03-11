using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BombProjectileBehaviour : MonoBehaviour
{
    [SerializeField] GameObject explosionObject;
    [SerializeField] ParticleSystem explosionSystem;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        explosionObject.transform.position = transform.position;
        explosionSystem.Clear();
        explosionSystem.Play();

        RoundManager.instance.eBombExploded.Invoke();

        Destroy(gameObject);
    }

    public void setExplosionSystem(GameObject o, ParticleSystem s) { explosionObject = o; explosionSystem = s; }
}
