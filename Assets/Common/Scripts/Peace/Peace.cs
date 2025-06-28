




using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Peace : MonoBehaviour
{
    [field: SerializeField] public Square Square;
    public List<Square> LegalMoves;
    public int _Color;
    public bool StartingPosition = true;
    public abstract List<Square> GetLegalSquares();
 
    // i dont like this virtual but the king will need it to light castle squares
    public virtual void Select()
    {
        foreach (var item in LegalMoves)
        {
            if (item != null)
            {
                item.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
    }

    public virtual void Deselect()
    {
        
        if (Square == null)
        {
            return;
        }

        foreach (var item in LegalMoves)
        {
            if (item != null)
            {
                item.gameObject.GetComponent<SpriteRenderer>().color = item.DefaultColor;
            }
        }
    }

    public void Move(Square to, bool startingPosition)
    {
        StartingPosition = startingPosition;
        Square = to;
        Square.Occupant = this;
        transform.position = Square.transform.position;
    }
}