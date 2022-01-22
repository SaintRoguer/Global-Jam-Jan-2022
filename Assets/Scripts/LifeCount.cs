using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeCount : MonoBehaviour
{
    public Image[] lives;
    public int livesRemaining;

    public void loseLife()
    {
        //Decrease the values of livesRemaining.
        livesRemaining--;
        //Change one of the lives images.(later)
        //Hide one of the life images.
        lives[livesRemaining].enabled = false;


        //If we run out of lives we lose game or other thing
        if(livesRemaining == 0)
        {
            Debug.Log("YOU LOST");
        }
    }

    private void Update()
    {
        
    }
}