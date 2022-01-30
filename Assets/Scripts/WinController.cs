using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class WinController : MonoBehaviour
{
    public GameObject player;
    private int itemsToWin = 3;

    public void GetSoul()
    {
        itemsToWin--;
        if (itemsToWin == 0) {
            SceneManager.LoadScene("Win Scene");            
        }
    }
}
