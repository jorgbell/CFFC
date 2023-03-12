using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public float spawnPosDelta = 0.6f;
	public EnemyType enemyType = EnemyType.lastEnemy;

	[SerializeField]
	ColorComponent colorComponent;

	[SerializeField]
	ParticleSystem wrongAnswerFail;

	[SerializeField]
	List<GameObject> colorSprites = new List<GameObject>();

	float timeAlive = 0f;


	// Start is called before the first frame update
	void Start()
	{
		GetComponent<MeshRenderer>().material.color = Color.red;
		colorComponent = GetComponent<ColorComponent>();
		colorComponent.OnColorChanged.AddListener(SetColor);
		wrongAnswerFail?.Stop();
	}

	// Update is called once per frame
	void Update()
	{
		timeAlive += Time.deltaTime;
	}

	void OnEnable()
	{
		timeAlive = 0f;
	}

	//public void ChangeColor()
	//{
	//	List<ColorType> colors = new List<ColorType>();
	//	for (int i = 0; i < ((int)ColorType.lastColor); i++)
	//	{
	//		if (i != ((int)colorType))

	//			colors.Add(((ColorType)i));
	//	}
	//	colorType = colors[Random.Range(0, ((int)ColorType.lastColor) - 1)];

	//	if (colorType == ColorType.red)
	//		GetComponent<MeshRenderer>().material.color = Color.red;
	//	else GetComponent<MeshRenderer>().material.color = Color.blue;
	//}

	public void SetColor(ColorType newColor)
	{
		if(colorSprites.Count < (int)ColorType.lastColor)
		{
			Debug.LogError("Missing Sprites for Enemy");
		}
		else
		{
			colorSprites.ForEach(sprite => sprite.SetActive(false));
            colorSprites[(int)newColor].SetActive(true);
		}


		//if (colorType == ColorType.red)
		//	GetComponent<MeshRenderer>().material.color = Color.red;
		//else if (colorType == ColorType.blue) GetComponent<MeshRenderer>().material.color = Color.blue;
		//else GetComponent<MeshRenderer>().material.color = Color.green;
	}

	public ColorType GetColor()
    {
		return colorComponent.GetColor();
    }

	public void Hit(ColorType hitColor)
	{
		if (hitColor == colorComponent.GetColor())
		{
			Death();
		}
		else
		{
			Duplicate();
			wrongAnswerFail?.Play();
		}
	}

	public void Duplicate()
	{
		//Lanza el evento de duplicar
		RoundManager.instance.eWrongAnswer.Invoke(this);
		//Vector2 dir = Vector2.Perpendicular(new Vector2(transform.position.x - player.position.x, transform.position.z - player.position.z));

		//Vector3 axis = new Vector3(dir.x, 0, dir.y).normalized * spawnPosDelta;
		//GameObject clone = new GameObject();
		//switch (enemyType)
		//{
		//    case EnemyType.baseEnemy:
		//        clone = BasicEnemyPool.Instance.GetPooledObject();
		//        break;
		//    case EnemyType.ranged:
		//        clone = RangedEnemyPool.Instance.GetPooledObject();
		//        break;
		//    case EnemyType.fast:
		//        clone = FastEnemyPool.Instance.GetPooledObject();
		//        break;
		//}
		//clone.SetActive(true);
		//clone.GetComponent<Enemy>().colorType = colorType;
		//transform.position += axis;
		//clone.transform.position -= axis;
	}

	public void Death()
	{
        RoundManager.instance.eEnemyDied.Invoke();
        gameObject.SetActive(false);
	}

	public void ChangeAnimation()
    {
		colorSprites[(int)GetColor()].GetComponentInChildren<EnemyAnimation>()?.NextFrame();
    }

	public void ResetAnimation()
    {
		colorSprites[(int)GetColor()].GetComponentInChildren<EnemyAnimation>()?.SetStartFrame();
	}

	public float GetTimeAlive() { return timeAlive; }
}
