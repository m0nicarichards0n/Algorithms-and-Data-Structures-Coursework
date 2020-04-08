using System;
using System.Collections;
using System.Collections.Generic;

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

        // Establish whether a player has won based on last move made
        public bool Winner(Board board, Player player)
        {
            char x = char.ToUpper(_moves.Peek().Key.XCoordinate);
            int y = _moves.Peek().Key.YCoordinate;

            SortedList diagonalSlotsA = new SortedList();
            SortedList diagonalSlotsB = new SortedList();
            SortedList horizontalSlots = new SortedList();
            SortedList verticalSlots = new SortedList();

            diagonalSlotsA[3] = x.ToString() + y;
            diagonalSlotsB[3] = x.ToString() + y;
            horizontalSlots[3] = x.ToString() + y;
            verticalSlots[3] = x.ToString() + y;

            // Based on x,y coordinates of last move made, store coordinates of surrounding slots to be checked
            for (int i = 1; i < 4; i++)
            {
                string A, B, C, D, E, F, G, H = "";

                char something = ((char)((int)x + i));

                if (((int)x+i) >= 65 && ((int)x + i) <= (64 + board.Width) 
                    && (y + i) > 0 && (y + i) <= board.Height)
                {
                    A = ((char)((int)x + i)).ToString() + (y + i);
                    diagonalSlotsA.Add(3 + i, A);
                }

                if (((int)x - i) >= 65 && ((int)x - i) <= (64 + board.Width)
                    && (y - i) > 0 && (y - i) <= board.Height)
                {
                    B = ((char)((int)x - i)).ToString() + (y - i);
                    diagonalSlotsA.Add(3 - i, B);
                }

                if (((int)x - i) >= 65 && ((int)x - i) <= (64 + board.Width)
                    && (y + i) > 0 && (y + i) <= board.Height)
                {
                    C = ((char)((int)x - i)).ToString() + (y + i);
                    diagonalSlotsB.Add(3 - i, C);
                }

                if (((int)x + i) >= 65 && ((int)x + i) <= (64 + board.Width)
                    && (y - i) > 0 && (y - i) <= board.Height)
                {
                    D = ((char)((int)x + i)).ToString() + (y - i);
                    diagonalSlotsB.Add(3 + i, D);
                }

                if ((y + i) > 0 && (y + i) <= board.Height)
                {
                    E = x.ToString() + (y + i);
                    horizontalSlots.Add(3 + i, E);
                }

                if ((y - i) > 0 && (y - i) <= board.Height)
                {
                    F = x.ToString() + (y - i);
                    horizontalSlots.Add(3 - i, F);
                }

                if (((int)x - i) >= 65 && ((int)x - i) <= (64 + board.Width))
                {
                    G = ((char)((int)x - i)).ToString() + y;
                    verticalSlots.Add(3 - i, G);
                }

                if (((int)x + i) >= 65 && ((int)x + i) <= (64 + board.Width))
                {
                    H = ((char)((int)x + i)).ToString() + y;
                    verticalSlots.Add(3 + i, H);
                }
            }
                
            // Check if four consecutive slots have been filled by player
            if (FourInARow(diagonalSlotsA, board, player)
                || FourInARow(diagonalSlotsB, board, player)
                || FourInARow(horizontalSlots, board, player)
                || FourInARow(verticalSlots, board, player))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Establish whether 4 consecutive slots have been populated by player
        public bool FourInARow(SortedList checkSlots, Board board, Player player)
        {
            bool fourUp = false;
            try
            {
                if (checkSlots.Count >=4)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (board.Slots[checkSlots.GetByIndex(i).ToString()].Content == PlayerNumber(player.Name))
                        {
                            if (board.Slots[checkSlots.GetByIndex(i + 1).ToString()].Content == PlayerNumber(player.Name))
                            {
                                if (board.Slots[checkSlots.GetByIndex(i + 2).ToString()].Content == PlayerNumber(player.Name))
                                {
                                    if (board.Slots[checkSlots.GetByIndex(i + 3).ToString()].Content == PlayerNumber(player.Name))
                                    {
                                        fourUp = true;
                                    }
                                }
                            }
                        }
                    }
                }
                return fourUp;
            }
            catch
            {
                return fourUp;
            }
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

        // Establish next player based on last move
        public Player NextPlayer()
        {
            KeyValuePair<Slot, Player> lastMove = _moves.Peek();
            if (_players[0].Name == lastMove.Value.Name)
            {
                return _players[1];
            }
            else
            {
                return _players[0];
            }
        }

        // Establish current player based on last move
        public Player CurrentPlayer()
        {
            Player currentPlayer = new Player("");
            try
            {
                currentPlayer = _moves.Peek().Value;
            }
            catch
            {
                return currentPlayer;
            }
            return currentPlayer;
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
