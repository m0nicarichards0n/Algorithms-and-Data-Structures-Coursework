using System;

namespace ConnectFour
{
    class Program
    {
        static void Main(string[] args)
        {
            Board board = new Board();
            Display display = new Display();
            bool valid = true;

            display.Welcome();

            do
            {
                int height = display.GetHeight();
                int width = display.GetWidth();

                try
                {
                    board.InitBoard(height, width);
                    display.PrintBoard(board);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    valid = false;
                }

            } while (!valid);
        }
    }
}
