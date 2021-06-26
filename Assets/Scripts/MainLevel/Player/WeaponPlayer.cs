using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPlayer : MonoBehaviour
{
    public float Speed;
    public int Damage;
    public float DestroyTime;

    [SerializeField] private GameObject BulletHit;
    [SerializeField] private GameObject BulletAsteroidHit;

    private void Start()
    {
        Destroy(gameObject, DestroyTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        HealthPointsController NewHPC = collision.GetComponent<HealthPointsController>();
        if (NewHPC && NewHPC.enabled)
        {
            string GameObjectName = NewHPC.GameObjectName;

            if ((GameObjectName != "Sphere" && GameObjectName != "Panel") ||
                (GameObjectName == "Sphere" && collision.GetComponent<SphereController>().Active) ||
                (GameObjectName == "Panel" && collision.GetComponent<PanelController>().Active))
            {
                if (GameObjectName != "Lightning")
                {
                    NewHPC.HealthPoints -= Damage;
                    NewHPC.RefreshHBRequest();
                }

                GameObject NewHit = BulletHit;      
                
                if (GameObjectName == "Asteroid")
                    NewHit = BulletAsteroidHit;

                NewHit = Instantiate(NewHit, transform.position, Quaternion.identity);
                Destroy(NewHit, 1);
                Destroy(gameObject);
            }
        }
        else if (collision.tag == "EnemyBullet")
        {
            GameObject NewHit = BulletHit;
            NewHit = Instantiate(NewHit, transform.position, Quaternion.identity);

            Destroy(NewHit, 1);
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
        else if (collision.tag == "BulletBoss")
        {
            GameObject NewHit = BulletHit;
            NewHit = Instantiate(NewHit, transform.position, Quaternion.identity);
            
            Destroy(NewHit, 1);
            Destroy(gameObject);
        }
    }
}
