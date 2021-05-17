using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineVFX : MonoBehaviour
{
    public GameObject Parent;
    public Transform EffectPosition;
    public GameObject TrackVFX;
    public Transform TracksPoint;
    public ParticleSystem Effect1;
    public ParticleSystem Effect2;
    public ParticleSystem Effect3;
    public ParticleSystem Effect4;

    private ParticleSystem[] Effects;

    private bool IsMoving = true;
    public bool Cancel = false;
    private bool End = false;
    private float rotate;

    void Start()
    {
        Effects = new ParticleSystem[4] { Effect1, Effect2, Effect3, Effect4 };
        StartCoroutine(Track());
    }
    void Update()
    {
        if (!Input.GetMouseButton(1))
        {
            IsMoving = false;
            if (!Cancel)
                Destroy(gameObject, 2);
            StartCoroutine(Timer());
            Cancel = true;
        }
        transform.position = Vector2.Lerp(transform.position, EffectPosition.position, 1f);

    }
    void FixedUpdate()
    {
        foreach (ParticleSystem MyPSEffect in Effects)
        {
            if (!IsMoving)
            {
                var NewMain = MyPSEffect.main;
                NewMain.loop = false;
            }
        }
        //rotate = Mathf.Atan2(Parent.GetComponent<Rigidbody2D>().velocity.y, Parent.GetComponent<Rigidbody2D>().velocity.x) * Mathf.Rad2Deg;
        //Debug.Log(Vector2.SignedAngle(Parent.transform.right, transform.right));
        //if(Mathf.Abs(Vector2.SignedAngle(Parent.transform.right, transform.right)) < 70)
           // transform.rotation = Quaternion.AngleAxis(rotate, Vector3.forward);
    }
    IEnumerator Track()
    {
        yield return new WaitForSeconds(0.3f);
        while (!End)
        {
            GameObject NewTrackVFX = Instantiate(TrackVFX, TracksPoint.position, Quaternion.identity);
            Destroy(NewTrackVFX, 1.7f);
            yield return new WaitForSeconds(0.05f);
        }
    }
    IEnumerator Timer()
    {
        yield return new WaitForSeconds(0.5f);
        End = true;
        yield break;
    }
}