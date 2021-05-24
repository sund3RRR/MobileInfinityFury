using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Editor variables
    private AudioSource AS;
    public AudioClip Clip0;
    public AudioClip Clip1;
    public Sprite Bullet0;
    public Sprite Bullet1;
    public Sprite DoubleBullet;
    public GameObject AsteroidHit;
    public GameObject DefaultHit;

    // Private variables   
    private Rigidbody2D rb2D;
    public GameObject Ship;
    public int damage;

    // Public variables
    public float speed;
    public int SpriteIndex = 0;
    public float DestroyTime;
    public Vector2 SightPosition;
     
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        AS = GetComponent<AudioSource>();
        //
        // Sprite changes
        //
        switch(SpriteIndex)
        {
            case 0:
                {
                    damage = 2;
                    GetComponent<SpriteRenderer>().sprite = Bullet0;
                    AS.clip = Clip0;
                }
                break;
            case 1:
                {
                    damage = 1;
                    transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                    GetComponent<SpriteRenderer>().sprite = Bullet1;
                    GetComponent<CircleCollider2D>().enabled = !enabled;
                    GetComponent<CapsuleCollider2D>().enabled = enabled;
                    AS.clip = Clip1;
                }
                break;
            case 2:
                {
                    damage = 2;
                    transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                    GetComponent<SpriteRenderer>().sprite = DoubleBullet;
                    GetComponent<CircleCollider2D>().enabled = !enabled;
                    GetComponent<CapsuleCollider2D>().enabled = !enabled;
                    GetComponent<BoxCollider2D>().enabled = enabled;
                    AS.clip = Clip1;
                }
                break;
            default:
                break;
        }
        //
        // Sprite changes
        //

        if (SightPosition.sqrMagnitude == 0)
            SightPosition = new Vector2(1.0f, 0);
        //
        // Rotations parameters
        //
        transform.rotation = Ship.transform.rotation;
        rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        //
        // Rotations parameters
        //
        //AS.Play();
        Destroy(gameObject, DestroyTime);
    }
    void FixedUpdate()
    {
        rb2D.velocity = (SightPosition * speed);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && collision.gameObject.name != "ZondVFX(Clone)")
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