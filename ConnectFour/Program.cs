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

            do
            {
                try
                {
                    // Establish size of board - user enters custom height/width
                    int height = display.GetBoardSize("Board height (1 - 26): ");
                    int width = display.GetBoardSize("Board width (1 - 26): ");
                
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

            display.BeginGame(game);
            display.PrintBoard(board);
            do
            {

                try
                {
                    display.PlayMove(board, game);
                    display.UpdateBoard(board, game);
                }
                catch (Exception e)
                {
                    // Throw error message if invalid move is made
                    Console.WriteLine(e.Message);
                    moveValid = false;
                }
            }
            // This while condition will eventually check for a win condition ("true == true" is just a placeholder)
            while (!moveValid || true == true);
            
            Console.ReadLine();
        }
    }
}
