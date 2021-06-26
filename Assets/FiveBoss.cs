using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiveBoss : MonoBehaviour
{
    //editor variables
    [Header("Laser")]
    [SerializeField] private GameObject LaserBossVFX;
    [SerializeField] private Transform LaserPosition1;
    [SerializeField] private Transform LaserPosition2;
    
    [Header("Bullet")]
    [SerializeField] private Transform BulletPosition1;
    [SerializeField] private Transform BulletPosition2;
    [SerializeField] private float LeftBorderTimeBtwShots;
    [SerializeField] private float RightBorderTimeBtwShots;
    [SerializeField] private GameObject BulletBoss;

    [Header("Boss")]
    [SerializeField] GameObject VFXCrash;
    [SerializeField] private float speed;
    [SerializeField, Range(0,1f)] private float SecondStageBoss;
    [SerializeField, Range(0, 1f)] private float ThirdStageBoss;

    [Header("Head")]
    [SerializeField] private GameObject LeftPiece1;
    [SerializeField] private GameObject LeftPiece2;
    [SerializeField] private GameObject RightPiece1;
    [SerializeField] private GameObject RightPiece2;

    [Header("Wings")]
    [SerializeField] private GameObject CircleLeft;
    [SerializeField] private GameObject WingLeft1;
    [SerializeField] private GameObject WingLeft2;
    [SerializeField] private GameObject WingLeft3;
    [SerializeField] private GameObject WingLeft4;

    [SerializeField] private GameObject CircleRight;
    [SerializeField] private GameObject WingRight1;
    [SerializeField] private GameObject WingRight2;
    [SerializeField] private GameObject WingRight3;
    [SerializeField] private GameObject WingRight4;

    
    //private variables
    private Vector2 MovePosition;
    private Vector2 ForcePosition = Vector2.zero;
    private Rigidbody2D rb2D;
    private GameObject Player;
    private float Timer;
    private GameObject NewBulletBossVFX1;
    private GameObject NewBulletBossVFX2;

    private Coroutine LaserCoroutine;
    /*
    public GameObject FirstPiece;
    public GameObject SecondPiece;
    public GameObject ThirdPiece;
    public GameObject FourthPiece;
    */
    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        StartCoroutine(Movement());
        StartCoroutine(SecondStage());
        Player = GameObject.FindGameObjectWithTag("PlayerBlue");      
    }

    void FixedUpdate()
    {
        ForcePosition = new Vector2(Player.transform.position.x, MovePosition.y) - (Vector2)transform.position;

        Timer += Time.deltaTime;
        rb2D.AddForce(ForcePosition.normalized * speed);
    }
    /*
    public IEnumerator DestroyMe()
    {
        StartCoroutine(DestroyController.DestroyBoss(gameObject));

        while (true)
        {
            speed /= 1.5f;
            yield return null;
        }
    }*/
    IEnumerator SecondStage()
    {
        yield return new WaitUntil(() => SecondStageBoss > 
        (float)GetComponent<HealthPointsController>().HealthPoints / (float)GetComponent<HealthPointsController>().BaseHealthPoints);

        BreakHead();

        yield return new WaitUntil(() => ThirdStageBoss >
        (float)GetComponent<HealthPointsController>().HealthPoints / (float)GetComponent<HealthPointsController>().BaseHealthPoints);

        BreakWings();
        StopCoroutine(LaserCoroutine);

        Destroy(NewBulletBossVFX1);
        Destroy(NewBulletBossVFX2);
    }
    IEnumerator Movement()
    {
        speed -= 1;
        MovePosition = new Vector2(0, 4f);

        yield return new WaitWhile(() => transform.position.y > 4f);
        speed += 1;

        LaserCoroutine = StartCoroutine(LaserShoot());
        StartCoroutine(BulletShoot(BulletPosition1));
        StartCoroutine(BulletShoot(BulletPosition2));

        while (true)
        {
            MovePosition = new Vector2(Player.transform.position.x, Random.Range(3.3f, 4f));
            Timer = 0;
            yield return new WaitWhile(() => (transform.position.y < 4f && transform.position.y > 3.3f) || Timer > 2f);
        }
    }
    IEnumerator BulletShoot(Transform BulletPoint)
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(LeftBorderTimeBtwShots, RightBorderTimeBtwShots));
            Instantiate(BulletBoss, BulletPoint.position, BulletBoss.transform.rotation);
        }
    }
    IEnumerator LaserShoot()
    {
        while (true)
        {
            LaserBossVFX.GetComponent<Laser>().Parent = LaserPosition1;
            NewBulletBossVFX1 = Instantiate(LaserBossVFX, LaserPosition1.position, Quaternion.identity);
            Destroy(NewBulletBossVFX1, 10);

            LaserBossVFX.GetComponent<Laser>().Parent = LaserPosition2;
            NewBulletBossVFX2 = Instantiate(LaserBossVFX, LaserPosition2.position, Quaternion.identity);
            Destroy(NewBulletBossVFX2, 10);

            yield return new WaitForSeconds(5f);
            float backup = speed;
            speed = 0;
            rb2D.mass = 20;
            yield return new WaitForSeconds(4f);
            speed = backup;
            rb2D.mass = 10;
            yield return new WaitForSeconds(2f);
        }
    }
    void BreakWings()
    {
        WingLeft1.GetComponent<FiveBossPiece>().ActivateObject(CircleLeft);
        WingLeft2.GetComponent<FiveBossPiece>().ActivateObject(CircleLeft);
        WingLeft3.GetComponent<FiveBossPiece>().ActivateObject(CircleLeft);
        WingLeft4.GetComponent<FiveBossPiece>().ActivateObject(CircleLeft);

        WingRight1.GetComponent<FiveBossPiece>().ActivateObject(CircleRight);
        WingRight2.GetComponent<FiveBossPiece>().ActivateObject(CircleRight);
        WingRight3.GetComponent<FiveBossPiece>().ActivateObject(CircleRight);
        WingRight4.GetComponent<FiveBossPiece>().ActivateObject(CircleRight);
        Instantiate(VFXCrash, CircleLeft.transform.position, Quaternion.identity);
        Instantiate(VFXCrash, CircleRight.transform.position, Quaternion.identity);
        CircleLeft.GetComponent<FiveBossPiece>().ActivateObject(gameObject);
        CircleRight.GetComponent<FiveBossPiece>().ActivateObject(gameObject);
    }
    void BreakHead()
    {
        Instantiate(VFXCrash, LeftPiece1.transform.position, Quaternion.identity);
        Instantiate(VFXCrash, LeftPiece2.transform.position, Quaternion.identity);
        LeftPiece2.GetComponent<FiveBossPiece>().ActivateObject(LeftPiece1);
        LeftPiece1.GetComponent<FiveBossPiece>().ActivateObject(gameObject);
        RightPiece2.GetComponent<FiveBossPiece>().ActivateObject(RightPiece1);
        RightPiece1.GetComponent<FiveBossPiece>().ActivateObject(gameObject);
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<EdgeCollider2D>().enabled = true;
    }
}
