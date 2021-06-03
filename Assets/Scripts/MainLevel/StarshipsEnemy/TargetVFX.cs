using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetVFX : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField] private float LifeTime;
    private float Timer;
    private GameObject Player;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("PlayerBlue");
    }

    void FixedUpdate()
    {
        Timer += Time.deltaTime;
        if (Timer < LifeTime)
            transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, Time.deltaTime * Speed);
    }
}
