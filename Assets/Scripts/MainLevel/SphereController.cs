using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereController : MonoBehaviour
{
    public GameObject Zond;
    public GameObject Exp;
    public GameObject Bar;
    public int PreviousHP;
    public int HealthPoints;
    public int BaseHealthPoints;
    public float speed;

    public GameObject FirstPiece;
    public GameObject SecondPiece;
    public GameObject ThirdPiece;
    // Private variables
    private GameObject HealthBar;
    private Rigidbody2D rb2D;
    private float LifeTime = 0;
    private Vector2 force;

    // Public variables
    public float Torque;
    public bool Active = false;
    public float hitTime = 2.5f;

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
            // Destroying
            //
            if (HealthPoints <= 0)
            {
                DestroyController.DestroySphere(gameObject);
            }
            //
            // Destroying
            //
            //
            // Teleporting
            //
            if (!GetComponent<Renderer>().isVisible)
                GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController>().TeleportObject(gameObject);
            //
            // Teleporting
            //
        }
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
        GetComponent<SpriteRenderer>().enabled = enabled;

        Vector2 ZondForce = Zond.GetComponent<Rigidbody2D>().velocity;
        Vector2 ForcePosition = transform.position - Zond.transform.position;
        force = (ZondForce + ForcePosition).normalized;
        speed = Newspeed;
        Active = true;
        Torque = Random.Range(-0.5f, 0.5f);
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Active)
        {
            if (collision.gameObject.tag == "Bullet")
            {
                PreviousHP -= collision.gameObject.GetComponent<Bullet>().damage;
                Zond.GetComponent<ZondController>().HealthPoints -= collision.gameObject.GetComponent<Bullet>().damage;
                Zond.GetComponent<ZondController>().hitTime = 0;
            }
            else if (collision.gameObject.tag == "Rocket")
            {
                PreviousHP -= collision.gameObject.GetComponent<RocketController>().damage;
                Zond.GetComponent<ZondController>().HealthPoints -= collision.gameObject.GetComponent<RocketController>().damage;
                Zond.GetComponent<ZondController>().hitTime = 0;
            }
        }
    }
}
