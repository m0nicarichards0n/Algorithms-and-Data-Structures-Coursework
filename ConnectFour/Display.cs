using System;
using System.Collections.Generic;
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
                                "Welcome to Connect Four!\n");
        }

        public void StartMenu()
        {
            Console.WriteLine("Select an option:\n" +
                                "1. Start New Game\n" +
                                "2. Replay Previous Game\n");
        }

        public int GetStartMenuSelection()
        {
            bool validInt;
            int userInput;

            validInt = Int32.TryParse(Console.ReadLine(), out userInput);
            if (validInt)
            {
                if (userInput == 1 || userInput == 2)
                {
                    return userInput;
                }
                else
                {
                    throw new Exception("Please select a valid option from the menu.");
                }
            }
            else
            {
                throw new Exception("Please select a valid option from the menu.");
            }
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
        public void PrintBoard(Game game)
        {
            // Array to store each formatted row in the board
            string[] rows = new string[game.Board.Height];
            string bottomRow = "";
            int row = 0;

            // Get height (1-26) and width (A-Z) values of board depending on custom height/width
            char[] xAxis = game.Board.GetXAxis(game.Board.Width);
            int[] yAxis = game.Board.GetYAxis(game.Board.Height);
            
            Console.WriteLine("");
            
            // For each row in board...
            foreach (int y in yAxis)
            {
                // Array to store all slots in that row
                Slot[] rowSlots = new Slot[game.Board.Width];
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
                    if (game.Board.Slots.TryGetValue((rowSlot.XCoordinate.ToString() + rowSlot.YCoordinate), out slot))
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
        public void PlayMove(Game game)
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
            if (game.ValidMove(input))
            {
                // Add move to stack
                Slot move = game.NextAvailableSlot(input);
                game.MakeMove(move, currentPlayer);
            }
            else if (input == "1")
            {
                try
                {
                    game.UndoMove();
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
                    game.RedoMove();
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
        public void UpdateBoard(Game game)
        {
            Console.Clear();
            Welcome();
            GameHeader(game);
            PrintBoard(game);
        }

        // Display congratulatory message to game winner, before returning to main menu on key press
        public void Winner(Player winner)
        {
            Console.WriteLine("\nCongratulations " + winner.Name + " you WIN !!!\n"
                            + "Press any key to continue...");
            Console.ReadKey();
            // Return to main menu
            Console.Clear();
            Welcome();
            StartMenu();
        }

        // Allow user to select a previous game and replay it
        public void ReplayGame(List<Game> games)
        {
            if (games.Count > 0)
            {
                int count = 0;
                int gameToReplay;
                bool validInt;

                Console.Clear();
                Welcome();

                // Display list of previous games
                Console.WriteLine("Which game do you want to replay?\n");
                foreach (Game game in games)
                {
                    count++;
                    Console.WriteLine(count + ". " + game.Players[0].Name + " vs. " + game.Players[1].Name + "\n");
                }

                // Get user selection
                validInt = Int32.TryParse(Console.ReadLine(), out gameToReplay);
                if (validInt)
                {
                    if (gameToReplay > 0 && gameToReplay <= games.Count)
                    {
                        // Identify the game they wish to replay
                        Game replay = games[gameToReplay - 1];
                        Stack<KeyValuePair<Slot,Player>> replayMoves = ReverseMoves(replay.Moves);

                        // Clear the game board
                        foreach (KeyValuePair<string, Slot> slot in replay.Board.Slots)
                        {
                            slot.Value.Content = 0;
                        }

                        // Reset console
                        Console.Clear();
                        Welcome();
                        Console.WriteLine("--- LIVE ACTION REPLAY ---\n"
                                        + replay.Players[0].Name + " VS. " + replay.Players[1].Name);
                        PrintBoard(replay);

                        // Replay game
                        foreach (KeyValuePair<Slot, Player> move in replayMoves)
                        {
                            Console.Clear();
                            Welcome();
                            Console.WriteLine("--- LIVE ACTION REPLAY ---\n"
                                        + replay.Players[0].Name + " VS. " + replay.Players[1].Name);
                            replay.MakeMove(move.Key, move.Value);
                            PrintBoard(replay);
                            System.Threading.Thread.Sleep(500);
                        }

                        Console.WriteLine("\n" + replay.Moves.Peek().Value.Name + " won!");
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        // Return to main menu
                        Console.Clear();
                        Welcome();
                        StartMenu();
                    }
                    else
                    {
                        throw new Exception("Invalid menu selection.");
                    }
                }
                else
                {
                    throw new Exception("Invalid menu selection.");
                }
            }
            else
            {
                throw new Exception("No previous games to replay");
            }
        }

        public Stack<KeyValuePair<Slot, Player>> ReverseMoves(Stack<KeyValuePair<Slot, Player>> moves)
        {
            Stack<KeyValuePair<Slot, Player>> reverse = new Stack<KeyValuePair<Slot, Player>>();
            foreach(KeyValuePair<Slot, Player> move in moves)
            {
                reverse.Push(move);
            }
            return reverse;
        }
    }
}