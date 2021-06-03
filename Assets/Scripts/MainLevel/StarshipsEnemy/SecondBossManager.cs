using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondBossManager : MonoBehaviour
{
    private GameObject Player;
    public float TurboTime;
    public float CircleSpeed;
    public GameObject SecondBoss1;
    public GameObject SecondBoss2;
    private Coroutine Coroutine1;
    private Coroutine Coroutine2;
    private Vector2 TargetPosition;
    private float Timer;
    public bool IsTurbo;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("PlayerBlue");
        Player.GetComponent<HeroController>().ParentForSecondBoss = gameObject;
        StartCoroutine(Manager());
    }
    IEnumerator Manager()
    {
        yield return new WaitWhile(() => SecondBoss1.GetComponent<HealthPointsController>().HealthPoints > SecondBoss1.GetComponent<HealthPointsController>().BaseHealthPoints / 2f);

        StartTurbo();

        yield return new WaitWhile(() => SecondBoss1.GetComponent<HealthPointsController>().HealthPoints > 2);

        StartTurbo();

        yield return new WaitWhile(() => SecondBoss1);

        SecondBoss2.GetComponent<SecondBoss>().SwapToUp();

        yield return new WaitWhile(() => SecondBoss2);

        Destroy(gameObject);
    }

    void StartTurbo()
    {
        StartCoroutine(Player.GetComponent<HeroController>().Stun());
        IsTurbo = true;
        transform.position = GameObject.FindGameObjectWithTag("PlayerBlue").transform.position;
        SecondBoss1.GetComponent<SecondBoss>().StopAllCoroutines();
        SecondBoss2.GetComponent<SecondBoss>().StopAllCoroutines();
        float dist1 = Mathf.Abs((new Vector2(transform.position.x + SecondBoss1.GetComponent<SecondBoss>().Radius, transform.position.y)
            - (Vector2)SecondBoss1.transform.position).magnitude),
              dist2 = Mathf.Abs((new Vector2(transform.position.x - SecondBoss2.GetComponent<SecondBoss>().Radius, transform.position.y)
              - (Vector2)SecondBoss2.transform.position).magnitude);
        Coroutine1 = StartCoroutine(SecondBoss1.GetComponent<SecondBoss>().Turbo(gameObject, transform.position.y > 0 ? dist1 / dist2 : 1, false));
        Coroutine2 = StartCoroutine(SecondBoss2.GetComponent<SecondBoss>().Turbo(gameObject, transform.position.y < 0 ? dist2 / dist1 : 1, true));
        StartCoroutine(ChangePositionTurbo());    
    }
    IEnumerator ChangePositionTurbo()
    {
        yield return new WaitForSeconds(4f);
        TargetPosition = new Vector2(Random.Range(-1f, 1f), Random.Range(-3f, 3f));
        while(true)
        {
            Timer += Time.deltaTime;
            transform.Translate((TargetPosition - (Vector2)transform.position).normalized * Time.deltaTime * CircleSpeed);
            if (((Vector2)transform.position - TargetPosition).sqrMagnitude < 1f)
                TargetPosition = new Vector2(Random.Range(-1f, 1f), Random.Range(-3f, 3f));
            if (Timer >= TurboTime && SecondBoss1.transform.position.x < SecondBoss2.transform.position.x && 
                Mathf.Abs(SecondBoss1.transform.position.y - SecondBoss2.transform.position.y) < 0.1f)
            {
                StopCoroutine(Coroutine1);
                StopCoroutine(Coroutine2);
                SecondBoss1.transform.SetParent(null);
                SecondBoss2.transform.SetParent(null);
                SecondBoss1.GetComponent<SecondBoss>().ResetDefault(false);
                SecondBoss2.GetComponent<SecondBoss>().ResetDefault(true);             
                Timer = 0;
                IsTurbo = false;
                yield break;
            }
            yield return null;
        }
    }
}