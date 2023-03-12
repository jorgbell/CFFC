using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject mainScreen;
    [SerializeField]
    GameObject creditsScreen;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowCredits()
	{
        mainScreen.SetActive(false);
        creditsScreen.SetActive(true);
	}

    public void ShowMain()
	{
        mainScreen.SetActive(true);
        creditsScreen.SetActive(false);
	}

    public void Exit()
	{
        GameManager._instance.SendCommand("Exit");
    }

    public void Play()
	{
        GameManager._instance.SendCommand("Play");
    }
}
