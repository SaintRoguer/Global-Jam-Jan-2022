using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class PauseController : MonoBehaviour
{
    [SerializeField] Canvas pauseMenu;
    [SerializeField] Canvas deadMenu;
    [SerializeField] GameObject player;
    Controls pauseControls;
    // Start is called before the first frame update

    private void Awake() {
        pauseControls = new Controls();
    }
    private void OnEnable() {
        pauseControls.Enable();
    }
    private void OnDisable() {
        pauseControls.Disable();
    }
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        pauseMenu.enabled = false;
        deadMenu.enabled = false;

        pauseControls.game.pause.performed += Pause;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void Pause(InputAction.CallbackContext context) {
        Debug.Log("pausa");
        GameState currentState = GameStateManager.Instance.CurrentGameState;
        GameState newGameState = currentState == GameState.Gameplay
            ? GameState.Pause
            : GameState.Gameplay;

        GameStateManager.Instance.SetState(newGameState);
        if (newGameState == GameState.Pause)
            pauseMenu.enabled = true;
        if (newGameState == GameState.Gameplay)
            pauseMenu.enabled = false;
        
        if (IsDead()) {
            currentState = GameStateManager.Instance.CurrentGameState;
            if (currentState != GameState.Gameover)
                GameStateManager.Instance.SetState(currentState);
            deadMenu.enabled = true;
        }
        else
            deadMenu.enabled = false;
    }
    bool IsDead() {
        return player.GetComponent<PlayerController>().IsDead();
    }


}