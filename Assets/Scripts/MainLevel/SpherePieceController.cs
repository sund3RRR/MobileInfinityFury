using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpherePieceController : MonoBehaviour
{
    // Editor variables
    public GameObject Sphere;
    public float speed;

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
    public void ActivateObject(int switcher)
    {
        if (!GetComponent<Rigidbody2D>())
        {
            rb2D = gameObject.AddComponent<Rigidbody2D>();
            rb2D.gravityScale = 0;
            rb2D.angularDrag = 0.5f;
            rb2D.drag = 0.5f;
            rb2D.interpolation = RigidbodyInterpolation2D.Interpolate;
        }

        GetComponent<CapsuleCollider2D>().enabled = enabled;
        GetComponent<SpriteRenderer>().enabled = enabled;
        switch(switcher)
        {
            case 1:
                force = (transform.position - Sphere.transform.position).normalized * speed;
                break;
            case 2:
                force = Mathf.Pow(-1, Random.Range(1, 3)) * Sphere.transform.right * speed;
                break;
            case 3:
                force = (transform.position - Sphere.transform.position).normalized * speed;
                break;
            default:
                break;
        }
        //force = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * speed;
        //force = (transform.position - Sphere.transform.position).normalized * speed;
        Active = true;
        Torque = Random.Range(-0.5f, 0.5f);

        transform.SetParent(null);
    }
}
