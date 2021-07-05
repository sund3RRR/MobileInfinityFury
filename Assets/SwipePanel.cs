using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipePanel : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private RectTransform tr;
    private Vector2 startPos;
    private Vector2 TargetPosition;
    private float offsetX;
    private Vector2 defaultPos;
    [HideInInspector] public bool IsMove;
    [HideInInspector] public bool IsMoving;
    private bool firstPhase;
    private bool secondPhase;

    private SwipeHangarPanel Panel;

    //[SerializeField] private Text Text1;
    //[SerializeField] private Text Text2;
    //[SerializeField] private Text Text3;

    void Start()
    {
        Panel = GameObject.Find("Panel").GetComponent<SwipeHangarPanel>();
        tr = transform as RectTransform;
        defaultPos = tr.anchoredPosition;
        if (PlayerPrefs.GetInt("FirstLaunch") == 0)
        {
            PlayerPrefs.SetInt("ResY", Screen.height);
            PlayerPrefs.SetInt("ResX", Screen.width);
            PlayerPrefs.SetInt("FirstLaunch", 1);
        }
        Screen.SetResolution(PlayerPrefs.GetInt("ResX"), PlayerPrefs.GetInt("ResY"), true, 120);
    }

    void Update()
    {
        //Text1.text = Screen.width.ToString();
        //Text2.text = TargetPosition.ToString();
        //Text3.text = tr.anchoredPosition.ToString();
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];

            if (touch.phase == TouchPhase.Began)
            {
                startPos = touch.position;
                IsMoving = false;
                secondPhase = false;
                firstPhase = false;
            }
            else if (touch.phase == TouchPhase.Moved && !firstPhase)
            {
                if (Mathf.Abs(startPos.x - touch.position.x) > 30)
                {
                    startPos = touch.position;
                    firstPhase = true;

                    if (!Panel.IsMoving)
                    {
                        secondPhase = true;
                        IsMoving = true;
                    }
                }                   
            }
            else if ((touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) && secondPhase)
            {
                offsetX = startPos.x - touch.position.x;
                TargetPosition = new Vector2(defaultPos.x - offsetX, 0);
                IsMove = true;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (offsetX > 100 && defaultPos.x >= -Screen.width + 10)
                {
                    defaultPos.x -= Screen.width;
                    TargetPosition = defaultPos;
                }
                else if (offsetX < -100 && defaultPos.x <= Screen.width - 10)
                {
                    defaultPos.x += Screen.width;
                    TargetPosition = defaultPos;
                }
                else
                    TargetPosition = defaultPos;
                IsMove = true;
            }           
        }
        if (IsMove && IsMoving)
        {
            tr.anchoredPosition = Vector2.Lerp(tr.anchoredPosition, TargetPosition, Time.deltaTime * moveSpeed);
            if (Mathf.Abs(((Vector2)tr.position - TargetPosition).sqrMagnitude) < 0.001f)
                IsMove = false;
        }
    }
}
