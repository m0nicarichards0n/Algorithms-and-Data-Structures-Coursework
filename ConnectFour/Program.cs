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
            bool aiMenuValid = true;
            bool validPlayerName = true;
            bool winner = true;
            bool stalemate = true;
            bool restart = true;

            do
            {
                Console.Clear();
                // Display welcome message
                display.Welcome();
                // Display start menu
                display.StartMenu();

                do
                {
                    try
                    {
                        // Retrieve user selection from start menu (1 - new game, 2 - replay)
                        int startMenuSelection = display.GetMenuSelection();

                        // If user selects 'new game'...
                        if (startMenuSelection == 1)
                        {
                            startMenuValid = true;
                            do
                            {
                                // Display player options menu (1 - AI, 2 - friend)
                                display.AIMenu();
                                try
                                {
                                    int aiMenuSelection = display.GetMenuSelection();
                                    
                                    // If user selects to play against AI...
                                    if (aiMenuSelection == 1)
                                    {
                                        aiMenuValid = true;

                                        // Establish human player
                                        Player humanPlayer = new Player("", false);
                                        // Establish AI player
                                        Player AI = new Player("Computer", true);

                                        do
                                        {
                                            try
                                            {
                                                // Get human player name
                                                humanPlayer = display.GetPlayer(1);
                                                validPlayerName = true;
                                            }
                                            // Ensure human player doesn't name themselves "Computer" as well
                                            catch (Exception e)
                                            {
                                                Console.WriteLine(e.Message);
                                                validPlayerName = false;
                                            }
                                        } while (!validPlayerName);

                                        // Instantiate new game
                                        Game game = new Game(humanPlayer, AI);
                                        //winner = false;

                                        // Establish board dimensions
                                        do
                                        {
                                            EstablishBoardSize(game);
                                        } while (!boardSizeValid);

                                        // Display game information & pick 1st player at random
                                        display.BeginGame(game);
                                        // Display Connect Four board
                                        display.PrintBoard(game);

                                        // Each user makes a move...
                                        do
                                        {
                                            winner = MakeAMove(game);
                                            stalemate = game.Board.Stalemate();
                                        }
                                        // Repeat:
                                        // 1. While move is invalid OR
                                        // 2. While neither player has won OR
                                        // 3. Until stalemate is reached
                                        while (ContinueGame());

                                        // If someone wins...
                                        if (winner)
                                        {
                                            // Store game in session history
                                            games.Add(game);
                                            // Display congratulatory message to winner
                                            display.Winner(game.CurrentPlayer());
                                        }
                                        else if (stalemate)
                                        {
                                            // Store game in session history
                                            games.Add(game);
                                            // Display stalemate message to players
                                            display.Stalemate();
                                        }
                                    }
                                    // If user user selects to play against a friend...
                                    else if (aiMenuSelection == 2)
                                    {
                                        aiMenuValid = true;

                                        // Establish players
                                        Player player1 = new Player("", false);
                                        Player player2 = new Player("", false);

                                        do
                                        {
                                            try
                                            {
                                                // Get player 1 name
                                                player1 = display.GetPlayer(1);
                                                validPlayerName = true;
                                            }
                                            // Ensure player doesn't name themselves "Computer"
                                            catch (Exception e)
                                            {
                                                Console.WriteLine(e.Message);
                                                validPlayerName = false;
                                            }
                                        } while (!validPlayerName);

                                        do
                                        {
                                            try
                                            {
                                                // Get player 2 name
                                                player2 = display.GetPlayer(2);
                                                validPlayerName = true;
                                            }
                                            // Ensure player doesn't name themselves "Computer"
                                            catch (Exception e)
                                            {
                                                Console.WriteLine(e.Message);
                                                validPlayerName = false;
                                            }
                                        } while (!validPlayerName);

                                        // Insantiate new game of players
                                        Game game = new Game(player1, player2);

                                        // Establish board dimensions
                                        do
                                        {
                                            EstablishBoardSize(game);
                                        } while (!boardSizeValid);

                                        // Display game information & pick 1st player at random
                                        display.BeginGame(game);
                                        // Display Connect Four board
                                        display.PrintBoard(game);

                                        // Each user makes a move...
                                        do
                                        {
                                            winner = MakeAMove(game);
                                            stalemate = game.Board.Stalemate();
                                        }
                                        // Repeat:
                                        // 1. While move is invalid OR
                                        // 2. While neither player has won OR
                                        // 3. Until stalemate is reached
                                        while (ContinueGame());

                                        // If someone wins...
                                        if (winner)
                                        {
                                            // Store game in session history
                                            games.Add(game);
                                            // Display congratulatory message to winner
                                            display.Winner(game.CurrentPlayer());
                                        }
                                        if (stalemate)
                                        {
                                            // Store game in session history
                                            games.Add(game);
                                            // Display stalemate message to players
                                            display.Stalemate();
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                    aiMenuValid = false;
                                }
                            } while (!aiMenuValid);
                        }
                        // If user selects 'replay game'...
                        else if (startMenuSelection == 2)
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

            void EstablishBoardSize(Game game)
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
            }

            bool MakeAMove(Game game)
            {
                winner = false;
                try
                {
                    // Allow current player to make move
                    display.PlayMove(game);
                    // Update Connect Four board to display move made
                    display.UpdateBoard(game);
                    // Check if current player has won after minimum 7 moves have been made
                    if (game.Moves.Count >= 7)
                    {
                        winner = game.Winner(game.CurrentPlayer());
                    }
                    return winner;
                }
                catch (Exception e)
                {
                    // Throw error message if invalid move is made
                    Console.WriteLine(e.Message);
                }
                return winner;
            }

            bool ContinueGame()
            {
                bool continueGame = true;

                if (winner == true)
                {
                    continueGame = false;
                }

                if (stalemate == true)
                {
                    continueGame = false;
                }

                return continueGame;
            }
        }
    }
}
