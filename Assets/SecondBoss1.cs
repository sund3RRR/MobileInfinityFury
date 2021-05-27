using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondBoss1 : MonoBehaviour
{
    [SerializeField] private GameObject Parent;
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

    private Coroutine myCoroutine;
    private Coroutine MoveCoroutine;
    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        MoveCoroutine = StartCoroutine(Movement());
    }

    void FixedUpdate()
    {
        Timer += Time.deltaTime;
        if (rb2D)
            rb2D.AddForce(ForcePosition * speed);
        if (myCoroutine == null && GetComponent<HealthPointsController>().HealthPoints <= 15)
        {
            StopAllCoroutines();
            myCoroutine = StartCoroutine(StartTurbo());
        }
    }
    IEnumerator Movement()
    {
        speed -= 1;
        MovePosition = new Vector2(0, 4.5f);
        ForcePosition = (MovePosition - (Vector2)transform.position).normalized;

        yield return new WaitWhile(() => transform.position.y > 4.5f);
        speed += 1;
        StartCoroutine(Shoot(BulletPoint1));
        while (true)
        {
            MovePosition = new Vector2(Random.Range(1.4f, 1.9f)* Mathf.Pow(-1, Random.Range(1, 3)), Random.Range(4f, 4.6f));
            ForcePosition = (MovePosition - (Vector2)transform.position).normalized;
            Timer = 0;
            yield return new WaitWhile(() => (Mathf.Abs(transform.position.x) < 1.9f && transform.position.y < 4.6f && transform.position.y > 4f) && Timer < 2f);
        }
    }
    IEnumerator Shoot(Transform BulletPoint)
    {
        while (true)
        {
            yield return new WaitForSeconds(TimeBtwShots);
            Instantiate(BulletBoss, BulletPoint.position, Quaternion.AngleAxis(90, Vector3.forward));
        }
    }
    IEnumerator StartTurbo()
    {
        Destroy(GetComponent<Rigidbody2D>());
        Vector2 StartPos = GameObject.FindGameObjectWithTag("PlayerBlue").transform.position;
        GameObject NewParent = Instantiate(Parent, StartPos, Quaternion.identity);
        transform.SetParent(NewParent.transform);
        Vector2 Baseforce = new Vector2(StartPos.x + Radius, StartPos.y) - (Vector2)transform.position, force = Baseforce;
        float speed = 0.2f;
        while (force.sqrMagnitude > 0.1f)
        {
            if (Baseforce.magnitude / 2f - (Baseforce.magnitude - force.magnitude) > 0)
                speed += 0.05f;
            else if (speed > 5)
                speed -= 0.05f;
            transform.Translate(force.normalized * Time.deltaTime * speed, Space.World);

            float rotate = Mathf.Atan2(force.y, force.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(rotate, Vector3.forward), 0.05f);

            force = new Vector2(StartPos.x + Radius, StartPos.y) - (Vector2)transform.position;

            yield return null;
        }
        while (true)
        {
            DistanceVector = NewParent.transform.position - transform.position;
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
            transform.Translate(force * Time.deltaTime, NewParent.transform);

            float rotate = Mathf.Atan2(force.y, force.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(rotate, Vector3.forward), 0.1f);

            yield return null;
        }
    }
}
