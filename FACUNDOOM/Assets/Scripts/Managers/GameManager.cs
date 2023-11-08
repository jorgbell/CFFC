using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using Dan.Main;

[System.Serializable]
public class Settings 
{
    [SerializeField]
    public float volume;
    [SerializeField]
    public float fov;
    [SerializeField]
    public float xSensitivity;
    [SerializeField]
    public float ySensitivity;
    public Settings(float v = 1, float f = 60, float x = 600, float y = 200) { volume = v; fov = f; xSensitivity = x; ySensitivity = y; }
}

[System.Serializable]
public class Score
{
    [SerializeField]
    public string name;
    [SerializeField]
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


/// <summary>
/// clase para permitir serializar una lista de scores
/// </summary>
/// 
[System.Serializable]
public class ScoreList
{
    [SerializeField]
    public List<Score> scoreboard;
    private string publicKey = "bca4614c1af58e9df7da0ed961324642d3c5ecb7322d5cb1851e75b17585c6e4";
    public ScoreList() { scoreboard = new List<Score>(); }
    public string getKey() { return publicKey; }
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

    public ScoreList localScoreboard = new ScoreList();
    public ScoreList globalScoreboard = new ScoreList();

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
        SetScreenSize();
    }
    private void SetScreenSize()
    {
        //// Obtener la resolución actual de la pantalla
        //int screenWidth = Screen.width;
        //int screenHeight = Screen.height;

        //// Establecer la relación de aspecto deseada
        //float targetAspect = 4f / 3f;

        //// Calcular el tamaño del viewport rect
        //float height = (screenWidth / targetAspect) / screenWidth; //ejemplo: 1920->1440 --> 1440/1920
        //float xOffset = (1f-height) / 2f;

        //// Establecer los valores de X, Y, W y H del viewport rect
        ////Camera.main.rect = new Rect(0f, yOffset / screenHeight, 1f, height / screenHeight);
        //Camera.main.rect = new Rect(xOffset, 0f, height, 1f);
    }
    private void Start()
    {
        eSetSettings.AddListener(SetSettings);
        SerializaztionManager._instance.LoadSettings();
        SerializaztionManager._instance.LoadScoreboard();
        for (int i = 0; i < 3; i++)
        {
            globalScoreboard.scoreboard.Add(new Score("FACUN2", 1000));
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void SetSettings(Settings s) {
        m_settings = s;
        AudioListener.volume = s.volume;
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

    public void addScore(Score s) { 
        localScoreboard.scoreboard.Add(s);
        localScoreboard.scoreboard.Sort(SortByScore);
        localScoreboard.scoreboard.Reverse();
    }
    static int SortByScore(Score p1, Score p2)
    {
        return p1.score.CompareTo(p2.score);
    }

}
