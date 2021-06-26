using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ScoreController : MonoBehaviour
{
    // Editor variables
    public Text myScore;
    public Text myLife;
    public Text myMultiplier;

    public GameObject Strelka1;
    public GameObject LVLTextCount;
    public GameObject Score1;
    public GameObject Score1Count;
    public GameObject Multi1;
    public GameObject Multi1Count;
    public GameObject Lvl1;
    public GameObject lvl1Next;
    public GameObject icon1;
    public GameObject LifeText1;
    public GameObject LifeCount1;

    // private variables
    private bool f = false;
    private GameObject Player;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("PlayerBlue");
    }
    void FixedUpdate()
    {
        switch(tag)
        {
            case "BlueController":
                myScore.text = Player.GetComponent<HeroController>().Experience.ToString();
                myLife.text = SceneController.ShipLifeBlue.ToString();
                myMultiplier.text = Player.GetComponent<HeroController>().BonusMultiplier.ToString();
                break;
        }     
        if (!f && tag == "GreenController" && Gamepad.current != null)
        {
            Strelka1.SetActive(true);
            LVLTextCount.SetActive(true);
            Score1.SetActive(true);
            Score1Count.SetActive(true);
            Multi1.SetActive(true);
            Multi1Count.SetActive(true);
            Lvl1.SetActive(true);
            lvl1Next.SetActive(true);
            icon1.SetActive(true);
            LifeCount1.SetActive(true);
            LifeText1.SetActive(true);
            f = true;
        }
    }
}
