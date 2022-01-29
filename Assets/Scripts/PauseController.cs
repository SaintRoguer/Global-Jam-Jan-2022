using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    [SerializeField] Canvas pauseMenu;
    [SerializeField] Canvas deadMenu;
    [SerializeField] GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        pauseMenu.enabled = false;
        deadMenu.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            GameState currentState = GameStateManager.Instance.CurrentGameState;
            GameState newGameState = currentState == GameState.Gameplay
                ? GameState.Pause
                : GameState.Gameplay;

            GameStateManager.Instance.SetState(newGameState);
            if(newGameState == GameState.Pause)
                pauseMenu.enabled = true;
            if (newGameState == GameState.Gameplay)
                pauseMenu.enabled = false;
        }
        if (IsDead()) {
            GameState currentState = GameStateManager.Instance.CurrentGameState;
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
