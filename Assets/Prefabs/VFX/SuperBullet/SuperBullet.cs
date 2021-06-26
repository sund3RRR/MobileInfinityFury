using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperBullet : MonoBehaviour
{
    private Rigidbody2D rb2D;
    public float speed;
    public GameObject Parent;

    void Start()
    {
        StartCoroutine(MyCoroutine());
    }

    IEnumerator MyCoroutine()
    {
        yield return new WaitForSeconds(5);

        rb2D = gameObject.AddComponent<Rigidbody2D>();
        rb2D.gravityScale = 0;
        rb2D.angularDrag = 0.5f;
        rb2D.drag = 0.5f;
        rb2D.interpolation = RigidbodyInterpolation2D.Interpolate;

        transform.SetParent(null);

        GetComponent<CircleCollider2D>().enabled = enabled;

        if (Parent)
            transform.rotation = Parent.transform.rotation;

        while (true)
        {
            rb2D.velocity = transform.right * speed;

            yield return null;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<HealthPointsController>())
        {
            DestroyController.DestroyObject(collision.GetComponent<HealthPointsController>().GameObjectName, collision.gameObject);
        }
    }
}
