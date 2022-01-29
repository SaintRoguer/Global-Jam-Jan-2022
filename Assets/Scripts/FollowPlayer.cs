using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset;
    [Range(1, 10)]
    public float smoothFactor;
    private GameObject[] camera;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Follow();
    }

    void Follow() {
        if (player != null) { 
            Vector3 playerPosition = player.transform.position + offset;
            Vector3 smoothPosition = Vector3.Lerp(player.transform.position, playerPosition, smoothFactor * Time.fixedDeltaTime);
            transform.position = smoothPosition;
        }
    }

    private void OnLevelWasLoaded(int level)
    {

        camera = GameObject.FindGameObjectsWithTag("MainCamera");

        if (camera.Length > 1)
            Destroy(camera[1]);
    }
}
