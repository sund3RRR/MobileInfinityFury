using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    public GameObject Trails;
    private Vector2 RotateVector;
    private Vector2 DistanceVector;
    private GameObject Player;
    private float DistanceVectorMulti;
    public float Radius;
    public bool IsClockwise;
    public float RotateSpeed;
    public float InsideForceMultiplier;
    private int LeftCoordinateMulti;
    private int RightCoordinateMulti;

    public int HealthPoints;
    public int damage;
    public GameObject DeadVFX;

    public GameObject BulletDrone;
    public Transform BulletPosition;
    private GameObject Target;
    private float rotate;

    public float TimeBtwBulletShots;

    private SceneController CurrentSceneController;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("PlayerBlue");
        CurrentSceneController = GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController>();
        if (IsClockwise)
        {
            LeftCoordinateMulti = -1;
            RightCoordinateMulti = 1;
        }
        else
        {
            LeftCoordinateMulti = 1;
            RightCoordinateMulti = -1;
        }
        StartCoroutine(Shoot());
        StartCoroutine(FindTarget());
    }

    void FixedUpdate()
    {
        DistanceVector = Player.transform.position - transform.position;
        RotateVector = new Vector2(LeftCoordinateMulti * DistanceVector.y, RightCoordinateMulti * DistanceVector.x).normalized;
        if (DistanceVector.sqrMagnitude - Radius > 0)
        {
            DistanceVectorMulti = DistanceVector.sqrMagnitude - Radius;
        }
        else if (DistanceVector.sqrMagnitude - Radius < 0)
        {
            DistanceVector = -DistanceVector;
            DistanceVectorMulti = (Radius - DistanceVector.sqrMagnitude) * InsideForceMultiplier;
        }       
        transform.Translate((RotateVector * RotateSpeed + DistanceVector.normalized * DistanceVectorMulti) * Time.deltaTime, Space.World);

        if (HealthPoints <= 0)
        {
            var myMain = Trails.GetComponent<ParticleSystem>().main;
            myMain.prewarm = false;
            myMain.loop = false;          
            Trails.transform.SetParent(null);
            Destroy(gameObject);
            Instantiate(DeadVFX, transform.position, Quaternion.identity);
        }

        if (Target)
            RotateObj(Target.transform.position - transform.position);
        else if (CurrentSceneController.EnemyArray.Length != 0)
            CurrentSceneController.ForceRefreshEnemyArray();
    }
    void RotateObj(Vector2 rotation)
    {
        rotate = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, (Quaternion.AngleAxis(rotate, Vector3.forward)), 0.1f);
    }
    
    IEnumerator Shoot()
    {
        while (true)
        {           
            yield return new WaitForSeconds(TimeBtwBulletShots);
            if (Target)
                Instantiate(BulletDrone, BulletPosition.position, transform.rotation);
        }
    }
    IEnumerator FindTarget()
    {
        while (true)
        {
            GameObject[] MyEnemyArray = CurrentSceneController.EnemyArray;

            if (MyEnemyArray.Length != 0)
            {
                float distance = Mathf.Infinity;
                GameObject closest = MyEnemyArray[0];
                foreach (GameObject Enemy in MyEnemyArray)
                {
                    if (!Enemy)
                        CurrentSceneController.ForceRefreshEnemyArray();
                    else
                    {
                        Vector2 difference = Enemy.transform.position - transform.position;
                        float CurrentDistance = difference.sqrMagnitude;

                        if (CurrentDistance < distance)
                        {
                            closest = Enemy;
                            distance = CurrentDistance;
                        }
                    }
                }
                Target = closest;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyBullet")
        {
            HealthPoints -= 1;
            Destroy(collision.gameObject);
        }
    }
}
