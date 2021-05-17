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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            GameObject NewHit = DefaultHit;

            switch (collision.gameObject.name)
            {
                case "Zond(Clone)":
                    collision.gameObject.GetComponent<ZondController>().HealthPoints -= damage;
                    collision.gameObject.GetComponent<ZondController>().hitTime = 0;
                    break;
                case "Sphere":
                    if (collision.gameObject.GetComponent<SphereController>().Active)
                    {
                        collision.GetComponent<SphereController>().HealthPoints -= damage;
                        collision.GetComponent<SphereController>().hitTime = 0;
                    }
                    break;
                case "Panel":
                    if (collision.gameObject.GetComponent<PanelController>().Active)
                    {
                        collision.gameObject.GetComponent<PanelController>().HealthPoints -= damage;
                        collision.gameObject.GetComponent<PanelController>().hitTime = 0;
                    }
                    break;
                case "BigPiece(Clone)":
                    collision.gameObject.GetComponent<BigPieceController>().HealthPoints -= damage;
                    collision.gameObject.GetComponent<BigPieceController>().hitTime = 0;
                    break;
                case "Sputnik(Clone)":
                    collision.gameObject.GetComponent<SputnikController>().HealthPoints -= damage;
                    collision.gameObject.GetComponent<SputnikController>().hitTime = 0;
                    break;
                case "SmallPiece(Clone)":
                    DestroyController.DestroyDefault(collision.gameObject);
                    break;
                case "GoldAsteroid(Clone)":
                    collision.gameObject.GetComponent<GoldAsteroidController>().HealthPoints -= damage;
                    collision.gameObject.GetComponent<GoldAsteroidController>().hitTime = 0;
                    break;
                case "FatStarshipEnemy(Clone)":
                    collision.GetComponent<FatStarshipEnemy>().HealthPoints -= damage;
                    collision.GetComponent<FatStarshipEnemy>().hitTime = 0;
                    break;
                case "DestroyerEnemyStarship(Clone)":
                    collision.GetComponent<DestroyerEnemyController>().HealthPoints -= damage;
                    collision.GetComponent<DestroyerEnemyController>().hitTime = 0;
                    break;
                case "SlimStarshipEnemy(Clone)":
                    collision.GetComponent<SlimEnemyController>().HealthPoints -= damage;
                    collision.GetComponent<SlimEnemyController>().hitTime = 0;
                    break;
                case "FirstBoss(Clone)":
                    collision.GetComponent<BossFirst>().HealthPoints -= damage;
                    collision.GetComponent<BossFirst>().hitTime = 0;
                    break;
                default:
                    if (collision.gameObject.GetComponent<AsteroidController>())
                    {
                        collision.gameObject.GetComponent<AsteroidController>().HealthPoints -= damage;
                        collision.gameObject.GetComponent<AsteroidController>().hitTime = 0;
                        NewHit = AsteroidHit;
                    }
                    break;
            }
            GameObject InstanceHit = Instantiate(NewHit, transform.position, Quaternion.identity);
            Destroy(InstanceHit, 1);
            Destroy(gameObject);
        }
    }
}