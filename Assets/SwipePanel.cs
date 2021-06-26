using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipePanel : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private RectTransform tr;
    private Vector2 startPos;
    private Vector2 TargetPosition;
    private float offsetPosition;
    private Vector2 defaultPos;
    private bool IsMove;

    void Start()
    {
        tr = transform as RectTransform;
        defaultPos = tr.anchoredPosition;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];

            if (touch.phase == TouchPhase.Began)
                startPos = touch.position;
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                offsetPosition = startPos.x - touch.position.x;
                TargetPosition = new Vector2(defaultPos.x - offsetPosition, 0);
                IsMove = true;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (offsetPosition > 100)
                {
                    defaultPos.x -= Screen.width;
                    TargetPosition = defaultPos;
                }
                else if (offsetPosition < -100)
                {
                    defaultPos.x += Screen.width;
                    TargetPosition = defaultPos;
                }
                else
                    TargetPosition = defaultPos;
                IsMove = true;
            }           
        }      
        if (IsMove)
        {
            tr.anchoredPosition = Vector2.Lerp(tr.anchoredPosition, TargetPosition, Time.deltaTime * moveSpeed);
            if (Mathf.Abs(((Vector2)tr.position - TargetPosition).sqrMagnitude) < 0.001f)
                IsMove = false;
        }
    }
}
