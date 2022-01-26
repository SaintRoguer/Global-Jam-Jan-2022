using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class PlayerController : MonoBehaviour {
    public Rigidbody2D rb;


    private Controls playerControls;
    public GameObject bulletPrefab;
    //Ground Check related
    private const float groundRadius = .2f;
    [SerializeField] private Transform groundCheckCollider;
    [SerializeField] private LayerMask groundLayer;
    private bool isOnGround = true;
    //Player Logic/UI related
    private Animator animator;
    private ColourSystem colourSystem;
    [SerializeField] private float speed = 400f;
    //Move related
    private float move;
    //Jump related
    [SerializeField] private float jumpForce;
    private int totalJumps;
    private int availableJumps;
    //Dash related
    [SerializeField] private float dashDistance;
    [SerializeField]private float dashTime;
    private Vector2 inicialDashPosition;
    private bool isDashing;

    private void Awake() {
        playerControls = new Controls();
        animator = GetComponent<Animator>();
        animator.SetFloat("Yellow", 1);
        colourSystem = GetComponent<ColourSystem>();
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
        playerControls.game.jump.canceled += Jump;
        playerControls.game.interact.performed += Interact;
        playerControls.game.dash.performed += Dash;
        playerControls.game.shoot.performed += Shoot;
        playerControls.game.switchGunColor.performed += SwitchGunColor;
        playerControls.game.switchPlayerColor.performed += SwitchPlayerColor;

        //amount of extra jumps
        totalJumps = 1;
        availableJumps = totalJumps;
        dashDistance = 3f;
        dashTime = 0.4f;

    }

    // Update is called once per frame
    private void Update() {
        
    }

    private void FixedUpdate() {
        if (!isDashing) {
            animator.SetFloat("yVelocity", rb.velocity.y);
            rb.velocity = new Vector2(move * speed * Time.fixedDeltaTime, rb.velocity.y);
        }
        GroundCheck();
        //Check if it is falling
        if(rb.velocity.y < 0 && isOnGround) {
            isOnGround = false;
            animator.SetBool("Jump", !isOnGround);
        }
    }
    public void Move(InputAction.CallbackContext context) {
        Vector3 currentScale = transform.localScale;
        move = playerControls.game.move.ReadValue<float>();
        if (move > 0 && transform.localScale.x > 0) {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        if (move < 0 && transform.localScale.x < 0) {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        animator.SetFloat("xVelocity", Mathf.Abs(playerControls.game.move.ReadValue<float>()));
        
    }
    public void Jump(InputAction.CallbackContext context) {
        if (context.performed && isOnGround) {

            isOnGround = false;
            availableJumps--;
            animator.SetBool("Jump", !isOnGround);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        else {
            if (context.performed && !isOnGround && availableJumps > 0) {
                availableJumps--;
                Debug.Log(availableJumps);
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }

        }
           
        if(context.canceled) {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * .5f);
        }
    }
    public void Interact(InputAction.CallbackContext context) {

    }
    public void Dash(InputAction.CallbackContext context) {
        if(!isDashing && move != 0) {
            StartCoroutine( Dash(move));
            Debug.Log("dash");
        }
       
    }
    private IEnumerator Dash(float direction) {
        isDashing = true;
        //Remuevo la gravedad
        rb.velocity = new Vector2(Mathf.Abs(rb.velocity.x) * dashDistance * direction, 0f);
        rb.AddForce(new Vector2(dashDistance * direction, 0f),ForceMode2D.Impulse);
        float gravity = rb.gravityScale;
        rb.gravityScale = 0f;
        animator.SetBool("Dash", true);
        yield return new WaitForSeconds(dashTime);
        animator.SetBool("Dash", false);
        isDashing = false;
        rb.gravityScale = gravity;
    }
    //This has to make the bullet go to the right side
    public void Shoot(InputAction.CallbackContext context) {
        GameObject actualBullet = Instantiate(bulletPrefab, transform.position, bulletPrefab.transform.rotation);
        actualBullet.GetComponent<BulletController>().SetDamage(10);
        actualBullet.GetComponent<BulletController>().SetPlayer(gameObject);
        if (move != 0)
            actualBullet.GetComponent<MoveForward>().SetDirection(move);
        animator.SetBool("Shoot", true);
    }
    public void SwitchGunColor(InputAction.CallbackContext context) {

    }
    public void SwitchPlayerColor(InputAction.CallbackContext context) {
        MainColours colourSelected = colourSystem.ChangeMainColour();
        switch (colourSelected)
        {
            case MainColours.YELLOW:
                ChangePlayerToYellow();
                break;
            case MainColours.BLUE:
                ChangePlayerToBlue();
                break;
            case MainColours.RED:
                ChangePlayerToRed();
                break;
            default:
                break;
        }
    }

    private void ChangePlayerToYellow()
    {
        animator.SetFloat("Yellow", 1);
        animator.SetFloat("Red", 0);
        animator.SetFloat("Blue", 0);
        //Cambiar da�o de la bala
    }

    private void ChangePlayerToBlue()
    {
        animator.SetFloat("Yellow", 0);
        animator.SetFloat("Red", 0);
        animator.SetFloat("Blue", 1);
        //Cambiar da�o de la bala

    }

    private void ChangePlayerToRed()
    {
        animator.SetFloat("Yellow", 0);
        animator.SetFloat("Red", 1);
        animator.SetFloat("Blue", 0);
        //Cambiar da�o de la bala

    }

    void GroundCheck() {
        bool wasOnGround = isOnGround;
        isOnGround = false;
        animator.SetBool("Jump", !isOnGround);

        Collider2D [] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundRadius, groundLayer);
        if (colliders.Length > 0) {
            isOnGround = true;
            animator.SetBool("Jump", !isOnGround);
            if (!wasOnGround) {
                availableJumps = totalJumps;
            }
        }
    }

}