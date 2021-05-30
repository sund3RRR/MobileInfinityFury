using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondBoss : MonoBehaviour
{
    public GameObject BulletBoss;
    public Transform BulletPoint1;

    public float DefaultSpeed;
    public float DefaultRotateSpeed;
    public float AccelerationStepBeforeTurbo;
    public float SwapToUpSpeed;
    public float TurboSpeed;
    public float Radius;
    public float TimeBtwShots;
    public float InsideForceMultiplier;

    private Vector2 MovePosition;
    private Vector2 ForcePosition;
    private float Timer;
    
    private Vector2 RotateVector;
    private Vector2 DistanceVector;
    private float DistanceVectorMulti;
    
    private int LeftCoordinateMulti = -1;
    private int RightCoordinateMulti = 1;
    private bool blyat;

    void Awake()
    {
        if (GetComponent<HealthPointsController>().GameObjectName == "SecondBoss1")
            StartCoroutine(Movement(1));
        else
            StartCoroutine(MovementDown(1));
    }

    void FixedUpdate()
    {
        Timer += Time.deltaTime;
    }
    private void Update()
    {
        if (blyat)
            transform.position = Vector2.Lerp(transform.position, MovePosition, Time.deltaTime * DefaultSpeed);
    }
    public IEnumerator MovementDown(float Mul)
    {
        DefaultSpeed = Mul;
        MovePosition = new Vector2(0, -4.5f);
        ForcePosition = (MovePosition - (Vector2)transform.position).normalized;

        while (transform.position.y < -4.6f || transform.position.y > -4.4f)
        {
            blyat = true;
            float rotate = Mathf.Atan2(ForcePosition.y, ForcePosition.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(rotate, Vector3.forward), Time.deltaTime * DefaultRotateSpeed);
            yield return null;
        }
        while (transform.rotation != Quaternion.AngleAxis(Mathf.Atan2(Vector2.up.y, Vector2.up.x) * Mathf.Rad2Deg, Vector3.forward))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(Mathf.Atan2(Vector2.up.y, Vector2.up.x)
                * Mathf.Rad2Deg, Vector3.forward), Time.deltaTime * DefaultRotateSpeed);
            if (Quaternion.Angle(transform.rotation, Quaternion.AngleAxis(Mathf.Atan2(Vector2.up.y, Vector2.up.x) * Mathf.Rad2Deg, Vector3.forward)) < 0.02f)
                transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(Vector2.up.y, Vector2.up.x) * Mathf.Rad2Deg, Vector3.forward);
            yield return null;
        }
        StartCoroutine(Shoot(BulletPoint1, -90));
        while (true)
        {
            MovePosition = new Vector2(Random.Range(1.4f, 1.9f) * Mathf.Pow(-1, Random.Range(1, 3)), Random.Range(-4f, -4.6f));
            Timer = 0;
            yield return new WaitWhile(() => (Mathf.Abs(transform.position.x) < 1.9f && 
            transform.position.y > -4.6f && transform.position.y < -4f) && Timer < 2f);   
        }
    }
    public IEnumerator Movement(float Mul)
    {
        DefaultSpeed = Mul;
        MovePosition = new Vector2(0, 4.5f);
        ForcePosition = (MovePosition - (Vector2)transform.position).normalized;
        
        while (transform.position.y > 4.6f || transform.position.y < 4.4f)
        {
            blyat = true;
            float rotate = Mathf.Atan2(ForcePosition.y, ForcePosition.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(rotate, Vector3.forward), Time.deltaTime * DefaultRotateSpeed);
            yield return null;
        }
        while (transform.rotation != Quaternion.AngleAxis(Mathf.Atan2(Vector2.down.y, Vector2.down.x) * Mathf.Rad2Deg, Vector3.forward))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(Mathf.Atan2(Vector2.down.y, Vector2.down.x)
                * Mathf.Rad2Deg, Vector3.forward), Time.deltaTime * DefaultRotateSpeed);
            if (Quaternion.Angle(transform.rotation, Quaternion.AngleAxis(Mathf.Atan2(Vector2.down.y, Vector2.down.x) * Mathf.Rad2Deg, Vector3.forward)) < 0.02f)
                transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(Vector2.down.y, Vector2.down.x) * Mathf.Rad2Deg, Vector3.forward);
            yield return null;
        }
        StartCoroutine(Shoot(BulletPoint1, 90));
        while (true)
        {
            MovePosition = new Vector2(Random.Range(1.4f, 1.9f)* Mathf.Pow(-1, Random.Range(1, 3)), Random.Range(4f, 4.6f));
            //transform.position = Vector2.Lerp(transform.position, MovePosition, Time.deltaTime);
            Timer = 0;
            yield return new WaitWhile(() => (Mathf.Abs(transform.position.x) < 1.9f && transform.position.y < 4.6f && transform.position.y > 4f) && Timer < 1f);
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
    public IEnumerator Turbo(GameObject Target, float ForceSpeed, bool IsSecondBoss2)
    {
        int radiusMul = 1;
        if (IsSecondBoss2)
            radiusMul = -1;
        blyat = false;
        GetComponent<HealthPointsController>().enabled = !enabled;
        Vector2 StartPos = Target.transform.position;
        transform.SetParent(Target.transform);
        Vector2 Baseforce = new Vector2(StartPos.x + radiusMul * Radius, StartPos.y) - (Vector2)transform.position, force = Baseforce;
        float speed = 0.2f;
        while (force.sqrMagnitude > 0.01f)
        {
            if (Baseforce.magnitude / 2f - (Mathf.Abs(Baseforce.magnitude) - Mathf.Abs(force.magnitude)) > 0)
                speed += AccelerationStepBeforeTurbo;
            else if (speed > TurboSpeed)
                speed -= AccelerationStepBeforeTurbo;
            transform.Translate(Baseforce.normalized * ForceSpeed * speed * Time.deltaTime, Space.World);
            
            float rotate = Mathf.Atan2(force.y, force.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(rotate, Vector3.forward), 0.1f);

            force = new Vector2(StartPos.x + radiusMul * Radius, StartPos.y) - (Vector2)transform.position;

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
            force = RotateVector * TurboSpeed + DistanceVector.normalized * DistanceVectorMulti;
            transform.Translate(force * Time.deltaTime, Target.transform);

            float rotate = Mathf.Atan2(force.y, force.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(rotate, Vector3.forward), 0.1f);

            yield return null;
        }
    }
    public void ResetDefault(bool IsSecondBoss2)
    {
        StopAllCoroutines();
        GetComponent<HealthPointsController>().enabled = enabled;
        if (IsSecondBoss2)
            StartCoroutine(MovementDown(DefaultSpeed));
        else
            StartCoroutine(Movement(DefaultSpeed));
    }
    public void SwapToUp()
    {
        StopAllCoroutines();
        StartCoroutine(Movement(SwapToUpSpeed));
    }
}
