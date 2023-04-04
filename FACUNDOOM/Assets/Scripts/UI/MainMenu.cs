using Dan.Main;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

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
    //scoreboard
    [SerializeField]
    GameObject TextFieldObject;
    [SerializeField]
    GameObject loadingObject;
    [SerializeField]
    RectTransform localScoreboardContent;
    [SerializeField]
    RectTransform globalScoreboardContent;
    [SerializeField]
    RectTransform selectScoreboardContent;
    [SerializeField]
    ScrollRect scoreboardScrollRect;
    List<GameObject> loadedScores = new List<GameObject>();

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
        LoadScoreboards();
    }
    void LoadScoreboards()
    {
        loadingObject.SetActive(true);

        foreach(GameObject instanced in loadedScores)
        {
            Destroy(instanced);
        }
        selectScoreboardContent.gameObject.SetActive(true);
        localScoreboardContent.gameObject.SetActive(false);
        globalScoreboardContent.gameObject.SetActive(false);
        scoreboardScrollRect.verticalScrollbar.size = 1;
        scoreboardScrollRect.content = selectScoreboardContent;

        //local load scoreboard
        foreach (Score s in GameManager._instance.localScoreboard.scoreboard)
        {
            GameObject newScore = Instantiate(TextFieldObject, localScoreboardContent.gameObject.transform);
            newScore.GetComponent<TMP_Text>().text = s.name + "   " + s.score;
            loadedScores.Add(newScore);
        }
        //load global scoreboard
        Debug.Log(GameManager._instance.globalScoreboard.getKey());
        Debug.Log("loading global leaderboard...");
        LeaderboardCreator.GetLeaderboard(GameManager._instance.globalScoreboard.getKey(),
            ((msg) =>
            {
                Debug.Log(msg.Length);
                //callback function que se llama cuando se ha cargado el leaderboard
                for(int i = 0; i<msg.Length; ++i)
                {
                    GameObject newScore = Instantiate(TextFieldObject, globalScoreboardContent.gameObject.transform);
                    newScore.GetComponent<TMP_Text>().text = msg[i].Username + "   " + msg[i].Score;
                    loadedScores.Add(newScore);
                }
                loadingObject.SetActive(false);
            }));

    }

    public void ShowScoreboardContent(bool local)
    {

        selectScoreboardContent.gameObject.SetActive(false);
        scoreboardScrollRect.content = local ? localScoreboardContent : globalScoreboardContent;
        localScoreboardContent.gameObject.SetActive(local);
        globalScoreboardContent.gameObject.SetActive(!local);
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
