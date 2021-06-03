using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public Transform Parent;

    [SerializeField] private GameObject BulletVFX;

    void FixedUpdate()
    {
        if (Parent)
            transform.position = Parent.position;
        else
            Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {      
        if(collision.tag == "BulletDrone" || collision.tag == "Bullet" || collision.tag == "Rocket")
        {
            Instantiate(BulletVFX, collision.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
        }
        else if (collision.tag == "Drone")
        {
            Instantiate(collision.GetComponent<Drone>().DeadVFX, collision.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
        }
    }
}
