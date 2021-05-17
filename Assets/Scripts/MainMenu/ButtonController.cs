using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public GameObject SinglePlayerButton;
    public GameObject MultiPlayerButton;
    public GameObject OptionsButton;
    public GameObject ExitButton;

    public GameObject ConfirmExit;
    public GameObject SingleMode;

    public void SinglePlayerTask()
    {
        SingleMode.SetActive(true);
        SinglePlayerButton.SetActive(false);
        MultiPlayerButton.SetActive(false);
        OptionsButton.SetActive(false);
        ExitButton.SetActive(false);
    }
    public void InfinityMode()
    {
        SceneManager.LoadScene("MainLevel");
        Reset();
    }
    public void BackToMenu()
    {
        SingleMode.SetActive(false);
        SinglePlayerButton.SetActive(true);
        MultiPlayerButton.SetActive(true);
        OptionsButton.SetActive(true);
        ExitButton.SetActive(true);
    }
    public void ExitTask()
    {
        ConfirmExit.SetActive(true);
        SinglePlayerButton.SetActive(false);
        MultiPlayerButton.SetActive(false);
        OptionsButton.SetActive(false);
        ExitButton.SetActive(false);
    }
    public void AgreeExit()
    {
        Application.Quit();
    }
    public void DisagreeExit()
    {
        ConfirmExit.SetActive(false);
        SinglePlayerButton.SetActive(true);
        MultiPlayerButton.SetActive(true);
        OptionsButton.SetActive(true);
        ExitButton.SetActive(true);
    }
    private void Reset()
    {
        HeroController.BonusMultiplier = 1;
        HeroController.upgradeTimeBulletShots = 0.5f;
        HeroController.upgradeTimeRocketShots = 0.3f;
        HeroController.Experience = 0;
        HeroController.WeaponIndex = 0;
        HeroController.RocketShot = false;
        HeroController.AimBot = false;
        HeroController.BulletSpeed = 10;
        HeroController.BulletSpriteIndex = 0;
        HeroController.CountOfUltimate = 3;
        HeroController.CrossHairEnabled = false;
        SceneController.isLifeBlue = false;
        SceneController.ShipLifeBlue = 3;
        HeroController.NeedSpawnSputnik = true;
        HeroController.WasSpawnedSputnik = false;
        HeroController.CurrentIndex = 0;
    }
}
