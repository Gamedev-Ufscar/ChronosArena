using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour {

    public static AudioManager AM;
    public Sound[] sounds;
    public bool mainMenu = true;

    [HideInInspector]
    bool alreadyInBattle = false;

    private void Start()
    {
        Play("MainMenuTheme");
    }

    void Awake () {
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        DontDestroyOnLoad(this);
        if (AM == null) {
            AM = this;
        } else {
            if (AM != this) {
                Destroy(this);
            }
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2) {
            Stop("MainMenuTheme");
        }

        if (SceneManager.GetActiveScene().buildIndex == 3 && !alreadyInBattle) {
            Stop("BattleTheme");
            Play("BattleTheme");
            alreadyInBattle = true;
        }
    }

    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => name == sound.name);
        if (s.source.loop == false)
        {
            s.source.PlayOneShot(s.source.clip);
        } else
        {
            s.source.Play();
        }

        //mainMenu = (name == "MainMenuTheme" || (name == "ShotKill" && mainMenu == true)) ? true : false;

    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => name == sound.name);
        
         s.source.Stop();

    }

    public void StopAll()
    {

        AudioSource[] audioallAudioSources = FindObjectsOfType<AudioSource>() as AudioSource[];
        foreach (AudioSource audioS in audioallAudioSources)
        {
            audioS.Stop();
        }

    }

    public void CardSound()
    {
        int r = UnityEngine.Random.Range(0, 5);
        string[] soundList = {"Card 1", "Card 2", "Card 3", "Card 4", "Card 5"};

        Play(soundList[r]);
    }
}
