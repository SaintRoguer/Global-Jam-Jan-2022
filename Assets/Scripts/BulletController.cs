using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Rigidbody2D bulletRb;
    private CircleCollider2D hitboxBullet;
    // Start is called before the first frame update
    void Start()
    {
        bulletRb = GetComponent<Rigidbody2D>();
        hitboxBullet = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag.Equals("Player")) {
            Destroy(collision.gameObject.gameObject);
            Debug.Log("Enemy destroy");
            //Cambiar por la vida del enemigo
        }
    }
}
