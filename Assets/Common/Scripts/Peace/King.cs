using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class King : Peace
{
    public override List<Square> GetLegalSquares()
    {
        List<Square> availableSquares = new()
        {
            Board.Instance.GetSquare(Square.Origin, new int[2]{ 0 , 1 } ), // N
            Board.Instance.GetSquare(Square.Origin, new int[2]{ 1 , 1 } ), // NE
            Board.Instance.GetSquare(Square.Origin, new int[2]{ 1, 0 } ), // E
            Board.Instance.GetSquare(Square.Origin, new int[2]{ 1 , -1 } ), // SE
            Board.Instance.GetSquare(Square.Origin, new int[2]{  0, -1 } ), // S
            Board.Instance.GetSquare(Square.Origin, new int[2]{ -1, -1 } ), // SW
            Board.Instance.GetSquare(Square.Origin, new int[2]{ -1, 0 } ), // W
            Board.Instance.GetSquare(Square.Origin, new int[2]{ -1, 1 } ), // NW
        };
        return availableSquares.FindAll(x => x != null).ToList();
    }
}
