using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class WinAudio : MonoBehaviour
{
    public static AudioClip win;
    public static AudioSource audioSrc;
    public AudioMixer masterMixer, sfxMixer;
    private float masterVolume = 1f;
    private float effectsVolume = 1f;
    // Start is called before the first frame update
    void Start() {
        win = Resources.Load<AudioClip>("Congratulations");

        audioSrc = GetComponent<AudioSource>();
        audioSrc.Stop();
        audioSrc.PlayOneShot(win);
    }

}
