using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondBossManager : MonoBehaviour
{
    public GameObject SecondBoss1;
    public GameObject SecondBoss2;
    private Coroutine myCoroutine1;
    private Coroutine myCoroutine2;
    private Vector2 TargetPosition;

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (SecondBoss1.GetComponent<HealthPointsController>().HealthPoints <= 15 && myCoroutine1 == null)
        {
            transform.position = GameObject.FindGameObjectWithTag("PlayerBlue").transform.position;
            SecondBoss1.GetComponent<SecondBoss1>().StopAllCoroutines();
            SecondBoss2.GetComponent<SecondBoss2>().StopAllCoroutines();
            myCoroutine1 = StartCoroutine(SecondBoss1.GetComponent<SecondBoss1>().Turbo(gameObject));
            myCoroutine2 = StartCoroutine(SecondBoss2.GetComponent<SecondBoss2>().Turbo(gameObject));
            StartCoroutine(ChangePositionTurbo());
        }
    }
    IEnumerator ChangePositionTurbo()
    {
        yield return new WaitForSeconds(4f);
        TargetPosition = new Vector2(Random.Range(-1.5f, 1.5f), Random.Range(-4.5f, 4.5f));
        while(true)
        {
            transform.Translate(TargetPosition.normalized * Time.deltaTime);
            if (transform.position.sqrMagnitude - TargetPosition.sqrMagnitude < 0.1f)
                TargetPosition = new Vector2(Random.Range(-1.5f, 1.5f), Random.Range(-4.5f, 4.5f));
            yield return null;
        }
    }
}