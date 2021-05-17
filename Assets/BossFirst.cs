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
    private GameObject HealthBar;
    public int HealthPoints;
    public int BaseHealthPoints;
    public float LeftBorderTimeBtwShots;
    public float RightBorderTimeBtwShots;
    public GameObject Bar;
    public float speed;
    public GameObject BulletBoss;
    public Transform BulletPoint1;
    public Transform BulletPoint2;
    public Transform BulletPoint3;
    public Transform BulletPoint4;

    private Coroutine MyCoroutine;

    void Awake()
    {
        GameObject Canvas = GameObject.FindGameObjectWithTag("Canvas");
        Bar.GetComponent<HealthBarController>().Target = gameObject;
        Bar.GetComponent<HealthBarController>().BaseHealthPoints = BaseHealthPoints;
        HealthBar = Instantiate(Bar, new Vector3(transform.position.x, transform.position.y), Quaternion.identity);
        HealthBar.transform.SetParent(Canvas.transform, false);

        rb2D = GetComponent<Rigidbody2D>();
        StartCoroutine(Movement());
    }

    void FixedUpdate()
    {
        hitTime += Time.deltaTime;
        Timer += Time.deltaTime;
        rb2D.AddForce(ForcePosition * speed);

        if (HealthBar && hitTime > 2.5f)
        {
            HealthBar.GetComponent<HealthBarController>().DisableHealthBar();
        }
        else if (HealthBar)
        {
            HealthBar.GetComponent<HealthBarController>().HealthPoints = HealthPoints;
            HealthBar.GetComponent<HealthBarController>().EnableHealthBar();
        }
        if (MyCoroutine == null && HealthPoints <= 0) 
            MyCoroutine = StartCoroutine(DestroyController.DestroyBoss(gameObject));
    }

    IEnumerator Movement()
    {
        MovePosition = new Vector2(0, 4.5f);
        ForcePosition = (MovePosition - (Vector2)transform.position).normalized;

        yield return new WaitWhile(() => transform.position.y > 4.5f);
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
