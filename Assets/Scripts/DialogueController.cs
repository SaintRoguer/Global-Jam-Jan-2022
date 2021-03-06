using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    List<string> historia;
    float coolDown = 20f;
    float nextText = 0;
    int index = 0;
    bool isOver;
    LevelLoader lvl;
    // Start is called before the first frame update
    void Start()
    {
        historia = new List<string>();
        //Add all the text
        historia.Add("You know, this is not an easy task. Being THE DEATH I mean.\n It's a lot of paperwork, and see who goes where and the things like that.\n But luckily they sent you, my new Little Angel!\n Other angels died before, but with the power of my new weapon, the mighty\n CHROMABLAST \n you can collect the colors to open portals among the realms.");
        historia.Add("Try to collect three souls, they are like little lights with heart pulsation.\n Keep in mind that each color destroys their oposite and...\n Good luck!");
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
	public void next() {
		if (index >= historia.Count) 
			IsOver();
		index++;
		nextText = Time.time;
	}
}
