using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossHairController : MonoBehaviour
{
    public GameObject Target;
    private Camera cam;
    private Vector3 TargetPosition;
    public GameObject PauseMenu;
    public GameObject ExampleCrosshair;

    void Start()
    {
        cam = Camera.main;
    }
    private void OnEnable()
    {
        float opacity = PauseMenu.GetComponent<PauseMenuControl>().opacityCrossHair;
        gameObject.transform.localScale = ExampleCrosshair.transform.localScale;
        GetComponent<Image>().CrossFadeAlpha(opacity, 0, false);
        GetComponent<Image>().color = ExampleCrosshair.GetComponent<Image>().color;
        transform.SetSiblingIndex(0);
    }
    void Update()
    {
        if (Target)
        {
            TargetPosition = cam.WorldToScreenPoint(Target.transform.position + Target.transform.right * 3f);
            transform.position = Vector3.Lerp(transform.position, TargetPosition, 1f);
        }
        else
            gameObject.SetActive(false);
    }
}
