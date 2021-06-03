using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonFourthBoss : MonoBehaviour
{
    // Editor variables  
    [SerializeField] private GameObject BulletSummon;
    [SerializeField] private Transform BulletPoint;
    [SerializeField] private float speed;
    [SerializeField] private float TimeBtwShots;
    [SerializeField] private float TimeBtwSummonMove;
    [SerializeField] private float FrontForceMulti;

    //private variables
    private Vector2 ForcePosition;
    private Vector2 FrontForce;
    private GameObject Target;

    //public variables
    [HideInInspector] public Vector2 StartPosition;
    [HideInInspector] public float StartSpeed;
    
    void Start()
    {
        Target = GameObject.FindGameObjectWithTag("PlayerBlue");
        ForcePosition = StartPosition.normalized * StartSpeed;
        StartCoroutine(Movement());
    }

    void FixedUpdate()
    {
        transform.Translate(ForcePosition * Time.deltaTime * speed, Space.World);
    }
    IEnumerator Movement()
    {
        StartCoroutine(Shoot());

        yield return new WaitForSeconds(0.3f);
        StartCoroutine(ChangeForce());

        while(gameObject)
        {
            if (Target && (Target.transform.position - transform.position).sqrMagnitude > 1f)
            {
                ForcePosition = (Vector2)(Target.transform.position - transform.position).normalized + FrontForce;
            }
            else if ((Target.transform.position - transform.position).sqrMagnitude < 1f)
                ForcePosition = (Target.transform.position - transform.position).normalized;
            else
                Destroy(gameObject);

            yield return null;
        }
    }
    IEnumerator ChangeForce()
    {
        while(gameObject)
        {
            FrontForce = Mathf.Pow(-1, Random.Range(1, 3)) * FrontForceMulti * transform.up;

            yield return new WaitForSeconds(TimeBtwSummonMove);
        }
    }
    IEnumerator Shoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(TimeBtwShots);
            Instantiate(BulletSummon, BulletPoint.position, Quaternion.AngleAxis(90, Vector3.forward));
        }
    }
}
