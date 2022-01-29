using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnSystem : MonoBehaviour
{
    public List<CheckerClass> doorChecker;
    public List<CheckerClass> itemChecker;

    // Start is called before the first frame update
    void Start()
    {
        doorChecker = new List<CheckerClass>();
        itemChecker = new List<CheckerClass>();
    }
}
