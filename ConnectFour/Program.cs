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
            
            /*
             * Created this method to update board in console:
             * display.UpdateBoard(board);
             * 
             */
        }
    }
}
