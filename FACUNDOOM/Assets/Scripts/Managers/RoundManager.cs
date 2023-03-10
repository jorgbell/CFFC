using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoundManager : MonoBehaviour
{
	public static RoundManager instance { get; private set; }

	float m_timeSurvived = 0.0f;
	bool m_bGameStarted = false;

	public UnityEvent eEnemyDied;
	public UnityEvent<Enemy> eWrongAnswer;
	public UnityEvent<Vector3> eMissedAttack;
	public UnityEvent eRandomizeColors;

	[SerializeField]
	PlayerController m_player;

	private void Awake()
	{
		if (instance == null)
		{
			Debug.Log("GameManager instanced");
			instance = this;
		}
		else
		{
			Destroy(this);
		}
	}

	// Start is called before the first frame update
	void Start()
	{
		if(eEnemyDied == null)
		{
			eEnemyDied = new UnityEvent();
		}
		if (eWrongAnswer == null)
		{
			eWrongAnswer = new UnityEvent<Enemy>();
		}
		if (eRandomizeColors == null)
		{
			eRandomizeColors = new UnityEvent();
		}
		if(eMissedAttack == null)
		{
			eMissedAttack = new UnityEvent<Vector3>();
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (m_bGameStarted)
		{
			m_timeSurvived += Time.deltaTime;
		}

		//If playerDead enseñar la UI de muerto y parar las entidades
	}

	public void StartRound()
	{

	}

	public PlayerController getPlayer()
	{
		return m_player;
	}
}
