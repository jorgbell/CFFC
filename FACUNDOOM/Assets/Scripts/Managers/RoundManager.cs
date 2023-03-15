using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


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
	public UnityEvent ePlayerDied;
	public UnityEvent eBombShot;
	public UnityEvent eBombExploded;
    public UnityEvent<Multiplier> eMultiplier;
    public UnityEvent eRandomizeColorsCountdown;

    [SerializeField]
    PlayerController m_player;
    [SerializeField]
    public Camera CurCam;

    [SerializeField]
    float timeBetweenChanges = 5.0f;

    bool bCountdownStarted = false;
    public int score { get; private set; } = 0;
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
            Debug.Log("RoundManager instanced");
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
            if (ePlayerDied == null)
            {
                ePlayerDied = new UnityEvent();
            }
            if (eBombShot == null)
			{
                eBombShot = new UnityEvent();
            }   
            if (eBombExploded == null)
			{
				eBombExploded = new UnityEvent();
			}
            if (eRandomizeColorsCountdown == null)
			{
                eRandomizeColorsCountdown = new UnityEvent();
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
        ePlayerDied.AddListener(endGame);
        GameManager._instance.eSetSettings.AddListener(SetSettings);
        GameManager._instance.ebackToMainMenu.AddListener(ShowMain);
        SetSettings(GameManager._instance.m_settings); //inicializa segun lo guardado en gamemanager

    }
    void SetSettings(Settings s)
    {
        AudioListener.volume = s.volume;
        getPlayer().GetComponent<PlayerRotation>().xSensitivity = s.xSensitivity;
        getPlayer().GetComponent<PlayerRotation>().ySensitivity = s.ySensitivity;
        CurCam.fieldOfView = s.fov;
    }
    void ShowMain() { GameManager._instance.SendCommand("Menu"); }
    // Update is called once per frame
    void Update()
    {
        if (m_bGameStarted)
        {
            m_timeSurvived += Time.deltaTime;
        }

        if(!bCountdownStarted && m_timeSurvived - lastColorChange > timeBetweenChanges - 3.0f)
		{
            bCountdownStarted = true;
            eRandomizeColorsCountdown.Invoke();
		}
        if (m_timeSurvived - lastColorChange > timeBetweenChanges)
        {
            lastColorChange = m_timeSurvived;
            eRandomizeColors.Invoke();
            bCountdownStarted = false;
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
        actualMultiplier = 0; streak = 0;
        score -= (int)(pointsPerDuplicate);
        Debug.Log("RESTA PUNTOS, " + score);
        if (score < 0) score = 0;
        eMultiplier.Invoke(multipliers[actualMultiplier]);
    }
    void scorePoints()
    {
        score += (int)(pointsPerDeath * multipliers[actualMultiplier].multValor);
        Debug.Log("SUMA PUNTOS, " + score);
        streak++;
        if (streak > multipliers[actualMultiplier].killsToPass)
        {
            //AudioManager.instance.Play("SFX_ComboUp");
            actualMultiplier++;
            eMultiplier.Invoke(multipliers[actualMultiplier]);
            AudioManager.instance.playStreak(actualMultiplier-1);
            Debug.Log(multipliers[actualMultiplier].text);
        }
    }

    void endGame()
    {
        m_bGameStarted = false;
    }

    public int GetScore() { return score; }
}
