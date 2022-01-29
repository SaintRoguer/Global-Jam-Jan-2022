using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    public static AudioClip normalEnemyDeathSound;
    static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        normalEnemyDeathSound = Resources.Load<AudioClip>("NormalEnemyDeathSound");

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
            case ("normalEnemyDeath"):
                audioSrc.PlayOneShot(normalEnemyDeathSound);
                break;
        }
    }
}
