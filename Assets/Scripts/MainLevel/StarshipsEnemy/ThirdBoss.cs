using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdBoss : MonoBehaviour
{
    [SerializeField] private GameObject TargetVFX;
    [SerializeField] private GameObject BulletBoss;
    [SerializeField] private Transform BulletPos1;
    [SerializeField] private Transform BulletPos2;
    [SerializeField] private float DefaultRotateSpeed;
    [SerializeField] private float DefaultSpeed;
    [SerializeField] private float SpeedRam;
    [SerializeField] private float TimeBtwBulletShots;
    [SerializeField] private float TimeBtwBulletBurst;
    [SerializeField] private float TimePrepareToRam;
    [SerializeField] private int CountOfBullets;
    private float Timer;
    private Vector2 MovePosition;
    private GameObject Player;
    private float SampleY;

    private Coroutine ShootCoroutine;
    private Coroutine MovementCoroutine;
    private Coroutine RotateCoroutine;

    void Start()
    {
        StartCoroutine(MovementOnStart());
        Player = GameObject.FindGameObjectWithTag("PlayerBlue");
    }

    void FixedUpdate()
    {
        Timer += Time.deltaTime;
    }
    IEnumerator MovementOnStart()
    {
        Vector2 MovePosition = new Vector2(0, 4.5f);
        SampleY = 3.5f;
        while(transform.position.y > 4.6f)
        {
            transform.position = Vector2.Lerp(transform.position, MovePosition, Time.deltaTime);
            yield return null;
        }
        MovementCoroutine = StartCoroutine(Movement());
        RotateCoroutine = StartCoroutine(RotateObject(Player));
        ShootCoroutine = StartCoroutine(Shoot());
    }
    IEnumerator Movement()
    {
        MovePosition = transform.position;
        Timer = 0;
        while(Timer < 10f)
        {
            if ((MovePosition - (Vector2)transform.position).sqrMagnitude < 0.1f)
            {
                MovePosition = new Vector2(Random.Range(-1.5f, 1.5f), Random.Range(SampleY - 1.5f, SampleY + 1.5f));
                
                if (MovePosition.y > 4.5f)
                    MovePosition.y = 4.5f;
                else if (MovePosition.y < -4.4f)
                    MovePosition.y = -4.4f;
            }
            transform.position = Vector2.Lerp(transform.position, MovePosition, Time.deltaTime * DefaultSpeed);
            yield return null;
        }
        while((MovePosition - (Vector2)transform.position).sqrMagnitude > 0.1f)
        {
            transform.position = Vector2.Lerp(transform.position, MovePosition, Time.deltaTime * DefaultSpeed);
            yield return null;
        }
        StartCoroutine(RAM());
    }
    IEnumerator RotateObject(GameObject Target)
    {
        Vector2 ForcePosition;
        while(gameObject)
        {
            ForcePosition = (Vector2)Target.transform.position - MovePosition;
            float rotate = Mathf.Atan2(ForcePosition.y, ForcePosition.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(rotate, Vector3.forward), Time.deltaTime * DefaultRotateSpeed);

            yield return null;
        }
    }
    IEnumerator Shoot()
    {
        int k = 0;
        while(gameObject)
        {
            Instantiate(BulletBoss, BulletPos1.position, transform.rotation);
            Instantiate(BulletBoss, BulletPos2.position, transform.rotation);
            k++;

            yield return new WaitForSeconds(TimeBtwBulletShots);

            if (k == CountOfBullets)
            {
                k = 0;
                yield return new WaitForSeconds(TimeBtwBulletBurst);
            }
        }
    }
    IEnumerator RAM()
    {
        
        StopCoroutine(MovementCoroutine);
        StopCoroutine(ShootCoroutine);
        StopCoroutine(RotateCoroutine);
        GameObject NewTargetVFX = Instantiate(TargetVFX, Player.transform.position, Quaternion.identity);
        Destroy(NewTargetVFX, TimePrepareToRam + 1);
        RotateCoroutine = StartCoroutine(RotateObject(NewTargetVFX));
        yield return new WaitForSeconds(TimePrepareToRam);
        StopCoroutine(RotateCoroutine);
        while(transform.position != NewTargetVFX.transform.position)
        {
            transform.position = Vector2.Lerp(transform.position, NewTargetVFX.transform.position, Time.deltaTime * SpeedRam);
            SampleY = transform.position.y;
            yield return null;
        }
        ShootCoroutine = StartCoroutine(Shoot());
        MovementCoroutine = StartCoroutine(Movement());
        RotateCoroutine = StartCoroutine(RotateObject(Player));
    }
}
