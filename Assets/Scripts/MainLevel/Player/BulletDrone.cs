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
