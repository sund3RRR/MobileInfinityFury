using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Volume : MonoBehaviour
{
    [SerializeField] private Sprite SoundOff;
    private Sprite DefaultSprite;

    private void Start()
    {
        DefaultSprite = GetComponent<Image>().sprite;
    }
    public void ChangeVolumeSFX()
    {
        if (GetComponent<Image>().sprite == DefaultSprite)
        {
            GetComponent<Image>().sprite = SoundOff;
        }
        else
        {
            GetComponent<Image>().sprite = DefaultSprite;
        }
    }
    public void ChangeVolumeMusic()
    {
        if (GetComponent<Image>().sprite == DefaultSprite)
        {
            GetComponent<Image>().sprite = SoundOff;
        }
        else
        {
            GetComponent<Image>().sprite = DefaultSprite;
        }
    }
}
