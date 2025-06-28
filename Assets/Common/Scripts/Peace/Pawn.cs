using System.Collections.Generic;
using System.Linq;

public class Pawn : Peace
{
    public int direction = -1;

    public override List<Square> GetLegalSquares()
    {
        List<Square> availableSquares = new()
        {
            Board.Instance.GetSquare(Square.Origin, new int[2]{ direction , 0 } ),
        };

        if (StartingPosition)
        {
            availableSquares.Add(Board.Instance.GetSquare(Square.Origin, new int[2] { direction + direction , 0 }));
        }
       
        if (availableSquares[0].Occupant != null)
        {
            availableSquares.Clear();
        }
        
        List<Square> takesquares = new()
        {
            Board.Instance.GetSquare(Square.Origin, new int[2]{ direction , -1 } ),
            Board.Instance.GetSquare(Square.Origin, new int[2]{ direction , +1 } ),
        };

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
