using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pawn : Peace
{
    public int direction = -1;
    public bool startingPosition = true;

    public override List<Square> GetLegalSquares()
    {
        List<Square> availableSquares = new()
        {
            Board.Instance.GetSquare(Square.Origin, new int[2]{ direction , 0 } ),
        };

        List<Square> takesquares = new()
        {
            Board.Instance.GetSquare(Square.Origin, new int[2]{ direction , -1 } ),
            Board.Instance.GetSquare(Square.Origin, new int[2]{ direction , +1 } ),
        };

        if (startingPosition)
        {
            availableSquares.Add(Board.Instance.GetSquare(Square.Origin, new int[2] { direction + direction , 0 }));
        }
       
        for (int i = 0; i < takesquares.Count; i++)
        {
            if (takesquares[i] != null)
            {
                if (takesquares[i].Occupant != null)
                {
                    if (!GameManager.Instance.CurrentPlayer().Peaces.Contains(takesquares[i].Occupant))
                    {
                        availableSquares.Add(takesquares[i]);
                    }
                }
            }
        }            
        
        return availableSquares.FindAll(x => x != null).ToList();
    }
}
