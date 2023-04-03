using Dan.Main;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class GameoverScene : MonoBehaviour
{
	[SerializeField]
	TMP_InputField inputField;
    public void Retry()
	{
		SaveAndLoadScore();
		GameManager._instance.SendCommand("Play");
	}

	public void GoMenu()
	{
		SaveAndLoadScore();
        GameManager._instance.SendCommand("Menu");
	}

	private void SaveAndLoadScore()
	{
        string name = (string.IsNullOrWhiteSpace(inputField.text)) ? "FACUN" : inputField.text;
        int score = GameManager._instance.GetPlayerScore();
		//local
        GameManager._instance.addScore(new Score(name, score));
        SerializaztionManager._instance.SaveScoreboard();
		//global
		
        LeaderboardCreator.UploadNewEntry(GameManager._instance.globalScoreboard.getKey(), name, score,
			((msg) =>
			{
				//callback function que se llama una vez se ha subido la nueva entrada
			}));

    }

}

