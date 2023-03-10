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
        GameObject enemy = Instantiate(basicEnemy, transform);
        enemy.SetActive(true);
        enemy.GetComponent<Enemy>().SetColor(GetComponent<ColorChanger>().colors[(int)enemy.GetComponent<Enemy>().enemyType]);
    }
}
