﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SceneController : MonoBehaviour
{
    // Editor variables
    [SerializeField] private GameObject Boss1;
    [SerializeField] private GameObject SecondBoss1, SecondBoss2;
    [SerializeField] private GameObject Boss4;
    [SerializeField] private GameObject ParentForSecondBoss;
    [SerializeField] private GameObject Ship;   
    [SerializeField] private GameObject AsteroidDeadVFX; 

    [SerializeField] private GameObject EnemySpawnVFX;
    [SerializeField] private GameObject EnemySpawnVFX2;
    [SerializeField] private GameObject SpawnVFX;
    [SerializeField] private GameObject FatEnemy;
    [SerializeField] private GameObject SlimEnemy;
    [SerializeField] private GameObject DestroyerEnemy;

    public GameObject Exp;
    public GameObject SmallPiece;
    public GameObject BigPiece;
    public GameObject Bonus;
    public GameObject DeadVFX;
    public GameObject ZondDeadVFX;
    public GameObject ZondExplosionVFX;
    public GameObject BulletHit;
    public GameObject BulletAsteroidHit;
    public GameObject HealthBar;
    public GameObject CurrentCanvas;
    // Private variables

    private Camera cam;
    private SpawnController Spawner;
    private Vector3 mousePosition;
    private Vector2 SightPosition;
    private float XPose, YPose;
    private int TimingAsteroids = 3;
    private int TimingZonds = 5;
    private Vector2 viewPortPos;
    private int bufferindex;

    // Public static variables
    public static bool isLifeBlue = false;
    public static int ShipLifeBlue = 3;
    public static bool isLifeGreen = false;
    public static int ShipLifeGreen = 3;

    [HideInInspector] public GameObject[] EnemyArray;

    // Editor Variables
    [SerializeField] private int TargetFrameRate;
    [SerializeField] private bool NeedSpawnZond;
    [SerializeField] private bool NeedSpawnAsteroid;
    [SerializeField] private bool NeedSpawnEnemyStarships;

    [SerializeField] private bool NeedSpawnFirstBoss;
    [SerializeField] private bool NeedSpawnSecondBoss;
    [SerializeField] private bool NeedSpawnFourthBoss;

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
        
        StartCoroutine(RefreshEnemyArray());
        
        if (NeedSpawnEnemyStarships)
            StartCoroutine(SpawnEnemyStarShipCore());
        if (NeedSpawnZond)
            StartCoroutine(SpawnZond());
        if (NeedSpawnAsteroid)
            StartCoroutine(SpawnAsteroids());

        if (NeedSpawnFirstBoss)
            SpawnBoss(1);
        if (NeedSpawnSecondBoss)
            SpawnBoss(2);
        if (NeedSpawnFourthBoss)
            SpawnBoss(4);
    }
    private void Update()
    {       
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        SightPosition = mousePosition - transform.position;
        SightPosition = SightPosition.normalized;
    }
    IEnumerator RefreshEnemyArray()
    {
        while (true)
        {
            EnemyArray = GameObject.FindGameObjectsWithTag("Enemy");
            yield return new WaitForSeconds(0.5f);
        }
    }
    public void ForceRefreshEnemyArray()
    {
        EnemyArray = GameObject.FindGameObjectsWithTag("Enemy");
    }
    public void SpawnBossFromDC()
    {
        //StartCoroutine(Spawnerrr());
        SpawnBoss(bufferindex);
    }
    IEnumerator Spawnerrr()
    {
        Coroutine myC1 = StartCoroutine(SpawnAsteroids());
        Coroutine myC2 = StartCoroutine(SpawnZond());
        Coroutine myC3 = StartCoroutine(SpawnEnemyStarShipCore());

        yield return new WaitForSeconds(60f);
        StopCoroutine(myC1);
        StopCoroutine(myC2);
        StopCoroutine(myC3);

        SpawnBoss(1);

        yield break;
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
    public void SpawnBoss(int index)
    {
        bufferindex = index;
        Vector2 SpawnPos;
        if (index == 1)
        {
            SpawnPos = new Vector2(Random.Range(-1.5f, 1.5f), 5.5f);
            Instantiate(Boss1, SpawnPos, Boss1.transform.rotation);
        }
        else if (index == 2)
        {
            SpawnPos = new Vector2(Random.Range(-1.5f, 1.5f), 5.5f);
            ParentForSecondBoss.GetComponent<SecondBossManager>().SecondBoss1 = Instantiate(SecondBoss1, SpawnPos, SecondBoss1.transform.rotation);
            SpawnPos = new Vector2(Random.Range(-1.5f, 1.5f), -5.5f);
            ParentForSecondBoss.GetComponent<SecondBossManager>().SecondBoss2 = Instantiate(SecondBoss2, SpawnPos, SecondBoss2.transform.rotation);

            Instantiate(ParentForSecondBoss);
        }
        else if (index == 4)
        {
            SpawnPos = new Vector2(Random.Range(-1.5f, 1.5f), 6.3f);
            Instantiate(Boss4, SpawnPos, Boss4.transform.rotation);
        }
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
}