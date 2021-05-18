using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSC : MonoBehaviour
{
    public float fps;
    public Text MyFPS;
    public Text MyMinFPS;
    private int min = 1000;
    
    private void Start()
    {
        StartCoroutine(MyFPSCounter());
    }
    void Update()
    {
        if (Time.deltaTime != 0)
            fps = 1.0f / Time.deltaTime;
        if (fps < min)
        {
            min = (int)fps;
            MyMinFPS.text = min.ToString();
        }
    }
    IEnumerator MyFPSCounter()
    {
        while (true)
        {           
            MyFPS.text = ((int)fps).ToString();
            yield return new WaitForSeconds(0.1f);
        }
    }
    public void Refresher()
    {
        min = 10000;
    }
}