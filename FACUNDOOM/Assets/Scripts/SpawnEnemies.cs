using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public GameObject basicEnemy;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnBasicEnemy", 0, 2);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void SpawnBasicEnemy()
    {
        Instantiate(basicEnemy, transform);
    }
}
