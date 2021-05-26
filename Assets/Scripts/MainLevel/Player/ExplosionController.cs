using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    public GameObject Explosion;

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
                else
                    collision.gameObject.GetComponent<SphereController>().ActivateObject(0.5f);
                break;
            case "Panel":
                if (collision.gameObject.GetComponent<PanelController>().Active)
                    DestroyController.DestroyDefault(collision.gameObject);
                else
                    collision.gameObject.GetComponent<PanelController>().ActivateObject(0.5f);
                break;
            case "BigPiece(Clone)":
                DestroyController.DestroyBigPiece(collision.gameObject);
                break;
            case "Sputnik(Clone)":
                DestroyController.DestroySputnik(collision.gameObject);
                break;
            case "BossPiece":
                DestroyController.DestroyDefault(collision.gameObject);
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