using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && collision.GetComponent<HealthPointsController>())
        {
            if (collision.name == "Sphere")
            {
                if (!collision.GetComponent<SphereController>().Active)
                    collision.GetComponent<SphereController>().PreviousHP -= 1f;
                else
                    collision.GetComponent<HealthPointsController>().HealthPoints -= 1;
            }
            else if (collision.name == "Panel")
            {
                if (!collision.GetComponent<PanelController>().Active)
                    collision.GetComponent<PanelController>().PreviousHP -= 1f;
                else
                    collision.GetComponent<HealthPointsController>().HealthPoints -= 1;
            }
            else
                collision.GetComponent<HealthPointsController>().HealthPoints -= 1;
        }
        else if (collision.tag == "Enemy" || collision.tag == "EnemyBullet")
        {
            Destroy(collision.gameObject);
        }
    }
}
