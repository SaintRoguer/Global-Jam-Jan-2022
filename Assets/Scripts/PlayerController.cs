using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    private Controls playerControls;
    public GameObject bulletPrefab;

    Animator animator;

    private float speed = 400f;
    public float jumpForce;

    private bool isOnGround = true;
    private float move = 0;
    
    private void Awake() {
        playerControls = new Controls();
        animator = GetComponent<Animator>();
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void OnDisable() {
        playerControls.Disable();
    }
    // Start is called before the first frame update
    void Start() {
       
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
    private void Update() {
        
    }

    private void FixedUpdate() {
        rb.velocity = new Vector2(move * speed * Time.fixedDeltaTime, rb.velocity.y);
    }
    public void Move(InputAction.CallbackContext context) {
        move = playerControls.game.move.ReadValue<float>();
        animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
    }
    public void Jump(InputAction.CallbackContext context) {
        if (context.performed && isOnGround) { 
            rb.AddForce(Vector2.up * jumpForce);
            isOnGround = false; 
        }
        Debug.Log("Jump");
    }
    public void Interact(InputAction.CallbackContext context) {

    }
    public void Dash(InputAction.CallbackContext context) {

    }
    //This has to make the bullet go to the right side
    public void Shoot(InputAction.CallbackContext context) {
        GameObject actualBullet = Instantiate(bulletPrefab, transform.position, bulletPrefab.transform.rotation);
        //actualBullet.GetComponent<MoveForward>().SetSpeed(move / Mathf.Abs(move));
    }
    public void SwitchGunColor(InputAction.CallbackContext context) {

    }
    public void SwitchPlayerColor(InputAction.CallbackContext context) {

    }
    private void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log(collision.ToString());
        isOnGround = true;
        Debug.Log("Colision");
    }
}
