using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFirst : MonoBehaviour
{
    private Vector2 MovePosition;
    private Vector2 ForcePosition;
    private Rigidbody2D rb2D;
    private float Timer;
    public float hitTime = 2.5f;
    public int HealthPoints;
    public float LeftBorderTimeBtwShots;
    public float RightBorderTimeBtwShots;
    public float speed;
    public GameObject BulletBoss;
    public Transform BulletPoint1;
    public Transform BulletPoint2;
    public Transform BulletPoint3;
    public Transform BulletPoint4;

    private Coroutine MyCoroutine;

    public GameObject FirstPiece;
    public GameObject SecondPiece;
    public GameObject ThirdPiece;
    public GameObject FourthPiece;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        StartCoroutine(Movement());
        HealthPoints = GetComponent<HealthPointsController>().HealthPoints;
    }

    void FixedUpdate()
    {
        hitTime += Time.deltaTime;
        Timer += Time.deltaTime;
        rb2D.AddForce(ForcePosition * speed);

        if (MyCoroutine == null && HealthPoints <= 0) 
            MyCoroutine = StartCoroutine(DestroyController.DestroyBoss(gameObject));
        if (HealthPoints <= 0)
            speed -= 0.1f;
    }

    IEnumerator Movement()
    {
        speed -= 1;
        MovePosition = new Vector2(0, 4.5f);
        ForcePosition = (MovePosition - (Vector2)transform.position).normalized;

        yield return new WaitWhile(() => transform.position.y > 4.5f);
        speed += 1;
        StartCoroutine(Shoot(BulletPoint1));
        StartCoroutine(Shoot(BulletPoint2));
        StartCoroutine(Shoot(BulletPoint3));
        StartCoroutine(Shoot(BulletPoint4));
        while (true)
        {
            MovePosition = new Vector2(Random.Range(-1.3f, 1.3f), Random.Range(3.5f, 4.4f));
            ForcePosition = (MovePosition - (Vector2)transform.position).normalized;
            Timer = 0;
            yield return new WaitWhile(() => (Mathf.Abs(transform.position.x) < 1.7f && transform.position.y < 4.4f && transform.position.y > 3.5f) && Timer < 2f);
        }
    }
    IEnumerator Shoot(Transform BulletPoint)
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(LeftBorderTimeBtwShots, RightBorderTimeBtwShots));
            Instantiate(BulletBoss, BulletPoint.position, BulletBoss.transform.rotation);            
        }
    }
}
