using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyController : MonoBehaviour
{
    public static SceneController CurrentScene;
    public static GameObject Exp;
    public static GameObject SmallPiece;
    public static GameObject BigPiece;
    public static GameObject Bonus;

    public static GameObject DeadVFX;
    public static GameObject ZondDeadVFX;
    public static GameObject ZondExplosionVFX;

    public static GameObject AsteroidDeadVFX;

    void Start()
    {
        CurrentScene = GameObject.FindWithTag("SceneController").GetComponent<SceneController>();
        Exp = CurrentScene.Exp;
        SmallPiece = CurrentScene.SmallPiece;
        BigPiece = CurrentScene.BigPiece;
        Bonus = CurrentScene.Bonus;
        DeadVFX = CurrentScene.DeadVFX;
        ZondDeadVFX = CurrentScene.ZondDeadVFX;
        ZondExplosionVFX = CurrentScene.ZondExplosionVFX;
    }
    public static void DestroyDefault(GameObject Target)
    {
        Instantiate(Exp, Target.transform.position, Quaternion.identity);

        Destroy(Target);
    }
    public static void DestroySphere(GameObject Target)
    {
        if (!Target)
            return;
        GameObject FirstPiece = Target.GetComponent<SphereController>().FirstPiece;
        GameObject SecondPiece = Target.GetComponent<SphereController>().SecondPiece;
        GameObject ThirdPiece = Target.GetComponent<SphereController>().ThirdPiece;
        if (FirstPiece)
        {
            FirstPiece.GetComponent<SpherePieceController>().ActivateObject(1);
            FirstPiece.GetComponent<SpherePieceController>().Torque = Random.Range(-1f, 1f);
            FirstPiece.transform.SetParent(null);
        }
        if (SecondPiece)
        {
            SecondPiece.GetComponent<SpherePieceController>().ActivateObject(2);
            SecondPiece.GetComponent<SpherePieceController>().Torque = Random.Range(-1f, 1f);
            SecondPiece.transform.SetParent(null);
        }
        if (ThirdPiece)
        {
            ThirdPiece.GetComponent<SpherePieceController>().ActivateObject(3);
            ThirdPiece.GetComponent<SpherePieceController>().Torque = Random.Range(-1f, 1f);
            ThirdPiece.transform.SetParent(null);
        }
        for (int i = 0; i < 2; i++)
        {
            SmallPiece.GetComponent<SmallPieceController>().ParentForce = Target.GetComponent<Rigidbody2D>().velocity;
            SmallPiece.GetComponent<SmallPieceController>().force = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            SmallPiece.GetComponent<SmallPieceController>().Torque = Random.Range(-1f, 1f);
            float SmallPieceAngle = Random.Range(0, 360);
            Instantiate(SmallPiece, Target.transform.position, Quaternion.AngleAxis(SmallPieceAngle, Vector3.forward));
        }
        Instantiate(Exp, Target.transform.position, Quaternion.identity);

        GameObject NewVFX = Instantiate(DeadVFX, Target.transform.position, Quaternion.identity);
        Destroy(NewVFX, 2);

        Destroy(Target.gameObject);
    }
    public static void DestroyFatEnemy(GameObject Target)
    {
        BigPiece.GetComponent<BigPieceController>().ParentForce = Target.GetComponent<Rigidbody2D>().velocity;
        BigPiece.GetComponent<BigPieceController>().force = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        BigPiece.GetComponent<BigPieceController>().Torque = Random.Range(-1f, 1f);
        float BigPieceAngle = Random.Range(0, 360);
        Instantiate(BigPiece, Target.transform.position, Quaternion.AngleAxis(BigPieceAngle, Vector3.forward));

        for (int i = 0; i < 3; i++)
        {
            SmallPiece.GetComponent<SmallPieceController>().ParentForce = Target.GetComponent<Rigidbody2D>().velocity;
            SmallPiece.GetComponent<SmallPieceController>().force = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            SmallPiece.GetComponent<SmallPieceController>().Torque = Random.Range(-1f, 1f);
            float SmallPieceAngle = Random.Range(0, 360);
            Instantiate(SmallPiece, Target.transform.position, Quaternion.AngleAxis(SmallPieceAngle, Vector3.forward));
        }
        Instantiate(Exp, Target.transform.position, Quaternion.identity);

        GameObject NewZondDeadVFX = Instantiate(ZondDeadVFX, Target.transform.position, Quaternion.identity);
        Destroy(NewZondDeadVFX, 2);

        GameObject NewZondExplosionVFX = Instantiate(ZondExplosionVFX, Target.transform.position, Quaternion.identity);
        Destroy(NewZondExplosionVFX, 2);
        Destroy(Target.gameObject);
    }
    public static void DestroySlimEnemy(GameObject Target)
    {
        for (int i = 0; i < 2; i++)
        {
            SmallPiece.GetComponent<SmallPieceController>().ParentForce = Target.GetComponent<Rigidbody2D>().velocity;
            SmallPiece.GetComponent<SmallPieceController>().force = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            SmallPiece.GetComponent<SmallPieceController>().Torque = Random.Range(-1f, 1f);
            float SmallPieceAngle = Random.Range(0, 360);
            Instantiate(SmallPiece, Target.transform.position, Quaternion.AngleAxis(SmallPieceAngle, Vector3.forward));
        }
        Instantiate(Exp, Target.transform.position, Quaternion.identity);

        GameObject NewZondDeadVFX = Instantiate(ZondDeadVFX, Target.transform.position, Quaternion.identity);
        Destroy(NewZondDeadVFX, 2);

        GameObject NewZondExplosionVFX = Instantiate(ZondExplosionVFX, Target.transform.position, Quaternion.identity);
        Destroy(NewZondExplosionVFX, 2);
        Destroy(Target.gameObject);
    }
    public static IEnumerator DestroyBoss(GameObject Boss)
    {
        Transform[] ArrayTransform = new Transform[4] {Boss.GetComponent<BossFirst>().BulletPoint1, Boss.GetComponent<BossFirst>().BulletPoint2,
            Boss.GetComponent<BossFirst>().BulletPoint3, Boss.GetComponent<BossFirst>().BulletPoint4 };
        foreach(Transform EffectPos in ArrayTransform)
        {
            float RandomScale = Random.Range(0.5f, 0.8f);
            ZondExplosionVFX.transform.localScale = new Vector3(RandomScale, RandomScale, RandomScale);
            Instantiate(ZondExplosionVFX, EffectPos.position, Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
        }
        ZondExplosionVFX.transform.localScale = Vector3.one;
        GameObject NewZondExplosionVFX = Instantiate(ZondExplosionVFX, Boss.transform.position, Quaternion.identity);
        Destroy(NewZondExplosionVFX, 2);

        Destroy(Boss.gameObject);
    }
    public static void DestroyZond(GameObject Zond)
    {
        if (!Zond)
            return;

        GameObject leftPanel = Zond.GetComponent<ZondController>().leftPanel;
        GameObject rightPanel = Zond.GetComponent<ZondController>().rightPanel;
        GameObject Sphere = Zond.GetComponent<ZondController>().Sphere;
        if (Sphere)
        {
            Sphere.GetComponent<SphereController>().ActivateObject(1);
            DestroySphere(Sphere);
            Sphere.GetComponent<SphereController>().Torque = Random.Range(-1f, 1f);
            Sphere.transform.SetParent(null);
        }
        if (leftPanel)
        {
            leftPanel.GetComponent<PanelController>().ActivateObject(1);
            leftPanel.GetComponent<PanelController>().Torque = Random.Range(-1f, 1f);
            leftPanel.transform.SetParent(null);
        }
        if (rightPanel)
        {
            rightPanel.GetComponent<PanelController>().ActivateObject(1);
            rightPanel.GetComponent<PanelController>().Torque = Random.Range(-1f, 1f);
            rightPanel.transform.SetParent(null);
        }

        BigPiece.GetComponent<BigPieceController>().ParentForce = Zond.GetComponent<Rigidbody2D>().velocity;
        BigPiece.GetComponent<BigPieceController>().force = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        BigPiece.GetComponent<BigPieceController>().Torque = Random.Range(-1f, 1f);
        float BigPieceAngle = Random.Range(0, 360);
        Instantiate(BigPiece, Zond.transform.position, Quaternion.AngleAxis(BigPieceAngle, Vector3.forward));

        for (int i = 0; i < 3; i++)
        {
            SmallPiece.GetComponent<SmallPieceController>().ParentForce = Zond.GetComponent<Rigidbody2D>().velocity;
            SmallPiece.GetComponent<SmallPieceController>().force = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            SmallPiece.GetComponent<SmallPieceController>().Torque = Random.Range(-1f, 1f);
            float SmallPieceAngle = Random.Range(0, 360);
            Instantiate(SmallPiece, Zond.transform.position, Quaternion.AngleAxis(SmallPieceAngle, Vector3.forward));
        }
        Instantiate(Exp, Zond.transform.position, Quaternion.identity);

        Bonus.GetComponent<BonusController>().force = Zond.GetComponent<Rigidbody2D>().velocity;
        Bonus.gameObject.tag = "MultiplierBonus";
        Instantiate(Bonus, Zond.transform.position, Quaternion.identity);

        //GameObject NewZondDeadVFX = Instantiate(ZondDeadVFX, Zond.transform.position, Quaternion.identity);
        //Destroy(NewZondDeadVFX, 2);

        GameObject NewZondExplosionVFX = Instantiate(ZondExplosionVFX, Zond.transform.position, Quaternion.identity);
        Destroy(NewZondExplosionVFX, 2);
        Destroy(Zond.gameObject);
    }
    public static void DestroyBigPiece(GameObject BigPiece)
    {
        for (int i = 0; i < 3; i++)
        {
            SmallPiece.GetComponent<SmallPieceController>().ParentForce = BigPiece.GetComponent<Rigidbody2D>().velocity;
            SmallPiece.GetComponent<SmallPieceController>().force = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            SmallPiece.GetComponent<SmallPieceController>().Torque = Random.Range(-1f, 1f);
            float SmallPieceAngle = Random.Range(0, 360);
            Instantiate(SmallPiece, BigPiece.transform.position, Quaternion.AngleAxis(SmallPieceAngle, Vector3.forward));
        }
        Instantiate(Exp, BigPiece.transform.position, Quaternion.identity) ;

        Destroy(BigPiece.gameObject);
    }
    public static void DestroySputnik(GameObject Sputnik)
    {
        Bonus.GetComponent<BonusController>().force = Sputnik.GetComponent<Rigidbody2D>().velocity;
        Bonus.gameObject.tag = "UpgradeBonus";
        Instantiate(Bonus, Sputnik.GetComponent<SputnikController>().CenterOfSputnik.position, Quaternion.identity);

        Instantiate(Exp, Sputnik.GetComponent<SputnikController>().CenterOfSputnik.position, Quaternion.identity);

        Destroy(Sputnik.gameObject);
    }
    public static void DestroyAsteroid(GameObject Asteroid)
    {
        if (Asteroid.GetComponent<AsteroidController>().indexScale > 0)
        {
            SpawnController Target = GameObject.FindWithTag("SceneController").GetComponent<SpawnController>();
            Vector2 pos = new Vector2(Asteroid.transform.position.x, Asteroid.transform.position.y);
            Target.RespawnAsteroids(pos, Asteroid.GetComponent<AsteroidController>().indexScale, Asteroid.GetComponent<AsteroidController>().force);
        }
        Instantiate(Exp, Asteroid.transform.position, Quaternion.identity);

        AsteroidDeadVFX = Asteroid.GetComponent<AsteroidController>().GetDeadVFX();
        GameObject NewAsteroidDeadVFX = Instantiate(AsteroidDeadVFX, Asteroid.transform.position, Quaternion.identity);
        Destroy(NewAsteroidDeadVFX, 2);

        Destroy(Asteroid.gameObject);
    }
    public static void DestroyGoldAsteroid(GameObject GoldAsteroid)
    {
        Instantiate(Exp, GoldAsteroid.transform.position, Quaternion.identity);

        Bonus.GetComponent<BonusController>().force = GoldAsteroid.GetComponent<Rigidbody2D>().velocity;
        Bonus.gameObject.tag = "LifeBonus";
        Instantiate(Bonus, GoldAsteroid.transform.position, Quaternion.identity);

        Destroy(GoldAsteroid.gameObject);
    }
    public static void DestroyCenterEnemies(GameObject[] EnemyArray)
    {
        for (int i = 0; i < EnemyArray.Length; i++) // поиск объектов, находящихся в центре для их уничтожения (для свободного спавна корабля)
            if (EnemyArray[i].transform.position.sqrMagnitude < 20f)
                switch (EnemyArray[i].name)
                {
                    case "Zond(Clone)":
                        DestroyZond(EnemyArray[i]);
                        break;
                    case "BigPiece(Clone)":
                        DestroyBigPiece(EnemyArray[i]);
                        break;
                    case "Sputnik(Clone)":
                        DestroySputnik(EnemyArray[i]);
                        break;
                    case "Panel":
                        if (EnemyArray[i].GetComponent<PanelController>().Active)
                            DestroyDefault(EnemyArray[i]);
                        break;
                    case "GoldAsteroid(Clone)":
                        DestroyGoldAsteroid(EnemyArray[i]);
                        break;
                    case "FatStarshipEnemy(Clone)":
                        DestroyFatEnemy(EnemyArray[i]);
                        break;
                    case "SlimStarshipEnemy(Clone)":
                        DestroySlimEnemy(EnemyArray[i]);
                        break;
                    default:
                        if (EnemyArray[i].GetComponent<AsteroidController>())
                            DestroyAsteroid(EnemyArray[i]);
                        else
                            DestroyDefault(EnemyArray[i]);
                        break;
                }           
    }
}
