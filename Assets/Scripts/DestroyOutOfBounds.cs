using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    //Lo ideal para esto seria tenes limites iguales para todos los niveles
    private float leftLimit = -50;
    private float rightLimit = 300;
    private float toplimit = 200;
    private float bottomLimit = -5;

    // Update is called once per frame
    void Update() {
        // Destroy dogs if x position less than left limit
        if (transform.position.x < leftLimit || transform.position.x > rightLimit) {
            Destroy(gameObject);
        }
        // Destroy balls if y position is less than bottomLimit
        else if (transform.position.y < bottomLimit || transform.position.y > toplimit) {
            Destroy(gameObject);
        }

    }
}
