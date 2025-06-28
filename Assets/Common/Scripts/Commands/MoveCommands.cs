
using UnityEngine;

namespace MoveCommands
{
    public class MoveCommand : ICommand
    {
        private Peace peace;
        public Square From;
        public Square To;
        private Player player;

        public MoveCommand(Peace peace, Square from, Square to, Player player)
        {
            this.peace = peace;
            this.To = to;
            this.From = from;
            this.player = player;
        }

        public void Excecute()
        {
            //  setup peaces
            if (From == null)
            {
                peace.Move(To, true);
            }
            else
            {
                peace.Square.Occupant = null;
                peace.Deselect();

                // // if (peace.GetType() == typeof(Pawn))
                // // {
                // //     ((Pawn)peace).startingPosition = false;
                // }
                GameEvents.Instance.PeaceMoved?.Invoke(From, To);
                peace.Move(To, false);
                player.Selected = null;
                player.Moves.Enqueue(this);
                GameManager.Instance.ChangeTurn();
            }
    
        }

        public void Undo()
        {
            From.Occupant = peace;
            peace.Square = From;
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
            GameManager.Instance.CurrentOpponent().Peaces.Remove(to.Occupant);
            to.Occupant = null;
            new MoveCommand(peace, from, to, player).Excecute();
        }

        public void Undo()
        {
        }
    }
}