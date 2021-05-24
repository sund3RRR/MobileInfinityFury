using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPieceAfterDead : MonoBehaviour
{
    // Editor variables
    public int HealthPoints;
    public int BaseHealthPoints;
    public GameObject Bar;
    public GameObject Boss;
    public float speed;

    // Private variables
    private GameObject HealthBar;
    private Rigidbody2D rb2D;
    private float LifeTime = 0;
    private Vector2 force;

    // Public variables
    public float hitTime = 2.5f;
    public float Torque;
    public bool Active = false;

    void FixedUpdate()
    {
        if (Active)
        {
            LifeTime += Time.deltaTime;
            hitTime += Time.deltaTime;

            //
            // HealthBar changes
            //
            if (HealthBar && hitTime > 2.5f)
            {
                HealthBar.GetComponent<HealthBarController>().DisableHealthBar();
            }
            else if (HealthBar)
            {
                HealthBar.GetComponent<HealthBarController>().HealthPoints = HealthPoints;
                HealthBar.GetComponent<HealthBarController>().EnableHealthBar();
            }
            //
            // HealthBar changes
            //
            //
            // object forcing
            //

            if (LifeTime < 0.5)
            {
                rb2D.velocity = force * speed;
                rb2D.angularVelocity = Torque * 30;
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
            // Destroying
            //
            if (HealthPoints <= 0)
            {
                DestroyController.DestroyDefault(gameObject);
            }
            //
            // Destroying
            //
            //
            // Teleporting
            //
            if (!gameObject.GetComponent<Renderer>().isVisible)
                GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController>().TeleportObject(gameObject);
            //
            // Teleporting
            //
        }
    }
    public void ActivateObject()
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
        GetComponent<EdgeCollider2D>().enabled = enabled;      

        force = new Vector2(Random.Range(-0.6f, 0.6f), -1f) * speed;
        Active = true;
        Torque = Mathf.Pow(-1, Random.Range(1, 3)) * Random.Range(0.4f, 0.8f);

        //
        // HealthBar INIT
        //
        GameObject Canvas = GameObject.FindGameObjectWithTag("Canvas");
        Bar.GetComponent<HealthBarController>().Target = gameObject;
        Bar.GetComponent<HealthBarController>().BaseHealthPoints = BaseHealthPoints;
        HealthBar = Instantiate(Bar, new Vector3(transform.position.x, transform.position.y), Quaternion.identity);
        HealthBar.transform.SetParent(Canvas.transform, false);
        //
        // HealthBar INIT
        //

        transform.SetParent(null);
    }
}
