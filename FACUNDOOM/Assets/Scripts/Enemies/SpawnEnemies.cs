using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
	public enum SpawnType { SpawnEnemy, SpawnEnemyRandomColor }
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
		RoundManager.instance.eRandomizeColors.AddListener(RandomizeColors);
	}

	// Update is called once per frame
	void Update()
	{
	}

	void SpawnEnemy()
	{
		GameObject enemy = null;
		switch ((EnemyType)Random.Range(0, (int)EnemyType.lastEnemy))
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
		if (enemy)
		{
			enemy.SetActive(true);
			enemy.transform.position = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f)).normalized * Random.Range(minSpawnDistance, maxSpawnDistance);
			//enemy.GetComponent<Enemy>().SetColor(GetComponent<ColorChanger>().colors[(int)enemy.GetComponent<Enemy>().enemyType]);
		}
	}

	void SpawnEnemyRandomColor()
	{
		GameObject enemy = null;
		switch ((EnemyType)Random.Range(0, (int)EnemyType.lastEnemy))
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
		if (enemy)
		{
			enemy.SetActive(true);
			enemy.GetComponent<Enemy>().SetColor(GetComponent<ColorChanger>().colors[Random.Range(0, GetComponent<ColorChanger>().colors.Length)]);
		}
	}

	void Duplicate(Enemy enemy)
	{
		Debug.Log("Copia");
		Transform player = RoundManager.instance.getPlayer().transform;
		Vector2 dir = Vector2.Perpendicular(new Vector2(enemy.transform.position.x - player.position.x, enemy.transform.position.z - player.position.z));

		Vector3 axis = new Vector3(dir.x, 0, dir.y).normalized * enemy.spawnPosDelta;
		GameObject clone = null;
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
		if (clone)
		{
			clone.SetActive(true);
			clone.GetComponent<ColorComponent>().SetColor(enemy.GetColor());
			//Esto no deberia hacer falta pero el mensaje no salta
			clone.GetComponent<Enemy>().SetColor(enemy.GetColor());
			clone.transform.position = enemy.transform.position - axis;
			enemy.transform.position += axis;
		}
	}

	void RandomizeColors()
	{
		Debug.Log("Randomize colors");
		if (BasicEnemyPool.Instance)
		{
			foreach (GameObject obj in BasicEnemyPool.Instance.pooledObjects)
			{
				if (obj.gameObject.activeInHierarchy)
				{
					obj.GetComponent<ColorComponent>().SetColor((ColorType)Random.Range(0, (int)ColorType.lastColor));
				}
			}
		}
		if (FastEnemyPool.Instance)
		{
			foreach (GameObject obj in FastEnemyPool.Instance?.pooledObjects)
			{
				if (obj.gameObject.activeInHierarchy)
				{
					obj.GetComponent<ColorComponent>().SetColor((ColorType)Random.Range(0, (int)ColorType.lastColor));
				}
			}
		}
		if (RangedEnemyPool.Instance)
		{
			foreach (GameObject obj in RangedEnemyPool.Instance?.pooledObjects)
			{
				if (obj.gameObject.activeInHierarchy)
				{
					obj.GetComponent<ColorComponent>().SetColor((ColorType)Random.Range(0, (int)ColorType.lastColor));
				}
			}
		}
	}
}
