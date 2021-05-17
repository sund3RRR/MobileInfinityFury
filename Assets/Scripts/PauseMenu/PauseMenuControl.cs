using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuControl : MonoBehaviour
{
    public GameObject MainMoveJoystick;
    public GameObject MainMoveJoystickHandle;
    public GameObject MainMoveJoystickBackground;
    public GameObject MainFireJoystickBackground;
    public GameObject MainFireJoystickHandle;
    public GameObject MainFireJoystick;
    public GameObject PauseButton;
    public GameObject PauseMenu;
    public GameObject Restart;
    public GameObject Resume;
    public GameObject Options;
    public GameObject ExitToMenu;
    public GameObject VFXLow;
    public GameObject VFXHigh;
    public GameObject VFXQ;
    public GameObject OptionsPanel;
    public GameObject MenuButtons;
    // Resume Task
    public GameObject MoveJoystick;
    public GameObject FireJoystick;
    // RestartTask
    public GameObject ConfirmRestart;

    // ExitTask
    public GameObject ConfirmExit;

    // OptionsTask
    public GameObject JoystickBtn;

    public GameObject JoystickOptions;
    public GameObject ExampleJoystick;
    public GameObject ExampleHandle;
    public GameObject OptionsWindow;

    public GameObject ExampleCrossHair;
    public GameObject CrossHairOptions;

    private float opacityJoystick = 1;
    public float opacityCrossHair = 1;
    public Color CrossHairColor;

    public GameObject EnableDisableCH;
    public Text EnableDisableCHText;
    //
    // Resume
    //
    public void ResumeTask()
    {
        Time.timeScale = 1f;
        PauseButton.SetActive(true);
        PauseMenu.SetActive(false);
        /*
        MoveJoystick.SetActive(true);
        FireJoystick.SetActive(true);
        */
    }
    //
    //
    // Resume
    //

    // Restart
    //
    public void RestartTask()
    {
        MenuButtons.SetActive(false);
        ConfirmRestart.SetActive(true);
        OptionsPanel.SetActive(false);
    }
    public void DisagreeRestartTask()
    {
        ConfirmRestart.SetActive(false);
        MenuButtons.SetActive(true);
        OptionsPanel.SetActive(true);
    }
    public void AgreeRestartTask()
    {
        SceneManager.LoadScene("MainLevel");
        ConfirmRestart.SetActive(false);
        MenuButtons.SetActive(true);
        OptionsPanel.SetActive(true);
        PauseMenu.SetActive(false);
        Reset();
    }
    //
    // Restart
    // 

    //
    // Options
    //
    public void OptionsTask()
    {
        MenuButtons.SetActive(false);
        OptionsWindow.SetActive(true);
    }
    public void BackToPauseMenu()
    {
        OptionsWindow.SetActive(false);
        MenuButtons.SetActive(true);
    }
    //
    // Options
    //

    //
    // Settings
    //
    public void JoystickTask()
    {
        OptionsWindow.SetActive(false);
        JoystickOptions.SetActive(true);
    }
    public void CrossHairTask()
    {
        OptionsWindow.SetActive(false);
        CrossHairOptions.SetActive(true);
    }
    public void ConfirmJoystickChangesAndBack()
    {
        MainMoveJoystickBackground.GetComponent<opacityJoystick>().opacity = opacityJoystick;
        MainFireJoystickBackground.GetComponent<opacityJoystick>().opacity = opacityJoystick;
        MainFireJoystickHandle.GetComponent<opacityJoystick>().opacity = opacityJoystick;
        MainMoveJoystickHandle.GetComponent<opacityJoystick>().opacity = opacityJoystick;
        ExampleJoystick.GetComponent<opacityJoystick>().opacity = opacityJoystick;
        MainMoveJoystick.transform.localScale = ExampleJoystick.transform.localScale;
        MainFireJoystick.transform.localScale = ExampleJoystick.transform.localScale;
        JoystickOptions.SetActive(false);
        OptionsWindow.SetActive(true);
    }
    public void ConfirmCrossHairChangesAndBack()
    {
        CrossHairOptions.SetActive(false);
        OptionsWindow.SetActive(true);
    }
    public void ChangeOpacityJoystick(float x)
    {
        ExampleJoystick.GetComponent<Image>().CrossFadeAlpha(1 - x, 0, false);
        ExampleHandle.GetComponent<Image>().CrossFadeAlpha(1 - x, 0, false);
        opacityJoystick = 1 - x;
    }
    public void ChangeSizeJoystick(float x)
    {
        ExampleJoystick.transform.localScale = new Vector3(x, x, x);
    }
    public void ChangeSizeCrossHair(float x)
    {
        ExampleCrossHair.transform.localScale = new Vector3(x, x, x);
    }
    public void ChangeOpacityCrossHair(float x)
    {
        ExampleCrossHair.GetComponent<Image>().CrossFadeAlpha(1 - x, 0, false);
        ExampleCrossHair.GetComponent<opacityJoystick>().opacity = 1 - x;
        opacityCrossHair = 1 - x;
    }
    //
    // Settings
    //

    //
    // Exit
    //
    public void ExitToMenuTask()
    {
        ConfirmExit.SetActive(true);
        MenuButtons.SetActive(false);
        OptionsPanel.SetActive(false);
    }
    public void AgreeExitToMenu()
    {
        SceneManager.LoadScene("MainMenu");
        ConfirmExit.SetActive(false);
        MenuButtons.SetActive(true);
        OptionsPanel.SetActive(false);
        PauseMenu.SetActive(false);      
    }
    public void DisagreeExitToMenu()
    {
        ConfirmExit.SetActive(false);
        MenuButtons.SetActive(true);
        OptionsPanel.SetActive(true);
    }
    //
    // Exit
    //

    public void EnableCrossHair()
    {
        if (EnableDisableCHText.text == "enabled")
            EnableDisableCHText.text = "disabled";
        else
            EnableDisableCHText.text = "enabled";

        HeroControllerGamePad.CrossHairEnabled = !HeroControllerGamePad.CrossHairEnabled;
    }
    public void ChangeColor(GameObject Target)
    {
        CrossHairColor = Target.GetComponent<Image>().color;
        ExampleCrossHair.GetComponent<Image>().color = CrossHairColor;
        ExampleCrossHair.GetComponent<opacityJoystick>().NewColor = CrossHairColor;
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
        HeroController.NeedSpawnSputnik = true;
        HeroController.WasSpawnedSputnik = false;
        HeroController.CurrentIndex = 0;
        HeroControllerGamePad.BonusMultiplier = 1;
        HeroControllerGamePad.upgradeTimeBulletShots = 0.5f;
        HeroControllerGamePad.upgradeTimeRocketShots = 0.3f;
        HeroControllerGamePad.Experience = 0;
        HeroControllerGamePad.WeaponIndex = 0;
        HeroControllerGamePad.RocketShot = false;
        HeroControllerGamePad.AimBot = false;
        HeroControllerGamePad.BulletSpeed = 10;
        HeroControllerGamePad.BulletSpriteIndex = 0;
        HeroControllerGamePad.CountOfUltimate = 3;
        HeroControllerGamePad.CrossHairEnabled = false;
        HeroControllerGamePad.NeedSpawnSputnik = true;
        HeroControllerGamePad.WasSpawnedSputnik = false;
        HeroControllerGamePad.CurrentIndex = 0;
        SceneController.isLifeBlue = false;
        SceneController.ShipLifeBlue = 3;
        SceneController.isLifeGreen = false;
        SceneController.ShipLifeGreen = 3;
    }
}
