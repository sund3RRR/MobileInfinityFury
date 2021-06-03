using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourthBoss : MonoBehaviour
{
    //editor variables
    [SerializeField] private GameObject BulletBossVFX;
    [SerializeField] private GameObject Summon;
    [SerializeField] private Transform BulletPoint;
    [SerializeField] private Transform SummonPosition;
    [SerializeField] private float speed;  
    [SerializeField] private float TimeBtwSummonSpawn;
    
    //private variables
    private Vector2 MovePosition;
    private Vector2 ForcePosition = Vector2.zero;
    private Rigidbody2D rb2D;
    private GameObject Player;
    private float Timer;    

    //private Coroutine MyCoroutine;
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
    IEnumerator Movement()
    {
        speed -= 1;
        MovePosition = new Vector2(0, 4.5f);

        yield return new WaitWhile(() => transform.position.y > 4.5f);
        speed += 1;
        StartCoroutine(Shoot(BulletPoint));
        StartCoroutine(SummonSpawner());
        while (true)
        {
            MovePosition = new Vector2(Player.transform.position.x, Random.Range(3.5f, 4.4f));
            Timer = 0;
            yield return new WaitWhile(() => (transform.position.y < 4.2f && transform.position.y > 3.5f) || Timer > 2f);
        }
    }
    IEnumerator SummonSpawner()
    {
        while(true)
        {
            Vector2 StartPos = new Vector2(transform.position.x + Random.Range(-2f, 2f),
                transform.position.y + Random.Range(-0.3f, 0.3f)) - (Vector2)transform.position;
            Summon.GetComponent<SummonFourthBoss>().StartPosition = StartPos;
            Summon.GetComponent<SummonFourthBoss>().StartSpeed = 1f;
            Instantiate(Summon, SummonPosition.position, Summon.transform.rotation);

            yield return new WaitForSeconds(TimeBtwSummonSpawn);
        }
    }
    IEnumerator Shoot(Transform BulletPoint)
    {
        while (true)
        {
            BulletBossVFX.GetComponent<Laser>().Parent = BulletPoint;
            GameObject NewBulletBossVFX = Instantiate(BulletBossVFX, BulletPoint.position, Quaternion.identity);
            Destroy(NewBulletBossVFX, 10);
            yield return new WaitForSeconds(5f);
            float backup = speed;
            speed = 0;
            rb2D.mass = 20;
            yield return new WaitForSeconds(4f);
            speed = backup;
            rb2D.mass = 1;
            yield return new WaitForSeconds(2f);
        }
    }
}
