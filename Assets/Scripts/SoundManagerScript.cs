using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManagerScript : MonoBehaviour
{
    public static AudioClip jump, dash, shoot, normalEnemyDeathSound, move,death,alive,win,introVoice;
    public static AudioSource[] audioSrc; 
    public AudioMixer masterMixer, sfxMixer;
    public static SoundManagerScript instance;
    private float masterVolume = 1f;
    private float effectsVolume = 1f;
    private static bool keepFadeIn;
    private static bool keepFadeOut;
    private void Awake() {
        DontDestroyOnLoad(gameObject);
        masterMixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("masterVolume"));
        sfxMixer.SetFloat("EffectsVolume", PlayerPrefs.GetFloat("effectsVolume"));
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        normalEnemyDeathSound = Resources.Load<AudioClip>("NormalEnemyDeathSound");
        jump = Resources.Load<AudioClip>("Jump");
        dash = Resources.Load<AudioClip>("Dash");
        shoot = Resources.Load<AudioClip>("Gunshot");
        move = Resources.Load<AudioClip>("Step");
        death = Resources.Load<AudioClip>("Death-Screen");
        alive = Resources.Load<AudioClip>("BGM 1");
        win = Resources.Load<AudioClip>("Congratulations");
        introVoice = Resources.Load<AudioClip>("Intro Voice");

        audioSrc = GetComponents<AudioSource>();
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
                audioSrc[1].PlayOneShot(normalEnemyDeathSound);
                break;
            case "jump":
                audioSrc[1].PlayOneShot(jump);
                break;
            case "dash":
                audioSrc[1].PlayOneShot(dash);
                break;
            case "shoot":
                audioSrc[1].PlayOneShot(shoot);
                break;
            case "move":
                audioSrc [1].clip = move;
                audioSrc [1].Play();
                break;
            case "death":
                //instance.StartCoroutine(FadeOut(audioSrc [0], 1f));
                audioSrc [0].Stop();
                audioSrc [0].clip = death;
                audioSrc [0].Play();
                break;
            case "alive":
                audioSrc [0].Stop();
                audioSrc [0].clip = alive;
                audioSrc [0].Play();
                break;
            case "win":
                audioSrc [0].Stop();
                audioSrc [0].PlayOneShot(win);
                break;
            case "intro-voice":
                audioSrc[1].PlayOneShot(introVoice);
                break;
        }
    }

    public static void Stop()
    {
        foreach (AudioSource a in audioSrc)
            a.Stop();
    }

    public static void StopSFX() {
        audioSrc [1].Stop();
        audioSrc [1].clip = null;
    }
    static IEnumerator FadeIn(AudioSource audioSource, float speed, float targetVolume) {
        keepFadeIn = true;
        keepFadeOut = false;
        audioSource.volume = 0;
        float audio = 0;
        while (audioSource.volume<targetVolume && keepFadeIn) {
            audio += speed;
            audioSource.volume = audio;
            Debug.Log(audio);
            yield return new WaitForSeconds(0.1f);
        }
    }
    static IEnumerator FadeOut(AudioSource audioSource, float speed) {
        keepFadeIn = false;
        keepFadeOut = true;
        float audio = audioSource.volume;
        Debug.Log(audio);
        while (audioSource.volume >= speed && keepFadeOut) {
            audio -= speed;
            audioSource.volume = audio;
            yield return new WaitForSeconds(0.1f);
        }
    }
    public void SetMasterVolume(float vol) {
        masterMixer.SetFloat("MasterVolume", Mathf.Log10(vol) * 20);
        PlayerPrefs.SetFloat("masterVolume", Mathf.Log10(vol) * 20);
    }
    public void SetEffectsVolume(float vol) {
        sfxMixer.SetFloat("EffectsVolume", Mathf.Log10(vol) * 20);
        PlayerPrefs.SetFloat("effectsVolume", Mathf.Log10(vol) * 20);
    }
}
