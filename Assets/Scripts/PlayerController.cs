using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Controls playerControls;

    private float speed = 1f;
    public float jumpForce;
    public float gravityModifier;
    private bool isOnGround = true;
    private Vector2 move;
    
    private void Awake() {
        playerControls = new Controls();
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void OnDisable() {
        playerControls.Disable();
    }
    // Start is called before the first frame update
    void Start() {
        rb = gameObject.GetComponent<Rigidbody2D>();
        Physics.gravity *= gravityModifier;
        rb.velocity = move;

        //suscripciones a los eventos
        playerControls.game.move.performed += Move;
        playerControls.game.jump.performed += Jump;
        playerControls.game.interact.performed += Interact;
        playerControls.game.dash.performed += Dash;
        playerControls.game.shoot.performed += Shoot;
        playerControls.game.switchGunColor.performed += SwitchGunColor;
        playerControls.game.switchPlayerColor.performed += SwitchPlayerColor;
    }

    // Update is called once per frame

    public void Move(InputAction.CallbackContext context) {
        move = playerControls.game.move.ReadValue<Vector2>();
        Debug.Log("Move");
    }
    public void Jump(InputAction.CallbackContext context) {
        rb.AddForce(Vector2.up * jumpForce);
        isOnGround = false;
        Debug.Log("Jump");
    }
    public void Interact(InputAction.CallbackContext context) {

    }
    public void Dash(InputAction.CallbackContext context) {

    }
    public void Shoot(InputAction.CallbackContext context) {

    }
    public void SwitchGunColor(InputAction.CallbackContext context) {

    }
    public void SwitchPlayerColor(InputAction.CallbackContext context) {

    }

    private void OnCollisionEnter(Collision collision) {
        isOnGround = true;
    }
}
