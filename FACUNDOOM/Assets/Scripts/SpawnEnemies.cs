using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public GameObject[] enemies;

    public enum SpawnType { SpawnEnemy, SpawnEnemyRandomColor}
    [SerializeField]
    SpawnType spawnType = SpawnType.SpawnEnemy;

    [SerializeField]
    private float summonFrequency = 2.0f, firstSummonTime = 0;

    [SerializeField]
    private float minSpawnDistance = 7, maxSpawnDistance = 12;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(spawnType.ToString(), firstSummonTime, summonFrequency);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void SpawnEnemy()
    {
        GameObject enemy = new GameObject();
        switch ((EnemyType)Random.Range(0, enemies.Length))
        {
            case EnemyType.baseEnemy:
                enemy = BasicEnemyPool.Instance.GetPooledObject();
                break;
            case EnemyType.ranged:
                enemy = RangedEnemyPool.Instance.GetPooledObject();
                break;
            case EnemyType.fast:
                enemy = FastEnemyPool.Instance.GetPooledObject();
                break;
        }
        enemy.SetActive(true);
        enemy.transform.position = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f)).normalized * Random.Range(minSpawnDistance, maxSpawnDistance);
        enemy.GetComponent<Enemy>().SetColor(GetComponent<ColorChanger>().colors[(int)enemy.GetComponent<Enemy>().enemyType]);
    }

    void SpawnEnemyRandomColor()
    {
        GameObject enemy = new GameObject();
        switch ((EnemyType)Random.Range(0, enemies.Length))
        {
            case EnemyType.baseEnemy:
                enemy = BasicEnemyPool.Instance.GetPooledObject();
                break;
            case EnemyType.ranged:
                enemy = RangedEnemyPool.Instance.GetPooledObject();
                break;
            case EnemyType.fast:
                enemy = FastEnemyPool.Instance.GetPooledObject();
                break;
        }
        enemy.SetActive(true);
        enemy.GetComponent<Enemy>().SetColor(GetComponent<ColorChanger>().colors[Random.Range(0, GetComponent<ColorChanger>().colors.Count)]);
    }
}
