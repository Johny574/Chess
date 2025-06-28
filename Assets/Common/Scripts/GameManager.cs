using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private GameObject[] blackpeaces, whitepeaces = new GameObject[8];
    [SerializeField] private GameObject blackpawn, whitepawn;
    [field:SerializeField] public bool TwoPlayer { get; private set; } = false;
    int _localPlayer = 0;
    int _currentTurn = 0;
    private Player[] _players;

    void Awake()
    {
        if (Instance == null) { Instance = this; }
    }

    void Start() => StartNewGame();

    void StartNewGame()
    {
        CreatePlayers();
        Bot.Instance.StartNewGame();
    }

    public void ChangeTurn()
    {
        _currentTurn = (_currentTurn + 1) % _players.Length;

        // Bot
        if (!TwoPlayer)
        {
            if (_currentTurn != _localPlayer)
            {
                StartCoroutine(Bot.Instance.GetMove(bestMove =>
                {
                    if (!string.IsNullOrEmpty(bestMove))
                    {
                        Debug.Log("Stockfish move: " + bestMove);
                        Tuple<Square, Square> movessquares = Board.Instance.ParseMove(bestMove); 

                        if (movessquares.Item2.Occupant != null)
                        {
                            new MoveCommands.TakeCommand(movessquares.Item1.Occupant, movessquares.Item1, movessquares.Item2, _players[(_localPlayer + 1) % _players.Length]).Excecute();
                        }
                        else
                        {
                            new MoveCommands.MoveCommand(movessquares.Item1.Occupant, movessquares.Item1, movessquares.Item2, _players[(_localPlayer + 1) % _players.Length]).Excecute();
                        }
                    }
                }));
            }
        }

        if (Board.Instance.Checkmate(_players[_currentTurn], _players[(_currentTurn + 1) % _players.Length]))
        {
            Debug.Log("white wins");
        }

        else if (Board.Instance.Checkmate(_players[(_currentTurn + 1) % _players.Length], _players[_currentTurn]))
        {
            Debug.Log("black wins");
        }
    }

    public Player CurrentPlayer() => _players[_currentTurn];
    public Player CurrentOpponent() => _players[(_currentTurn + 1) % _players.Length];
    public Player LocalPlayer() => _players[_localPlayer];
    public Player GetPlayerAtIndex(int index) => _players[(_currentTurn + 1) % _players.Length];

    void CreatePlayers()
    {
        _players = new Player[2];
        // create players
        for (int i = 0; i < 2; i++)
        {
            _players[i] = CreatePlayer(i);
        }

        // loop over players after both are created.
        foreach (Player player in _players)
        {
            // set peace squares
            foreach (Peace peace in player.Peaces)
            {
                peace.LegalMoves = peace.GetLegalSquares();
            }
        }
    }

    Player CreatePlayer(int color)
    {
        Player player = new();
        player.Peaces = CreatePeaces(color, player);
        return player;
    }

    List<Peace> CreatePeaces(int color, Player player)
    {
        List<Peace> peaces = new();
        GameObject peace;
        for (int i = 0; i < Board.Instance.Width; i++)
        {
            // pawns
            peace = Instantiate(color == 0 ? whitepawn : blackpawn);
            peaces.Add(peace.GetComponent<Peace>());
            peace.GetComponent<Peace>()._Color = color;
            new MoveCommands.MoveCommand(peace.GetComponent<Peace>(), null, Board.Instance.GetSquare(new int[2] { color == 0 ? 1 : 6, i }, new int[2] { 0, 0 }), player).Excecute();

            // other peaces
            peace = Instantiate(color == 0 ? whitepeaces[i] : blackpeaces[i]);
            peaces.Add(peace.GetComponent<Peace>());
            peace.GetComponent<Peace>()._Color = color;
            new MoveCommands.MoveCommand(peace.GetComponent<Peace>(), null, Board.Instance.GetSquare(new int[2] { color == 0 ? 0 : 7, i }, new int[2] { 0, 0 }), player).Excecute();
        }
        return peaces;
    }
}