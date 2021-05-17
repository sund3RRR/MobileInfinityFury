using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
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
                case "Sputnik(Clone)":
                    DestroyController.DestroySputnik(collision.gameObject);
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
                default:
                    if (collision.gameObject.GetComponent<AsteroidController>())
                        DestroyController.DestroyAsteroid(collision.gameObject);
                    break;
            }
        }
    }
}
