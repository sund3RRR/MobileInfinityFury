using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigPieceController : MonoBehaviour
{
    // Editor variables
    public GameObject SmallPiece;
    public GameObject Bar;
    public int BaseHealthPoints;
    public int HealthPoints;
    public float speed;

    // Private variables
    private GameObject HealthBar;
    private Rigidbody2D rb2D;
    private float LifeTime = 0;

    // Public variables
    public Vector2 ParentForce; 
    public Vector2 force;
    public float Torque;
    public float hitTime = 2.5f;  

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        Vector2 ForcePosition = ParentForce.normalized;
        force = (ForcePosition + force).normalized;

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
    }

    void FixedUpdate()
    {
        hitTime += Time.deltaTime;
        LifeTime += Time.deltaTime;

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
        if (LifeTime < 0.5f)
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
            DestroyController.DestroyBigPiece(gameObject);
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
