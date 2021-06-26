using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPointsController : MonoBehaviour
{
    //editor and public 
    public int HealthPoints;
    public int BaseHealthPoints;
    public string GameObjectName;

    //private variables
    private SceneController CurrentScene;
    private GameObject Bar;
    private HealthBarController HBContr;
    private Coroutine HBCoroutine;

    void Awake()
    {
        CurrentScene = GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController>();

        Bar = CurrentScene.HealthBar;

        BaseHealthPoints = HealthPoints;
        if (HealthPoints > 1)
        {
            GameObject Canvas = CurrentScene.CurrentCanvas;
            Bar.GetComponent<HealthBarController>().Target = gameObject;
            Bar.GetComponent<HealthBarController>().BaseHealthPoints = BaseHealthPoints;
            GameObject HealthBar = Instantiate(Bar, new Vector3(transform.position.x, transform.position.y), Quaternion.identity);
            HealthBar.transform.SetParent(Canvas.transform, false);
            HBContr = HealthBar.GetComponent<HealthBarController>();
        }
        StartCoroutine(DestroyThisObj());
    }

    IEnumerator RefreshHealthBar()
    {
        HBContr.EnableHealthBar();
        HBContr.HealthPoints = HealthPoints;
        yield return new WaitForSeconds(2.5f);
        HBContr.DisableHealthBar();
    }
    public void RefreshHBRequest()
    {
        if (HBCoroutine != null)
            StopCoroutine(HBCoroutine);
        if (HBContr)
            HBCoroutine = StartCoroutine(RefreshHealthBar());
    }   
    IEnumerator DestroyThisObj()
    {
        yield return new WaitWhile(() => HealthPoints > 0);
        if (GameObjectName != "FirstBoss")
            DestroyController.DestroyObject(GameObjectName, gameObject);
        else
            StartCoroutine(GetComponent<BossFirst>().DestroyMe());
    }
}
