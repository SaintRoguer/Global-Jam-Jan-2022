using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeCount : MonoBehaviour
{
    public Image[] lives;
    public int maxLives;
    int livesRemaining;
    [SerializeField] GameObject player;

    public void LoseLife()
    {
        //If no lives remaining, do nothing.
        if (livesRemaining == 0)
            return;
        //Decrease the values of livesRemaining.
        livesRemaining--;
        //Change one of the lives images.(later)
        //Hide one of the life images.
        lives[livesRemaining].enabled = false;


        //If we run out of lives we lose game or other thing
        if(livesRemaining == 0)
        {
            Debug.Log("Me mori");
            player.GetComponent<PlayerController>().Die();
            //FindObjectOfType<LevelManager>().Restart();
        }
    }
    public void Reset() {
        livesRemaining = maxLives;
    }
    private void Update()
    {
        
    }
}
