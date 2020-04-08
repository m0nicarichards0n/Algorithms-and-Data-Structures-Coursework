using System;
using Pastel;

namespace ConnectFour
{
    class Display
    {
        // Display welcome message to user
        public void Welcome()
        {
            Console.WriteLine("-------------------------------------\n" +
                                "~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~\n" +
                                "~*~*~*~*~*~ CONNECT FOUR ~*~*~*~*~*~\n" + 
                                "~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~\n" +
                                "-------------------------------------\n\n" +
                                "Welcome to Connect Four!\n\n"); 
        }

        public void GameHeader(Game game)
        {
            Console.WriteLine("\n------ LET THE BATTLE COMMENCE ------\n\n" +
                                game.Players[0].Name + " VS " + game.Players[1].Name + "\n\n" +
                                "To make a move, just type the letter of the column you want to fill.\n" +
                                "Enter 1 at any time to UNDO a move.\n" +
                                "Enter 2 at any time to REDO a move.\n");
        }

        // Get player names from user
        public Player GetPlayer(int playerNum)
        {
            Console.WriteLine("Player " + playerNum + ", enter your name: ");
            string input = Console.ReadLine();
            Player player = new Player(input);
            return player;
        }

        // Get height/width of board from user
        public int GetBoardSize(string message)
        {
            int input;
            bool success;

            Console.WriteLine(message);
            
            success = Int32.TryParse(Console.ReadLine(), out input);
            if (success)
            {
                if (input >=4 && input <=26)
                {
                    return input;
                }
                else
                {
                    throw new Exception("Please enter a number between 4 and 26");
                }
            }
            else
            {
                throw new Exception("Please enter a numeric value");
            }
        }

        // Print formatted board in its current state
        public void PrintBoard(Board board)
        {
            // Array to store each formatted row in the board
            string[] rows = new string[board.Height];
            string bottomRow = "";
            int row = 0;

            // Get height (1-26) and width (A-Z) values of board depending on custom height/width
            char[] xAxis = board.GetXAxis(board.Width);
            int[] yAxis = board.GetYAxis(board.Height);
            
            Console.WriteLine("");
            
            // For each row in board...
            foreach (int y in yAxis)
            {
                // Array to store all slots in that row
                Slot[] rowSlots = new Slot[board.Width];
                int column = 0;
                // String to build formatted row
                string printLine = "";

                if (y.ToString().Length == 1)
                {
                    printLine = "  " + y + " | ";
                }
                else if (y.ToString().Length == 2)
                {
                    printLine = " " + y + " | ";
                }
                
                // Store all slots for current row in rowSlots[] array
                foreach (char x in xAxis)
                {
                    Slot currentSlot = new Slot(x, y);
                    rowSlots[column] = currentSlot;
                    column++;
                }
                // Append formatted version of each slot value to row
                foreach (Slot rowSlot in rowSlots)
                {
                    Slot slot;
                    if (board.Slots.TryGetValue((rowSlot.XCoordinate.ToString() + rowSlot.YCoordinate), out slot))
                    {
                        // Player 1 slots are yellow
                        if (slot.Content == 1)
                        {
                            printLine += (slot.Content.ToString().Pastel("#FFDF00")) + " | ";
                        }
                        // Player 2 slots are red
                        else if (slot.Content == 2)
                        {
                            printLine += (slot.Content.ToString().Pastel("#FF0700")) + " | ";
                        }
                        else
                        {
                            printLine += slot.Content + " | ";
                        }
                        
                    }
                }

                // Add formatted row to array of rows
                rows[row] = printLine;
                row++;
            }
            
            // Print all rows
            for (int i=0; i<rows.Length; i++)
            {
                Console.WriteLine(rows[i]);
            }
            
            // Format and print bottom row with x axis values (A-Z depending on width)
            bottomRow = "    | ";
            foreach (char x in xAxis)
            {
                bottomRow += x + " | ";
            }
            Console.WriteLine(bottomRow);
        }

        // Display game heading info and establish who will make the first move
        public void BeginGame(Game game)
        {
            // Display game header
            GameHeader(game);

            // Pick the first player at random
            Random random = new Random();
            game.FirstPlayer = random.Next(1, 2);
        }

        // Allow player to enter move and update board accordingly
        public void PlayMove(Board board, Game game)
        {
            Player currentPlayer;
            string input = "";
            // If this is the first move...
            if (game.Moves.Count == 0)
            {
                currentPlayer = game.Players[game.FirstPlayer];
                // Get player's first move
                Console.WriteLine("\n" + currentPlayer.Name + " you are the chosen one! Make the first move...");
                input = Console.ReadLine();
            }
            // If this is not the first move...
            else
            {
                // Establish who the next player is
                currentPlayer = game.NextPlayer();
                // Get next player's move
                Console.WriteLine("\n" + currentPlayer.Name + ", your turn! Make your move...");
                input = Console.ReadLine();
            }
            // Check move is valid
            if (game.ValidMove(input, board))
            {
                // Add move to stack
                Slot move = game.NextAvailableSlot(input, board);
                game.MakeMove(move, currentPlayer, board);
            }
            else if (input == "1")
            {
                try
                {
                    game.UndoMove(board);
                }
                catch(Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
            else if (input == "2")
            {
                try
                {
                    game.RedoMove(board);
                }
                catch(Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
            else
            {
                throw new Exception("Sorry, that's not a valid move. Please enter a different move.");
            }
        }

        // Remove the board from console and print latest version of board
        public void UpdateBoard(Board board, Game game)
        {
            Console.Clear();
            Welcome();
            GameHeader(game);

            PrintBoard(board);
        }

        public void Winner(Player winner)
        {
            Console.WriteLine("\nCongratulations " + winner.Name + " you WIN !!!");
        }
    }
}
