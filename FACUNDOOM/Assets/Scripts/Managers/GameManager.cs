using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public struct Settings 
{
    [SerializeField]
    public float volume;
    [SerializeField]
    public float fov;
    [SerializeField]
    public float xSensitivity;
    [SerializeField]
    public float ySensitivity;
    public Settings(float v, float f, float x, float y) { volume = v; fov = f; xSensitivity = x; ySensitivity = y; }
}

public struct Score
{
    public string name;
    public int score;
    public Score(string n, int s) { name = n; score = s; }
    public static bool operator <(Score s1, Score s2) 
    {
        bool sol;
        if (s1.score < s2.score) sol =  true;
        else sol= false;
        return sol;
    }
    public static bool operator >(Score s1, Score s2)
    {
        return s1 < s2;
    }
    public static bool operator ==(Score s1, Score s2)
    {
        return s1.score == s2.score;
    }
    public static bool operator !=(Score s1, Score s2)
    {
        return s1 != s2;
    }
    public static bool operator >=(Score s1, Score s2)
    {
        return s1 > s2 || s1==s2;
    }
    public static bool operator <=(Score s1, Score s2)
    {
        return s1 < s2 || s1 == s2;
    }

}


public class GameManager : MonoBehaviour
{
    public static GameManager _instance { get; private set; }
    bool GameRunning = true;

    [SerializeField]
    RoundManager m_roundManager;

    [SerializeField]
    public Settings m_settings;

    int m_playerPoints = 3;

    public List<Score> scoreboard = new List<Score>();

    public UnityEvent<Settings> eSetSettings;
    public UnityEvent ebackToMainMenu;

    public bool IsGameRunning()
    { return GameRunning; }
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
        eSetSettings.AddListener(SetSettings);
    }   

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void SetSettings(Settings s) { m_settings = s; }

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
        }
        else if (command == "EndRound")
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
        if (scene.name == "GameScene")
        {
            AudioManager.instance.Stop("BGM_MainMenu");
            AudioManager.instance.Stop("BGM_GameOver");
            string actualBGM = "BGM_Game" + UnityEngine.Random.Range(1, 3).ToString();
            AudioManager.instance.Play(actualBGM);

            AudioManager.instance.AddListeners();
            m_roundManager.StartRound();
            Invoke("SendSettingsMessage", 0.005f);
        }
        else if (scene.name == "GameOverScene")
        {
            AudioManager.instance.Stop("BGM_Game1");
            AudioManager.instance.Stop("BGM_Game2");
            AudioManager.instance.Play("BGM_GameOver");
            Cursor.lockState = CursorLockMode.None;
        }
        else if (scene.name == "MainMenuScene")
        {
            AudioManager.instance.Stop("BGM_Game2");
            AudioManager.instance.Stop("BGM_Game1");
            AudioManager.instance.Stop("BGM_GameOver");
            AudioManager.instance.Play("BGM_MainMenu");
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

    public void addScore(Score s) { scoreboard.Add(s); }

}
