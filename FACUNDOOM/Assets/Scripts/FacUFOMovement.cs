using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacUFOMovement : MonoBehaviour
{
	// Start is called before the first frame update
	public float HeightVariance;
	public float BaseRadius;
	public float RadiusVariance;

	float baseHeight;
	Vector3 CurrentPos;
	void Start()
	{
		baseHeight = transform.position.y;
	}

	// Update is called once per frame
	void Update()
	{
		CurrentPos = transform.position;
		Vector3 newPos = new Vector3(
			BaseRadius * Mathf.Sin(Time.time) + RadiusVariance * Mathf.Sin(Time.time*4.1f)
			, baseHeight + HeightVariance * Mathf.Sin(Time.time * 0.5f)
			, BaseRadius * Mathf.Cos(Time.time) + RadiusVariance * Mathf.Cos(Time.time*2.3f)); ;

		Vector3 look = newPos - CurrentPos;

		transform.LookAt(newPos);
		transform.position = newPos;
	}
}
