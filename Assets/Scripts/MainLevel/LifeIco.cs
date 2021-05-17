using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeIco : MonoBehaviour
{
    public Text LifeCount;
    private Color basecolor;

    private void Start()
    {
        basecolor = GetComponent<Image>().color;
    }
    void FixedUpdate()
    {
        if (LifeCount.text == "0")
        {
            if (GetComponent<CanvasRenderer>().GetAlpha() < 0.01f)
                GetComponent<Image>().CrossFadeAlpha(1f, 1f, false);
            else if (GetComponent<CanvasRenderer>().GetAlpha() > 0.99f)
                GetComponent<Image>().CrossFadeAlpha(0f, 1f, false);
            GetComponent<Image>().color = Color.red;
        }
        else
        {
            GetComponent<Image>().CrossFadeAlpha(1f, 0, false);
            GetComponent<Image>().color = basecolor;
        }
    }
}
