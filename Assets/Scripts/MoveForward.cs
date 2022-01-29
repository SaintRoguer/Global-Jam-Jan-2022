using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    private float speed = 10;
    private float direction;
    // Start is called before the first frame update
    void Start()
    {
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }
    private void OnDestroy() {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 rightOrLeft;
        if (direction < 0)
            rightOrLeft = Vector3.left;
        else
            rightOrLeft = Vector3.right;
        transform.Translate(rightOrLeft * speed * Time.deltaTime ); 
    }

    public void SetDirection(float d) {
        direction = d;
    }
    public void SetSpeed(float s) {
        speed = s;
    }
    public void OnGameStateChanged(GameState gm) {
        enabled = gm == GameState.Gameplay;
    }
}
