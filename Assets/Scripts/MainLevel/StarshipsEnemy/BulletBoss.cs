using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBoss : MonoBehaviour
{
    private Rigidbody2D rb2D;
    public float speed;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        rb2D.velocity = -transform.right.normalized * speed;
        if (!GetComponent<Renderer>().isVisible)
            Destroy(gameObject);
    }
}
