using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField]
    [Range(1, 10)]
    private int damage;
    private SpriteRenderer spriteRenderer;
    private CombinationState combinationState;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        //bulletRb = GetComponent<Rigidbody2D>();
        //hitboxBullet = GetComponent<CircleCollider2D>();
        damage = 10;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        combinationState = player.GetComponent<ColourSystem>().combinationState;
        switch (combinationState) {
            case CombinationState.BLUE:
                animator.SetFloat("Blue", 1f);
                animator.SetFloat("Red", 0f);
                animator.SetFloat("Yellow", 0f);
                animator.SetFloat("Green", 0f);
                animator.SetFloat("Orange", 0f);
                animator.SetFloat("Violet", 0f);
                break;
            case CombinationState.YELLOW:
                animator.SetFloat("Blue", 0f);
                animator.SetFloat("Red", 0f);
                animator.SetFloat("Yellow", 1f);
                animator.SetFloat("Green", 0f);
                animator.SetFloat("Orange", 0f);
                animator.SetFloat("Violet", 0f);
                break;
            case CombinationState.RED:
                animator.SetFloat("Blue", 0f);
                animator.SetFloat("Red", 1f);
                animator.SetFloat("Yellow", 0f);
                animator.SetFloat("Green", 0f);
                animator.SetFloat("Orange", 0f);
                animator.SetFloat("Violet", 0f);
                break;
            case CombinationState.VIOLET:
                animator.SetFloat("Blue", 0f);
                animator.SetFloat("Red", 0f);
                animator.SetFloat("Yellow", 0f);
                animator.SetFloat("Green", 0f);
                animator.SetFloat("Orange", 0f);
                animator.SetFloat("Violet", 1f);
                break;
            case CombinationState.GREEN:
                animator.SetFloat("Blue", 0f);
                animator.SetFloat("Red", 0f);
                animator.SetFloat("Yellow", 0f);
                animator.SetFloat("Green", 1f);
                animator.SetFloat("Orange", 0f);
                animator.SetFloat("Violet", 0f);
                break;
            case CombinationState.ORANGE:
                animator.SetFloat("Blue", 0f);
                animator.SetFloat("Red", 0f);
                animator.SetFloat("Yellow", 0f);
                animator.SetFloat("Green", 0f);
                animator.SetFloat("Orange", 1f);
                animator.SetFloat("Violet", 0f);
                break;

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetDamage(int d) {
        damage = d;
    }
    public int GetDamage() {
        return damage;
    }

    public void SetPlayer(GameObject play) {
        player = play;
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Enemy")) {
            Debug.Log("Enemy destroy");
            //Este es el que use que destruye desde el root o sea el mas alto de la jerarquia.
            collision.GetComponent<EnemyAI>().TakeDamage(damage);
            player.GetComponent<Animator>().SetBool("Shoot", false);
            Destroy(gameObject);


        }
        if (collision.CompareTag("Door")) {
            Debug.Log("Door Hit");
            bool isOpen = collision.GetComponent<DoorController>().Opposite(player.GetComponent<ColourSystem>().combinationState);
            if (isOpen)
                collision.GetComponent<DoorController>().Open();
            //Open dictates if the colour is correct and opens the door.
            player.GetComponent<Animator>().SetBool("Shoot", false);
            Destroy(gameObject);


        }
        if (collision.CompareTag("Ground")) {
            Debug.Log("Ground hit");
            player.GetComponent<Animator>().SetBool("Shoot", false);
            Destroy(gameObject);
        }
               
    }
}
