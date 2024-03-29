using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatStarshipEnemy : MonoBehaviour
{   
    // Editor Variables
    public GameObject Lightning;
    
    public GameObject bullet;
    public Transform BulletP1;
    public Transform BulletP2;
    public Transform LightningPoint;

    public Sprite BulletSp1;
    public Sprite BulletSp2;

    public float speed;
    public float TorqueSpeed;
    public float TimeBtwBulletShots;

    // Public variables
    

    // Private variables
    
    private GameObject Target;
    private Rigidbody2D rb2D;
    private Camera cam;  
    private Vector2 force;
    private Vector2 torotate;
    private Vector2 fromrotate;
    private Coroutine ShootingCoroutine;
    private Coroutine ChangingPositionCoroutine;

    private bool FindPlayer = false;
    private bool changed = false;
    private bool Shooting = false;

    void Start()
    {
        cam = Camera.main;
        
        rb2D = GetComponent<Rigidbody2D>();

        ChangingPositionCoroutine = StartCoroutine(ChangeForcePosition());
    }

    void FixedUpdate()
    {
        Vector2 viewportPosition = cam.WorldToViewportPoint(transform.position);

        rb2D.AddTorque(RotateObj() * TorqueSpeed);
        rb2D.AddForce(force * speed);

        if (Target)
        {
            if (!Shooting)
            {
                ShootingCoroutine = StartCoroutine(Shoot());
            }
        }
        else if (!FindPlayer)
        {
            StartCoroutine(FindTarget());
            if (Shooting)
            {
                StopCoroutine(ShootingCoroutine);
                Shooting = false;
            }
        }

        if ((viewportPosition.x > 0.9f || viewportPosition.x < 0.1f || viewportPosition.y > 0.9f || viewportPosition.y < 0.1f) && !changed)
        {
            StopCoroutine(ChangingPositionCoroutine);
            ChangingPositionCoroutine = StartCoroutine(ChangeForcePosition());
            changed = true;
        }
        else if (viewportPosition.x < 0.9f && viewportPosition.x > 0.1f && viewportPosition.y < 0.9f && viewportPosition.y > 0.1f)
            changed = false;    
    }
    float RotateObj()
    {
        if (Target)
            torotate = Target.transform.position - transform.position;
        else
            torotate = force;

        fromrotate = transform.right;

        return Vector2.SignedAngle(fromrotate, torotate);
    }

    IEnumerator ChangeForcePosition()
    {
        float waitTime;

        while (true)
        {
            force = new Vector2(Random.Range(-2f, 2f), Random.Range(-4f, 4f));
            force = (force - new Vector2(transform.position.x, transform.position.y)).normalized;

            if (Target)
                waitTime = 5;
            else
                waitTime = 10;

            yield return new WaitForSeconds(waitTime);
        }
    }
    IEnumerator Shoot()
    {
        Shooting = true;

        while (true)
        {            
            if (Target)
            {
                yield return new WaitForSeconds(TimeBtwBulletShots);
                
                bullet.GetComponent<BulletEnemy>().Parent = gameObject;
                bullet.GetComponent<SpriteRenderer>().sprite = BulletSp1;
                Lightning.GetComponent<LightningEffect>().Bullet1 = Instantiate(bullet, BulletP1.position, Quaternion.identity);
                
                
                bullet.GetComponent<BulletEnemy>().Parent = gameObject;
                bullet.GetComponent<SpriteRenderer>().sprite = BulletSp2;
                Lightning.GetComponent<LightningEffect>().Bullet2 = Instantiate(bullet, BulletP2.position, Quaternion.identity);

                Lightning.GetComponent<LightningEffect>().Parent = gameObject;
                Instantiate(Lightning, LightningPoint.position, Quaternion.identity);
            }
            else
            {
                Shooting = false;

                yield break;
            }
        }
    }

    IEnumerator FindTarget()
    {
        FindPlayer = true;
        while (true)
        {
            Target = GameObject.FindGameObjectWithTag("PlayerBlue");
            if (Target)
            {
                FindPlayer = false;

                yield break;
            }
            else
            {
                yield return new WaitForSeconds(1);
            }
        }
    }
}
