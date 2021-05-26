using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDrone : MonoBehaviour
{
    private Rigidbody2D rb2D;
    public float speed;
    public int damage;
    public GameObject AsteroidHit;
    public GameObject DefaultHit;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        rb2D.velocity = transform.right.normalized * speed;
        if (!GetComponent<Renderer>().isVisible)
            Destroy(gameObject);
    }
}
