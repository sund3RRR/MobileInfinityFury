using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusController : MonoBehaviour
{
    // Editor variables
    [SerializeField] private Sprite MultiplierBonusSprite;
    [SerializeField] private Sprite LifeBonusSprite;
    [SerializeField] private Sprite UltimateBonusSprite;
    [SerializeField] private Sprite UpgradeBonusSprite;
    [SerializeField] private GameObject UpgradeBonusVFX;
    [SerializeField] private int DestroyTime;   
    [SerializeField] private float speed;
    // Private variables
    private GameObject Player;
    private float LifeTime = 0;
    private float myAlpha = 0;
    private GameObject IconUpgrade;

    // Public variables
    public Vector2 force;
    
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("PlayerBlue");
        IconUpgrade = GameObject.FindGameObjectWithTag("BlueLvl");

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
        transform.Translate(force.normalized * Time.deltaTime * speed, Space.World);

        if (!gameObject.GetComponent<Renderer>().isVisible)
            Destroy(gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerBlue")
        {
            if (gameObject.tag == "UltimateBonus")
                Player.GetComponent<HeroController>().CountOfUltimate += 1;
            else if (gameObject.tag == "LifeBonus")
                SceneController.ShipLifeBlue += 1;
            else if (gameObject.tag == "MultiplierBonus")
                collision.GetComponent<HeroController>().BonusMultiplier += 1;
            else if (gameObject.tag == "UpgradeBonus")
            {

                collision.GetComponent<HeroController>().WeaponIndex += 1;
                //IconUpgrade.GetComponent<NexLevel>().ChangeIcon(collision.GetComponent<HeroController>().WeaponIndex);
                collision.GetComponent<HeroController>().UpgradeWeapon();
                //GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController>().TimingUpgrade();
                collision.GetComponent<HeroController>().AnimationUpgrade();
                
            }
            
            Destroy(gameObject);
        }
    }
}
