using System.Collections.Generic;
using System.Linq;

public class Bishop : Peace
{
    public override List<Square> GetLegalSquares()
    {
        List<Square> availableSquares = new();
        int[] directions = { -1, +1 };

        foreach (var dirx in directions)
        {
            foreach (var diry in directions)
            {
                for (int i = 1; i < Board.Instance.Width; i++)
                {
                    var target = Board.Instance.GetSquare(Square.Origin, new int[2] { dirx * i, diry * i });

                    if (target == null)
                    {
                        break;
                    }

                    if (target.Occupant != null)
                    {
                        if (target.Occupant._Color != _Color)
                        {
                            availableSquares.Add(target);
                        }
                        break;
                    }
                    availableSquares.Add(target);
                }       
            }
        }
        
        return availableSquares.FindAll(x => x != null).ToList();
    }
}
