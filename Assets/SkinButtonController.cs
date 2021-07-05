using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SkinButtonController : MonoBehaviour
{
    [SerializeField] private Color DefaultColor;
    [SerializeField] private Color ExampleColor;
    [SerializeField] private GameObject Ship;
    [SerializeField] private GameObject Rocket;
    [SerializeField] private GameObject ExampleShip;

    [SerializeField] private Sprite Ship1;
    [SerializeField] private Sprite Ship2;
    [SerializeField] private Sprite Ship3;
    [SerializeField] private Sprite Ship4;
    [SerializeField] private Sprite Ship5;
    [SerializeField] private Sprite Ship6;
    [SerializeField] private Sprite Ship7;
    [SerializeField] private Sprite Ship8;
    [SerializeField] private Sprite Ship9;
    [SerializeField] private Sprite Ship10;

    [SerializeField] private Sprite Rocket1;
    [SerializeField] private Sprite Rocket2;

    private GameObject LastButtonShip;
    private GameObject LastButtonRocket;

    private GameObject CurrentBurronShip;
    private GameObject CurrentButtonRocket;

    private void Start()
    {
        if (PlayerPrefs.GetInt("ShipIndex") != 0)
        {
            CurrentBurronShip = transform.GetChild(0).transform.GetChild(0).transform.GetChild(PlayerPrefs.GetInt("ShipIndex")).gameObject;
            CurrentBurronShip.GetComponent<Image>().color = ExampleColor;
        }
        if (PlayerPrefs.GetInt("RocketIndex") != 0)
        {
            CurrentButtonRocket = transform.GetChild(0).transform.GetChild(0).transform.GetChild(11 + PlayerPrefs.GetInt("RocketIndex")).gameObject;
            CurrentButtonRocket.GetComponent<Image>().color = ExampleColor;
        }
        switch (PlayerPrefs.GetInt("ShipIndex"))
        {
            case 0:
                Ship.GetComponent<SpriteRenderer>().sprite = Ship1;
                break;
            case 1:
                Ship.GetComponent<SpriteRenderer>().sprite = Ship1;
                break;
            case 2:
                Ship.GetComponent<SpriteRenderer>().sprite = Ship2;
                break;
            case 3:
                Ship.GetComponent<SpriteRenderer>().sprite = Ship3;
                break;
            case 4:
                Ship.GetComponent<SpriteRenderer>().sprite = Ship4;
                break;
            case 5:
                Ship.GetComponent<SpriteRenderer>().sprite = Ship5;
                break;
            case 6:
                Ship.GetComponent<SpriteRenderer>().sprite = Ship6;
                break;
            case 7:
                Ship.GetComponent<SpriteRenderer>().sprite = Ship7;
                break;
            case 8:
                Ship.GetComponent<SpriteRenderer>().sprite = Ship8;
                break;
            case 9:
                Ship.GetComponent<SpriteRenderer>().sprite = Ship9;
                break;
            case 10:
                Ship.GetComponent<SpriteRenderer>().sprite = Ship10;
                break;
        }
        switch(PlayerPrefs.GetInt("RocketIndex"))
        {
            case 0:
                Rocket.GetComponent<SpriteRenderer>().sprite = Rocket1;
                break;
            case 1:
                Rocket.GetComponent<SpriteRenderer>().sprite = Rocket1;
                break;
            case 2:
                Rocket.GetComponent<SpriteRenderer>().sprite = Rocket2;
                break;
        }

        ExampleShip.GetComponent<Image>().sprite = Ship.GetComponent<SpriteRenderer>().sprite;
        ExampleShip.GetComponent<Image>().SetNativeSize();
    }
    public void ChangeSkin(GameObject ButtonImage)
    {
        GameObject Target = null;
        GameObject ThisParent = ButtonImage.transform.parent.gameObject;
        Sprite NewSkin = ButtonImage.GetComponent<Image>().sprite;
        string GameObjectID = ButtonImage.name;      

        switch (GameObjectID)
        {
            case "Ship":
                Target = Ship;
                ExampleShip.GetComponent<Image>().sprite = NewSkin;
                ExampleShip.GetComponent<Image>().SetNativeSize();
                ThisParent.GetComponent<Image>().color = ExampleColor;
                int r = Convert.ToInt32(ButtonImage.transform.parent.name);
                PlayerPrefs.SetInt("ShipIndex", r);
                if (CurrentBurronShip)
                {
                    CurrentBurronShip.GetComponent<Image>().color = DefaultColor;
                    CurrentBurronShip = null;
                }
                if (LastButtonShip && LastButtonShip != ThisParent)
                    LastButtonShip.GetComponent<Image>().color = DefaultColor;
                LastButtonShip = ThisParent;
                break;
            case "Rocket":
                Target = Rocket;
                r = Convert.ToInt32(ButtonImage.transform.parent.name);
                ThisParent.GetComponent<Image>().color = ExampleColor;
                PlayerPrefs.SetInt("RocketIndex", r);
                if (CurrentButtonRocket)
                {
                    CurrentButtonRocket.GetComponent<Image>().color = DefaultColor;
                    CurrentButtonRocket = null;
                }
                if (LastButtonRocket && LastButtonRocket != ThisParent)
                    LastButtonRocket.GetComponent<Image>().color = DefaultColor;
                LastButtonRocket = ThisParent;
                break;
            default:
                break;
        }

        Target.GetComponent<SpriteRenderer>().sprite = NewSkin;
    }
}
