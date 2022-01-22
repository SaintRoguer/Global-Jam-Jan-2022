using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void Restart()
    {
        //1- Restart the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //Reset the players position
        //Save the players initial position when game starts
        //When respawning simply reposition the player to that init position 
    }
}
