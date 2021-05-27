using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondBoss2 : MonoBehaviour
{
    private Vector2 MovePosition;
    private Vector2 ForcePosition;
    private Rigidbody2D rb2D;
    private float Timer;
    public int HealthPoints;
    public float TimeBtwShots;
    public float speed;
    public GameObject BulletBoss;
    public Transform BulletPoint1;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        StartCoroutine(Movement());
    }

    void FixedUpdate()
    {
        Timer += Time.deltaTime;
        rb2D.AddForce(ForcePosition * speed);
    }
    IEnumerator Movement()
    {
        speed -= 1;
        MovePosition = new Vector2(0, -4.5f);
        ForcePosition = (MovePosition - (Vector2)transform.position).normalized;

        yield return new WaitWhile(() => transform.position.y < -4.5f);
        speed += 1;
        StartCoroutine(Shoot(BulletPoint1));
        while (true)
        {
            MovePosition = new Vector2(Random.Range(1.4f, 1.9f) * Mathf.Pow(-1, Random.Range(1, 3)), Random.Range(-4f, -4.6f));
            ForcePosition = (MovePosition - (Vector2)transform.position).normalized;
            Timer = 0;
            yield return new WaitWhile(() => (Mathf.Abs(transform.position.x) < 1.9f && transform.position.y > -4.6f && transform.position.y < -4f) && Timer < 2f);
        }
    }
    IEnumerator Shoot(Transform BulletPoint)
    {
        while (true)
        {
            yield return new WaitForSeconds(TimeBtwShots);
            Instantiate(BulletBoss, BulletPoint.position, Quaternion.AngleAxis(-90, Vector3.forward));
        }
    }
}
