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
    GameObject prefab;
    [SerializeField]
    Transform contentSlider;
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
        scoreScreen.SetActive(false);
	}

    public void ShowMain()
	{
        mainScreen.SetActive(true);
        scoreScreen.SetActive(false);
        creditsScreen.SetActive(false);
	}
    public void ShowScore()
    {
        mainScreen.SetActive(false);
        creditsScreen.SetActive(false);
        scoreScreen.SetActive(true);
        List<Score> scoreboard = GameManager._instance.scoreboard;
        scoreboard.Sort(SortByScore);
        scoreboard.Reverse();
        foreach(Score s in scoreboard){
            GameObject newScore = Instantiate(prefab, contentSlider);
            newScore.GetComponent<TMP_Text>().text = s.name + "   "+ s.score;
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
