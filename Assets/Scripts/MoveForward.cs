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
        direction = 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector3.right * speed * direction * Time.deltaTime ); 
    }

    public void SetDirection(float d) {
        direction = d;
    }
    public void SetSpeed(float s) {
        speed = s;
    }
}
