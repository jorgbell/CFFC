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
		GameManager._instance.scoreboard.Add(new Score(inputField.text, GameManager._instance.GetPlayerScore()));
		GameManager._instance.SendCommand("Play");
	}

	public void GoMenu()
	{
        GameManager._instance.scoreboard.Add(new Score(inputField.text, GameManager._instance.GetPlayerScore()));
        GameManager._instance.SendCommand("Menu");
	}


}
