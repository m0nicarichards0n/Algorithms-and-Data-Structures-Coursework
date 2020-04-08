using System;

namespace ConnectFour
{
    class Program
    {
        static void Main(string[] args)
        {
            Board board = new Board();
            Display display = new Display();
            bool boardSizeValid = true;
            bool moveValid = true;

            // Display welcome message
            display.Welcome();

            // Establish players - user enters player names
            Player player1 = display.GetPlayer(1);
            Player player2 = display.GetPlayer(2);

            // Insantiate new game of players
            Game game = new Game(player1,player2);
            bool winner = false;
            do
            {
                try
                {
                    // Establish size of board - user enters custom height/width
                    int height = display.GetBoardSize("Board height (4 - 26): ");
                    int width = display.GetBoardSize("Board width (4 - 26): ");
                
                    // Initialise and display board
                    board.InitBoard(height, width);
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
            display.PrintBoard(board);
            do
            {

                try
                {
                    moveValid = true;
                    // Allow current player to make move
                    display.PlayMove(board, game);
                    // Update Connect Four board to display move made
                    display.UpdateBoard(board, game);
                    // Check if current player has won after minimum 7 moves have been made
                    if (game.Moves.Count >= 7)
                    {
                        winner = game.Winner(board, game.CurrentPlayer());
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
                // Display congratulatory message to winner
                display.Winner(game.CurrentPlayer());
            }
            Console.ReadLine();
        }
    }
}
