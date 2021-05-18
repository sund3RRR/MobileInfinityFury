using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
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

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("PlayerBlue");
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
            Destroy(gameObject);
            Instantiate(DeadVFX, transform.position, Quaternion.identity);
        }
        FindTarget();

        if (Target)
            RotateObj(Target.transform.position - transform.position);     
    }
    void RotateObj(Vector2 rotation)
    {
        rotate = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(rotate, Vector3.forward);
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
    void FindTarget()
    {
        GameObject[] MyEnemyArray = SceneController.EnemyArray;
        float distance = Mathf.Infinity;
        GameObject closest = MyEnemyArray[0];
        foreach (GameObject Enemy in MyEnemyArray)
        {
            Vector2 difference = Enemy.transform.position - transform.position;
            float CurrentDistance = difference.sqrMagnitude;

            if (CurrentDistance < distance)
            {
                closest = Enemy;
                distance = CurrentDistance;
            }
        }
        Target = closest;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            switch (collision.name)
            {
                case "Zond(Clone)":
                    collision.gameObject.GetComponent<ZondController>().HealthPoints -= damage;
                    collision.gameObject.GetComponent<ZondController>().hitTime = 0;
                    break;
                case "Sphere":
                    if (collision.gameObject.GetComponent<SphereController>().Active)
                    {
                        collision.GetComponent<SphereController>().HealthPoints -= damage;
                        collision.GetComponent<SphereController>().hitTime = 0;
                    }
                    break;
                case "Panel":
                    if (collision.gameObject.GetComponent<PanelController>().Active)
                    {
                        collision.gameObject.GetComponent<PanelController>().HealthPoints -= damage;
                        collision.gameObject.GetComponent<PanelController>().hitTime = 0;
                    }
                    break;
                case "BigPiece(Clone)":
                    collision.gameObject.GetComponent<BigPieceController>().HealthPoints -= damage;
                    collision.gameObject.GetComponent<BigPieceController>().hitTime = 0;
                    break;
                case "Sputnik(Clone)":
                    collision.gameObject.GetComponent<SputnikController>().HealthPoints -= damage;
                    collision.gameObject.GetComponent<SputnikController>().hitTime = 0;
                    break;
                case "SmallPiece(Clone)":
                    DestroyController.DestroyDefault(collision.gameObject);
                    break;
                case "GoldAsteroid(Clone)":
                    collision.gameObject.GetComponent<GoldAsteroidController>().HealthPoints -= damage;
                    collision.gameObject.GetComponent<GoldAsteroidController>().hitTime = 0;
                    break;
                case "FatStarshipEnemy(Clone)":
                    collision.GetComponent<FatStarshipEnemy>().HealthPoints -= damage;
                    collision.GetComponent<FatStarshipEnemy>().hitTime = 0;
                    break;
                case "DestroyerEnemyStarship(Clone)":
                    collision.GetComponent<DestroyerEnemyController>().HealthPoints -= damage;
                    collision.GetComponent<DestroyerEnemyController>().hitTime = 0;
                    break;
                case "SlimStarshipEnemy(Clone)":
                    collision.GetComponent<SlimEnemyController>().HealthPoints -= damage;
                    collision.GetComponent<SlimEnemyController>().hitTime = 0;
                    break;
                case "FirstBoss(Clone)":
                    collision.GetComponent<BossFirst>().HealthPoints -= damage;
                    collision.GetComponent<BossFirst>().hitTime = 0;
                    break;
                default:
                    if (collision.gameObject.GetComponent<AsteroidController>())
                    {
                        collision.gameObject.GetComponent<AsteroidController>().HealthPoints -= damage;
                        collision.gameObject.GetComponent<AsteroidController>().hitTime = 0;
                    }
                    break;

            }
            Destroy(gameObject);
            Instantiate(DeadVFX, transform.position, Quaternion.identity);
        }
        else if (collision.tag == "EnemyBullet")
        {
            HealthPoints -= 1;
            Destroy(collision.gameObject);
        }
    }
}
