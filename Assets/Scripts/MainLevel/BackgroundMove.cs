using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
    void FixedUpdate()
    {
        transform.Translate(-Vector3.up * Time.deltaTime);

        if (transform.position.y < -10)
            transform.position = new Vector2(0, 10);
    }
}
