using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class HeroControllerGamePad : MonoBehaviour
{
    public GameObject CrossHairPrefab;
    private GameObject CrossHair;
    public GameObject Ship;
    public GameObject Explosion;
    public float range;
    public static int BonusMultiplier = 1;

    // Start function
    private Rigidbody2D rb2D;
    private List<int> PlayerLevels = new List<int> { 5, 25, 40, 80, 150, 300, 450, 600 };
    // Update, FixedUpdate function
    public static int CountOfUltimate = 3;
    private Vector2 MovePosition = new Vector2(0, 0);
    private Vector2 FirePosition = new Vector2(0, 0);
    private float TimeBtwBulletShots;
    private float TimeBtwRocketShots;
    private bool MoveFlag = false;
    private bool FireFlag = false;
    private float LifeTime = 0;
    public static bool CrossHairEnabled = false;
    public static bool WasSpawnedSputnik = false;
    public static bool NeedSpawnSputnik = true;
    public static int CurrentIndex = 0;
    // UpgradeWeapon function
    public static float upgradeTimeBulletShots = 0.5f;
    public static float upgradeTimeRocketShots = 0.3f;
    public static int Experience = 0;
    public static int WeaponIndex = 0;
    public static bool RocketShot = false;
    public static bool AimBot = false;
    public static int BulletSpeed = 10;
    public static int BulletSpriteIndex = 0;

    // RotateObj function
    private float rotate;

    // Shoot function
    public Transform bulletPoint;
    public GameObject bullet;
    public GameObject rocket;

    //MoveObj function
    public float speed;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (Gamepad.current.leftStick.x.ReadValue() != 0 || Gamepad.current.leftStick.y.ReadValue() != 0)
        {
            MovePosition = new Vector2(Gamepad.current.leftStick.x.ReadValue(), Gamepad.current.leftStick.y.ReadValue());
            MoveFlag = true;
            if (Gamepad.current.rightStick.x.ReadValue() + Gamepad.current.rightStick.y.ReadValue() == 0)
                RotateObj(MovePosition);
        }
        else
            MoveFlag = false;
        
        if (Gamepad.current.rightStick.x.ReadValue() != 0 || Gamepad.current.rightStick.y.ReadValue() != 0)
        {
            if (CrossHairEnabled)
                CrossHair.SetActive(true);
            else
                CrossHair.SetActive(false);
            FirePosition = new Vector2(Gamepad.current.rightStick.x.ReadValue(), Gamepad.current.rightStick.y.ReadValue());
            FireFlag = true;
            RotateObj(FirePosition);
        }
        else
        {
            CrossHair.SetActive(false);
            FireFlag = false;
        }

        if (!gameObject.GetComponent<Renderer>().isVisible)
            Destroy(gameObject);
    }
    void FixedUpdate()
    {
        if (rb2D.velocity.sqrMagnitude < 0.001f)
            rb2D.velocity = new Vector2(0, 0);

        if (MoveFlag)
            MoveObj(MovePosition);
        if (FireFlag)
            Shoot(FirePosition);

        LifeTime += Time.deltaTime;
        TimeBtwBulletShots += Time.deltaTime;
        TimeBtwRocketShots += Time.deltaTime;
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
    void MoveObj(Vector2 force)
    {
        force = force * speed;

        rb2D.AddForce(force);
    }
    void RotateObj(Vector2 rotation)
    {
        rotate = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(rotate, Vector3.forward);
    }
    public void Shoot(Vector2 BaseForce)
    {
        if (TimeBtwBulletShots >= upgradeTimeBulletShots)
        {
            TimeBtwBulletShots = 0;
            bullet.GetComponent<Bullet>().SightPosition = BaseForce.normalized;
            GameObject NewBullet = Instantiate(bullet, bulletPoint.position, transform.rotation);
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
                rocket.GetComponent<RocketController>().SightPosition = BaseForce.normalized;
                rocket.GetComponent<RocketController>().Ship = gameObject;
                Instantiate(rocket, bulletPoint.position, transform.rotation);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && LifeTime > 1f)
        {
            SceneController.isLifeGreen = false;
            BonusMultiplier = 1;
            Destroy(gameObject);
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
}
