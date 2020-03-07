using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectFour
{
    class Game
    {
        private Stack<string> _moves = new Stack<string>();
        private Player[] _players = new Player[2];

        public Game(Player player1, Player player2)
        {
            _players[0] = player1;
            _players[1] = player2;
        }

        public void UndoMove()
        {
            if (_moves.Count != 0)
            {
                _moves.Pop();
            }
            else
            {
                throw new Exception("No moves to undo!");
            }
        }

        public Player[] Players
        {
            get => _players;
        }

        public Stack<string> Move
        {
            get => _moves;
        }
    }
}
