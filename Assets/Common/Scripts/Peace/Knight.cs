



using System.Collections.Generic;
using System.Linq;

public class Knight : Peace
{
    public override List<Square> GetLegalSquares()
    {
        List<Square> availableSquares = new()
        {
            Board.Instance.GetSquare(Square.Origin, new int[2]{ +1, +2 } ),
            Board.Instance.GetSquare(Square.Origin, new int[2]{ +2, +1 } ),
            Board.Instance.GetSquare(Square.Origin, new int[2]{ +1, -2 } ),
            Board.Instance.GetSquare(Square.Origin, new int[2]{ +2, -1 } ),
            Board.Instance.GetSquare(Square.Origin, new int[2]{ -1, +2 } ),
            Board.Instance.GetSquare(Square.Origin, new int[2]{ -2, +1 } ),
            Board.Instance.GetSquare(Square.Origin, new int[2]{ -1, -2 } ),
            Board.Instance.GetSquare(Square.Origin, new int[2]{ -2, -1 } ),
        };
        return availableSquares.FindAll(x => x != null).ToList();
    }
}