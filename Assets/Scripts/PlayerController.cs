using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
public class PlayerController : MonoBehaviour {
    public Rigidbody2D rb;
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
    float coolTimeShoot = 0.4f;
    float nextShootTime;
    //Dash related
    [SerializeField] private float dashDistance;
    [SerializeField]private float dashTime;
    private bool isDashing;
    float coolTimeDash = 2;
    float nextDashTime;

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
    private string respawnPoint = "PosFromMainMenuToRoom1";

    private void Awake() {
        playerControls ??= new Controls();
        animator = GetComponent<Animator>();
        animator.SetFloat("Yellow", 1);
        animator.SetFloat("YellowGun", 1);
        lastDirection = 1;
        colourSystem = GetComponent<ColourSystem>();
    }

    internal bool IsDead() {
        return isDead;
    }

    private void OnEnable() {
        if (playerControls == null)
            playerControls = new Controls();
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
        SoundManagerScript.PlaySound("alive");
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
        totalJumps = 0;
        availableJumps = totalJumps;
        dashDistance = 4f;
        dashTime = 0.25f;
        isDashing = true;
    }

    private void FixedUpdate() {
        if (!isDashing || GetComponent<ColourSystem>().mainState != MainColours.RED) {
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
        enabled = !(gm == GameState.Pause);
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
        if (move != 0)
            SoundManagerScript.PlaySound("move");
        else
            SoundManagerScript.StopSFX();
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
        if (!isDashing) {
            if (Time.time > nextDashTime) {
                StartCoroutine(Dash(lastDirection));
                nextDashTime = Time.time + coolTimeDash;
            }
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
        if (Time.time > nextShootTime) {
            SoundManagerScript.PlaySound("shoot");
            actualBullet = Instantiate(bulletPrefab, transform.position + new Vector3(lastDirection * 1, -0.3f, 0), bulletPrefab.transform.rotation);
            CombinationState color = GetComponent<ColourSystem>().combinationState;
            actualBullet.GetComponent<BulletController>().SetPlayer(gameObject);
            actualBullet.GetComponent<MoveForward>().SetDirection(lastDirection);
            if (lastDirection < 0)
                actualBullet.GetComponent<SpriteRenderer>().flipX = false;
            else
                actualBullet.GetComponent<SpriteRenderer>().flipX = true;
            animator.SetBool("Shoot", true);
            nextShootTime = Time.time + coolTimeShoot;
        }
    }
    public void SwitchGunColor(InputAction.CallbackContext context) {
        MainColours colourSelected = colourSystem.ChangeSecondaryColour();
        switch (colourSelected)
        {
            case MainColours.YELLOW:
                ChangeGunToYellow();
                break;
            case MainColours.BLUE:
                ChangeGunToBlue();
                break;
            case MainColours.RED:
                ChangeGunToRed();
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
        //Cambiar daño de la bala
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
                availableJumps = 0;
                totalJumps = 0;
                isDashing = true;
                break;
            case MainColours.BLUE:
                ChangePlayerToBlue();
                totalJumps +=1;
                isDashing = true;
                break;
            case MainColours.RED:
                ChangePlayerToRed();
                availableJumps = 0;
                totalJumps = 0;
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
        //Cambiar daño de la bala
    }

    private void ChangePlayerToBlue()
    {
        animator.SetFloat("Yellow", 0);
        animator.SetFloat("Red", 0);
        animator.SetFloat("Blue", 1);
        //Cambiar daño de la bala

    }

    private void ChangePlayerToRed()
    {
        animator.SetFloat("Yellow", 0);
        animator.SetFloat("Red", 1);
        animator.SetFloat("Blue", 0);
        //Cambiar daño de la bala

    }

    void GroundCheck() {
        bool wasOnGround = isOnGround;
        isOnGround = false;

        Collider2D [] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundRadius, groundLayer);
        if (colliders.Length > 0) {
            isOnGround = true;
            availableJumps = totalJumps;
            
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
        SoundManagerScript.PlaySound("death");
        SoundManagerScript.StopSFX();

        //Instanciar menu
    }
    public void Respawn() {
        OnEnable();
        Start();
        GameStateManager.Instance.SetState(GameState.Gameplay);

        animator.SetBool("Dead", false);
        isDead = false;
        GetComponent<LifeCount>().Respawn();
        speed = 400f;
        move = 0;

        SoundManagerScript.PlaySound("alive");
        transform.position = GameObject.FindWithTag(respawnPoint).transform.position;
    }

    private void OnLevelWasLoaded(int level)
    {
        FindStartPos(previousLevel, level);

        if (level == 0) {
            GameObject [] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject g in players) {
                Destroy(g);
            }
            Destroy(GameObject.FindGameObjectWithTag("LivesUI"));
            GameObject [] games = GameObject.FindGameObjectsWithTag("GameController");
            if (games.Length > 1) {
                Destroy(games [1]);
            }
            GameObject [] sound = GameObject.FindGameObjectsWithTag("Sound");
            if (sound.Length > 1) {
                Destroy(sound [0]);
            }
            Destroy(GameObject.FindGameObjectWithTag("MainCamera"));

        }
        else {
            lives = GameObject.FindGameObjectsWithTag("LivesUI");

            if (lives.Length > 1)
                Destroy(lives [0]);

            players = GameObject.FindGameObjectsWithTag("Player");
            int cant = players.Count(A => A.transform.parent == null);
            if (cant >= 2) {
                for (int i = 2; i < players.Length; i++) {

                    if (players [i] != null && players [i].transform.parent == null) {
                        Destroy(players [i]);

                        break;
                    }

                }
            }
        }
            

        previousLevel = level;
    }

    void FindStartPos(int previousLevel, int level)
    {
        switch (level)
        {
            case 1:
                FindPositionRoom1(previousLevel);
                break;
            case 2:
                FindPositionRoom2(previousLevel);
                break;
            case 3:
                FindPositionRoom3(previousLevel);
                break;
            case 4:
                FindPositionRoom4(previousLevel);
                break;
            case 5:
                FindPositionRoom5();
                break;
            case 6:
                FindPositionRoom6();
                break;
            case 7:
                FindPositionRoom7();
                break;
        }
    }

    void FindPositionRoom1(int previousLevel)
    {
        switch (previousLevel)
        {
            case 2:
                transform.position = GameObject.FindWithTag("StartPos").transform.position;
                respawnPoint = "StartPos";
                break;
            case 0:
                transform.position = GameObject.FindWithTag("PosFromMainMenuToRoom1").transform.position;
                respawnPoint = "PosFromMainMenuToRoom1";
                break;
        }
    }

    void FindPositionRoom2(int previousLevel)
    {
        switch (previousLevel)
        {
            case 1:
                transform.position = GameObject.FindWithTag("PosFromRoom1ToRoom2").transform.position;
                respawnPoint = "PosFromRoom1ToRoom2";
                break;
            case 3:
                transform.position = GameObject.FindWithTag("PosFromRoom3ToRoom2").transform.position;
                respawnPoint = "PosFromRoom3ToRoom2";
                break;
        }
    }

    void FindPositionRoom3(int previousLevel)
    {
        switch (previousLevel)
        {
            case 2:
                transform.position = GameObject.FindWithTag("PosFromRoom2ToRoom3").transform.position;
                respawnPoint = "PosFromRoom2ToRoom3";
                break;
            case 4:
                transform.position = GameObject.FindWithTag("PosFromRoom4ToRoom3").transform.position;
                respawnPoint = "PosFromRoom4ToRoom3";
                break;
            case 7:
                transform.position = GameObject.FindWithTag("PosFromRoom7ToRoom3").transform.position;
                respawnPoint = "PosFromRoom7ToRoom3";
                break;
        }
    }

    void FindPositionRoom4(int previousLevel)
    {
        switch (previousLevel)
        {
            case 3:
                transform.position = GameObject.FindWithTag("PosFromRoom3ToRoom4").transform.position;
                respawnPoint = "PosFromRoom3ToRoom4";
                break;
            case 6:
                transform.position = GameObject.FindWithTag("PosFromRoom6ToRoom4").transform.position;
                respawnPoint = "PosFromRoom6ToRoom4";
                break;
            case 5:
                transform.position = GameObject.FindWithTag("PosFromRoom5ToRoom4").transform.position;
                respawnPoint = "PosFromRoom5ToRoom4";
                break;
        }
    }

    void FindPositionRoom5()
    {
        transform.position = GameObject.FindWithTag("PosFromRoom4ToRoom5").transform.position;
        respawnPoint = "PosFromRoom4ToRoom5";
    }

    void FindPositionRoom6()
    {
        transform.position = GameObject.FindWithTag("PosFromRoom4ToRoom6").transform.position;
        respawnPoint = "PosFromRoom4ToRoom6";
    }

    void FindPositionRoom7()
    {
        transform.position = GameObject.FindWithTag("PosFromRoom3ToRoom7").transform.position;
        respawnPoint = "PosFromRoom3ToRoom7";
    }

}