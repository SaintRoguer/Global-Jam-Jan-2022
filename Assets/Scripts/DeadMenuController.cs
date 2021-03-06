using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadMenuController : MonoBehaviour 
 { 
    PauseController pauseController;

    private void Start() {
        pauseController = FindObjectOfType<PauseController>();
    }
    public void Respawn() {
        pauseController.Respawn();
    }
    public void Exit() {
        pauseController.Exit();
    }
}
