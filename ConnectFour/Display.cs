using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectFour
{
    class Display
    {
        public void Welcome()
        {
            Console.WriteLine("-------------------------------------");
            Console.WriteLine("~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~");
            Console.WriteLine("~*~*~*~*~*~ CONNECT FOUR ~*~*~*~*~*~");
            Console.WriteLine("~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~*~");
            Console.WriteLine("-------------------------------------");
            
        }

        public int GetHeight()
        {
            Console.WriteLine("Board height (1-26): ");
            int input = Int32.Parse(Console.ReadLine());
            return input;
        }

        public int GetWidth()
        {
            Console.WriteLine("Board width (1-26): ");
            int input = Int32.Parse(Console.ReadLine());
            return input;
        }

        public void PrintBoard(Board board)
        {
            int row = 0;
            string[] rows = new string[board.Height];

            char[] xAxis = board.GetXAxis(board.Width);
            int[] yAxis = board.GetYAxis(board.Height);

            Console.WriteLine("");

            foreach (int y in yAxis)
            {
                row++;
                int column = 0;
                string printLine = "";
                if (y.ToString().Length == 1)
                {
                    printLine = "  " + y + " | ";
                }
                else if (y.ToString().Length == 2)
                {
                    printLine = " " + y + " | ";
                }
                Slot[] rowSlots = new Slot[board.Width];
                
                foreach (char x in xAxis)
                {
                    Slot currentSlot = new Slot(x, y);
                    rowSlots[column] = currentSlot;
                    column++;
                }
                foreach (Slot rowSlot in rowSlots)
                {
                    Slot slot;
                    if (board.Slots.TryGetValue((rowSlot.XCoordinate.ToString() + rowSlot.YCoordinate), out slot))
                    {
                        printLine += slot.Content + " | ";
                    }
                }
                rows[row-1] = printLine;
            }
            
            for (int i=0; i<rows.Length; i++)
            {
                Console.WriteLine(rows[i]);
            }

            string bottomRow = "";

            if (board.Height.ToString().Length == 2)
            {
                bottomRow = "    | ";
            }
            else if (board.Height.ToString().Length == 1)
            {
                bottomRow = "   | ";
            }

            foreach (char x in xAxis)
            {
                bottomRow += x + " | ";
            }
            Console.WriteLine(bottomRow);
        }
    }
}
