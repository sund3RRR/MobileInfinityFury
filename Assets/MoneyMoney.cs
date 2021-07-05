using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class MoneyMoney : MonoBehaviour, IUnityAdsListener
{
    [SerializeField] private Text Galo;
    [SerializeField] private Text Zlotii;

    private void Start()
    {
        if (Advertisement.isSupported)
        {
            Advertisement.Initialize("4198063", false);
            StartCoroutine(Coroutine());
        }
        Advertisement.AddListener(this);
    }
    void FixedUpdate()
    {
        Galo.text = PlayerPrefs.GetFloat("Galo").ToString();
        Zlotii.text = PlayerPrefs.GetInt("Zlotii").ToString();
    }
    IEnumerator Coroutine()
    {
        while(!Advertisement.IsReady("Banner"))
        {
            yield return new WaitForSeconds(0.5f);
        }
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Show("Banner");
    }
    public void GandonAd()
    {
        StartCoroutine(Coroutine1());
      
    }
    IEnumerator Coroutine1()
    {
        while (!Advertisement.IsReady("MyVideoAd"))
        {
            yield return new WaitForSeconds(0.5f);
        }
        Advertisement.Show("MyVideoAd");
    }
    public void OnUnityAdsReady(string placementID)
    {

    }
    public void OnUnityAdsDidError(string placementID)
    {

    }
    public void OnUnityAdsDidStart(string placementID)
    {

    }
    public void OnUnityAdsDidFinish(string placementID, ShowResult Result)
    {
        if (placementID == "MyVideoAd" && Result == ShowResult.Finished)
        {
            PlayerPrefs.SetInt("Zlotii", PlayerPrefs.GetInt("Zlotii") + 5);
        }
    }
}
