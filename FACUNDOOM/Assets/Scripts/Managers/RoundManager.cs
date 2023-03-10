using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    float m_timeSurvived = 0.0f;
    bool m_bGameStarted = false;
    //Aqui las pools de enemigos


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (m_bGameStarted)
		{
            m_timeSurvived += Time.deltaTime;
		}

        //If playerDead enseñar la UI de muerto y parar las entidades
    }

    public void StartRound()
	{

	}
}
