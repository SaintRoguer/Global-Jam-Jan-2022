using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class PauseController : MonoBehaviour
{
    [SerializeField] Canvas pauseMenu;
    Canvas actualPause;
   [SerializeField] Canvas deadMenu;
    Canvas actualDead;
    [SerializeField] PlayerController player;
    Controls pauseControls;
    LevelLoader levelLoader;
    GameState currentState;
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
        player = FindObjectOfType<PlayerController>();
        DontDestroyOnLoad(gameObject);
        currentState = GameState.Gameplay;
        pauseMenu.enabled = false;
        deadMenu.enabled = false;
        levelLoader = GetComponent<LevelLoader>();
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        pauseControls.game.pause.performed += Pause;
    }
    private void OnDestroy() {
        pauseControls.game.pause.performed -= Pause;

        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void OnGameStateChanged(GameState gm) {
        if(gm == GameState.Gameover)
            Pause(default);
    }
    public void Pause(InputAction.CallbackContext context) {
        currentState = GameStateManager.Instance.CurrentGameState;
        GameState newGameState;

        if (currentState == GameState.Gameplay) {
            newGameState = GameState.Pause;
        }
        else if (currentState == GameState.Pause) {
            newGameState = GameState.Gameplay;
        }
        else
            newGameState = GameState.Gameover;

        GameStateManager.Instance.SetState(newGameState);

        currentState = GameStateManager.Instance.CurrentGameState;
        if (currentState == GameState.Gameover) {
            actualDead = Instantiate(deadMenu);
            actualDead.enabled = true;
            deadMenu.enabled = true;
            if (actualPause != null) {
                Destroy(actualPause.gameObject);
                pauseMenu.enabled = false;
            }
        }
        else {
            if (actualDead != null) {
                Destroy(actualDead.gameObject);
                deadMenu.enabled = false;
            }
        }
        
        if (newGameState == GameState.Pause) {
            pauseMenu.enabled = true;
            if(actualDead != null) {
                Destroy(actualDead.gameObject);
                deadMenu.enabled = false;
            }
            actualPause = Instantiate(pauseMenu);
            actualPause.enabled = true;
        }
        if (newGameState == GameState.Gameplay) {
            if (actualDead != null) {
                Destroy(actualDead.gameObject);
                deadMenu.enabled = false;
            }
            if (actualPause != null) { 
                Destroy(actualPause.gameObject);
                pauseMenu.enabled = false;
            }
        }

        
       
    }
    public void Continue() {
        Pause(default);
    }
    public void Exit() {
        if (actualDead != null)
            Destroy(actualDead.gameObject);

        if (actualPause != null)
            Destroy(actualPause.gameObject);
        levelLoader.sLevelToLoad = "Main Menu";
        levelLoader.LoadScene();
    }
    public void Respawn() {
        player = FindObjectOfType<PlayerController>();
        player.Respawn();
        actualDead.enabled = false;
        deadMenu.enabled = false;
        //Pause(default);
    }

}
