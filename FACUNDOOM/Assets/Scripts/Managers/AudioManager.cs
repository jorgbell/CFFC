using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    public Sound[] sounds;

    public Sound[] streakSounds;
    private void Awake()
    {
        if (instance == null)
        {
            Debug.Log("AudioManager instanced");
            instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(gameObject);
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.pitch = s.minPitch;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }
        foreach (Sound s in streakSounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.pitch = s.minPitch;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        AddListeners();
    }
    public void AddListeners()
    {
        if (RoundManager.instance != null)
        {
            RoundManager.instance.eWrongAnswer.AddListener(PlayWrongAnswer);
            RoundManager.instance.eBombExploded.AddListener(PlayBombExploded);
            RoundManager.instance.eEnemyDied.AddListener(PlayEnemyDied);
        }
    }
    void PlayWrongAnswer(Enemy e)
    {
        Play("SFX_WA");
    }
    void PlayBombExploded()
    {
        AudioManager.instance.Play("SFX_explosion");
    }
    void PlayEnemyDied()
    {
        AudioManager.instance.Play("SFX_Kill");
    }


    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("NO SE ENCUENTRA EL AUDIO: " + name);
            return;

        }
        if (s.minPitch != s.maxPitch)
        {
            s.source.pitch = UnityEngine.Random.Range(s.minPitch, s.maxPitch);
        }
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("NO SE ENCUENTRA EL AUDIO: " + name);
            return;

        }
        if (s.source.isPlaying) s.source.Stop();
    }

    public void playStreak(int index)
    {
        if (index > 0)
        {
            if (streakSounds[index - 1].source.isPlaying)
                streakSounds[index - 1].source.Stop();
        }
        streakSounds[index].source.Play();
    }
}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1.0f;
    [Range(1f, 3f)]
    public float minPitch = 1.0f;
    [Range(1f, 3f)]
    public float maxPitch = 1.0f;

    public bool loop;



    [HideInInspector]
    public AudioSource source;
}


