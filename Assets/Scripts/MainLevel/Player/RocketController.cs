using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : WeaponPlayer
{
    [SerializeField] private float acceleration;
    [SerializeField] private Vector2 xOffset;
    [SerializeField] private Vector2 yOffset;
    private Vector2 offsetPosition;

    void Start()
    {
        float x = Random.Range(xOffset.x, xOffset.y);
        float y = Random.Range(yOffset.x, yOffset.y);

        offsetPosition = new Vector2(transform.position.x + x, transform.position.y - y);

        StartCoroutine(Move());
        Destroy(gameObject, DestroyTime);
    }
    IEnumerator Move()
    {
        while((Vector2)transform.position != offsetPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, offsetPosition, Time.deltaTime * Speed);
            yield return null;
        }
        while(gameObject)
        {
            Speed += acceleration;
            transform.Translate(-transform.up * Time.deltaTime * Speed);
            yield return null;
        }
    }
}