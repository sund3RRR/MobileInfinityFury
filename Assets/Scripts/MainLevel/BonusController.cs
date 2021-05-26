using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusController : MonoBehaviour
{
    // Editor variables
    public Sprite MultiplierBonusSprite;
    public Sprite LifeBonusSprite;
    public Sprite UltimateBonusSprite;
    public Sprite UpgradeBonusSprite;
    public int DestroyTime;
    public GameObject UpgradeBonusVFX;

    // Private variables
    private Rigidbody2D rb2D;
    private float LifeTime = 0;
    private float myAlpha = 0;
    private GameObject IconUpgrade;
    private GameObject IconGreenUpgrade;

    // Public variables
    public Vector2 force;
    
    void Start()
    {
        IconUpgrade = GameObject.FindGameObjectWithTag("BlueLvl");
        IconGreenUpgrade = GameObject.FindGameObjectWithTag("GreenLVL");

        if (gameObject.tag == "UltimateBonus")
            gameObject.GetComponent<SpriteRenderer>().sprite = UltimateBonusSprite;
        else if (gameObject.tag == "LifeBonus")
        {
            transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            gameObject.GetComponent<SpriteRenderer>().sprite = LifeBonusSprite;
        }
        else if (gameObject.tag == "MultiplierBonus")
        {
            transform.localScale = new Vector3(1, 1, 1);
            gameObject.GetComponent<SpriteRenderer>().sprite = MultiplierBonusSprite;
        }
        else if (gameObject.tag == "UpgradeBonus")
        {
            transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            gameObject.GetComponent<SpriteRenderer>().sprite = UpgradeBonusSprite;
            Instantiate(UpgradeBonusVFX, transform.position, Quaternion.identity, transform);
        }
        rb2D = GetComponent<Rigidbody2D>();       
    }

    void FixedUpdate()
    {
        LifeTime += Time.deltaTime;
        if (gameObject.tag != "UpgradeBonus" && LifeTime >= DestroyTime)
        {
            myAlpha += Time.deltaTime;
            Color MyColor = GetComponent<SpriteRenderer>().color;
            GetComponent<SpriteRenderer>().color = new Color(MyColor.r, MyColor.g, MyColor.b, 1 - myAlpha);
            if (GetComponent<SpriteRenderer>().color.a <= 0)
                Destroy(gameObject);
        }
        rb2D.velocity = force;

        if (!gameObject.GetComponent<Renderer>().isVisible)
            Destroy(gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerBlue")
        {
            if (gameObject.tag == "UltimateBonus")
                HeroController.CountOfUltimate += 1;
            else if (gameObject.tag == "LifeBonus")
                SceneController.ShipLifeBlue += 1;
            else if (gameObject.tag == "MultiplierBonus")
                HeroController.BonusMultiplier += 1;
            else if (gameObject.tag == "UpgradeBonus")
            {
                HeroController.WeaponIndex += 1;
                IconUpgrade.GetComponent<NexLevel>().ChangeIcon(HeroController.WeaponIndex);
                collision.gameObject.GetComponent<HeroController>().UpgradeWeapon();
                GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController>().TimingUpgrade();
                collision.gameObject.GetComponent<HeroController>().AnimationUpgrade();
            }
            
            Destroy(gameObject);
        }
        else if (collision.tag == "PlayerGreen")
        {
            if (gameObject.tag == "UltimateBonus")
                HeroControllerGamePad.CountOfUltimate += 1;
            else if (gameObject.tag == "LifeBonus")
                SceneController.ShipLifeGreen += 1;
            else if (gameObject.tag == "MultiplierBonus")
                HeroControllerGamePad.BonusMultiplier += 1;
            else if (gameObject.tag == "UpgradeBonus")
            {
                HeroControllerGamePad.WeaponIndex += 1;               
                collision.gameObject.GetComponent<HeroControllerGamePad>().UpgradeWeapon();
                IconGreenUpgrade.GetComponent<NexLevel>().ChangeIcon(HeroControllerGamePad.WeaponIndex);
                GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController>().TimingUpgrade();
            }

            Destroy(gameObject);
        }
    }
}
