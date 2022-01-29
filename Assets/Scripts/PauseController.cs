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
    [SerializeField] GameObject player;
    Controls pauseControls;
    LevelLoader levelLoader;
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
        levelLoader = GetComponent<LevelLoader>();

        pauseControls.game.pause.performed += Pause;
    }
    private void OnDestroy() {
        pauseControls.game.pause.performed -= Pause;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDead() && actualDead==null)
            Pause(default);
    }

    public void Pause(InputAction.CallbackContext context) {
        GameState currentState = GameStateManager.Instance.CurrentGameState;
        GameState newGameState = currentState == GameState.Gameplay
            ? GameState.Pause
            : GameState.Gameplay;

        GameStateManager.Instance.SetState(newGameState);

        if (IsDead()) {
            currentState = GameStateManager.Instance.CurrentGameState;
            if (currentState != GameState.Gameover) {
                GameStateManager.Instance.SetState(currentState);

                actualDead = Instantiate(deadMenu, player.transform.position, deadMenu.transform.rotation);
                actualDead.enabled = true;
            }
            else {
                if (actualDead != null) {
                    Destroy(actualDead.gameObject);
                    deadMenu.enabled = false;
                }
            }
            return;
        }

        if (newGameState == GameState.Pause) {

            actualPause = Instantiate(pauseMenu, player.transform.position, pauseMenu.transform.rotation);
            actualPause.enabled = true;
        }
        if (newGameState == GameState.Gameplay) {
            Destroy(actualPause.gameObject);
            pauseMenu.enabled = false;
        }

        
       
    }
    bool IsDead() {
        return player.GetComponent<PlayerController>().IsDead();
    }
    public void Continue() {
        Pause(default);
    }
    public void Exit() {
        //levelLoader.sLevelToLoad = "Main Menu";
        levelLoader.LoadScene();
    }
    public void Respawn() {
        player.GetComponent<PlayerController>();
        Pause(default);
    }

}
