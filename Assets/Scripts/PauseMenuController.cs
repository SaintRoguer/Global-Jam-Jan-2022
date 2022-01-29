using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    PauseController pauseController;

    private void Start() {
        pauseController = FindObjectOfType<PauseController>();
    }
    public void Continue() {
        pauseController.Continue();
    }
    public void Exit() {
        pauseController.Exit();
    }
}
