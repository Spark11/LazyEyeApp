﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour {

    public AudioSource backgroundSource;
    public AudioSource sfxSource;

    public static SoundManager instance = null;
    public enum SFX { CLICK, CORRECT, WRONG, VICTORY, LOSS, SCROLL };

    private static AudioClip[] sfxClips = { Resources.Load<AudioClip>("sfx_click"), Resources.Load<AudioClip>("sfx_correct"), Resources.Load<AudioClip>("sfx_wrong"), Resources.Load<AudioClip>("sfx_victory"), Resources.Load<AudioClip>("sfx_loss"), Resources.Load<AudioClip>("sfx_scroll") };

    private bool musicOff;
    private bool sfxOff;

    private GameObject musicButton;
    private GameObject sfxButton;

    //  this class will be a singleton
    void Awake () {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        musicOff = PlayerPrefs.GetInt("music") == 1;
        sfxOff = PlayerPrefs.GetInt("effects") == 1;
        backgroundSource.volume = musicOff ? 0 : 1;
                
        if(sfxClips[0] == null)
        {
            sfxClips[0] = Resources.Load<AudioClip>("sfx_click");
            sfxClips[1] = Resources.Load<AudioClip>("sfx_correct");
            sfxClips[2] = Resources.Load<AudioClip>("sfx_wrong");
            sfxClips[3] = Resources.Load<AudioClip>("sfx_victory");
            sfxClips[4] = Resources.Load<AudioClip>("sfx_loss");
            sfxClips[4] = Resources.Load<AudioClip>("sfx_scroll");
        }
    }


    //  initialize sound buttons with correct references and sprites
    public void InitSoundButtons()
    {
        musicButton = GameObject.Find("musicButton");
        sfxButton = GameObject.Find("sfxButton");

        musicButton.GetComponent<Image>().sprite = musicOff ? Resources.Load<Sprite>("musicOff") : Resources.Load<Sprite>("musicOn");
        sfxButton.GetComponent<Image>().sprite = sfxOff ? Resources.Load<Sprite>("sfxOff") : Resources.Load<Sprite>("sfxOn");
    }


    //  background music
    public void PlayBackgroundMusic(AudioClip clip)
    {
        backgroundSource.clip = clip;
        backgroundSource.Play();
    }

    public void PauseResumeBackgroundMusic()
    {
        if (backgroundSource.isPlaying)
            backgroundSource.Pause();
        else
            backgroundSource.UnPause();
    }

    public void StopBackgroundMusic()
    {
        backgroundSource.Stop();
    }


    //  sfx
    public void PlayEffect(SFX effect)
    {
        if (!sfxOff)
        {
            sfxSource.clip = sfxClips[(int)effect];
            sfxSource.Play();
        }
    }


    //  user settings
    public void ToggleBackgroundMusic()
    {
        musicOff = !musicOff;
        PlayerPrefs.SetInt("music", musicOff ? 1 : 0);
        backgroundSource.volume = musicOff ? 0 : 1;
        musicButton.GetComponent<Image>().sprite = musicOff ? Resources.Load<Sprite>("musicOff") : Resources.Load<Sprite>("musicOn");
    }

    public void ToggleEffects()
    {
        sfxOff = !sfxOff;
        PlayerPrefs.SetInt("effects", sfxOff ? 1 : 0);
        sfxButton.GetComponent<Image>().sprite = sfxOff ? Resources.Load<Sprite>("sfxOff") : Resources.Load<Sprite>("sfxOn");
    }
	
}
