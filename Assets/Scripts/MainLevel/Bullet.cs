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
        if (collision.tag == "Enemy")
        {
            GameObject NewHit = DefaultHit;

            //
            // Damaging
            //
            switch (collision.name)
            {
                case "Zond(Clone)":
                    collision.GetComponent<ZondController>().HealthPoints -= damage;
                    collision.GetComponent<ZondController>().hitTime = 0;
                    break;
                case "Sphere":
                    if (collision.gameObject.GetComponent<SphereController>().Active)
                    {
                        collision.GetComponent<SphereController>().HealthPoints -= damage;
                        collision.GetComponent<SphereController>().hitTime = 0;
                    }
                    break;
                case "FatStarshipEnemy(Clone)":
                    collision.GetComponent<FatStarshipEnemy>().HealthPoints -= damage;
                    collision.GetComponent<FatStarshipEnemy>().hitTime = 0;
                    break;
                case "DestroyerEnemyStarship(Clone)":
                    collision.GetComponent<DestroyerEnemyController>().HealthPoints -= damage;
                    collision.GetComponent<DestroyerEnemyController>().hitTime = 0;
                    break;                 
                case "SlimStarshipEnemy(Clone)":
                    collision.GetComponent<SlimEnemyController>().HealthPoints -= damage;
                    collision.GetComponent<SlimEnemyController>().hitTime = 0;
                    break;
                case "Panel":
                    if (collision.gameObject.GetComponent<PanelController>().Active)
                    {
                        collision.GetComponent<PanelController>().HealthPoints -= damage;
                        collision.GetComponent<PanelController>().hitTime = 0;
                    }
                    break;
                case "BigPiece(Clone)":
                    collision.GetComponent<BigPieceController>().HealthPoints -= damage;
                    collision.GetComponent<BigPieceController>().hitTime = 0;
                    break;
                case "Sputnik(Clone)":
                    collision.GetComponent<SputnikController>().HealthPoints -= damage;
                    collision.GetComponent<SputnikController>().hitTime = 0;
                    break;
                case "SmallPiece(Clone)":
                    DestroyController.DestroyDefault(collision.gameObject);
                    break;
                case "SpherePiece":
                    DestroyController.DestroyDefault(collision.gameObject);
                    break;
                case "GoldAsteroid(Clone)":
                    collision.GetComponent<GoldAsteroidController>().HealthPoints -= damage;
                    collision.GetComponent<GoldAsteroidController>().hitTime = 0;
                    break;
                case "FirstBoss(Clone)":
                    collision.GetComponent<BossFirst>().HealthPoints -= damage;
                    collision.GetComponent<BossFirst>().hitTime = 0;
                    break;
                default:
                    if (collision.GetComponent<AsteroidController>())
                    {
                        collision.GetComponent<AsteroidController>().HealthPoints -= damage;
                        collision.GetComponent<AsteroidController>().hitTime = 0;
                        NewHit = AsteroidHit;
                    }
                    break;
            }
            //
            // Damaging
            //
            if (collision.gameObject.name != "ZondVFX(Clone)")
            {
                GameObject InstanceHit = Instantiate(NewHit, transform.position, Quaternion.identity);

                Destroy(InstanceHit, 1);
                Destroy(gameObject);
            }
        }
    }
}