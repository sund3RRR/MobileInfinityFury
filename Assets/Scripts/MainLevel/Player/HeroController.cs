using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HeroController : MonoBehaviour
{
    [Header("Hero")]     
    [SerializeField] private GameObject Explosion;
    [SerializeField] private GameObject CyberShieldVFX;
    [SerializeField] private GameObject EngineThrustVFX;
    [SerializeField] private GameObject UpgradeVFX;

    [SerializeField] private float speed;
    private float TimeBtwTouches = 0;
    private bool FireFlag = false;
    private float LifeTime = 0;
    private GameObject NewCyberShield;
    public int BonusMultiplier;
    public int Experience = 0;
    public int WeaponIndex = 1;
    private bool IsStunned;


    [Header("SecondBoss")]  
    [SerializeField] private float RadiusForSecondBoss;
    [HideInInspector] public GameObject ParentForSecondBoss;
    public int CountOfUltimate = 3;
    private Vector2 mousePosition = new Vector2(0, -4.365f);


    [Header("Shoot")]
    [SerializeField] private float TimeBtwBulletShots = 0.25f;
    [SerializeField] private float TimeBtwRocketShots = 0.15f;

    [SerializeField] private int BulletSpeed1;
    [SerializeField] private int BulletSpeed2;   
    [SerializeField] private Transform bulletPoint;
    [SerializeField] private GameObject Bullet1;
    [SerializeField] private GameObject Bullet2;
    [SerializeField] private GameObject rocket;

    [SerializeField] private bool RocketShot = true;

    private GameObject CurrentBullet;
    private float TimerTimeBtwBulletShots;
    private float TimerTimeBtwRocketShots;
 

    [Header("Drones")]
    [SerializeField] private bool IsDroneWeapon;
    [SerializeField] private GameObject Drone;
    [SerializeField] private int DroneCount;
    [SerializeField] private float MinRadius;
    [SerializeField] private float RadiusStep;
    [SerializeField] private float TimeBtwDroneSpawn;
    private float[] DroneRadiusArray;
    private GameObject[] Drones;   
    private float TimerDrones = 0;

    private GameObject NewUltimate;
   
    void Start()
    {
        CurrentBullet = Bullet1;
        CurrentBullet.GetComponent<WeaponPlayer>().Speed = BulletSpeed1;
        TimeBtwBulletShots = 0.45f;

        UpgradeWeapon();

        DroneRadiusArray = new float[DroneCount];
        Drones = new GameObject[DroneCount];
        for (int i = 0; i < DroneCount; i++)
        {
            DroneRadiusArray[i] = MinRadius + RadiusStep * i;
        }
        
    }
    void Update()
    {
        if (Time.timeScale > 0)
        {
            TimeBtwTouches += Time.deltaTime;
            //
            // MOBILE CONTROLS
            //
            if (Input.touchCount > 0)
            {
                Touch myTouch = Input.GetTouch(0);
                mousePosition = Camera.main.ScreenToWorldPoint(myTouch.position);

                if (myTouch.phase == TouchPhase.Stationary || myTouch.phase == TouchPhase.Moved)
                {
                    Shoot();
                }
                if (myTouch.tapCount == 2 && IsDoubleTap() && CountOfUltimate > 0 && !NewUltimate) // вызывается здесь, так как не работает в FixedUpdate :(
                {
                    NewUltimate = Instantiate(Explosion, transform.position, Quaternion.identity);
                    Destroy(NewUltimate, 1.1f);
                    CountOfUltimate--;
                }
            }
            //
            // MOUSE CONTROLS
            //
            /*
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButton(0)) // вызывается здесь, так как не работает в FixedUpdate :(
            {
                FireFlag = true;
            }
            else
            {
                FireFlag = false;
            }

            if (Input.GetMouseButtonDown(2) && CountOfUltimate > 0) // вызывается здесь, так как не работает в FixedUpdate :(
            {
                GameObject NewUltimate = Instantiate(Explosion, transform.position, Quaternion.identity);
                Destroy(NewUltimate, 1.1f);
                CountOfUltimate--;
            }
            */
            // MOUSE CONTROLS   
            if (IsStunned)
                return;
            else if (ParentForSecondBoss && ParentForSecondBoss.GetComponent<SecondBossManager>().IsTurbo &&
                (Mathf.Abs(((Vector2)ParentForSecondBoss.transform.position - mousePosition).sqrMagnitude) < RadiusForSecondBoss * RadiusForSecondBoss))
                transform.position = Vector3.Lerp(transform.position, mousePosition, 0.1f);
            else if ((!ParentForSecondBoss || !ParentForSecondBoss.GetComponent<SecondBossManager>().IsTurbo) && mousePosition.y < 4f)
                transform.position = Vector3.Lerp(transform.position, mousePosition, 0.1f);
        }
    }
    void FixedUpdate()
    {
        if (Time.timeScale > 0)
        {
            LifeTime += Time.deltaTime;
            TimerDrones += Time.deltaTime;
            // MOUSE CONTROL  
            
            if (FireFlag)
            {
                Shoot();
            }
            
            if (IsDroneWeapon)
                SpawnDrone();

            TimerTimeBtwBulletShots += Time.deltaTime;
            TimerTimeBtwRocketShots += Time.deltaTime;
        }
    }
    public IEnumerator Stun()
    {
        IsStunned = true;
        yield return new WaitForSeconds(1.5f);
        IsStunned = false;
    }
    public void UpgradeWeapon()
    {
        switch (WeaponIndex)
        {
            case 4:
                CurrentBullet = Bullet2;
                CurrentBullet.GetComponent<WeaponPlayer>().Speed = BulletSpeed2;
                TimeBtwBulletShots = 0.25f;
                break;
            case 5:
                RocketShot = true;
                break;
            case 6:
                    TimerTimeBtwRocketShots -= 0.15f;
                break;
            case 7:
                    TimerTimeBtwRocketShots -= 0.06f;
                break;
            case 8:
                IsDroneWeapon = true;
                break;
            default:
                break;
        }
    }
    bool IsDoubleTap()
    {
        if (Input.touchCount > 0)
        {
            Touch myTouch = Input.GetTouch(0);
            if (myTouch.phase == TouchPhase.Began)
                TimeBtwTouches = 0;
        }
        if (TimeBtwTouches < 0.5f)
            return true;
        else
            return false;
    }
    public void Shoot()
    {
        if (TimerTimeBtwBulletShots >= TimeBtwBulletShots)
        {
            if (WeaponIndex == 1 || WeaponIndex == 4)
            {
                Instantiate(CurrentBullet, bulletPoint.position, transform.rotation);
            }
            else if (WeaponIndex == 2 || WeaponIndex == 5)
            {
                Instantiate(CurrentBullet, new Vector2(bulletPoint.position.x - 0.1f, bulletPoint.position.y), transform.rotation);
                Instantiate(CurrentBullet, new Vector2(bulletPoint.position.x + 0.1f, bulletPoint.position.y), transform.rotation);
            }
            else if (WeaponIndex == 3 || WeaponIndex >= 6)
            {
                Instantiate(CurrentBullet, new Vector2(bulletPoint.position.x - 0.1f, bulletPoint.position.y), transform.rotation);
                Instantiate(CurrentBullet, new Vector2(bulletPoint.position.x + 0.1f, bulletPoint.position.y), transform.rotation);
                           
                GameObject NewBullet =  Instantiate(CurrentBullet, new Vector2(bulletPoint.position.x - 0.2f, bulletPoint.position.y - 0.1f),
                    Quaternion.AngleAxis(105, Vector3.forward));
                NewBullet.GetComponent<Bullet>().Damage = 1;
                NewBullet.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);

                NewBullet = Instantiate(CurrentBullet, new Vector2(bulletPoint.position.x + 0.2f, bulletPoint.position.y - 0.1f),
                    Quaternion.AngleAxis(75, Vector3.forward));
                NewBullet.GetComponent<Bullet>().Damage = 1;
                NewBullet.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            }
            TimerTimeBtwBulletShots = 0;
        }
        if (RocketShot)
        {
            if (TimerTimeBtwRocketShots >= TimeBtwRocketShots)
            {
                TimerTimeBtwRocketShots = 0;
                Instantiate(rocket, bulletPoint.position, transform.rotation);
            }
        }
    }
    void SpawnDrone()
    {
        if (TimerDrones > TimeBtwDroneSpawn)
        {         
            for (int i = 0; i < DroneCount; i++)
            {
                if (!Drones[i])
                {
                    TimerDrones = 0;
                    Drone.GetComponent<Drone>().IsClockwise = i % 2 == 0 ? true : false;
                    Drone.GetComponent<Drone>().Radius = DroneRadiusArray[i];
                    Drones[i] = Instantiate(Drone, new Vector2(transform.position.x, transform.position.y + 0.01f), Quaternion.identity);
                    break;
                }
            }
        }
    }
    public void AnimationUpgrade()
    {
        GameObject NewUpgradeVFX = Instantiate(UpgradeVFX, transform.position, Quaternion.identity, transform);
        Destroy(NewUpgradeVFX, 2);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "Enemy" || collision.tag == "EnemyBullet") && LifeTime > 1f)
        {
            if (collision.GetComponent<HealthPointsController>()?.GameObjectName == "Summon")
                Destroy(collision.gameObject);
            if (!NewCyberShield)
            {
                SceneController.ShipLifeBlue--;
                NewCyberShield = Instantiate(CyberShieldVFX, transform.position, Quaternion.identity, transform);
                NewCyberShield.GetComponent<CyberShield>().Parent = gameObject;
                Destroy(NewCyberShield, 5);
            }
        }
        if (collision.gameObject.tag == "Experience")
        {
            PlayerPrefs.SetFloat("Galo", PlayerPrefs.GetFloat("Galo") + BonusMultiplier * 0.001f);
            Experience += BonusMultiplier;
            Destroy(collision.gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "Enemy" || collision.tag == "EnemyBullet" || collision.tag == "BulletBoss") && LifeTime > 1f)
        {
            if (!NewCyberShield)
            {
                SceneController.ShipLifeBlue--;
                NewCyberShield = Instantiate(CyberShieldVFX, transform.position, Quaternion.identity, transform);
                NewCyberShield.GetComponent<CyberShield>().Parent = gameObject;
                Destroy(NewCyberShield, 5);
            }
        }
    }
}