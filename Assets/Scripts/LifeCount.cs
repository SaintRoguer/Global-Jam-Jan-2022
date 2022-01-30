using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeCount : MonoBehaviour
{
    public Image[] lives;
    public Image[] noLives;
    public int maxLives;
    public int livesRemaining;
    [SerializeField] GameObject player;

    private void Start()
    {
        livesRemaining = maxLives;

        for (int i = 0; i < lives.Length; i++)
        {
          noLives[i].GetComponent<Image>().enabled = false;
        }
    }

    public void LoseLife()
    {
        //If no lives remaining, do nothing.
        if (livesRemaining == 0)
            return;
        //Decrease the values of livesRemaining.
        livesRemaining--;
        //Change one of the lives images.(later)
        noLives[livesRemaining].GetComponent<Image>().enabled = true;
        //Hide one of the life images.
        lives[livesRemaining].GetComponent<Image>().enabled = false;

        //If we run out of lives we lose game or other thing
        if(livesRemaining == 0)
        {
            player.GetComponent<PlayerController>().Die();
        }
    }
    public void Respawn() {
        livesRemaining = maxLives;
        for(int i =0; i<lives.Length; i++) {
            lives[i].GetComponent<Image>().enabled = true;
            noLives[i].GetComponent<Image>().enabled = false;
        }
    }
}
