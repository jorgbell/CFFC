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
            m_roundManager.StartRound();
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

    /// <summary>
    /// llamar cuando se duplique un enemigo y meter efectos en pantalla y ¿restar puntuacion?
    /// </summary>
    public void WrongAnswer()
    {

    }
    
    /// <summary>
    /// quitar vida al jugador (tiene 2 slots discretos) y activa efectos visuales
    /// </summary>
    public void Hit()
    {

    }

    /// <summary>
    /// cambiar el arma activa y ¿cambiar el color del marco del crt segun el arma?
    /// </summary>
    public void ChangeWeapon()
    {

    }

    /// <summary>
    /// activar la ruleta (por definir=
    /// </summary>
    public void Roulette()
    {

    }

    /// <summary>
    /// cambiar escena
    /// </summary>
    public void ChangeScene()
    {

    }

    //--------------------METODOS QUE HAY QUE DISEÑAR-------
    /* public void camaraLenta() -> activar camara lenta mediante input
     * public void increaseVelocity()-> aumentar la velocidad del gameplay, habria que modificar la velocidad de todos los objetos??
     */

}
