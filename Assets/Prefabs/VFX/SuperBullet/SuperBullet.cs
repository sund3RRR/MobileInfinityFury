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
            if (Parent)
            {
                rb2D.velocity = transform.right * speed;
            }
            else
                Destroy(gameObject);
            yield return null;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != Parent)
        {
            switch (collision.name)
            {
                case "Zond(Clone)":
                    DestroyController.DestroyZond(collision.gameObject);
                    break;
                case "Sphere":
                    if (collision.gameObject.GetComponent<SphereController>().Active)
                        DestroyController.DestroySphere(collision.gameObject);
                    break;
                case "Panel":
                    if (collision.gameObject.GetComponent<PanelController>().Active)
                        DestroyController.DestroyDefault(collision.gameObject);
                    break;
                case "BigPiece(Clone)":
                    DestroyController.DestroyBigPiece(collision.gameObject);
                    break;
                case "Sputnik(Clone)":
                    DestroyController.DestroySputnik(collision.gameObject);
                    break;
                case "BulletEnemy(Clone)":
                    Destroy(collision.gameObject);
                    break;
                case "SmallPiece(Clone)":
                    DestroyController.DestroyDefault(collision.gameObject);
                    break;
                case "SpherePiece":
                    DestroyController.DestroyDefault(collision.gameObject);
                    break;
                case "GoldAsteroid(Clone)":
                    DestroyController.DestroyGoldAsteroid(collision.gameObject);
                    break;
                case "FatStarshipEnemy(Clone)":
                    DestroyController.DestroyFatEnemy(collision.gameObject);
                    break;
                case "SlimStarshipEnemy(Clone)":
                    DestroyController.DestroySlimEnemy(collision.gameObject);
                    break;
                case "DestroyerEnemyStarship(Clone)":
                    DestroyController.DestroyFatEnemy(collision.gameObject);
                    break;
                default:
                    if (collision.gameObject.GetComponent<AsteroidController>())
                        DestroyController.DestroyAsteroid(collision.gameObject);
                    break;
            }
        }
    }
}
