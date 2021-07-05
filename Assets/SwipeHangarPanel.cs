using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeHangarPanel : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private GameObject Parent;
    private RectTransform tr;
    private Vector2 startPos;
    private Vector2 TargetPosition;
    private float offsetY;
    private Vector2 defaultPos;
    private bool firstPhase;
    private bool secondPhase;
    [HideInInspector] public bool IsMoving;
    private SwipePanel Panel;

    private bool IsMove;

    void Start()
    {
        Parent = GameObject.Find("BigParent");
        Panel = Parent.GetComponent<SwipePanel>();
        tr = transform as RectTransform;
    }

    void Update()
    {
        if (Input.touchCount > 0 && Parent.transform.position.x > Screen.width - 100)
        {
            Touch touch = Input.touches[0];

            if (touch.phase == TouchPhase.Began)
            {
                startPos = touch.position;
                IsMoving = false;
                firstPhase = false;
                secondPhase = false;
            }
            else if (touch.phase == TouchPhase.Moved && !firstPhase)
            {
                if (Mathf.Abs(startPos.y - touch.position.y) > 30)
                {
                    startPos = touch.position;
                    firstPhase = true;

                    if (!Panel.IsMoving)
                    {
                        IsMoving = true;
                        secondPhase = true;
                    }
                }
            }
            else if ((touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) && secondPhase)
            {   
                offsetY = startPos.y - touch.position.y;
                TargetPosition = new Vector2(0, defaultPos.y - offsetY);

                if (TargetPosition.y > 325)
                    TargetPosition.y = 325;
                else if (TargetPosition.y < 0)
                    TargetPosition.y = 0;

                defaultPos = TargetPosition;
            }
        }
        if (IsMoving || IsMove)
        {
            IsMove = true;
            tr.anchoredPosition = Vector2.Lerp(tr.anchoredPosition, TargetPosition, Time.deltaTime * moveSpeed);
            if (Mathf.Abs(((Vector2)tr.position - TargetPosition).sqrMagnitude) < 0.001f)
            {
                IsMove = false;
            }               
        }
    }
}
