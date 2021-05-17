using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningEffect : MonoBehaviour
{
    public ParticleSystem effect1;
    public ParticleSystem effect2;
    public ParticleSystem effect3;
    public ParticleSystem effect4;
    public ParticleSystem effect5;
    public ParticleSystem effect6;
    public ParticleSystem effect7;
    public ParticleSystem effect8;
    public ParticleSystem effect9;
    public GameObject DustPos;
    private ParticleSystem[] PSArray;

    public GameObject Parent;
    public GameObject CrashEffect;
    public GameObject Bullet1;
    public GameObject Bullet2;

    private Vector2 MoveVector;

    private Rigidbody2D rb2D;
    public float speed;
    
    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();

        var main = effect1.main;
        ParticleSystem[] PSArray = new ParticleSystem[] { effect1, effect2, effect3, effect4, effect5, effect6, effect7, effect8, effect9 };      

        foreach (ParticleSystem PS in PSArray)
        {
            main = PS.main;

            if (PS == effect2 || PS == effect4)        
                main.startRotation = Mathf.Deg2Rad * (Vector2.SignedAngle(Parent.transform.right, transform.right) + 180);
            else
                main.startRotation = Mathf.Deg2Rad * (Vector2.SignedAngle(Parent.transform.right, transform.right));
        }

        DustPos.transform.rotation = Parent.transform.rotation;
        MoveVector = Parent.transform.right;
        Destroy(gameObject, 3.9f);
    }
    private void FixedUpdate()
    {
        rb2D.velocity = (MoveVector * speed);

        if (!Bullet1 || !Bullet2)
            CrashObject();
    }
    public void CrashObject()
    {     
        GameObject NewCrashEffect = Instantiate(CrashEffect, transform.position, Quaternion.identity);
        NewCrashEffect.GetComponent<Rigidbody2D>().velocity = MoveVector * speed;
        Destroy(gameObject);
    }    
}
