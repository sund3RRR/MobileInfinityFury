using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ScoreController : MonoBehaviour
{
    // Editor variables
    [SerializeField] private Text myScore;
    [SerializeField] private Text myLife;
    [SerializeField] private Text myMultiplier;
    [SerializeField] private Text myUlti;

    // private variables
    private GameObject Player;

    private void Start()
    {
        StartCoroutine(Coroutine());
    }
    void FixedUpdate()
    {
        if (Player)
        {
            myScore.text = PlayerPrefs.GetFloat("Galo").ToString();
            myLife.text = SceneController.ShipLifeBlue.ToString();
            myMultiplier.text = Player.GetComponent<HeroController>().BonusMultiplier.ToString();
            myUlti.text = Player.GetComponent<HeroController>().CountOfUltimate.ToString();
        }
    }
    IEnumerator Coroutine()
    {
        while (!Player)
        {
            Player = GameObject.FindGameObjectWithTag("PlayerBlue");
            yield return null;
        }
    }
}
