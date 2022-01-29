using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManagerScript : MonoBehaviour
{
    public static AudioClip jump, dash, shoot, normalEnemyDeathSound;
    static AudioSource audioSrc;
    public AudioMixer masterMixer, sfxMixer;
    private float masterVolume = 1f;
    private float effectsVolume = 1f;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        masterMixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("masterVolume"));
        sfxMixer.SetFloat("EffectsVolume", PlayerPrefs.GetFloat("effectsVolume"));

        normalEnemyDeathSound = Resources.Load<AudioClip>("NormalEnemyDeathSound");
        jump = Resources.Load<AudioClip>("Jump");
        dash = Resources.Load<AudioClip>("Dash");
        shoot = Resources.Load<AudioClip>("Gunshot");

        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "normalEnemyDeath":
                audioSrc.PlayOneShot(normalEnemyDeathSound);
                break;
            case "jump":
                audioSrc.PlayOneShot(jump);
                break;
            case "dash":
                audioSrc.PlayOneShot(dash);
                break;
            case "shoot":
                audioSrc.PlayOneShot(shoot);
                break;
        }
    }
    public void SetMasterVolume(float vol) {
        masterMixer.SetFloat("MasterVolume", Mathf.Log10(vol) * 20);
        PlayerPrefs.SetFloat("masterVolume", Mathf.Log10(vol) * 20);
    }
    public void SetEffectsVolume(float vol) {
        masterMixer.SetFloat("EffectsVolume", Mathf.Log10(vol) * 20);
        PlayerPrefs.SetFloat("effectsVolume", Mathf.Log10(vol) * 20);
    }
}
