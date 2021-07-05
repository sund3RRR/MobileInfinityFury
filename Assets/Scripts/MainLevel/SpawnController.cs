using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    // Editor variables
    public GameObject Asteroid_01;
    public GameObject Asteroid_02;
    public GameObject Asteroid_03;
    public GameObject Asteroid_04;
    public GameObject Asteroid_05;
    public GameObject Asteroid_06;
    public GameObject Asteroid_07;
    public GameObject Asteroid_08;
    public GameObject Asteroid_09;
    public GameObject Asteroid_10;
    public GameObject Asteroid_11;
    public GameObject Asteroid_12;
    public GameObject Asteroid_13;
    public GameObject Asteroid_14;
    public GameObject Asteroid_15;

    public GameObject Ship;
    public GameObject ShipGamepad;
    public GameObject Zond;
    public GameObject Sputnik;
    public GameObject GoldAsteroid;
    
    // Private variables
    private GameObject[] BigAsteroidsArray = new GameObject[3];
    private GameObject[] MediumAsteroidsArray = new GameObject[4];
    private GameObject[] SmallAsteroidsArray = new GameObject[4];
    private GameObject[] SmallestAsteroidsArray = new GameObject[4];

    private Vector2 forcePosition;
    private Vector2 MovePosition;
    private Vector2 force;
    private float speed;
    private int Angle;
    private double CountOfAsteroids;
    private int i;
    private float PosX, PosY;

    void Awake()
    {
        BigAsteroidsArray[0] = Asteroid_01;
        BigAsteroidsArray[1] = Asteroid_02;
        BigAsteroidsArray[2] = Asteroid_03;
        MediumAsteroidsArray[0] = Asteroid_04;
        MediumAsteroidsArray[1] = Asteroid_05;
        MediumAsteroidsArray[2] = Asteroid_06;
        MediumAsteroidsArray[3] = Asteroid_07;
        SmallAsteroidsArray[0] = Asteroid_08;
        SmallAsteroidsArray[1] = Asteroid_09;
        SmallAsteroidsArray[2] = Asteroid_10;
        SmallAsteroidsArray[3] = Asteroid_11;
        SmallestAsteroidsArray[0] = Asteroid_12;
        SmallestAsteroidsArray[1] = Asteroid_13;
        SmallestAsteroidsArray[2] = Asteroid_14;
        SmallestAsteroidsArray[3] = Asteroid_15;
    }
    public void SpawnAsteroid(Vector2 defaultPos, int AsteroidIndex, bool isRespawn, Vector2 defaultforce)
    {
        GameObject NewAsteroid = Asteroid_01;

        switch(AsteroidIndex)
        {
            case 3:
                NewAsteroid = BigAsteroidsArray[Random.Range(0, 3)];
                break;
            case 2:
                NewAsteroid = MediumAsteroidsArray[Random.Range(0, 4)];
                break;
            case 1:
                NewAsteroid = SmallAsteroidsArray[Random.Range(0, 4)];
                break;
            case 0:
                NewAsteroid = SmallestAsteroidsArray[Random.Range(0, 4)];
                break;
            default:
                break;
        }

        NewAsteroid.GetComponent<AsteroidController>().Torque = Random.Range(-1f, 1f);
        if (isRespawn)
        {
            force = new Vector2(defaultforce.x + Random.Range(-1f, 1f), defaultforce.y + Random.Range(-1f, 1f));            
        }
        else
        {
            forcePosition = new Vector2(Random.Range(-2f, 2f), -4.5f);
            MovePosition = (forcePosition - defaultPos).normalized;
            speed = Random.Range(1, 2);
            force = MovePosition * speed;
        }

        NewAsteroid.GetComponent<AsteroidController>().force = force;
        Angle = Random.Range(0, 359);
        NewAsteroid.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        Instantiate(NewAsteroid, defaultPos, Quaternion.AngleAxis(Angle, Vector3.forward));
    }
    public void RespawnAsteroids(Vector2 defaultPos, int AsteroidIndex, Vector2 defaultforce)
    {
        i = Random.Range(0, AsteroidIndex - 1); // рандомное определение размера астероидов
        CountOfAsteroids = Mathf.Pow(2, AsteroidIndex - i); // определение их количества, отталкиваясь от размера

        while (CountOfAsteroids > 0)
        {
            PosX = defaultPos.x + Random.Range(-0.2f, 0.2f);
            PosY = defaultPos.y + Random.Range(-0.2f, 0.2f);
            Vector2 SpawnPosition = new Vector2(PosX, PosY);

            SpawnAsteroid(SpawnPosition, i, true, defaultforce);

            CountOfAsteroids--;
        }
    }
    public void SpawnGoldAsteroid(Vector2 defaultPos)
    {
        forcePosition = new Vector2(Random.Range(-7f, 7f), Random.Range(-3f, 3f));
        MovePosition = (forcePosition - defaultPos).normalized;
        speed = Random.Range(1, 2);
        force = MovePosition * speed;

        GoldAsteroid.GetComponent<GoldAsteroidController>().Torque = Random.Range(-1f, 1f);
        GoldAsteroid.GetComponent<GoldAsteroidController>().force = force;
        Angle = Random.Range(0, 359);

        Instantiate(GoldAsteroid, defaultPos, Quaternion.AngleAxis(Angle, Vector3.forward));
    }
    public void SpawnSpaceShipBlue()
    {
        // JOYSTICK CONTROL
        /*
        Ship.GetComponent<HeroController>().moveJoy = GameObject.FindGameObjectWithTag("MoveJoystick");
        Ship.GetComponent<HeroController>().fireJoy = GameObject.FindGameObjectWithTag("FireJoystick");
        */
        // JOYSTICK CONTROL
        Instantiate(Ship, new Vector2(0, 0), Quaternion.AngleAxis(90, Vector3.forward));
        SceneController.ShipLifeBlue -= 1;
        SceneController.isLifeBlue = true;

        StartCoroutine(GetComponent<SceneController>().DestroyerCenterEnemies());
    }
    public void SpawnSpaceShipGreen()
    {
        // JOYSTICK CONTROL
        /*
        Ship.GetComponent<HeroController>().moveJoy = GameObject.FindGameObjectWithTag("MoveJoystick");
        Ship.GetComponent<HeroController>().fireJoy = GameObject.FindGameObjectWithTag("FireJoystick");
        */
        // JOYSTICK CONTROL
        Instantiate(ShipGamepad, new Vector2(0, 0), Quaternion.AngleAxis(90, Vector3.forward));
        SceneController.ShipLifeGreen -= 1;
        SceneController.isLifeGreen = true;

        StartCoroutine(GetComponent<SceneController>().DestroyerCenterEnemies());
    }
    public void SpawnZond(Vector2 defaultPos)
    {
        forcePosition = new Vector2(Random.Range(-2f, 2f), -4.5f);
        MovePosition = (forcePosition - defaultPos).normalized;

        Zond.GetComponent<ZondController>().force = MovePosition;
        Zond.GetComponent<ZondController>().Torque = Random.Range(-1f, 1f);
        Instantiate(Zond, defaultPos, Quaternion.identity);
    }
    public void SpawnSputnik(Vector2 defaultPos)
    {
        forcePosition = new Vector2(Random.Range(-2f, 2f), -4.5f);
        MovePosition = (forcePosition - defaultPos).normalized;

        Sputnik.GetComponent<SputnikController>().MovePosition = MovePosition;
        Sputnik.GetComponent<SputnikController>().Torque = Random.Range(-0.5f, 0.5f);
        Instantiate(Sputnik, defaultPos, Quaternion.identity);
    }
}