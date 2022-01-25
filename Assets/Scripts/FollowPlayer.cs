using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset;
    [Range(1, 10)]
    public float smoothFactor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Follow();
    }

    void Follow() {
        Vector3 playerPosition = player.transform.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(player.transform.position, playerPosition, smoothFactor * Time.fixedDeltaTime);
        transform.position = smoothPosition;
    }
}
