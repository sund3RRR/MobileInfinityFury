using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    public GameObject VFX;
    private Rigidbody2D rb2D;
    public GameObject Parent;
    public GameObject Lightning;
    public float DestroyTime;
    public float speed;
    public int damage;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        transform.rotation = Parent.transform.rotation;
        rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;

        VFX.GetComponent<PositionLocker>().Target = gameObject;
        Instantiate(VFX, transform.position, Quaternion.identity);


        Destroy(gameObject, DestroyTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb2D.velocity = (transform.right * speed);
    }
}
