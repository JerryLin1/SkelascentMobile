using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private Sound theme;
    private int oldValue;
    public Sound[] sounds;
    public bool isGameAM = false;
    public static AudioManager instance;
    void Awake()
    {
        if (isGameAM == true) {
            if (instance == null) instance = this;
            else {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
        }
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }
    void Start() {
        theme = Array.Find(sounds, sound => sound.name == "Theme");
        if (isGameAM == true) {
            theme.source.Play();
        }
    }

    void Update() {
        if (isGameAM == true && PlayerPrefs.GetInt("muteMusic") != oldValue) {
            if (PlayerPrefs.GetInt("muteMusic") == 1) {
                theme.source.Pause(); 
            } else {
                theme.source.Play();
            }
            oldValue = PlayerPrefs.GetInt("muteMusic");
        } 
    }


    public void Play(string name)
    {
        if (name != "Theme" && PlayerPrefs.GetInt("muteSounds") == 1) return; 
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }
        s.source.Play();
        
    }

}
