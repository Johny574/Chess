using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private GameObject[] blackpeaces, whitepeaces = new GameObject[8];
    [SerializeField] private GameObject blackpawn, whitepawn;
    void Awake()
    {
        if (Instance == null) { Instance = this; }
    }

    void Start()
    {
        CreatePlayers();        
    }

    public int PlayerTurn = 0;
    public Dictionary<Player.Color, Player> Players;
    public void ChangeTurn()
    {
        if (Players.Count < 2)
        {
            return;
        }

        PlayerTurn = (PlayerTurn + 1) % Players.Count;

        if (Board.Instance.Checkmate(Players[Player.Color.Black], Players[Player.Color.White]))
        {
            Debug.Log("white wins");
        }
        
        else if (Board.Instance.Checkmate(Players[Player.Color.White], Players[Player.Color.Black]))
        {
            Debug.Log("black wins");
        }
    }

    public Player CurrentPlayer() => Players.ElementAt(PlayerTurn).Value;
    public void CreatePlayers()
    {
        Players = new();
        foreach (var color in Enum.GetValues(typeof(Player.Color)))
        {
            Players.Add((Player.Color)color, CreatePlayer((Player.Color)color));
        }
        
        foreach (var player in Players.Values)
        {
            foreach (var peace in player.Peaces)
            {
                peace.LegalMoves = peace.GetLegalSquares();   
            }   
        }
    }
    public Player CreatePlayer(Player.Color color)
    {
        List<Peace> peaces = new();
        GameObject peace;
        Player player = new();

        for (int i = 0; i < Board.Instance.Width; i++)
        {
            peace = Instantiate(color == Player.Color.White ? whitepawn : blackpawn);
            peaces.Add(peace.GetComponent<Peace>());
            peace.GetComponent<Peace>()._Color = color;
            new MoveCommands.MoveCommand(peace.GetComponent<Peace>(), null, Board.Instance.GetSquare(new int[2] { color == Player.Color.White ? 1 : 6, i }, new int[2] { 0, 0 }), player).Excecute();

            peace = Instantiate(color == Player.Color.White ? whitepeaces[i] : blackpeaces[i]);
            peaces.Add(peace.GetComponent<Peace>());
            peace.GetComponent<Peace>()._Color = color;
            new MoveCommands.MoveCommand(peace.GetComponent<Peace>(), null, Board.Instance.GetSquare(new int[2] { color == Player.Color.White ? 0 : 7, i }, new int[2] { 0, 0 }), player).Excecute();
        }

        player.Peaces = peaces;
        return player;
    }
}