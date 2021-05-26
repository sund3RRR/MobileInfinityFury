using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldAsteroidController : MonoBehaviour
{
    // Editor variables
    public GameObject Asteroid;
    public GameObject Bar;
    public Sprite Asteroid_01;
    public Sprite Asteroid_02;
    public Sprite Asteroid_03;

    // Private variables
    private GameObject Canvas;
    private GameObject HealthBar;
    private Rigidbody2D rb2D;
    private float LifeTime = 0;
    private bool ChangedSprite1 = false;
    private bool ChangedSprite2 = false;

    // Public variables
    public float Torque;
    public Vector2 force;
    public int HealthPoints;
    public int BaseHealthPoints;
    public float hitTime = 2.5f;

    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = Asteroid_01;
        BaseHealthPoints = HealthPoints;
        rb2D = GetComponent<Rigidbody2D>();

        GameObject Canvas = GameObject.FindGameObjectWithTag("Canvas");
        Bar.GetComponent<HealthBarController>().Target = gameObject;
        Bar.GetComponent<HealthBarController>().BaseHealthPoints = BaseHealthPoints;
        HealthBar = Instantiate(Bar, new Vector3(transform.position.x, transform.position.y), Quaternion.identity);
        HealthBar.transform.SetParent(Canvas.transform, false);
    }

    void FixedUpdate()
    {
        LifeTime += Time.deltaTime;
        hitTime += Time.deltaTime;

        if (HealthBar && hitTime > 2.5f)
        {
            HealthBar.GetComponent<HealthBarController>().DisableHealthBar();
        }
        else if (HealthBar)
        {
            HealthBar.GetComponent<HealthBarController>().HealthPoints = HealthPoints;
            HealthBar.GetComponent<HealthBarController>().EnableHealthBar();
        }

        if (!gameObject.GetComponent<Renderer>().isVisible)
            Destroy(gameObject);

        if (LifeTime < 0.5f) // Проверка жизни астероида, чтобы придать ему движение через velocity или AddForce в зависимости от времени
        {
            rb2D.velocity = force;
            rb2D.AddTorque(Torque);
        }
        else
        {
            rb2D.AddForce(force);
            rb2D.AddTorque(Torque);
        }

        if (HealthPoints <= 100 && HealthPoints >= 90 && !ChangedSprite1)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = Asteroid_02;
            ChangedSprite1 = true;
        }

        if (HealthPoints <= 20 && !ChangedSprite2)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = Asteroid_03;
            ChangedSprite2 = true;
        }

        if (HealthPoints <= 0) // Проверка ХП астероида
            DestroyController.DestroyGoldAsteroid(gameObject);
    }
}