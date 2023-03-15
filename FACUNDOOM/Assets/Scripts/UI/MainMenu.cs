using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject mainScreen;
    [SerializeField]
    GameObject creditsScreen;
    [SerializeField]
    GameObject scoreScreen;
    [SerializeField]
    GameObject optionsScreen;
    [SerializeField]
    GameObject prefab;
    [SerializeField]
    Transform contentSlider;

    private void Start()
    {
        GameManager._instance.ebackToMainMenu.AddListener(ShowMain);
    }

    public void ShowCredits()
    {
        optionsScreen.SetActive(false);
        mainScreen.SetActive(false);
        creditsScreen.SetActive(true);
        scoreScreen.SetActive(false);
    }

    public void ShowMain()
    {
        optionsScreen.SetActive(false);
        mainScreen.SetActive(true);
        scoreScreen.SetActive(false);
        creditsScreen.SetActive(false);
    }
    public void ShowOptions()
    {
        mainScreen.SetActive(false);
        creditsScreen.SetActive(false);
        scoreScreen.SetActive(false);
        optionsScreen.SetActive(true);
    }
    public void ShowScore()
    {
        optionsScreen.SetActive(false);
        mainScreen.SetActive(false);
        creditsScreen.SetActive(false);
        scoreScreen.SetActive(true);
        List<Score> scoreboard = GameManager._instance.scoreboard;
        scoreboard.Sort(SortByScore);
        scoreboard.Reverse();
        foreach (Score s in scoreboard)
        {
            GameObject newScore = Instantiate(prefab, contentSlider);
            newScore.GetComponent<TMP_Text>().text = s.name + "   " + s.score;
        }
    }
    static int SortByScore(Score p1, Score p2)
    {
        return p1.score.CompareTo(p2.score);
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
