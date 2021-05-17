using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionLocker : MonoBehaviour
{
    public GameObject Target;

    private void Start()
    {
        switch(Target.gameObject.tag)
        {
            case "PlayerBlue":
                Destroy(gameObject, 2);
                break;
            default:
                break;             
        }
        
    }
    void Update()
    {
        if (Target)
            transform.position = Vector3.Lerp(transform.position, Target.transform.position, 1f);
        else
            Destroy(gameObject);
    }
}
