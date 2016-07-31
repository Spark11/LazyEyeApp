﻿using UnityEngine;
using System.Collections;

public class RotateOnTouchScript : MonoBehaviour {

    private int speed;

    void Start()
    {
        //  init speed
#if UNITY_EDITOR
        speed = 150;
#elif UNITY_ANDROID
        speed = 20;
#endif

        //  init sound buttons
        SoundManager.instance.InitSoundButtons();
        SoundManager.instance.PlayBackgroundMusic(Resources.Load<AudioClip>("main_music"));

        //  scale elements on android devices
#if UNITY_EDITOR
        // do nothing - its already perfect!
#elif UNITY_ANDROID
        Vector3 scaleUp = new Vector3(3, 3, 1);
        GameObject.Find("soundButtons").transform.localScale = scaleUp;
        
#endif
    }

    // rotate planets on mouse/finger move
    void Update()
    {

#if UNITY_EDITOR        
        if (Input.GetMouseButton(0))
            foreach (Transform child in transform)
                child.RotateAround(Vector3.zero, Vector3.up, Input.GetAxis("Mouse X") * speed * Time.deltaTime);
#elif UNITY_ANDROID
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
            foreach (Transform child in transform)
                child.RotateAround(Vector3.zero, Vector3.up, Input.GetTouch(0).deltaPosition.x * speed * Time.deltaTime);        
#endif

    }


    //  Toggle sound buttons
    public void ToggleMusic()
    {
        SoundManager.instance.ToggleBackgroundMusic();
        SoundManager.instance.PlayEffect(SoundManager.SFX.CLICK);
    }

    public void ToggleEffects()
    {
        SoundManager.instance.ToggleEffects();
        SoundManager.instance.PlayEffect(SoundManager.SFX.CLICK);
    }
}
