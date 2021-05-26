using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    // Editor variables
    public GameObject Asteroid;
    public Sprite Asteroid_01;
    public Sprite Asteroid_02;
    public Sprite Asteroid_03;
    public GameObject DeadVFX;
    // Private variables
    private Rigidbody2D rb2D;
    private float LifeTime = 0;
    private bool ChangedSprite1 = false;
    private bool ChangedSprite2 = false;
    private bool IsVisibled = false;
    // Public variables
    public float Torque;
    public Vector2 force;
    public int indexScale;
    private int HealthPoints;
    private int BaseHealthPoints;

    void Start()
    {
        HealthPoints = GetComponent<HealthPointsController>().HealthPoints;
        BaseHealthPoints = HealthPoints;
        GetComponent<SpriteRenderer>().sprite = Asteroid_01;      
        rb2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        LifeTime += Time.deltaTime;

        //
        // object forcing
        //
        if (LifeTime < 0.5f)
        {
            rb2D.velocity = force;
            rb2D.AddTorque(Torque);
        }
        else
        {
            rb2D.AddForce(force);
            rb2D.AddTorque(Torque);
        }
        //
        // object forcing
        //

        //
        // sprite changing
        //
        if (((float)HealthPoints / (float)BaseHealthPoints <= 0.6f) && ((float)HealthPoints / (float)BaseHealthPoints >= 0.4f) && !ChangedSprite1)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = Asteroid_02;
            ChangedSprite1 = true;
        }

        if (Asteroid_03 && HealthPoints == 1 && !ChangedSprite2)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = Asteroid_03;
            ChangedSprite2 = true;
        }
        //
        // sprite changing
        //

        //
        // Teleporting
        //
        if (gameObject.GetComponent<Renderer>().isVisible)
            IsVisibled = true;
        if (!gameObject.GetComponent<Renderer>().isVisible && IsVisibled)
            Destroy(gameObject);
        //
        // Teleporting
        //
    }
    public GameObject GetDeadVFX()
    {
        return DeadVFX;
    }
}