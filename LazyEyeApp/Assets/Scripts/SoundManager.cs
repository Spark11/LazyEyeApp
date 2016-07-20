using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

    public AudioSource backgroundSource;
    public AudioSource sfxSource;

    public static SoundManager instance = null;
    public enum SFX { CLICK, CORRECT, WRONG, VICTORY, LOSS };

    private static AudioClip[] sfxClips = { Resources.Load<AudioClip>("sfx_click"), Resources.Load<AudioClip>("sfx_correct"), Resources.Load<AudioClip>("sfx_wrong"), Resources.Load<AudioClip>("sfx_victory"), Resources.Load<AudioClip>("sfx_loss"), };

    private bool musicOff;
    private bool effectsOff;
    

    //  this class will be a singleton
    void Awake () {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        musicOff = PlayerPrefs.GetInt("music") == 1;
        effectsOff = PlayerPrefs.GetInt("effects") == 1;
        backgroundSource.volume = musicOff ? 0 : 1;
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
        if (!effectsOff)
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
    }

    public void ToggleEffects()
    {
        effectsOff = !effectsOff;
        PlayerPrefs.SetInt("effects", effectsOff ? 1 : 0);
    }
	
}
