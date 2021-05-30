using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPointsController : MonoBehaviour
{
    public GameObject Bar;
    private GameObject HealthBar;
    public float hitTime = 2.5f;
    public int HealthPoints;
    public int BaseHealthPoints;
    private Coroutine myCoroutine;
    private GameObject BulletHit;
    private GameObject BulletAsteroidHit;

    public string GameObjectName;

    void Awake()
    {
        BulletHit = GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController>().BulletHit;
        BulletAsteroidHit = GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController>().BulletAsteroidHit;

        BaseHealthPoints = HealthPoints;
        if (HealthPoints > 1)
        {
            GameObject Canvas = GameObject.FindGameObjectWithTag("Canvas");
            Bar.GetComponent<HealthBarController>().Target = gameObject;
            Bar.GetComponent<HealthBarController>().BaseHealthPoints = BaseHealthPoints;
            HealthBar = Instantiate(Bar, new Vector3(transform.position.x, transform.position.y), Quaternion.identity);
            HealthBar.transform.SetParent(Canvas.transform, false);
        }
    }

    void FixedUpdate()
    {
        hitTime += Time.deltaTime;

        if (HealthBar && hitTime > 2.5f)
        {
            HealthBar.GetComponent<HealthBarController>().DisableHealthBar();
        }
        else if (HealthBar)
        {
            HealthBar.GetComponent<HealthBarController>().HealthPoints = HealthPoints;
            HealthBar.GetComponent<HealthBarController>().EnableHealthBar();
        }

        if (HealthPoints <= 0 && GameObjectName != "FirstBoss")
            DestroyController.DestroyObject(GameObjectName, gameObject);
        else if (HealthPoints <= 0 && GameObjectName == "FirstBoss" && myCoroutine == null)
            myCoroutine = StartCoroutine(GetComponent<BossFirst>().DestroyMe());
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag == "Bullet" || collision.tag == "Rocket" || collision.tag == "BulletDrone" || collision.tag == "Drone") && enabled)
        {
            if ((GameObjectName != "Sphere" && GameObjectName != "Panel") ||
                (GameObjectName == "Sphere" && GetComponent<SphereController>().Active) ||
                (GameObjectName == "Panel" && GetComponent<PanelController>().Active))
            {
                switch (collision.tag)
                {
                    case "Bullet":
                        HealthPoints -= collision.GetComponent<Bullet>().damage;
                        break;
                    case "Rocket":
                        HealthPoints -= collision.GetComponent<RocketController>().damage;
                        break;
                    case "BulletDrone":
                        HealthPoints -= collision.GetComponent<BulletDrone>().damage;
                        break;
                    case "Drone":
                        HealthPoints -= collision.GetComponent<Drone>().damage;
                        break;
                }
                GameObject NewHit = BulletHit;

                if (GameObjectName == "Asteroid")
                {
                    NewHit = BulletAsteroidHit;
                }
                GameObject InstanceHit = Instantiate(NewHit, collision.transform.position, Quaternion.identity);

                Destroy(InstanceHit, 1);
                Destroy(collision.gameObject);
                hitTime = 0;
            }
        }
    }
}
