using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SputnikController : MonoBehaviour
{
    // Editor variables
    public Transform CenterOfSputnik;
    public float speed;

    // Private variables
    private GameObject HealthBar;  
    private Rigidbody2D rb2D;   
    private float LifeTime;

    // Public variables  
    public float Torque;
    public Vector2 MovePosition;
     
    
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        LifeTime += Time.deltaTime;

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
    }
}
