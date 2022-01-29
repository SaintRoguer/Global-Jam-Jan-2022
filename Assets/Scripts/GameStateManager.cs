using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    private static GameStateManager _instance;

    public static GameStateManager Instance {
        get{
            if (_instance == null)
                _instance = new GameStateManager();

            return _instance;
        }
    }

    public delegate void GameStateChangeHandler(GameState newGameState);
    public event GameStateChangeHandler OnGameStateChanged;

    public GameState CurrentGameState { get; private set; }
    
    public void SetState(GameState gm) {
        if (gm == CurrentGameState)
            return;
        CurrentGameState = gm;
        OnGameStateChanged?.Invoke(gm);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
