using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManagerScript : MonoBehaviour
{
    public static AudioClip jump, dash, shoot, normalEnemyDeathSound, move,death,alive;
    public static AudioSource[] audioSrc; 
    public AudioMixer masterMixer, sfxMixer;
    public static SoundManagerScript instance;
    private float masterVolume = 1f;
    private float effectsVolume = 1f;

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
                instance.StartCoroutine(StartFade(audioSrc [0], 1f, 0f));
                //audioSrc [0].Stop();
                //StartFade(audioSrc [0], 10000f, PlayerPrefs.GetFloat("masterVolume"));
                //audioSrc [0].PlayOneShot(death);
                break;
            case "alive":
                audioSrc [0].Stop();
                audioSrc [0].PlayOneShot(alive);
                break;
        }
    }

    public static void StopSFX() {
        audioSrc [1].Stop();
        audioSrc [1].clip = null;
    }

    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume) {
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration) {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
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
