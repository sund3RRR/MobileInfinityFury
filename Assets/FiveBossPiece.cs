using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiveBossPiece : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private Vector2 force;

    private bool Active;
    private float Torque;
    private float speed;

    private void Awake()
    {
        if(GetComponent<HealthPointsController>())
            GetComponent<HealthPointsController>().enabled = false;
      
    }
    void FixedUpdate()
    {
        if(Active)
        {
            rb2D.velocity = force * speed;
            rb2D.AddTorque(Torque);
        }
    }
    public void ActivateObject(GameObject Target)
    {
        if (!GetComponent<Rigidbody2D>())
        {
            rb2D = gameObject.AddComponent<Rigidbody2D>();
            rb2D.gravityScale = 0;
            rb2D.angularDrag = 0.5f;
            rb2D.drag = 0.5f;
            rb2D.interpolation = RigidbodyInterpolation2D.Interpolate;
        }

        GetComponent<Collider2D>().enabled = enabled;

        force = ((Vector2)(transform.position - Target.transform.position).normalized + new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f))).normalized;
        speed = 2f;
        Active = true;
        Torque = Random.Range(-0.5f, 0.5f);

        GetComponent<HealthPointsController>().enabled = enabled;

        transform.SetParent(null);
    }
}
