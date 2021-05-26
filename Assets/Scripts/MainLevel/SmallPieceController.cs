using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallPieceController : MonoBehaviour
{
    // Private variables
    private Rigidbody2D rb2D;
    private float LifeTime = 0;

    // Public variables
    public Vector2 ParentForce;
    public Vector2 force; 
    public float Torque;  

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        Vector2 ForcePosition = ParentForce.normalized;
        force = (ForcePosition + force).normalized;
    }

    void FixedUpdate()
    {
        LifeTime += Time.deltaTime;

        //
        // object forcing
        //
        if (LifeTime < 0.5f)
        {
            rb2D.velocity = force * 2f;
            rb2D.AddTorque(Torque);
        }
        else
        {
            rb2D.AddForce(force * 1f);
            rb2D.AddTorque(Torque);
        }
        //
        // object forcing
        //

        //
        // Teleporting
        //
        if (!gameObject.GetComponent<Renderer>().isVisible)
            Destroy(gameObject);
        //
        // Teleporting
        //
    }
}
