
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static Board Instance;
    [SerializeField] private List<Column> _board = new();
    [SerializeField] private GameObject _whiteSquare, _darkSquare;
    public int Width = 8, Height = 8;
    [SerializeField] private GameObject _tag;
    [SerializeField] private Canvas _canvas;
    void Awake()
    {
        if (Instance == null) { Instance = this; }
    }

    public void Clear()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
        
        for (int i = 0; i < _canvas.transform.childCount; i++)
        {
            DestroyImmediate(_canvas.transform.GetChild(0).gameObject);   
        }

    }

    public void Create()
    {
        _board = new();
        for (int x = 0; x < Width; x++)
        {
            _board.Add(new Column());
            var tag = GameObject.Instantiate(_tag);

            tag.transform.position = new Vector2(-1, 1f * x);
            tag.gameObject.SetActive(true);
            tag.transform.SetParent(_canvas.transform);
            tag.GetComponent<TextMeshProUGUI>().text = (x + 1).ToString();

            for (int y = 0; y < Height; y++)
            {
                var variant = (x + y) % 2 == 0 ? _whiteSquare : _darkSquare;
                Square newSquare = Instantiate(variant).GetComponent<Square>();

                newSquare.transform.SetParent(transform);
                tag.transform.SetParent(_canvas.transform);
                newSquare.Origin[0] = x;
                newSquare.Origin[1] = y;
                _board[x].Rows[y] = newSquare;
                newSquare.transform.position = new Vector2(1f * y, 1f * x);
            }
        }

        for (int y = 0; y < Height; y++)
        {
            var tag = GameObject.Instantiate(_tag);
            tag.transform.position = new Vector2(1f * y, -1);
            tag.gameObject.SetActive(true);
            tag.transform.SetParent(_canvas.transform);
            tag.GetComponent<TextMeshProUGUI>().text = ((char)(y + 65)).ToString();
        }
    }

    public Square GetSquare(int[] origin, int[] offset)
    {
        int newX = origin[0] + offset[0];
        int newY = origin[1] + offset[1];

        if (newX < 0 || newX >= Width || newY < 0 || newY >= Height)
        {
            return null;
        }

        return _board[newX].Rows[newY];
    }

    public Tuple<Square, Square> ParseMove(string move)
    {
        int[] from = new int[2]
        {
            move[1] - '0' - 1,
            move[0] - 'a'
        };

        int[] to = new int[2]
        {
            move[3] - '0' - 1,
            move[2] - 'a'
        };
        Square fromSquare = GetSquare(from, new int[2] { 0, 0 });
        Square toSquare = GetSquare(to, new int[2] { 0, 0 });
        return Tuple.Create(fromSquare, toSquare);
    }

    public bool InCheck(Player player, Player opponent, Peace occupant)
    {
        Peace king = player.Peaces.Find(x => x.GetType() == typeof(King));
        foreach (var peace in opponent.Peaces)
        {
            foreach (var square in peace.GetLegalSquares())
            {
                if (peace != occupant)
                {
                    if (square.Occupant == king)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public void SetLegalSquares(Peace peace, Player player, Player opponent)
    {
        List<Square> moves = peace.GetLegalSquares();
        List<Square> legalmoves = new();

        for (int i = 0; i < moves.Count; i++)
        {
            Square current = peace.Square;
            Peace occupant = moves[i].Occupant;

            current.Occupant = null;
            peace.Square = moves[i];
            moves[i].Occupant = peace;

            if (!InCheck(player, opponent, occupant))
            {
                if (!player.Peaces.Contains(occupant))
                {
                    legalmoves.Add(moves[i]);
                }
            }

            current.Occupant = peace;
            peace.Square = current;
            moves[i].Occupant = occupant;
        }

        peace.LegalMoves = legalmoves;
    }

    public bool Checkmate(Player player, Player opponent)
    {
        foreach (var peace in player.Peaces)
        {
            SetLegalSquares(peace, player, opponent);
        }

        foreach (var peace in opponent.Peaces)
        {
            SetLegalSquares(peace, opponent, player);
        }


        if (!InCheck(player, opponent, null))
        {
            return false;
        }

        return player.Peaces.FindAll(x => x.LegalMoves.Count > 0).Count == 0;
    }

    public bool Stalemate(Player player, Player opponent)
    {
        Peace king = player.Peaces[0];

        if (player.Peaces.Count > 1)
            return false;
        else if (player.Peaces[0] != king)
            return false;

        if (InCheck(player, opponent, king))
            return false;

        List<Square> kingMoves = king.GetLegalSquares();
        kingMoves.RemoveAll(move =>
        {
            Square originalSquare = king.Square;
            king.Square = move;
            bool stillInCheck = InCheck(player, opponent, king);
            king.Square = originalSquare;
            return stillInCheck;
        });

        return kingMoves.Count == 0;
    }
}

[Serializable]
public class Column
{
    public Square[] Rows;
    public Column()
    {
        Rows = new Square[8];
    }
}