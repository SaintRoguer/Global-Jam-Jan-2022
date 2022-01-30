using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    List<string> historia;
    float coolDown = 1f;
    float nextText = 0;
    int index = 0;
    bool isOver;
    LevelLoader lvl;
    // Start is called before the first frame update
    void Start()
    {
        historia = new List<string>();
        //Add all the text
        historia.Add("Dialogo de prueba \n Numero 1");
        historia.Add("Dialogo de prueba2");
        historia.Add("Dialogo de prueba3");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time > nextText) {
            Text texto = GetComponentInChildren<Text>();
            if (index < historia.Count) {
                texto.text = historia [index];
                index++;
                nextText = Time.time + coolDown;
            }
         }
        if (isOver) {
            
            lvl = GetComponent<LevelLoader>();
            lvl.sLevelToLoad = "Room 1";
            lvl.LoadScene();

        }
    }
    public void IsOver() {
        isOver = true;
    }
}
