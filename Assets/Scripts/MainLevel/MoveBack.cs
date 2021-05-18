using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBack : MonoBehaviour
{
    void FixedUpdate()
    {
        transform.Translate(-Vector3.up * Time.deltaTime);
    }
}
