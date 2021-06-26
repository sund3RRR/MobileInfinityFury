using UnityEngine;

public class Bullet : WeaponPlayer
{ 
    void FixedUpdate()
    {
        transform.Translate(transform.right * Time.deltaTime * Speed, Space.World);
    }
}