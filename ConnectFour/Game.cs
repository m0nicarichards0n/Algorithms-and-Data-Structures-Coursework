using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectFour
{
    class Game
    {
        private Stack<KeyValuePair<Slot, Player>> _moves = new Stack<KeyValuePair<Slot, Player>>();
        private Player[] _players = new Player[2];
        private int _firstPlayer;

        public Game(Player player1, Player player2)
        {
            _players[0] = player1;
            _players[1] = player2;
        }

        // Establish whether or not the move entered by player is valid
        public bool ValidMove(string move, Board board)
        {
            bool validX = false;
            bool columnEmpty = false;
            char[] xAxis = board.GetXAxis(board.Width);

            // Ensure that only 1 character is entered
            if (move.Length == 1)
            {
                char x = char.Parse(move);
                // Check that the character entered is a valid X axis value
                for (int i = 0; i < xAxis.Length; i++)
                {
                    if (xAxis[i] == x || xAxis[i] == char.ToUpper(x))
                    {
                        validX = true;
                    }
                }

                if (validX)
                {
                    // Check that the column has empty slots
                    for (int i = 0; i < board.Height; i++)
                    {
                        string slot = x.ToString().ToUpper() + (i + 1).ToString();
                        if (board.Slots[slot].Content == 0)
                        {
                            columnEmpty = true;
                        }
                    }
                }

                if (validX && columnEmpty)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        // Get coordinates of the next available slot in chosen column
        public Slot NextAvailableSlot(string column, Board board)
        {
            for (int i = 0; i < board.Height; i++)
            {
                string slot = column.ToUpper() + (i + 1).ToString();
                if (board.Slots[slot].Content == 0)
                {
                    Slot nextAvailable = new Slot(char.Parse(column), (i + 1));
                    return nextAvailable;
                }
            }
            return null;
        }

        // Estbalish whether player is player 1 or 2 (for display on board)
        public int PlayerNumber (string playerName)
        {
            if (_players[0].Name == playerName)
            {
                return 1;
            }
            else if (_players[1].Name == playerName)
            {
                return 2;
            }
            else
            {
                return 0;
            }
        }

        // Establish who the next player is based on who the last player was
        public Player GetNextPlayer(Stack<KeyValuePair<Slot, Player>> moves)
        {
            KeyValuePair<Slot, Player> lastMove = moves.Peek();
            if (_players[0].Name == lastMove.Value.Name)
            {
                return _players[1];
            }
            else
            {
                return _players[0];
            }
        }
        
        public void MakeMove(Slot slot, Player player, Board board)
        {
            // Update position on board
            string moveLocation = slot.XCoordinate.ToString().ToUpper() + slot.YCoordinate.ToString();
            board.Slots[moveLocation].Content = PlayerNumber(player.Name);

            // Add to stack of game moves
            KeyValuePair<Slot, Player> move = new KeyValuePair<Slot, Player>(slot, player);
            _moves.Push(move);
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

        public Stack<KeyValuePair<Slot, Player>> Moves
        {
            get => _moves;
        }
        
        public int FirstPlayer
        {
            get => _firstPlayer;
            set
            {
                if (value == 0 || value == 1)
                {
                    _firstPlayer = value;
                }
                else
                {
                    throw new Exception("Invalid player index encountered.");
                }
            }
        }
    }
}
