using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Rigidbody2D bulletRb;
    private CircleCollider2D hitboxBullet;

    public int damage = 10;
    // Start is called before the first frame update
    void Start()
    {
        bulletRb = GetComponent<Rigidbody2D>();
        hitboxBullet = GetComponent<CircleCollider2D>();
        //GetComponent<CircleCollider2D>().isTrigger = true;
        bulletRb.transform.position = transform.position;
        hitboxBullet.transform.position = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Enemy") {
            Debug.Log("Enemy destroy");
            //Este es el que use que destruye desde el root o sea el mas alto de la jerarquia.
            //Destroy(transform.root.gameObject);
            collision.GetComponent<EnemyAI>().TakeDamage(damage);
        }
    }
}
