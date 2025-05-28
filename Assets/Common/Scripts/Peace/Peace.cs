




using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Peace : MonoBehaviour
{    
    [field: SerializeField] public Square Square;
    public List<Square> LegalMoves;
    public Player.Color _Color;
    public abstract List<Square> GetLegalSquares();
    public void Select()
    {
        foreach (var item in LegalMoves)
        {
            if (item != null)
            {
                item.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
    }

    public void Deselect()
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
}