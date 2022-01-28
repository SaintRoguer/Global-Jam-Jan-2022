using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    public AudioMixer masterMixer;
    private float masterVolume = 1f;
    private void Start() {
        masterMixer.SetFloat("MasterVolume",PlayerPrefs.GetFloat("masterVolume"));
    }
    public void SetVolume (float vol) {
        masterMixer.SetFloat("MasterVolume",Mathf.Log10(vol) * 20);
        PlayerPrefs.SetFloat("masterVolume", Mathf.Log10(vol) * 20);
    }
}
