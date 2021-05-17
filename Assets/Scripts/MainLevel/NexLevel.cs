using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NexLevel : MonoBehaviour
{
    public Sprite lvl0;
    public Sprite lvl1;
    public Sprite lvl2;
    public Sprite lvl3;
    public Sprite lvl4;
    public Sprite lvl5;
    public Sprite lvl6;
    public Sprite lvl7;
    public Sprite lvl8;
    private Sprite[] MySprites;

    public Image CurrentLvl;
    public GameObject Strelka;

    void Start()
    {
        MySprites = new Sprite[9] { lvl0, lvl1, lvl2, lvl3, lvl4, lvl5, lvl6, lvl7, lvl8 };
    }

    void FixedUpdate()
    {
        if (GetComponent<CanvasRenderer>().GetAlpha() < 0.01f)
            GetComponent<Image>().CrossFadeAlpha(1f, 1f, false);
        else if (GetComponent<CanvasRenderer>().GetAlpha() > 0.99f)
            GetComponent<Image>().CrossFadeAlpha(0f, 1f, false);

        Strelka.GetComponent<CanvasRenderer>().SetAlpha(GetComponent<CanvasRenderer>().GetAlpha());
    }
    public void ChangeIcon(int level)
    {
        CurrentLvl.GetComponent<Image>().sprite = GetComponent<Image>().sprite;
        if (level < 8)
            GetComponent<Image>().sprite = MySprites[level + 1];
        else
        {
            Destroy(gameObject);
            Destroy(Strelka);
        }
    }
}
