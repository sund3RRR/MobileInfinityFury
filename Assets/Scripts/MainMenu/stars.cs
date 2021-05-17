using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class stars : MonoBehaviour
{
    // Private variables
    private Vector2 MoveVector;
    private Image Background;

    void Start()
    {
        Time.timeScale = 1f;
        Background = GetComponent<Image>();
        Color backupColor = Background.color;
        backupColor.a = 1;
        Background.color = backupColor;
        Background.CrossFadeAlpha(0, 0f, false);
    }

    void FixedUpdate()
    {
        transform.Translate(MoveVector * Time.deltaTime * 10);
    }
    void ScreenOn()
    {
        GetComponent<Image>().CrossFadeAlpha(1, 0.5f, false);
    }
    public void ScreenOff()
    {
        GetComponent<Image>().CrossFadeAlpha(0, 0.5f, false);
    }
    public void ChangePosition(Vector3 Position)
    {
        transform.position = Position;
        MoveVector = new Vector2(-Position.normalized.x, -Position.normalized.y);
        ScreenOn();
    }
}