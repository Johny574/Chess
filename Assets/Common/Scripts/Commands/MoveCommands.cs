
using UnityEngine;

namespace MoveCommands
{
    public class MoveCommand : ICommand
    {
        private Peace peace;
        private Square from;
        private Square to;
        private Player player;

        public MoveCommand(Peace peace, Square from, Square to, Player player)
        {
            this.peace = peace;
            this.to = to;
            this.from = from;
            this.player = player;
        }

        public void Excecute()
        {
            //  setup
            if (from != null)
            {
                peace.Square.Occupant = null;
                peace.Deselect();

                if (peace.GetType() == typeof(Pawn))
                {
                    ((Pawn)peace).startingPosition = false;
                }
            }

            peace.Square = to;
            peace.Square.Occupant = peace;
            peace.transform.position = peace.Square.transform.position;
    
            player.Selected = null;
            player.Moves.Enqueue(this);
            GameManager.Instance.ChangeTurn();
        }

        public void Undo()
        {
            from.Occupant = peace;
            peace.Square = from;
            peace.transform.position = peace.Square.transform.position;
            player.Selected = null;
        }
    }

    public class TakeCommand : ICommand
    {
        private Square to;
        private Square from;
        private Player player;
        private Peace peace;

        public TakeCommand(Peace peace, Square from, Square to, Player player)
        {
            this.to = to;
            this.from = from;
            this.player = player;
            this.peace = peace;
        }

        public void Excecute()
        {
            GameObject.Destroy(to.Occupant.gameObject);
            GameManager.Instance.Players[to.Occupant._Color].Peaces.Remove(to.Occupant);
            to.Occupant = null;
            new MoveCommand(peace, from, to, player).Excecute();
        }

        public void Undo()
        {
        }
    }
}