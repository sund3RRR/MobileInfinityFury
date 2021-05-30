using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HeroController : MonoBehaviour
{
    // JOYSTICK CONTROL
    /*
    public GameObject moveJoy;
    public GameObject fireJoy;
    private Joystick MoveJoystick;
    private Joystick FireJoystick;
    */
    // JOYSTICK CONTROL
    public float RadiusForSecondBoss;
    public GameObject ParentForSecondBoss;
    private GameObject NewUltimate;
    public GameObject CyberShieldVFX;
    public GameObject EngineThrustVFX;
    private GameObject NewEngineVFX;
    public Transform EngineVFXPoint;
    private AudioSource AS;
    public AudioClip Clip0;
    public AudioClip Clip1;
    private List<int> PlayerLevels = new List<int> { 5, 25, 40, 80, 150, 300, 450, 600 };

    public GameObject CrossHairPrefab;
    public GameObject Ship;
    public GameObject Explosion;  
    public float range;
    public static int BonusMultiplier = 1;
    public GameObject UpgradeVFX;
    // Start function
    //private Rigidbody2D rb2D;
    private float TimeBtwTouches = 0;
    // Update, FixedUpdate function
    public static int CountOfUltimate = 100;
    private Vector2 mousePosition = Vector2.zero;
    private float TimeBtwBulletShots;
    private float TimeBtwRocketShots;
    private bool FireFlag = false;
    private float LifeTime = 0;
    public static bool CrossHairEnabled = false;
    public static bool WasSpawnedSputnik = false;
    public static bool NeedSpawnSputnik = true;
    public static int CurrentIndex = 0;
    // UpgradeWeapon function
    public static float upgradeTimeBulletShots = 0.25f;
    public static float upgradeTimeRocketShots = 0.15f;
    public static int Experience = 0;
    public static int WeaponIndex = 0;
    public static bool RocketShot = true;
    public static bool AimBot = false;
    public static int BulletSpeed = 16;
    public static int BulletSpriteIndex = 2;

    private GameObject NewCyberShield;
    
    // Shoot function
    public Transform bulletPoint;
    public GameObject bullet;
    public GameObject rocket;
    
    //MoveObj function
    public float speed;

    public bool IsDroneWeapon;
    public GameObject Drone;
    public int DroneCount;
    public float MinRadius;
    public float RadiusStep;
    private float[] DroneRadiusArray;
    private GameObject[] Drones;
    public float TimeBtwDroneSpawn;
    private float TimerDrones = 0;

    private bool IsStunned;

    void Start()
    {
        DroneRadiusArray = new float[DroneCount];
        Drones = new GameObject[DroneCount];
        for (int i = 0; i < DroneCount; i++)
        {
            DroneRadiusArray[i] = MinRadius + RadiusStep * i;
        }
    }
    void Update()
    {
        TimeBtwTouches += Time.deltaTime;
        //
        // MOBILE CONTROLS
        //
        /*        
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
        */
        //
        // MOUSE CONTROLS
        //
        
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

        // MOUSE CONTROLS   
        if (IsStunned)
            return;
        else if (ParentForSecondBoss && ParentForSecondBoss.GetComponent<SecondBossManager>().IsTurbo &&
            (Mathf.Abs(((Vector2)ParentForSecondBoss.transform.position - mousePosition).sqrMagnitude) < RadiusForSecondBoss * RadiusForSecondBoss))
            transform.position = Vector3.Lerp(transform.position, mousePosition, 0.1f);
        else if (!ParentForSecondBoss || !ParentForSecondBoss.GetComponent<SecondBossManager>().IsTurbo)
            transform.position = Vector3.Lerp(transform.position, mousePosition, 0.1f);
    }
    void FixedUpdate()
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
  
        TimeBtwBulletShots += Time.deltaTime;
        TimeBtwRocketShots += Time.deltaTime;
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
            case 1:
                    upgradeTimeBulletShots -= 0.15f;
                break;
            case 2:
                {
                    upgradeTimeBulletShots -= 0.15f;
                }
                break;
            case 3:
                {
                    upgradeTimeBulletShots -= 0.1f;
                    BulletSpeed = 16;
                    BulletSpriteIndex = 1;
                }
                break;
            case 4:
                BulletSpriteIndex = 2;
                break;
            case 5:
                RocketShot = true;
                break;
            case 6:
                    upgradeTimeRocketShots -= 0.15f;
                break;
            case 7:
                    upgradeTimeRocketShots -= 0.06f;
                break;
            case 8:
                AimBot = true;
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
        if (TimeBtwBulletShots >= upgradeTimeBulletShots)
        {
            TimeBtwBulletShots = 0;
            bullet.GetComponent<Bullet>().SightPosition = Vector2.up;
            GameObject NewBullet =  Instantiate(bullet, bulletPoint.position, transform.rotation);
            NewBullet.GetComponent<Bullet>().speed = BulletSpeed;
            NewBullet.GetComponent<Bullet>().SpriteIndex = BulletSpriteIndex;
            NewBullet.GetComponent<Bullet>().Ship = gameObject;
        }
        if (RocketShot)
        {
            if (TimeBtwRocketShots >= upgradeTimeRocketShots)
            {
                TimeBtwRocketShots = 0;

                if (AimBot)
                    rocket.GetComponent<RocketController>().AimBot = true;
                else
                    rocket.GetComponent<RocketController>().AimBot = false;
                rocket.GetComponent<RocketController>().SightPosition = Vector2.up;
                rocket.GetComponent<RocketController>().Ship = gameObject;
                Instantiate(rocket, bulletPoint.position, transform.rotation);
            }
        }
    }
    public void SpawnDrone()
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
            if (!NewCyberShield)
            {
                SceneController.ShipLifeBlue--;
                NewCyberShield = Instantiate(CyberShieldVFX, transform.position, Quaternion.identity, transform);
                NewCyberShield.GetComponent<CyberShield>().Parent = gameObject;
                Destroy(NewCyberShield, 9);
            }
        }
        if (collision.gameObject.tag == "Experience")
        {
            Experience += BonusMultiplier;
            if (NeedSpawnSputnik)
            {
                if (Experience >= PlayerLevels[CurrentIndex] && !WasSpawnedSputnik)
                {
                    GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController>().SpawnSputnik();
                    WasSpawnedSputnik = true;
                    if (PlayerLevels.Count() - 1 > CurrentIndex)
                        CurrentIndex++;
                    else
                        NeedSpawnSputnik = false;
                }
                else if (Experience > PlayerLevels[CurrentIndex])
                    WasSpawnedSputnik = false;
            }
            Destroy(collision.gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "Enemy" || collision.tag == "EnemyBullet") && LifeTime > 1f)
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