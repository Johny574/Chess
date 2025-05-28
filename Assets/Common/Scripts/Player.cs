



using System;
using System.Collections.Generic;

[Serializable]
public class Player
{
    public List<Peace> Peaces;
    public Peace Selected = null;
    public Queue<ICommand> Moves;

    public Player()
    {
        Moves = new();
    }

    public enum Color
    {
        White,
        Black
    }
}