using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class background : MonoBehaviour
{
    // Editor variables
    public GameObject Stars;
    public Sprite Blue;
    public Sprite Red;
    public Sprite Pink;

    // Private variables
    private Sprite[] MyBackgrounds = new Sprite[3];
    private Vector2 SpawnPosition;
    private Vector2 MoveVector;
    private float LifeTime = 0;
    private Image Background;

    void Start()
    {
        MyBackgrounds[0] = Blue;
        MyBackgrounds[1] = Red;
        MyBackgrounds[2] = Pink;
        Time.timeScale = 1f;
        Background = GetComponent<Image>();
        Color backupColor = Background.color;
        backupColor.a = 1;
        Background.color = backupColor;
        Background.CrossFadeAlpha(0, 0f, false);
        
        ScreenOn();
        ChangeBackground();
    }
    void FixedUpdate()
    {
        LifeTime += Time.deltaTime;
        MoveBackground();
        if (LifeTime > 15)
        {
            ScreenOff();
            Stars.GetComponent<stars>().ScreenOff();
        }
        if (GetComponent<CanvasRenderer>().GetAlpha() <= 0.05f)
        {
            ChangeBackground();
        }
    }
    void ScreenOff()
    {
        GetComponent<Image>().CrossFadeAlpha(0, 0.5f, false);
    }
    void ScreenOn()
    {
        GetComponent<Image>().CrossFadeAlpha(1, 0.5f, false);
    }
    void ChangeBackground()
    {
        GetComponent<Image>().sprite = MyBackgrounds[Random.Range(0, 2)];
        LifeTime = 0;
        SpawnPosition = new Vector2(150 * Mathf.Pow(-1, Random.Range(1, 2)) * Random.Range(-1.5f, 1.5f),
            120 * Mathf.Pow(-1, Random.Range(1, 2)) * Random.Range(-1.5f, 1.5f));
        transform.position = SpawnPosition;
        MoveVector = new Vector2(-SpawnPosition.normalized.x, -SpawnPosition.normalized.y);
        Stars.GetComponent<stars>().ChangePosition(SpawnPosition);
        ScreenOn();
    }
    void MoveBackground()
    {
        transform.Translate(MoveVector * Time.deltaTime * 20);
    }
}
