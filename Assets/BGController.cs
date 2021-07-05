using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGController : MonoBehaviour
{
    [SerializeField] private GameObject CurrentBG;
    [SerializeField] private GameObject PrefabBG;
    [SerializeField] private MeshRenderer MR;
    [SerializeField] private GameObject Parent;
    [SerializeField] private Material Mat1;
    [SerializeField] private Material Mat2;
    [SerializeField] private Material Mat3;
    [SerializeField] private Material Mat4;
    [SerializeField] private Material Mat5;
    [SerializeField] private Material Mat6;

    private MeshRenderer NewMR;

    void Start()
    {
        NewMR = Parent.transform.GetChild(2).GetComponent<MeshRenderer>();
    }
    public void ChangeBG(string ID)
    {
        Material curMat = null;
        switch(ID)
        {
            case "1":
                curMat = Mat1;
                break;
            case "2":
                curMat = Mat2;
                break;
            case "3":
                curMat = Mat3;
                break;
            case "4":
                curMat = Mat4;
                break;
            case "5":
                curMat = Mat5;
                break;
            case "6":
                curMat = Mat6;
                break;
        }
        MR.material = curMat;
        NewMR.material = curMat;
        Destroy(CurrentBG);
        PrefabBG.GetComponent<BackgroundMovingController>().scrollingSpeed = new Vector2(0, 5);
        CurrentBG = Instantiate(PrefabBG);
        NewMR = Parent.transform.GetChild(2).GetComponent<MeshRenderer>();
        MR = NewMR;
    }
}
