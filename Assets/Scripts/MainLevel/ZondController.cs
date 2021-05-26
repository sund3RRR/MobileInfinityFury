using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZondController : MonoBehaviour
{
    // Private variables
    private Rigidbody2D rb2D;
    private float LifeTime = 0;
    private bool IsVisibled = false;
    // Public variables
    public Vector2 force;
    public float Torque; 
    public float speed;

    public GameObject Sphere;
    public GameObject leftPanel;
    public GameObject rightPanel;

    void Start()
    {
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
            rb2D.velocity = force * speed;
            rb2D.AddTorque(Torque);
        }
        else
        {
            rb2D.AddForce(force * speed);
            rb2D.AddTorque(Torque);
        }
        //
        // object forcing
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
}