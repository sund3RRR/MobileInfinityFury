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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && collision.name != "ZondVFX(Clone)")
        {
            GameObject NewHit = DefaultHit;

            if (collision.GetComponent<HealthPointsController>().GameObjectName == "Asteroid")
            {
                NewHit = AsteroidHit;
            }
            GameObject InstanceHit = Instantiate(NewHit, transform.position, Quaternion.identity);

            Destroy(InstanceHit, 1);
            Destroy(gameObject);
        }
    }
}
