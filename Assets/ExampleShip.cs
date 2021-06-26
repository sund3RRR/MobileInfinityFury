using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleShip : MonoBehaviour
{
    public GameObject CurrentBullet;
    public GameObject CurrentRocket;
    public GameObject CurrentDrone;
    public Transform Panel;
    private float TimerTimeBtwBulletShots;
    private float TimerTimeBtwRocketShots;
    private float TimerDrones;
    private float TimeBtwDroneSpawn = 3f;
    private float TimeBtwBulletShots = 0.2f;
    private float TimeBtwRocketShots = 0.2f;

    private float[] DroneRadiusArray;
    private GameObject[] Drones;

    void Start()
    {
        CurrentBullet.GetComponent<WeaponPlayer>().Speed = 8;
        DroneRadiusArray = new float[5];
        Drones = new GameObject[5];
        for (int i = 0; i < 5; i++)
        {
            DroneRadiusArray[i] = 0.8f + 0.5f * i;
        }
    }

    void FixedUpdate()
    {
        TimerTimeBtwBulletShots += Time.deltaTime;
        TimerTimeBtwRocketShots += Time.deltaTime;
        //TimerDrones += Time.deltaTime;
        Shoot();
        SpawnDrone();
    }
    public void Shoot()
    {/*
        if (TimerTimeBtwBulletShots >= TimeBtwBulletShots)
        {
            Instantiate(CurrentBullet, new Vector2(transform.position.x - 0.1f, transform.position.y), transform.rotation);
            Instantiate(CurrentBullet, new Vector2(transform.position.x + 0.1f, transform.position.y), transform.rotation);

            GameObject NewBullet = Instantiate(CurrentBullet, new Vector2(transform.position.x - 0.2f, transform.position.y - 0.1f),
                Quaternion.AngleAxis(105, Vector3.forward));
            NewBullet.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);

            NewBullet = Instantiate(CurrentBullet, new Vector2(transform.position.x + 0.2f, transform.position.y - 0.1f),
                Quaternion.AngleAxis(75, Vector3.forward));
            NewBullet.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            TimerTimeBtwBulletShots = 0;
        }*/
        if (TimerTimeBtwRocketShots >= TimeBtwRocketShots)
        {
            TimerTimeBtwRocketShots = 0;
            GameObject NewRocket = Instantiate(CurrentRocket, transform.position, transform.rotation);
            NewRocket.transform.SetParent(Panel);
        }
    }
    void SpawnDrone()
    {
        if (TimerDrones > TimeBtwDroneSpawn)
        {
            for (int i = 0; i < 5; i++)
            {
                if (!Drones[i])
                {
                    TimerDrones = 0;
                    CurrentDrone.GetComponent<Drone>().IsClockwise = i % 2 == 0 ? true : false;
                    CurrentDrone.GetComponent<Drone>().Radius = DroneRadiusArray[i];
                    Drones[i] = Instantiate(CurrentDrone, new Vector2(transform.position.x, transform.position.y + 0.01f), Quaternion.identity);
                    break;
                }
            }
        }
    }
}
