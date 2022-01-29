using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class PlayerController : MonoBehaviour {
    public Rigidbody2D rb;
    private PauseController pauseController;
    private Controls playerControls;
    //Ground Check related
    private const float groundRadius = .2f;
    [SerializeField] private Transform groundCheckCollider;
    [SerializeField] private LayerMask groundLayer;
    private bool isOnGround = true;
    //Player Logic/UI related
    private Animator animator;
    private ColourSystem colourSystem;
    [SerializeField] private float speed = 400f;
    private bool isDead;
    //Move related
    private float move;
    private float lastDirection;
    //Jump related
    [SerializeField] private float jumpForce;
    private int totalJumps;
    private int availableJumps;
    //Shoot related
    GameObject actualBullet;
    public GameObject bulletPrefab;
    private int damage;
    //Dash related
    [SerializeField] private float dashDistance;
    [SerializeField]private float dashTime;
    private Vector2 inicialDashPosition;
    private bool isDashing;

    //Interaction related
    //Detection Point
    public Transform detectionPoint;
    //Detection Radius
    private const float detectionRadius = 0.2f;
    //Detection Layer
    public LayerMask detectionLayer;

    //Level
    private GameObject[] players;
    private int previousLevel = 3;
    private GameObject [] lives;
    private void Awake() {
        playerControls = new Controls();
        animator = GetComponent<Animator>();
        animator.SetFloat("Yellow", 1);
        animator.SetFloat("YellowGun", 1);
        lastDirection = -1;
        colourSystem = GetComponent<ColourSystem>();
    }

    internal bool IsDead() {
        return isDead;
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void OnDisable() {
        playerControls.Disable();
    }
    private void OnDestroy() {
        //suscripciones a los eventos
        playerControls.game.move.performed -= Move;
        playerControls.game.jump.performed -= Jump;
        playerControls.game.jump.canceled -= Jump;
        playerControls.game.interact.performed -= Interact;
        playerControls.game.dash.performed -= Dash;
        playerControls.game.shoot.performed -= Shoot;
        playerControls.game.switchGunColor.performed -= SwitchGunColor;
        playerControls.game.switchPlayerColor.performed -= SwitchPlayerColor;
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }
    // Start is called before the first frame update
    void Start() {

        DontDestroyOnLoad(gameObject);
        //suscripciones a los eventos
        playerControls.game.move.performed += Move;
        playerControls.game.jump.performed += Jump;
        playerControls.game.jump.canceled += Jump;
        playerControls.game.interact.performed += Interact;
        playerControls.game.dash.performed += Dash;
        playerControls.game.shoot.performed += Shoot;
        playerControls.game.switchGunColor.performed += SwitchGunColor;
        playerControls.game.switchPlayerColor.performed += SwitchPlayerColor;
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        //amount of extra jumps
        totalJumps = 1;
        availableJumps = totalJumps;
        dashDistance = 4f;
        dashTime = 0.25f;
        damage = bulletPrefab.GetComponent<BulletController>().GetDamage();

    }

    // Update is called once per frame
    private void Update() {
        
    }

    private void FixedUpdate() {
        if (!isDashing) {
            if(rb.velocity.y > 1 || rb.velocity.y < -1)
                animator.SetFloat("yVelocity", rb.velocity.y);
            else
                animator.SetFloat("yVelocity", 0f);
            rb.velocity = new Vector2(move * speed * Time.fixedDeltaTime, rb.velocity.y);
        }
        else
            animator.SetFloat("yVelocity", 0f);
        GroundCheck();
    }
    public void OnGameStateChanged(GameState gm) {
        Debug.Log(gm);
        enabled = gm == GameState.Gameplay;
    }
    public void Move(InputAction.CallbackContext context) {
        Vector3 currentScale = transform.localScale;
        move = playerControls.game.move.ReadValue<float>();
        if (context.performed && move!=0) {
            lastDirection = move;
        }
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
            SoundManagerScript.PlaySound("jump");
            availableJumps--;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetBool("Jump", true);
        }
        else {
            if (context.performed && !isOnGround && availableJumps > 0) {
                SoundManagerScript.PlaySound("jump");
                availableJumps--;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }

        }
           
        if(context.canceled) {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * .5f);
        }
    }
    public void Interact(InputAction.CallbackContext context) {
        Collider2D obj = Physics2D.OverlapCircle(detectionPoint.position, detectionRadius, detectionLayer);

        if (obj)
            obj.GetComponent<Item>().Interact(this);
    }



    public void Dash(InputAction.CallbackContext context) {
        if(!isDashing) {
            StartCoroutine( Dash(lastDirection));
        }
       
    }
    private IEnumerator Dash(float direction) {
        isDashing = true;
        SoundManagerScript.PlaySound("dash");
        //Remuevo la gravedad
        if (rb.velocity.x!=0)
            rb.velocity = new Vector2(Mathf.Abs(rb.velocity.x) * dashDistance * direction, 0f);
        else 
            rb.velocity = new Vector2(speed * Time.fixedDeltaTime * dashDistance * direction, 0f);


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
        SoundManagerScript.PlaySound("shoot");

        actualBullet = Instantiate(bulletPrefab, transform.position+new Vector3(lastDirection*1,-0.3f,0), bulletPrefab.transform.rotation);
        actualBullet.GetComponent<BulletController>().SetDamage(damage);
        actualBullet.GetComponent<BulletController>().SetPlayer(gameObject);
        actualBullet.GetComponent<MoveForward>().SetDirection(lastDirection);
        if(lastDirection<0)
            actualBullet.GetComponent<SpriteRenderer>().flipX = false;
        else
            actualBullet.GetComponent<SpriteRenderer>().flipX = true;
        animator.SetBool("Shoot", true);
    }
    public void SwitchGunColor(InputAction.CallbackContext context) {
        MainColours colourSelected = colourSystem.ChangeSecondaryColour();
        switch (colourSelected)
        {
            case MainColours.YELLOW:
                ChangeGunToYellow();
                if (damage <= 5)
                    damage += 5;
                break;
            case MainColours.BLUE:
                ChangeGunToBlue();
                if (damage >= 5)
                    damage -= 5;
                break;
            case MainColours.RED:
                ChangeGunToRed();
                if (damage >= 5)
                    damage -= 5;
                break;
            default:
                break;
        }
    }

    private void ChangeGunToYellow()
    {
        animator.SetFloat("YellowGun", 1);
        animator.SetFloat("RedGun", 0);
        animator.SetFloat("BlueGun", 0);
        //Cambiar da�o de la bala
    }

    private void ChangeGunToBlue()
    {
        animator.SetFloat("YellowGun", 0);
        animator.SetFloat("RedGun", 0);
        animator.SetFloat("BlueGun", 1);
    }

    private void ChangeGunToRed()
    {
        animator.SetFloat("YellowGun", 0);
        animator.SetFloat("RedGun", 1);
        animator.SetFloat("BlueGun", 0);
    }

    public void SwitchPlayerColor(InputAction.CallbackContext context) {
        MainColours colourSelected = colourSystem.ChangeMainColour();
        switch (colourSelected)
        {
            case MainColours.YELLOW:
                ChangePlayerToYellow();
                if (damage <= 5)
                    damage += 5;
                availableJumps = 1;
                isDashing = true;
                break;
            case MainColours.BLUE:
                ChangePlayerToBlue();
                if (damage >= 5)
                    damage -= 5;
                availableJumps += 1;
                isDashing = true;
                break;
            case MainColours.RED:
                ChangePlayerToRed();
                if (damage >= 5)
                    damage -= 5;
                availableJumps = 1;
                isDashing = false;
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

        Collider2D [] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundRadius, groundLayer);
        if (colliders.Length > 0) {
            isOnGround = true;
            //animator.SetBool("Jump", !isOnGround);
            if (!wasOnGround) {
                availableJumps = totalJumps;
            }
        }

        animator.SetBool("Jump", !isOnGround);
    }

    public void Die() {
        OnDisable();
        isDead = true;
        speed = 0f;
        animator.SetBool("Dead", true);
        animator.SetFloat("xVelocity", 0f);
        //suscripciones a los eventos
        playerControls.game.move.performed -= Move;
        playerControls.game.jump.performed -= Jump;
        playerControls.game.jump.canceled -= Jump;
        playerControls.game.interact.performed -= Interact;
        playerControls.game.dash.performed -= Dash;
        playerControls.game.shoot.performed -= Shoot;
        playerControls.game.switchGunColor.performed -= SwitchGunColor;
        playerControls.game.switchPlayerColor.performed -= SwitchPlayerColor;
        GameStateManager.Instance.SetState(GameState.Gameover);
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
        rb.velocity = new Vector2(0f, 0f);

        //Instanciar menu
    }
    public void Respawn() {
        OnEnable();
        Start();
        animator.SetBool("Dead", false);
        isDead = false;
        GetComponent<LifeCount>().Respawn();
        speed = 400f;
        move = 0;
    }

    private void OnLevelWasLoaded(int level)
    {
        Debug.Log("Entre a onLevelWasLoaded");
        Debug.Log("previous level : " + previousLevel + "level : " + level);
        FindStartPos(previousLevel, level);

        if(level == 3) {
            Destroy(GameObject.FindGameObjectWithTag("Player"));
            Destroy(GameObject.FindGameObjectWithTag("LivesUI"));
            GameObject [] games = GameObject.FindGameObjectsWithTag("GameController");
            if (games.Length > 1) {
                Destroy(games [1]);
            }
            GameObject [] sound = GameObject.FindGameObjectsWithTag("Sound");
            if (sound.Length > 1) {
                Destroy(sound [1]);
            }
            Destroy(GameObject.FindGameObjectWithTag("MainCamera"));
            
        }
        lives = GameObject.FindGameObjectsWithTag("LivesUI");

        if (lives.Length > 1)
            Destroy(lives [1]);

        players = GameObject.FindGameObjectsWithTag("Player");
        
        if (players.Length > 2)
            Destroy(players[2]);

        previousLevel = level;
    }

    void FindStartPos(int previousLevel, int level)
    {
        switch (level)
        {
            case 0:
                FindPositionRoom1(previousLevel);
                break;
            case 1:
                FindPositionRoom2(previousLevel);
                break;
            case 2:
                FindPositionRoom3(previousLevel);
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
        }
    }

    void FindPositionRoom1(int previousLevel)
    {
        switch (previousLevel)
        {
            case 1:
                transform.position = GameObject.FindWithTag("StartPos").transform.position;
                break;
            case 3:
                transform.position = GameObject.FindWithTag("PosFromMainMenuToRoom1").transform.position;
                break;
        }
    }

    void FindPositionRoom2(int previousLevel)
    {
        switch (previousLevel)
        {
            case 0:
                transform.position = GameObject.FindWithTag("PosFromRoom1ToRoom2").transform.position;
                break;
            case 2:
                transform.position = GameObject.FindWithTag("PosFromRoom3ToRoom2").transform.position;
                break;
        }
    }

    void FindPositionRoom3(int previousLevel)
    {
        switch (previousLevel)
        {
            case 1:
                transform.position = GameObject.FindWithTag("PosFromRoom2ToRoom3").transform.position;
                break;
        }
    }
}