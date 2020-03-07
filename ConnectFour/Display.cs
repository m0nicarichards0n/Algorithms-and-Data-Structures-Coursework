using System;
using System.Collections.Generic;
using System.Text;

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
            Console.WriteLine(message);
            int input;
            bool success = Int32.TryParse(Console.ReadLine(), out input);
            if (success)
            {
                if (input >=1 && input <=26)
                {
                    return input;
                }
                else
                {
                    throw new Exception("Please enter a number between 1 and 26");
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
            // Array to store each row in the board
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
                // String to build formatted row string
                string printLine = "";

                if (y.ToString().Length == 1)
                {
                    printLine = "  " + y + " | ";
                }
                else if (y.ToString().Length == 2)
                {
                    printLine = " " + y + " | ";
                }
                
                // Store all slots in for current row in rowSlots[] array
                foreach (char x in xAxis)
                {
                    Slot currentSlot = new Slot(x, y);
                    rowSlots[column] = currentSlot;
                    column++;
                }
                // Append formatted version of each slot value to "printLine" row string
                foreach (Slot rowSlot in rowSlots)
                {
                    Slot slot;
                    if (board.Slots.TryGetValue((rowSlot.XCoordinate.ToString() + rowSlot.YCoordinate), out slot))
                    {
                        printLine += slot.Content + " | ";
                    }
                }

                // Add formatted row string to array of rows
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

        // Display "Battle Commence" message and estbalish first move
        public void BeginGame(Game game)
        {
            Console.WriteLine("\n------ LET THE BATTLE COMMENCE ------\n\n" +
                                game.Players[0].Name + " VS " + game.Players[1].Name + "\n");

            // Pick the first player at random
            Random random = new Random();
            int firstPlayer = random.Next(1, 2);

            Console.WriteLine(game.Players[firstPlayer].Name + " you are the chosen one! Make your first move...");


        }

        // Remove the board from console and print latest version of board
        public void UpdateBoard(Board board)
        {
            for(int i=0; i<(board.Height + 5); i++)
            {
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                ClearConsoleLine();
            }
            PrintBoard(board);
        }

        // Clear a line in console
        public void ClearConsoleLine()
        {
            int cursorTop = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(0, cursorTop);
        }
    }
}
