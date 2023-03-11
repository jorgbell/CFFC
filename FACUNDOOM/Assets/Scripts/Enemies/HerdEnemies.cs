using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerdEnemies : MonoBehaviour
{
	// Start is called before the first frame update
	Transform player;

	[SerializeField]
	public float maxDistance = 30;
    void Start()
    {
		player = RoundManager.instance.getPlayer().transform;
    }

    // Update is called once per frame
    void Update()
    {
		Vector3 distance;
		if (BasicEnemyPool.Instance)
		{
			foreach (GameObject obj in BasicEnemyPool.Instance.pooledObjects)
			{
				if (obj.gameObject.activeInHierarchy)
				{
					distance = new Vector3(obj.transform.position.x - player.position.x, 0, obj.transform.position.z - player.position.z);
					if (distance.magnitude > maxDistance)
                    {
						distance = player.position + distance.normalized * maxDistance;
						Debug.Log(distance);
						obj.transform.position = new Vector3(distance.x, obj.transform.position.y, distance.z);
                    }
				}
			}
		}
		if (FastEnemyPool.Instance)
		{
			foreach (GameObject obj in FastEnemyPool.Instance?.pooledObjects)
			{
				if (obj.gameObject.activeInHierarchy)
				{
					distance = new Vector3(obj.transform.position.x - player.position.x, 0, obj.transform.position.z - player.position.z);
					if (distance.magnitude > maxDistance)
					{
						distance = player.position + distance.normalized * maxDistance;
						obj.transform.position = new Vector3(distance.x, obj.transform.position.y, distance.z);
					}
				}
			}
		}
		if (RangedEnemyPool.Instance)
		{
			foreach (GameObject obj in RangedEnemyPool.Instance?.pooledObjects)
			{
				if (obj.gameObject.activeInHierarchy)
				{
					distance = new Vector3(obj.transform.position.x - player.position.x, 0, obj.transform.position.z - player.position.z);
					if (distance.magnitude > maxDistance)
					{
						distance = player.position + distance.normalized * maxDistance;
						obj.transform.position = new Vector3(distance.x, obj.transform.position.y, distance.z);
					}
				}
			}
		}
	}
}
