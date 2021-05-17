using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VFXQControl : MonoBehaviour
{
    public GameObject Bullet;
    public GameObject Rocket;
    public GameObject AsteroidHit;
    public GameObject DefaultHit;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(TaskOnClick);
    }

    void Update()
    {
        
    }
    void TaskOnClick()
    {
        switch(gameObject.name)
        {
            case "Low":
                Bullet.GetComponent<Bullet>().AsteroidHit = DefaultHit;
                Rocket.GetComponent<RocketController>().AsteroidHit = DefaultHit;
                break;
            case "High":
                Bullet.GetComponent<Bullet>().AsteroidHit = AsteroidHit;
                Rocket.GetComponent<RocketController>().AsteroidHit = AsteroidHit;
                break;
        }
    }
}
