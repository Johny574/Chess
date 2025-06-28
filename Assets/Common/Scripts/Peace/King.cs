using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class King : Peace
{
    public List<MoveCommands.MoveCommand[]> CastleCommands = new()
    {
        null,
        null
    };
    public override List<Square> GetLegalSquares()
    {

        int[] directions = { -1, +1 };

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

        if (StartingPosition == false)
        {
            Debug.Log("nts");
            return availableSquares.Where(x => x != null).ToList();
        }


        for (int d = 0; d < directions.Length; d++)
        {
            for (int i = 1; i < Board.Instance.Width; i++)
            {
                var target = Board.Instance.GetSquare(Square.Origin, new int[2] { 0, directions[d] * i });

                if (target != null)
                {
                    if (target?.Occupant != null && target?.Occupant is Rook && target?.Occupant?._Color == _Color)
                    {
                        bool clearPath = true;
                        for (int j = 1; j < i; j++)
                        {
                            var betweensquare = Board.Instance.GetSquare(Square.Origin, new int[2] { 0, directions[d] * j });
                            if (betweensquare?.Occupant != null)
                            {
                                clearPath = false;
                                break;
                            }
                        }

                        if (clearPath)
                        {
                            Rook rook = (Rook)target.Occupant;
                            int oppositeDirection = directions[(d + 1) % directions.Length];
                            Square castleSquare = Board.Instance.GetSquare(rook.Square.Origin, new int[2] { 0, oppositeDirection });
                            Square rookSquare = Board.Instance.GetSquare(castleSquare.Origin, new int[2] { 0, oppositeDirection });

                            CastleCommands[d] = new MoveCommands.MoveCommand[2]
                            {
                                new MoveCommands.MoveCommand(this, Square, castleSquare, GameManager.Instance.GetPlayerAtIndex(_Color)),
                                new MoveCommands.MoveCommand(rook, rook.Square, rookSquare, GameManager.Instance.GetPlayerAtIndex(_Color))
                            };
                            Debug.Log(CastleCommands);
                        }
                    }
                }

            }
        }
        return availableSquares.FindAll(x => x != null).ToList();
    }

    public override void Select()
    {
        base.Select();

        for (int i = 0; i < CastleCommands.Count; i++)
        {
            if (CastleCommands[i] != null)
            {
                CastleCommands[i][0].To.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
            }   
        }
    }

    public override void Deselect()
    {
        base.Deselect();
        for (int i = 0; i < CastleCommands.Count; i++)
        {
            if (CastleCommands[i] != null)
            {
                CastleCommands[i][0].To.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }   
        }
    }
}

