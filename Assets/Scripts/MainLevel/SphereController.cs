using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereController : MonoBehaviour
{
    public GameObject Zond;
    public GameObject Exp;
    public float PreviousHP;
    public float speed;

    public GameObject FirstPiece;
    public GameObject SecondPiece;
    public GameObject ThirdPiece;
    // Private variables
    private Rigidbody2D rb2D;
    private float LifeTime = 0;
    private Vector2 force;

    // Public variables
    public float Torque;
    public bool Active = false;

    void FixedUpdate()
    {
        if (Active)
        {
            LifeTime += Time.deltaTime;

            //
            // object forcing
            //
            if (LifeTime < 0.5)
            {
                rb2D.velocity = force * 2f * speed;
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
            // Teleporting
            //
            if (!GetComponent<Renderer>().isVisible)
                Destroy(gameObject);
            //
            // Teleporting
            //
        }
        if (PreviousHP <= 0 && !Active)
            ActivateObject(0.5f);
    }
    public void ActivateObject(float Newspeed)
    {
        if (!GetComponent<Rigidbody2D>())
        {
            rb2D = gameObject.AddComponent<Rigidbody2D>();
            rb2D.gravityScale = 0;
            rb2D.angularDrag = 0.5f;
            rb2D.drag = 0.5f;
            rb2D.interpolation = RigidbodyInterpolation2D.Interpolate;
        }

        GetComponent<CircleCollider2D>().enabled = enabled;

        Vector2 ZondForce = Zond.GetComponent<Rigidbody2D>().velocity;
        Vector2 ForcePosition = transform.position - Zond.transform.position;
        force = (ZondForce + ForcePosition).normalized;
        speed = Newspeed;
        Active = true;
        Torque = Random.Range(-0.5f, 0.5f);

        GetComponent<HealthPointsController>().enabled = enabled;

        transform.SetParent(null);      
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Active)
        {
            if (collision.gameObject.tag == "Bullet")
            {
                PreviousHP -= collision.gameObject.GetComponent<Bullet>().Damage;
            }
            else if (collision.gameObject.tag == "Rocket")
            {
                PreviousHP -= collision.gameObject.GetComponent<RocketController>().Damage;
            }
        }
    }
}
