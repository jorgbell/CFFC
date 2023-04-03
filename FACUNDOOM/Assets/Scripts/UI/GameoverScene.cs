using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameoverScene : MonoBehaviour
{
	[SerializeField]
	TMP_InputField inputField;
    public void Retry()
	{
		string name = (string.IsNullOrWhiteSpace(inputField.text)) ? "FACUN" : inputField.text;
		GameManager._instance.addScore(new Score(name, GameManager._instance.GetPlayerScore()));
		SerializaztionManager._instance.SaveScoreboard();
		GameManager._instance.SendCommand("Play");
	}

	public void GoMenu()
	{
        string name = (string.IsNullOrWhiteSpace(inputField.text)) ? "FACUN" : inputField.text;
        GameManager._instance.addScore(new Score(name, GameManager._instance.GetPlayerScore()));
        SerializaztionManager._instance.SaveScoreboard();
        GameManager._instance.SendCommand("Menu");
	}


}
