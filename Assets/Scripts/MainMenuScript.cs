using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MainMenuScript : MonoBehaviour
{
    public static AudioClip intro;
    public static AudioSource audioSrc;
    public AudioMixer masterMixer, sfxMixer;
    // Start is called before the first frame update
    void Start()
    {
        //SoundManagerScript.PlaySound("intro-voice");
        intro = Resources.Load<AudioClip>("Intro Voice");

        SoundManagerScript.Stop();
        audioSrc = GetComponent<AudioSource>();
        audioSrc.Stop();
        audioSrc.PlayOneShot(intro);
        SoundManagerScript.PlaySound("alive");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
