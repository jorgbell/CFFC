using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.Mathematics;


public struct Multiplier
{
    public string text;
    public float multValor;
    public int killsToPass;
}

public class RoundManager : MonoBehaviour
{
    public static RoundManager instance { get; private set; }

    float m_timeSurvived = 0.0f;
    float lastColorChange = 0.0f;
    [SerializeField]
    bool m_bGameStarted = false;

    public UnityEvent eEnemyDied;
    public UnityEvent<Enemy> eWrongAnswer;
    public UnityEvent<Vector3> eMissedAttack;
    public UnityEvent eRandomizeColors;
    public UnityEvent<Multiplier> eMultiplier;

    [SerializeField]
    PlayerController m_player;

    int score = 0;
    int streak = 0;
    [SerializeField]
    Multiplier[] multipliers = new Multiplier[] {
        new Multiplier(){text = "",multValor= 1.0f, killsToPass = 2},
        new Multiplier(){text = "LEMON",multValor= 1.4f, killsToPass =3},
        new Multiplier(){text = "WHO LET YOU COOK?",multValor= 2.2f, killsToPass = 4},
        new Multiplier(){text = "KINDA MID",multValor= 3.5f, killsToPass = 5},
        new Multiplier(){text = "CHILLIN",multValor= 4.3f,killsToPass = 7},
        new Multiplier(){text = "BOMBASTIC",multValor= 5.1f,killsToPass = 9},
        new Multiplier(){text = "CRAZY",multValor= 5.9f, killsToPass = 12},
        new Multiplier(){text = "GOD",multValor= 7.2f, killsToPass = 15},
        new Multiplier(){text = "SCRUM MASTER",multValor= 8.5f, killsToPass = 20},
        new Multiplier(){text = "TOTAL FACUNDOOM",multValor= 11.4f, killsToPass = int.MaxValue}
    };
    [SerializeField]
    int actualMultiplier = 0;
    [SerializeField]
    float pointsPerDeath = 50;
    [SerializeField]
    float pointsPerDuplicate = 50;

    private void Awake()
    {
        if (instance == null)
        {
            Debug.Log("GameManager instanced");
            instance = this;

            if (eEnemyDied == null)
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
            if (eMissedAttack == null)
            {
                eMissedAttack = new UnityEvent<Vector3>();
            }
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        eWrongAnswer.AddListener(wrongAnswerRM);
        eEnemyDied.AddListener(scorePoints);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_bGameStarted)
        {
            m_timeSurvived += Time.deltaTime;
        }

        //If playerDead enseñar la UI de muerto y parar las entidades

        if (m_timeSurvived - lastColorChange > 5.0f)
        {
            lastColorChange = m_timeSurvived;
            //Debug.Log("TOPo");
            eRandomizeColors.Invoke();
        }
    }

    public void StartRound()
    {

    }

    public PlayerController getPlayer()
    {
        return m_player;
    }

    void wrongAnswerRM(Enemy e)
    {
        Debug.Log("RESTA PUNTOS, " + score);
        actualMultiplier = 0; streak = 0;
        score -= (int)(pointsPerDuplicate);
        if (score < 0) score = 0;
        eMultiplier.Invoke(multipliers[actualMultiplier]);
    }
    void scorePoints()
    {
        Debug.Log("SUMA PUNTOS, " + score);
        score += (int)(pointsPerDeath * multipliers[actualMultiplier].multValor);
        streak++;
        if (streak > multipliers[actualMultiplier].killsToPass)
        {
            actualMultiplier++;
            eMultiplier.Invoke(multipliers[actualMultiplier]);
            Debug.Log(multipliers[actualMultiplier].text);
        }
    }
}
