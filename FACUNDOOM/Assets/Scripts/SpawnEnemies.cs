using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public GameObject[] enemies;
    public GameObject basicEnemy;
    public GameObject rangedEnemy;
    public GameObject fastEnemy;
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
        GameObject enemy = Instantiate(enemies[Random.Range(0, enemies.Length)], transform);
        enemy.SetActive(true);
        enemy.GetComponent<Enemy>().SetColor(GetComponent<ColorChanger>().colors[(int)enemy.GetComponent<Enemy>().enemyType]);
    }
}
