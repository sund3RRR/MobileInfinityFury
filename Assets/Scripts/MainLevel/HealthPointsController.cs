using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPointsController : MonoBehaviour
{
    //editor and public 
    public int HealthPoints;
    public int BaseHealthPoints;
    public string GameObjectName;

    //public variables
    [HideInInspector] public float hitTime = 2.5f;

    //private variables
    private SceneController CurrentScene;
    private GameObject Bar;
    private GameObject HealthBar;
    private Coroutine myCoroutine;
    private GameObject BulletHit;
    private GameObject BulletAsteroidHit;

    void Awake()
    {
        CurrentScene = GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController>();

        BulletHit = CurrentScene.BulletHit;
        BulletAsteroidHit = CurrentScene.BulletAsteroidHit;
        Bar = CurrentScene.HealthBar;

        BaseHealthPoints = HealthPoints;
        if (HealthPoints > 1)
        {
            GameObject Canvas = CurrentScene.CurrentCanvas;
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
                GameObject NewHit = BulletHit;
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
                        NewHit = collision.GetComponent<Drone>().DeadVFX;
                        break;
                }            

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
