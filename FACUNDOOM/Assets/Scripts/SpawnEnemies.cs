using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public GameObject[] enemies;

    public enum SpawnType { SpawnEnemy, SpawnEnemyRandomColor}
    [SerializeField]
    SpawnType spawnType = SpawnType.SpawnEnemy;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(spawnType.ToString(), 0, 2);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemies[Random.Range(0, enemies.Length)], transform);
        enemy.SetActive(true);
        enemy.GetComponent<Enemy>().SetColor(GetComponent<ColorChanger>().colors[(int)enemy.GetComponent<Enemy>().enemyType]);
    }

    void SpawnEnemyRandomColor()
    {
        GameObject enemy = Instantiate(enemies[Random.Range(0, enemies.Length)], transform);
        enemy.SetActive(true);
        enemy.GetComponent<Enemy>().SetColor(GetComponent<ColorChanger>().colors[Random.Range(0, GetComponent<ColorChanger>().colors.Count)]);
    }
}
