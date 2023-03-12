using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance { get; private set; }
    bool GameRunning = true;

    [SerializeField]
    RoundManager m_roundManager;

    int m_playerPoints = 3;

    public bool IsGameRunning() { return GameRunning; }
    public void PauseUnpause()
	{
        GameRunning = !GameRunning;
        Time.timeScale = (GameRunning) ? 1f : 0f;
	}

    private void Awake()
    {
        if (_instance == null)
        {
            Debug.Log("GameManager instanced");
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            //Asigna el roundmanager de la escena cuando se carga
            _instance.m_roundManager = m_roundManager;
            Destroy(gameObject);
        }
    }
    private void Start()
    {
    }

    private void Update()
    {

    }

	private void OnEnable()
	{
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

	public void SendCommand(string command)
    {
        if (command == "Start")
        {
            Debug.Log("Vamo a jugal ... cuando llegue el coche");
        }
        else if (command == "Menu")
        {
            StartCoroutine(LoadSceneAsync("MainMenuScene"));
        }
        else if (command == "Play")
        {
            StartCoroutine(LoadSceneAsync("GameScene"));
        }else if(command == "EndRound")
		{
            StartCoroutine(LoadSceneAsync("GameOverScene"));
		}
        else if (command == "Exit")
        {
            Debug.Log("Saliendo del juego");
            Application.Quit();
        }
    }

    public IEnumerator LoadSceneAsync(string sceneName)
    {
        Debug.Log("Loading scene: " + sceneName);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "GameScene")
		{
            AudioManager.instance.Stop("BGM_MainMenu");
            AudioManager.instance.Stop("BGM_GameOver");
            string actualBGM = "BGM_Game" + Random.Range(1, 3).ToString();
            //AudioManager.instance.Play(actualBGM);

            AudioManager.instance.AddListeners();
            m_roundManager.StartRound();
		}else if(scene.name == "GameOverScene")
		{
            AudioManager.instance.Stop("BGM_Game1");
            AudioManager.instance.Stop("BGM_Game2");
            //AudioManager.instance.Play("BGM_GameOver");
            Cursor.lockState = CursorLockMode.None;
        }
        else if (scene.name == "MainMenuScene")
        {
            AudioManager.instance.Stop("BGM_Game2");
            AudioManager.instance.Stop("BGM_Game1");
            AudioManager.instance.Stop("BGM_GameOver");
            //AudioManager.instance.Play("BGM_MainMenu");
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public RoundManager GetRoundManager()
	{
        return m_roundManager;
	}

    public int GetPlayerScore()
	{
        return m_playerPoints;
	}

    public void SetPlayerScore(int score)
	{
        m_playerPoints = score;
	}

}
