using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    bool isActive;
    // Start is called before the first frame update
    void Start()
    {
        hideTutorial();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive) {
            if(Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X)) {
                hideTutorial();
            }

        }
    }

    public void showTutorial() {
        isActive = true;
        gameObject.SetActive(true);
    }
    public void hideTutorial() {
        isActive = false;
        gameObject.SetActive(false);
    }
}
