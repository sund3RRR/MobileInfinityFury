using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyberShield : MonoBehaviour
{
    public GameObject Parent;
    private Vector2 viewportPosition;
    private Camera Cam;

    void Start()
    {
        Cam = Camera.main;
    }

    void FixedUpdate()
    {
        if (!Parent)
            Destroy(gameObject);

        viewportPosition = Cam.WorldToViewportPoint(transform.position);
        if (viewportPosition.x > 1 || viewportPosition.x < 0 || viewportPosition.y > 1 || viewportPosition.y < 0)
            GetComponent<CircleCollider2D>().radius = 0.5f;
        else
            GetComponent<CircleCollider2D>().radius = 1.1f;
    }
    public void OnTriggerEnter2D(Collider2D collision)
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
            case "SuperBulletVFX(Clone)":
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
