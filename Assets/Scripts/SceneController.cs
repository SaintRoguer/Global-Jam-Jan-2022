using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneController : MonoBehaviour
{
    private int indiceEscena;
    private GameObject tutorial;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeScene(int index) {
        SceneManager.LoadScene(index);
    }
    public void summonTutorial() {
        
    }
}
