using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    // Editor variables   
    public Image bar;
    public Image MyBackground;

    // Private variables
    private float fill;
    private float delta;
    private int offset;
    private Camera cam;
    private Vector3 TargetPosition;
    private Vector3 HBPosition;

    // Public variables
    public GameObject Target;
    public float BaseHealthPoints;
    public float HealthPoints;

    private void Awake()
    {
        MyBackground.GetComponent<Image>().CrossFadeAlpha(0f, 0, false);
        bar.GetComponent<Image>().CrossFadeAlpha(0f, 0, false);
        transform.SetSiblingIndex(0);
        cam = Camera.main;
        transform.localScale = new Vector3(transform.localScale.x * 4f, transform.localScale.y * 4f, transform.localScale.z);
        if (Target)
            switch (Target.name)
            {
                case "Zond(Clone)":
                    gameObject.transform.localScale = new Vector3(transform.localScale.x * 2.5f, transform.localScale.y, transform.localScale.z);
                    offset = 130;
                    break;
                case "FirstBoss(Clone)":
                    gameObject.transform.localScale = new Vector3(transform.localScale.x * 6f, transform.localScale.y, transform.localScale.z);
                    offset = 130;
                    break;
                case "ThirdBoss(Clone)":
                    gameObject.transform.localScale = new Vector3(transform.localScale.x * 5f, transform.localScale.y, transform.localScale.z);
                    offset = 150;
                    break;
                case "FourthBoss(Clone)":
                    gameObject.transform.localScale = new Vector3(transform.localScale.x * 5f, transform.localScale.y, transform.localScale.z);
                    offset = 150;
                    break;
                case "Panel":
                    gameObject.transform.localScale = new Vector3(transform.localScale.x * 1.5f, transform.localScale.y, transform.localScale.z);
                    offset = 90;
                    break;
                case "BossPiece":
                    gameObject.transform.localScale = new Vector3(transform.localScale.x * 1.5f, transform.localScale.y, transform.localScale.z);
                    offset = 90;
                    break;
                case "BigPiece(Clone)":
                    gameObject.transform.localScale = new Vector3(transform.localScale.x * 1.5f, transform.localScale.y, transform.localScale.z);
                    offset = 90;
                    break;
                case "Sputnik(Clone)":
                    gameObject.transform.localScale = new Vector3(transform.localScale.x * 1.5f, transform.localScale.y, transform.localScale.z);
                    offset = 100;
                    break;
                case "Sphere":
                    gameObject.transform.localScale = new Vector3(transform.localScale.x * 1.5f, transform.localScale.y, transform.localScale.z);
                    offset = 100;
                    break;
                case "GoldAsteroid(Clone)":
                    gameObject.transform.localScale = new Vector3(transform.localScale.x * 2.5f, transform.localScale.y, transform.localScale.z);
                    offset = 150;
                    break;
                case "SlimStarshipEnemy(Clone)":
                    gameObject.transform.localScale = new Vector3(transform.localScale.x * 1.5f, transform.localScale.y, transform.localScale.z);
                    offset = 80;
                    break;
                case "FatStarshipEnemy(Clone)":
                    gameObject.transform.localScale = new Vector3(transform.localScale.x * 2.5f, transform.localScale.y, transform.localScale.z);
                    offset = 130;
                    break;
                case "DestroyerEnemyStarship(Clone)":
                    gameObject.transform.localScale = new Vector3(transform.localScale.x * 2.5f, transform.localScale.y, transform.localScale.z);
                    offset = 130;
                    break;
                default:
                    if (Target.GetComponent<AsteroidController>())
                        switch (Target.GetComponent<AsteroidController>().indexScale)
                        {
                            case 3:
                                offset = 150;
                                gameObject.transform.localScale = new Vector3(transform.localScale.x * 1.3f, transform.localScale.y, transform.localScale.z);
                                break;
                            case 2:
                                offset = 110;
                                break;
                            case 1:
                                offset = 80;
                                gameObject.transform.localScale = new Vector3(transform.localScale.x * 0.75f, transform.localScale.y, transform.localScale.z);
                                break;
                            default:
                                break;
                        }
                    break;
            }
    }
    void Update()
    {     
        if (Target)
        {
            if (Target.name == "Sputnik(Clone)")
                TargetPosition = Target.GetComponent<SputnikController>().CenterOfSputnik.position;
            else
                TargetPosition = Target.transform.position;
            delta = HealthPoints / BaseHealthPoints;

            HBPosition = cam.WorldToScreenPoint(TargetPosition);
            HBPosition = new Vector2(HBPosition.x, HBPosition.y + offset);

            transform.position = Vector3.Lerp(transform.position, HBPosition, 1f);
            
            fill = 1f - delta;          
            bar.fillAmount = fill;
        }
        else
            Destroy(gameObject);      
    }
    public void EnableHealthBar()
    {
        transform.SetSiblingIndex(0);
        MyBackground.GetComponent<Image>().CrossFadeAlpha(1f, 0, false);
        bar.GetComponent<Image>().CrossFadeAlpha(1f, 0, false);
    }
    public void DisableHealthBar()
    {
        MyBackground.GetComponent<Image>().CrossFadeAlpha(0f, 0.2f, false);
        bar.GetComponent<Image>().CrossFadeAlpha(0f, 0.2f, false);
    }
}