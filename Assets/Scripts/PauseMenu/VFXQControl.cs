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
    }
}
