using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SputnikController : MonoBehaviour
{
    // Editor variables
    public GameObject Bar;
    public Transform CenterOfSputnik;
    public float BaseHealthPoints;
    public float HealthPoints;
    public float speed;

    // Private variables
    private GameObject HealthBar;  
    private Rigidbody2D rb2D;   
    private float LifeTime;

    // Public variables
    public float hitTime = 2.5f;
    
    public float Torque;
    public Vector2 MovePosition;
     
    
    void Start()
    {
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
            GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController>().TeleportObject(gameObject);

        if (LifeTime < 0.5f)
        {
            rb2D.velocity = MovePosition * speed;
            rb2D.AddTorque(Torque);
        }
        else
        {
            rb2D.AddForce(MovePosition * speed);
            rb2D.AddTorque(Torque);
        }

        if (HealthPoints <= 0)
            DestroyController.DestroySputnik(gameObject);
    }
}
