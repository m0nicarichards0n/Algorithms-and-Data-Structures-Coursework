using System;
using System.Collections.Generic;

namespace ConnectFour
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Game> games = new List<Game>();
            Display display = new Display();
            bool boardSizeValid = true;
            bool moveValid = true;
            bool startMenuValid = true;
            bool restart = true;

            do
            {
                Console.Clear();
                // Display welcome message
                display.Welcome();
                display.StartMenu();

                do
                {
                    try
                    {
                        // Display start menu and retrieve user option (1 - new game, 2 - replay)
                        int option = display.GetStartMenuSelection();

                        // If user selects 'new game'...
                        if (option == 1)
                        {
                            startMenuValid = true;
                            // Establish players - user enters player names
                            Player player1 = display.GetPlayer(1);
                            Player player2 = display.GetPlayer(2);

                            // Insantiate new game of players
                            Game game = new Game(player1, player2);
                            bool winner = false;
                            do
                            {
                                try
                                {
                                    // Establish size of board - user enters custom height/width
                                    int height = display.GetBoardSize("Board height (4 - 26): ");
                                    int width = display.GetBoardSize("Board width (4 - 26): ");

                                    // Initialise and display board
                                    game.Board.InitBoard(height, width);
                                    boardSizeValid = true;
                                }
                                catch (Exception e)
                                {
                                    // Throw error message if board height/width parameters are invalid
                                    Console.WriteLine(e.Message);
                                    boardSizeValid = false;
                                }

                            } while (!boardSizeValid);

                            // Display game information & pick 1st player at random
                            display.BeginGame(game);
                            // Display Connect Four board
                            display.PrintBoard(game);
                            do
                            {

                                try
                                {
                                    moveValid = true;
                                    // Allow current player to make move
                                    display.PlayMove(game);
                                    // Update Connect Four board to display move made
                                    display.UpdateBoard(game);
                                    // Check if current player has won after minimum 7 moves have been made
                                    if (game.Moves.Count >= 7)
                                    {
                                        winner = game.Winner(game.CurrentPlayer());
                                    }
                                }
                                catch (Exception e)
                                {
                                    // Throw error message if invalid move is made
                                    Console.WriteLine(e.Message);
                                    moveValid = false;
                                }
                            }
                            // Repeat while move is invalid or neither player has won
                            while (!moveValid || !winner);

                            if (winner)
                            {
                                // Store game in session history
                                games.Add(game);
                                // Display congratulatory message to winner
                                display.Winner(game.CurrentPlayer());
                            }
                        }
                        // If user selects 'replay game'...
                        else if (option == 2)
                        {
                            startMenuValid = true;
                            bool selectReplay = true;
                            do
                            {
                                try
                                {
                                    display.ReplayGame(games);
                                    selectReplay = true;
                                }
                                catch (Exception e)
                                {
                                    if (e.Message == "Invalid menu selection.")
                                    {
                                        selectReplay = false;
                                    }
                                    else
                                    {
                                        Console.WriteLine(e.Message);
                                        startMenuValid = false;
                                    }
                                }
                            } while (!selectReplay);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        startMenuValid = false;
                    }
                } while (!startMenuValid);
            } while (restart);
        }
    }
}
