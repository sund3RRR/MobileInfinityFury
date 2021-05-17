using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseControl : MonoBehaviour
{
    public Button Btn;
    public GameObject PauseMenu;
    public GameObject MoveJoystick;
    public GameObject FireJoystick;

    void Start()
    {
        Btn.onClick.AddListener(TaskOnclick);
    }

    void TaskOnclick()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
        gameObject.SetActive(false);
        MoveJoystick.SetActive(false);
        FireJoystick.SetActive(false);
    }
}
