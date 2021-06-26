using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDrone : WeaponPlayer
{
    void FixedUpdate()
    {
        transform.Translate(transform.right * Time.deltaTime * Speed, Space.World);
    }
}
