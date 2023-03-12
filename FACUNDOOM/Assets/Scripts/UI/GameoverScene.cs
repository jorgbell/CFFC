using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameoverScene : MonoBehaviour
{
    public void Retry()
	{
		GameManager._instance.SendCommand("Play");
	}

	public void GoMenu()
	{
		GameManager._instance.SendCommand("Menu");
	}
}
