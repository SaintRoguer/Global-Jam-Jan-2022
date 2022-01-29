using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    public static AudioClip jump, dash, shoot, normalEnemyDeathSound;
    static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);

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
}
