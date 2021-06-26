using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimEnemyController : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private float TimeBetweenBullets;
    [SerializeField] private int CountOfBullets;
    [SerializeField] private int RamAngle;
    [SerializeField] private float SpeedMultiplierRam;
    [SerializeField] private float baseSpeed;

    private Rigidbody2D rb2D;
    private bool rotateObj = true;
    private GameObject Target;
    private Vector2 force;   
    private Vector2 torotate;
    private Vector2 fromrotate;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        Target = GameObject.FindGameObjectWithTag("PlayerBlue");

        StartCoroutine(ShootToTarget());
        Vector2 NewForce = new Vector2(Random.Range(-1.5f, 1.5f), Random.Range(3f, 4.5f));;
        force = (NewForce - (Vector2)transform.position).normalized;
    }
    void FixedUpdate()
    {
        rb2D.AddForce(force * baseSpeed);
        rb2D.AddTorque(RotateObjToTarget());
    }
    IEnumerator ShootToTarget()
    {
        int k = 0;
        yield return new WaitForSeconds(0.5f);
        while (k < CountOfBullets)
        {
            k++;
            bullet.GetComponent<BulletEnemy>().Parent = gameObject;
            Instantiate(bullet, transform.position, Quaternion.identity);

            yield return new WaitForSeconds(TimeBetweenBullets);
        }

        StartCoroutine(RamTarget());
    }
    IEnumerator RamTarget()
    {
        while(true)
        {           
            if (Target && Vector2.Angle(transform.right, Target.transform.position - transform.position) < RamAngle)
            {
                force = (Target.transform.position - transform.position).normalized * 0.1f;
                force = force.normalized;
                baseSpeed += Time.deltaTime * SpeedMultiplierRam;

                yield return null;
            }
            else if (Target)
            {
                yield return new WaitForSeconds(0.5f);
                DestroyController.DestroySlimEnemy(gameObject);
                yield break;
            }
        }
    }   
    float RotateObjToTarget()
    {
        if (rotateObj)
        {
            torotate = Target.transform.position - transform.position;
            fromrotate = transform.right;

            return Vector2.SignedAngle(fromrotate, torotate);
        }
        else
            return 0;
    }
}
