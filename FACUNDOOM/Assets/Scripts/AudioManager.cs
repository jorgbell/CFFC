using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public GameObject player;

    [Header("Running Sounds")]
    public AudioClip grassSound;


    private PlayerMovement playerMovement;
    private AudioSource playerAudio;

    private AudioClip currentSound;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
        playerAudio = player.GetComponent<AudioSource>();
        currentSound = grassSound;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovement.isMoving())
        {
            if (playerAudio.clip != currentSound) playerAudio.clip = currentSound;
            else if (!playerAudio.isPlaying)
            {
                playerAudio.loop = true;
                playerAudio.Play();
            }
        }

        else playerAudio.loop = false;
    }
}
