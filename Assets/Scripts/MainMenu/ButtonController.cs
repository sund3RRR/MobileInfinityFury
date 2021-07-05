using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public void StandartModePlay()
    {
        SceneManager.LoadScene("MainLevel");
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    public void UnPauseGame()
    {
        StartCoroutine(Coroutine());
    }
    IEnumerator Coroutine()
    {
        yield return new WaitForSecondsRealtime(0.01f);
        Time.timeScale = 1;
    }
}
