using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectFour
{
    class Board
    {
        public IDictionary<string, Slot> Slots = new Dictionary<string, Slot>();
        private int _height;
        private int _width;

        // Initialise empty board with custom height and width values (up to 26x26)
        public void InitBoard(int height, int width)
        {
            if (height >=1 && height <=26
                && width >= 1 && width <=26)
            {
                _height = height;
                _width = width;

                char[] xAxis = GetXAxis(width);
                int[] yAxis = GetYAxis(height);

                foreach (int y in yAxis)
                {
                    foreach (char x in xAxis)
                    {
                        Slot slot = new Slot(x, y, 0);
                        Slots.Add(slot.XCoordinate.ToString() + slot.YCoordinate, slot);
                    }
                }
            }
            else
            {
                throw new Exception("Please only enter dimensions up to 26x26");
            }
        }

        // Return X axis of equivalent ASCII characters ('A' - 'Z') depending on chosen width
        public char[] GetXAxis(int width)
        {
            char[] xAxis = new char[width];
            for (int i = 0; i < width; i++)
            {
                xAxis[i] = (char)(i + 65);
            }
            return xAxis;
        }

        // Return Y axis of integers (1 - 26) depending on chosen height
        public int[] GetYAxis(int height)
        {
            int[] yAxis = new int[height];
            for (int i = 0; i < height; i++)
            {
                yAxis[i] = i + 1;
            }

            // Reverse Y axis array to ensure coordinates are stored in printing order
            Array.Reverse(yAxis);

            return yAxis;
        }

        public int Height
        {
            get => _height;
        }

        public int Width
        {
            get => _width;
        }
    }
}
