using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SceneController : MonoBehaviour
{
    // Editor variables
    public GameObject Boss1;
    public GameObject Ship;
    public GameObject Exp;
    public GameObject SmallPiece;
    public GameObject BigPiece;
    public GameObject Bonus;
    public GameObject CrossHair;
    public GameObject DeadVFX;
    public GameObject ZondDeadVFX;
    public GameObject AsteroidDeadVFX;
    public GameObject ZondExplosionVFX;
    public GameObject TwentyOnNine;
    public GameObject NineteenAndHalfOnNine;
    public GameObject EighteenOnNine;
    public GameObject SixteenOnNine;
    public GameObject SixteeenOnTen;
    public GameObject FourOnThree;

    public GameObject EnemySpawnVFX;
    public GameObject EnemySpawnVFX2;
    public GameObject SpawnVFX;

    public GameObject FatEnemy;
    public GameObject SlimEnemy;
    public GameObject DestroyerEnemy;
    // Private variables

    private Camera cam;
    private SpawnController Spawner;
    private Vector3 mousePosition;
    private Vector2 SightPosition;
    private float XPose, YPose;
    private int TimingAsteroids = 3;
    private int TimingZonds = 5;
    private bool ActivateAimBot = false;
    private Vector2 viewPortPos;

    // Public static variables
    public static bool isLifeBlue = false;
    public static int ShipLifeBlue = 3;
    public static bool isLifeGreen = false;
    public static int ShipLifeGreen = 3;
    public static GameObject[] EnemyArray;

    public int TargetFrameRate;
    public bool NeedSpawnZond;
    public bool NeedSpawnAsteroid;
    public bool NeedSpawnEnemyStarships;
    public bool NeedSpawnBoss;

    private void Awake()
    {
        Application.targetFrameRate = TargetFrameRate;
    }
    private void Start()
    {
        cam = Camera.main;
        viewPortPos = cam.ViewportToWorldPoint(new Vector2(1, 1));
        Spawner = GetComponent<SpawnController>();

        Time.timeScale = 1f;

        if (NeedSpawnEnemyStarships)
            StartCoroutine(SpawnEnemyStarShipCore());
        if (NeedSpawnZond)
            StartCoroutine(SpawnZond());
        if (NeedSpawnAsteroid)
            StartCoroutine(SpawnAsteroids());
        if (NeedSpawnBoss)
            StartCoroutine(SpawnBoss(Boss1));
    }
    private void Update()
    {       
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        SightPosition = mousePosition - transform.position;
        SightPosition = SightPosition.normalized;
    }

    void FixedUpdate()
    {
        EnemyArray = GameObject.FindGameObjectsWithTag("Enemy");
    }
    public void SpawnBossFromDC()
    {
        StartCoroutine(SpawnBoss(Boss1));
    }
    IEnumerator SpawnStarShipEnemy(GameObject StarShipEnemy)
    {    
        Vector2 SpawnPos = new Vector2(Random.Range(-3.3f, 3.3f), Random.Range(2.5f, 6f));

        GameObject NewEnemySpawnVFX = Instantiate(EnemySpawnVFX, SpawnPos, Quaternion.identity);
        Destroy(NewEnemySpawnVFX, 2);
        GameObject NewEnemySpawnVFX2 = Instantiate(EnemySpawnVFX2, SpawnPos, Quaternion.identity);
        Destroy(NewEnemySpawnVFX2, 2);

        float Angle = Random.Range(0, 359);
        yield return new WaitForSeconds(0.3f);

        Instantiate(StarShipEnemy, SpawnPos, Quaternion.AngleAxis(Angle, Vector3.forward));
        yield break;
    }
    public IEnumerator DestroyerCenterEnemies()
    {
        for (int i = 0; i < 5; i++)
        {
            EnemyArray = GameObject.FindGameObjectsWithTag("Enemy");
            DestroyController.DestroyCenterEnemies(EnemyArray);

            yield return new WaitForSeconds(0.2f);
        }
        yield break;
    }
    public IEnumerator SpawnBoss(GameObject Boss)
    {
        Vector2 SpawnPos = new Vector2(Random.Range(-1.5f, 1.5f), 5.5f);
        Instantiate(Boss, SpawnPos, Boss.transform.rotation);
        yield break;
    }

    IEnumerator SpawnEnemyStarShipCore()
    {
        while(true)
        {
            int Index = Random.Range(0, 3);
            if (Index == 0)
                StartCoroutine(SpawnStarShipEnemy(FatEnemy));
            else if (Index == 1)
                StartCoroutine(SpawnStarShipEnemy(SlimEnemy));
            else if (Index == 2)
                StartCoroutine(SpawnStarShipEnemy(DestroyerEnemy));
            yield return new WaitForSeconds(5f);
        }
    }
    IEnumerator SpawnZond()
    {
        while (true)
        {
            XPose = Random.Range(-1.7f, 1.7f);
            YPose = 6.5f;

            Vector2 ZondPoint = new Vector2(XPose, YPose);
            Spawner.SpawnZond(ZondPoint);

            yield return new WaitForSeconds(TimingZonds);
        }
    }
    IEnumerator SpawnAsteroids()
    {
        while (true)
        {
            XPose = Random.Range(-2f, 2f);
            YPose = 6f;

            Vector2 AsteroidPoint = new Vector2(XPose, YPose);
            Spawner.SpawnAsteroid(AsteroidPoint, 3, false, Vector2.zero);

            XPose = Random.Range(-2f, 2f);
            YPose = 6f;

            AsteroidPoint = new Vector2(XPose, YPose);
            Spawner.SpawnAsteroid(AsteroidPoint, 3, false, Vector2.zero);

            yield return new WaitForSeconds(TimingAsteroids);
        }
    }
    IEnumerator SpawnGoldAsteroid()
    {
        yield return new WaitWhile(() => Time.time < 120);

        while (true)
        {
            XPose = Mathf.Pow(-1, Random.Range(1, 3)) * (viewPortPos.x + 1.2f);
            YPose = Random.Range(-5f, 5f);

            Vector2 AsteroidPoint = new Vector2(XPose, YPose);
            Spawner.SpawnGoldAsteroid(AsteroidPoint);

            yield return new WaitForSeconds(120);
        }
    }
    
    public void TimingUpgrade()
    {
        switch (Mathf.Max(HeroController.WeaponIndex, HeroControllerGamePad.WeaponIndex))
        {
            case 0:
                TimingAsteroids = 1;
                 TimingZonds = 3;
                break;
            case 1:
                TimingAsteroids = 22;
                break;
            case 2:
                TimingAsteroids = 18;
                break;
            case 3:
                TimingAsteroids = 15;
                break;
            case 4:
                TimingAsteroids = 12;
                TimingZonds = 25;
                break;
            case 5:
                TimingAsteroids = 10;
                TimingZonds = 20;
                break;
            case 6:
                TimingAsteroids = 7;
                break;
            case 7:
                TimingAsteroids = 6;
                break;
            case 8:
                ActivateAimBot = true;
                TimingAsteroids = 4;
                TimingZonds = 15;
                break;
            default:
                break;
        }
    }
  
    public void SpawnSputnik()
    {
        XPose = Mathf.Pow(-1, Random.Range(1, 3)) * (viewPortPos.x + 1.2f);
        YPose = Random.Range(-5f, 5f);

        Vector2 SputnikPoint = new Vector2(XPose, YPose);
        Spawner.SpawnSputnik(SputnikPoint);
    }
    public void TeleportObject(GameObject obj)
    {
        Destroy(obj);
    }
}