using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class EnemyAI : MonoBehaviour

{
    // Reference to waypoints.
    public List<Transform> points;
    //The integer for the next point index.
    public int nextID = 0;
    //The value of that applies to ID for changing.
    int idChangeValue = 1;
    //Speed of movement or flying.
    public float speed = 2f;
    //Life of the enemy
    public int lifePoints = 10;

    private void Reset()
    {
        Init();
    }

    void Init()
    {
        //Make box collider trigger.
        GetComponent<BoxCollider2D>().isTrigger = true;

        //Create Root object
        GameObject root = new GameObject(name + "_Root");

        //Reset position of Root to this object.
        root.transform.position = transform.position;
        //Set enemy object as child of root.
        transform.SetParent(root.transform);
        //Create waypoints object.
        GameObject waypoints = new GameObject("Waypoints");
        //Reset waypoints position to root.
        //Make waypoint object child of root.
        waypoints.transform.SetParent(root.transform);
        waypoints.transform.position = root.transform.position;
        //Create two points (gameobject) and reset position to waypoint objects.
        //Make the points children of waypoint object.
        GameObject point1 = new GameObject("Point1");
        point1.transform.SetParent(waypoints.transform);
        point1.transform.position = root.transform.position;

        GameObject point2 = new GameObject("Point2");
        point2.transform.SetParent(waypoints.transform);
        point2.transform.position = root.transform.position;

        //Init points list then add the points to it.
        points = new List<Transform>();
        points.Add(point1.transform);
        points.Add(point2.transform);
    }

    private void Update()
    {
        MoveToNextPoint();
    }

    void MoveToNextPoint()
    {
        //Get the next Point transform.
        Transform goalPoint = points[nextID];

        //Flip the enemy transform to look into the point's direction.
        if (goalPoint.transform.position.x < transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);

        //Move the enemy towards the goal point.
        transform.position = Vector2.MoveTowards(transform.position,goalPoint.position,speed*Time.deltaTime);

        //Check the distance between enemy and goal point to trigger next point.
        if(Vector2.Distance(transform.position, goalPoint.position) < 0.2f)
        {
            //Check if we are at the end of the line (make the change -1).
            if (nextID == points.Count - 1)
                idChangeValue = -1;
            //Check if we are at the start of the line (make the change +1).
            if (nextID == 0)
                idChangeValue = 1;
            //Apply the change to the nextID.
            nextID += idChangeValue;
        }
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("The bullet has caused " + damage + " damage to the enemy");
        lifePoints -= damage;
        Debug.Log("Enemy has " + lifePoints + " lifePoints remaining");
        if (lifePoints <= 0)
            Die();
    }

    void Die()
    {
        Destroy(transform.root.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log($"{name} colisiono");
            Debug.Log("Prueba consola");

            FindObjectOfType<LifeCount>().LoseLife();
        }
    }
}
