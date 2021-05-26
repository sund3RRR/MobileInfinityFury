using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SceneController;

public class RocketController : MonoBehaviour
{
    // Editor variables
    public float speed;
    public GameObject AsteroidHit;
    public GameObject DefaultHit;
    public int damage;
    public float DestroyTime;

    // Private variables
    private SceneController CurrentSceneController;
    private System.Random rand = new System.Random();
    private GameObject[] EnemyArray;
    private GameObject NearestEnemy;
    private Rigidbody2D rb2D;
    public GameObject Ship;
    private Vector2 UnSightPosition;
    private Vector3 MovePosition;
    private Vector3 difference;
    private Vector2 force;
    private float XCoordinate;
    private float YCoordinate;
    private float LifeTime = 0;
    private float distance;
    private float CurrentDistance;
    private GameObject closest;

    // Public variables
    public Vector2 SightPosition;
    public bool AimBot = false;  
      
    void Start()
    {      
        rb2D = GetComponent<Rigidbody2D>();
        CurrentSceneController = GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController>();
        if (SightPosition.sqrMagnitude == 0)
            SightPosition = new Vector2(1.0f, 0);
        UnSightPosition = -SightPosition;
        XCoordinate = UnSightPosition.normalized.x + (float)rand.NextDouble() * 6f - 3;
        YCoordinate = UnSightPosition.normalized.y + (float)rand.NextDouble() * 6f - 3;

        transform.rotation = Ship.transform.rotation;

        rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        
        Destroy(gameObject, DestroyTime);
    }
    void FixedUpdate()
    {
        LifeTime += Time.deltaTime;
        if (LifeTime < 0.15f) // движение ракеты чуть в сторону в начале спавна
            rb2D.velocity = new Vector2(XCoordinate, YCoordinate);
        else if (LifeTime > 0.15f && LifeTime < 0.16f) // настройка скорости для плавного перехода от velocity к AddForce
            rb2D.velocity = (SightPosition.normalized);
        else if (LifeTime < 0.4f) // придание силы к ракете
            rb2D.AddForce(SightPosition.normalized * speed);
        /*
        else if (AimBot)
        {
            EnemyArray = SceneController.EnemyArray;
            NearestEnemy = FindClosestEnemy();
            if (NearestEnemy)
            {
                MovePosition = NearestEnemy.transform.position - gameObject.transform.position;
                force = MovePosition.normalized * speed;
                rb2D.AddForce(force);

                float rotate = Mathf.Atan2(rb2D.velocity.y, rb2D.velocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(rotate, Vector3.forward);
            }
            else
            {
                force = rb2D.velocity;
                rb2D.AddForce(force);
            }

        }
        */
        else
        {
            rb2D.AddForce(SightPosition.normalized * speed);
        }
    }
    private GameObject FindClosestEnemy()
    {
        distance = Mathf.Infinity;
        foreach (GameObject Enemy in EnemyArray)
        {
            difference = Enemy.transform.position - gameObject.transform.position;
            CurrentDistance = difference.sqrMagnitude;
            float Angle = Vector2.SignedAngle(rb2D.velocity, difference);

            if (CurrentDistance < distance && Angle > -45 && Angle < 45)
            {
                closest = Enemy;
                distance = CurrentDistance;
            }
        }
        return closest;
    }
}