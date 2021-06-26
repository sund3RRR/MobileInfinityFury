using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiveBossHeadPiece : MonoBehaviour
{
    [SerializeField] GameObject Parent;
    [SerializeField] GameObject Piece;
    [SerializeField] int PreviousHP;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<WeaponPlayer>())
        {
            PreviousHP -= collision.GetComponent<WeaponPlayer>().Damage;
            if (PreviousHP <= 0)
            {
                Piece.GetComponent<FiveBossPiece>().ActivateObject(gameObject);
                GetComponent<FiveBossPiece>().ActivateObject(Parent);
                Destroy(this);
            }
        }
    }
}
