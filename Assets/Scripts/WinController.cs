using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinController : MonoBehaviour
{
    public GameObject player;
    private int itemsToWin = 3;

    public void GetSoul()
    {
        itemsToWin--;
        Debug.Log("Items left to win : " + itemsToWin);
        if (itemsToWin == 0)
            Debug.Log("I WIN");
    }
}
