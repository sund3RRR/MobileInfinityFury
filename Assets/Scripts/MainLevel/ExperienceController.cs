using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ExperienceController : MonoBehaviour
{
    public GameObject Experience;
    private GameObject ShipBlue;
    private GameObject ShipGreen;
    private GameObject Target;

    // Start function
    private List<int> PlayerLevels = new List<int> { 5, 25, 40, 80, 150, 300, 450, 600 };

    private Rigidbody2D rb2D;
    public float XPoseToForce, YPoseToForce;
    public float speed;
    private Vector2 force;

    // ExperienceGravityToShip function
    private Vector2 MovePosition;
  
    private void Start()
    {
        ShipBlue = GameObject.FindGameObjectWithTag("PlayerBlue");
        ShipGreen = GameObject.FindGameObjectWithTag("PlayerGreen");
        rb2D = GetComponent<Rigidbody2D>();
        XPoseToForce = Random.Range(-2f, 2f);
        YPoseToForce = -4.5f;
        speed = Random.Range(2, 4);
        force = new Vector2(XPoseToForce, YPoseToForce).normalized;
        force *= speed;

        Destroy(gameObject, 15);
    }
    void FixedUpdate()
    {
        if (ShipBlue && ShipGreen)
        {
            if((ShipBlue.transform.position - transform.position).sqrMagnitude < (ShipGreen.transform.position - transform.position).sqrMagnitude)
            {
                Target = ShipBlue;
            }
            else
            {
                Target = ShipGreen;
            }
            ExperienceGravityToShip();
        }
        else if (ShipBlue)
        {
            Target = ShipBlue;
            ExperienceGravityToShip();
            ShipGreen = GameObject.FindGameObjectWithTag("PlayerGreen");
        }
        else if (ShipGreen)
        {
            Target = ShipGreen;
            ExperienceGravityToShip();
            ShipBlue = GameObject.FindGameObjectWithTag("PlayerBlue");
        }
        else
        {
            force = MovePosition.normalized * speed;
            rb2D.AddForce(force);
            ShipBlue = GameObject.FindGameObjectWithTag("PlayerBlue");
            ShipGreen = GameObject.FindGameObjectWithTag("PlayerGreen");
        }

        if (!gameObject.GetComponent<Renderer>().isVisible)
            GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController>().TeleportObject(gameObject);
    }
    void ExperienceGravityToShip()
    {
        MovePosition = Target.transform.position - transform.position;
        if (MovePosition.sqrMagnitude != 0 && MovePosition.sqrMagnitude < 16f)
        {
            force = (MovePosition.normalized - force.normalized) * 20.0f / (MovePosition.sqrMagnitude / 3.0f + 0.3f);
            rb2D.AddForce(force);
        }
        else
            rb2D.AddForce(force);
    }
}