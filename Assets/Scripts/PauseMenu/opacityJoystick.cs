using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class opacityJoystick : MonoBehaviour
{
    public Image myobject;
    public float opacity;
    public Color NewColor;

    private void OnEnable()
    {
        gameObject.GetComponent<Image>().CrossFadeAlpha(opacity, 0, false);
        if (myobject)
            myobject.GetComponent<Image>().CrossFadeAlpha(opacity, 0, false);
        if (gameObject.tag == "ExampleCrossHair")
            gameObject.GetComponent<Image>().color = NewColor;
    }
}
