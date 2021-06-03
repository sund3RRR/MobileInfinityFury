using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondBoss2 : MonoBehaviour
{
    public GameObject metka;
    private Vector2 MovePosition;
    private Vector2 ForcePosition;
    private Rigidbody2D rb2D;
    private float Timer;
    public int HealthPoints;
    public float TimeBtwShots;
    public float speed;
    public GameObject BulletBoss;
    public Transform BulletPoint1;

    private Vector2 RotateVector;
    private Vector2 DistanceVector;
    private GameObject Player;
    private float DistanceVectorMulti;
    public float Radius;
    public bool IsClockwise;
    public float RotateSpeed;
    public float InsideForceMultiplier;
    private int LeftCoordinateMulti = -1;
    private int RightCoordinateMulti = 1;

    private bool IsRotate;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        StartCoroutine(MovementDown(1));
    }

    void FixedUpdate()
    {
        Timer += Time.deltaTime;
        if(rb2D)
            rb2D.AddForce(ForcePosition * speed);
    }
    public IEnumerator MovementDown(float Mul)
    {
        speed -= 1;
        MovePosition = new Vector2(0, -4.5f);
        ForcePosition = (MovePosition - (Vector2)transform.position).normalized * Mul;
        
        while (transform.position.y < -4.6f || transform.position.y > -4.5f)
        {
            float rotate = Mathf.Atan2(ForcePosition.y, ForcePosition.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(rotate, Vector3.forward), 0.1f);
            yield return null;
        }   
        while (transform.rotation != Quaternion.AngleAxis(Mathf.Atan2(Vector2.up.y, Vector2.up.x) * Mathf.Rad2Deg, Vector3.forward))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(Mathf.Atan2(Vector2.up.y, Vector2.up.x) * Mathf.Rad2Deg, Vector3.forward), 0.1f);
            yield return null;
        }
        
        speed += 4;
        StartCoroutine(Shoot(BulletPoint1, -90));
        while (true)
        {
            MovePosition = new Vector2(Random.Range(1.4f, 1.9f) * Mathf.Pow(-1, Random.Range(1, 3)), Random.Range(-4f, -4.6f));
            ForcePosition = (MovePosition - (Vector2)transform.position).normalized;
            Timer = 0;
            yield return new WaitWhile(() => (Mathf.Abs(transform.position.x) < 1.9f && transform.position.y > -4.6f && transform.position.y < -4f) && Timer < 2f);
        }
    }
    public IEnumerator Movement(float Mul)
    {
        speed -= 1;
        MovePosition = new Vector2(0, 4.5f);
        ForcePosition = (MovePosition - (Vector2)transform.position).normalized * Mul;

        while (transform.position.y > 4.6f || transform.position.y < 4.5f)
        {
            float rotate = Mathf.Atan2(ForcePosition.y, ForcePosition.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(rotate, Vector3.forward), 0.1f);
            yield return null;
        }
        while (transform.rotation != Quaternion.AngleAxis(Mathf.Atan2(Vector2.down.y, Vector2.down.x) * Mathf.Rad2Deg, Vector3.forward))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(Mathf.Atan2(Vector2.down.y, Vector2.down.x) * Mathf.Rad2Deg, Vector3.forward), 0.1f);
            yield return null;
        }

        speed += 4;
        StartCoroutine(Shoot(BulletPoint1, 90));
        while (true)
        {
            MovePosition = new Vector2(Random.Range(1.4f, 1.9f) * Mathf.Pow(-1, Random.Range(1, 3)), Random.Range(4f, 4.6f));
            ForcePosition = (MovePosition - (Vector2)transform.position).normalized;
            Timer = 0;
            yield return new WaitWhile(() => (Mathf.Abs(transform.position.x) < 1.9f && transform.position.y < 4.6f && transform.position.y > 4f) && Timer < 2f);
        }
    }
    IEnumerator Shoot(Transform BulletPoint, float Angle)
    {
        while (true)
        {
            yield return new WaitForSeconds(TimeBtwShots);
            Instantiate(BulletBoss, BulletPoint.position, Quaternion.AngleAxis(Angle, Vector3.forward));
        }
    }
    public IEnumerator Turbo(GameObject Target, float ForceSpeed)
    {
        Destroy(GetComponent<Rigidbody2D>());
        GetComponent<HealthPointsController>().enabled = !enabled;
        Vector2 StartPos = Target.transform.position;
        transform.SetParent(Target.transform);
        Vector2 Baseforce = new Vector2(StartPos.x - Radius, StartPos.y) - (Vector2)transform.position, force = Baseforce;
        float speed = 0.2f;
        while (force.sqrMagnitude > 0.01f)
        {
            if (Baseforce.magnitude / 2f - (Mathf.Abs(Baseforce.magnitude) - Mathf.Abs(force.magnitude)) > 0)
                speed += 0.1f;
            else if (speed > RotateSpeed)
                speed -= 0.1f;

            transform.Translate(Baseforce.normalized * ForceSpeed * speed * Time.deltaTime, Space.World);
            float rotate = Mathf.Atan2(force.y, force.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(rotate, Vector3.forward), 0.05f);

            force = new Vector2(StartPos.x - Radius, StartPos.y) - (Vector2)transform.position;

            yield return null;
        }
        while (true)
        {
            DistanceVector = Target.transform.position - transform.position;
            RotateVector = new Vector2(LeftCoordinateMulti * DistanceVector.y, RightCoordinateMulti * DistanceVector.x).normalized;
            if (DistanceVector.magnitude - Radius > 0)
            {
                if (DistanceVector.magnitude - Radius < 2)
                    DistanceVectorMulti = DistanceVector.magnitude - Radius;
                else
                    DistanceVectorMulti = 2;
            }
            else if (DistanceVector.magnitude - Radius < 0)
            {
                DistanceVector = -DistanceVector;
                DistanceVectorMulti = (Radius - DistanceVector.magnitude) * InsideForceMultiplier;
            }
            force = RotateVector * RotateSpeed + DistanceVector.normalized * DistanceVectorMulti;
            transform.Translate(force * Time.deltaTime, Target.transform);

            float rotate = Mathf.Atan2(force.y, force.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(rotate, Vector3.forward), 0.1f);

            yield return null;
        }
    }
    public void ResetDefault()
    {
        StopAllCoroutines();
        GetComponent<HealthPointsController>().enabled = enabled;

        StartCoroutine(MovementDown(2f));
    }
    public void SwapToUp()
    {
        StopAllCoroutines();
        StartCoroutine(Movement(2f));
    }
}
