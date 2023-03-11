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
        RoundManager.instance.eWrongAnswer.AddListener(Duplicate);
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

    void Duplicate(Enemy enemy)
    {
        Transform player = RoundManager.instance.getPlayer().transform;
        Vector2 dir = Vector2.Perpendicular(new Vector2(enemy.transform.position.x - player.position.x, enemy.transform.position.z - player.position.z));

        Vector3 axis = new Vector3(dir.x, 0, dir.y).normalized * enemy.spawnPosDelta;
        GameObject clone = new GameObject();
        switch (enemy.enemyType)
        {
            case EnemyType.baseEnemy:
                clone = BasicEnemyPool.Instance.GetPooledObject();
                break;
            case EnemyType.ranged:
                clone = RangedEnemyPool.Instance.GetPooledObject();
                break;
            case EnemyType.fast:
                clone = FastEnemyPool.Instance.GetPooledObject();
                break;
        }
        clone.SetActive(true);
        clone.GetComponent<Enemy>().colorType = enemy.colorType;
        clone.transform.position = enemy.transform.position - axis;
        enemy.transform.position += axis;
    }
}
