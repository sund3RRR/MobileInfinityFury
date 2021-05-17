using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimEnemyController : MonoBehaviour
{
    //Editor Variables
    
    public GameObject bullet;
    public GameObject Bar; 

    public int HealthPoints;
    public int BaseHealthPoints;

    public float TimeBetweenBullets;
    public int CountOfBullets;
    public int RamAngle;
    public float SpeedMultiplierRam;
    public float SpeedMultiplierBrake;
    public float PatruleSpeed;


    // Public Variables
    public float hitTime = 2.5f;

    //Private Variables
    private GameObject HealthBar;
    private Camera cam;
    private Rigidbody2D rb2D;
    private AudioSource AS;


    private bool IsChangedPosition = false;
    private bool AttackTargetBool = false;
    private bool FindTargetBool = false;
    private bool RamTargetBool = false;
    private bool rotateObj = true;
    private bool DefForceCh = false;
    private bool ShootToTargetBool = false;

    
    private Coroutine ChangeForcePositionCoroutine;
    private GameObject Target;
    private GameObject Target1;
    private float speed;
    private Vector2 force;   
    private Vector2 torotate;
    private Vector2 fromrotate;


    void Start()
    {
        cam = Camera.main;
        rb2D = GetComponent<Rigidbody2D>();
        AS = GetComponent<AudioSource>();
        //
        // HealthBar INIT
        //
        GameObject Canvas = GameObject.FindGameObjectWithTag("Canvas");
        Bar.GetComponent<HealthBarController>().Target = gameObject;
        Bar.GetComponent<HealthBarController>().BaseHealthPoints = BaseHealthPoints;
        HealthBar = Instantiate(Bar, new Vector3(transform.position.x, transform.position.y), Quaternion.identity);
        HealthBar.transform.SetParent(Canvas.transform, false);
        //
        // HealthBar INIT
        //
    }
    void FixedUpdate()
    {
        hitTime += Time.deltaTime;

        Vector2 viewportPosition = cam.WorldToViewportPoint(transform.position);

        if (HealthBar && hitTime > 2.5f)
        {
            HealthBar.GetComponent<HealthBarController>().DisableHealthBar();
        }
        else if (HealthBar)
        {
            HealthBar.GetComponent<HealthBarController>().HealthPoints = HealthPoints;
            HealthBar.GetComponent<HealthBarController>().EnableHealthBar();
        }

        if (HealthPoints <= 0)
            DestroyController.DestroySlimEnemy(gameObject);

        if (Target)
        {
            if (!AttackTargetBool)
            {
                StartCoroutine(AttackTarget());
            }
        }
        else
        {
            rotateObj = true;
            if (!FindTargetBool)
                StartCoroutine(FindTarget());
            if (!DefForceCh)
                ChangeForcePositionCoroutine = StartCoroutine(ChangeForcePosition());
        }



        rb2D.AddForce(force * speed);
        rb2D.AddTorque(RotateObjToTarget() * 0.5f);



        if ((viewportPosition.x > 0.9f || viewportPosition.x < 0.1f || viewportPosition.y > 0.9f || viewportPosition.y < 0.1f) && !IsChangedPosition)
        {
            if (DefForceCh)
                StopCoroutine(ChangeForcePositionCoroutine);
            ChangeForcePositionCoroutine = StartCoroutine(ChangeForcePosition());
            IsChangedPosition = true;
        }
        else if (viewportPosition.x < 0.9f && viewportPosition.x > 0.1f && viewportPosition.y < 0.9f && viewportPosition.y > 0.1f)
            IsChangedPosition = false;

    }
    IEnumerator AttackTarget()
    {
        AttackTargetBool = true;

        while (Target)
        {
            StartCoroutine(ShootToTarget());
            yield return new WaitUntil(() => ShootToTargetBool == false);

            StartCoroutine(RamTarget());
            yield return new WaitUntil(() => RamTargetBool == false);
        }
        AttackTargetBool = false;
        yield break;
    }
    IEnumerator ShootToTarget()
    {
        rotateObj = true;
        ShootToTargetBool = true;
        int k = 0;

        while (k < CountOfBullets)
        {
            k++;
            if (Target)
            {
                yield return new WaitForSeconds(TimeBetweenBullets);

                if (!Target)
                {
                    ShootToTargetBool = false;
                    yield break;
                }
                AS.Play();
                bullet.GetComponent<BulletEnemy>().Parent = gameObject;
                Instantiate(bullet, transform.position, Quaternion.identity);      
            }
            else
            {
                ShootToTargetBool = false;
                yield break;
            }
        }
        ShootToTargetBool = false;
        yield break;
    }
    IEnumerator RamTarget()
    {
        rotateObj = true;
        RamTargetBool = true;

        Coroutine AccelerateToTargetCoroutine = StartCoroutine(AccelerateToTarget());

        while(true)
        {           
            if (Target && Vector2.Angle(transform.right, Target.transform.position - transform.position) < RamAngle)
            {
                speed += Time.deltaTime * SpeedMultiplierRam;
                yield return null;
            }
            else if (Target)
            {
                force = transform.right.normalized;
                rotateObj = false;

                StopCoroutine(AccelerateToTargetCoroutine);
                StartCoroutine(Brake());     
                               
                yield break;
            }
            else
            {
                RamTargetBool = false;
                yield break;
            }
        }
    }
    
    IEnumerator Brake()
    {
        float SpeedBrake = SpeedMultiplierBrake;

        while (speed > 2)
        {
            speed -= Time.deltaTime * SpeedBrake;
            SpeedBrake += 0.5f;
            yield return null;
        }

        RamTargetBool = false;
        yield break;
    }


    IEnumerator AccelerateToTarget()
    {
        while(true)
        {
            if (Target)
            {
                force = (Target.transform.position - transform.position).normalized * 0.1f;
                force = force.normalized;
                yield return null;
            }
            else
                yield break;
        }
    }    
    float RotateObjToTarget()
    {
        if (rotateObj || speed < 2)
        {
            if (Target)
                torotate = Target.transform.position - transform.position;
            else
                torotate = force;

            fromrotate = transform.right;

            return Vector2.SignedAngle(fromrotate, torotate);
        }
        else
            return 0;
    }
    IEnumerator FindTarget()
    {
        while (true)
        {
            FindTargetBool = true;

            Target1 = GameObject.FindGameObjectWithTag("PlayerGreen");
            Target = GameObject.FindGameObjectWithTag("PlayerBlue");

            if (Target1 && Target && (transform.position - Target1.transform.position).sqrMagnitude < (transform.position - Target.transform.position).sqrMagnitude)
                Target = Target1;
            else if (Target1 && Target && (transform.position - Target1.transform.position).sqrMagnitude > (transform.position - Target.transform.position).sqrMagnitude)
                Target = Target;
            else if (Target1)
                Target = Target1;

            if (Target)
            {
                FindTargetBool = false;
                yield break;
            }
            else
            {
                yield return new WaitForSeconds(1);
            }
        }
    }
    IEnumerator ChangeForcePosition()
    {
        DefForceCh = true;
        float waitTime = 10;

        while (true)
        {
            speed = PatruleSpeed;
            force = new Vector2(Random.Range(-8f, 8f), Random.Range(-4f, 4f));
            force = (force - new Vector2(transform.position.x, transform.position.y)).normalized;

            yield return new WaitForSeconds(waitTime);
            
        }
    }
}
