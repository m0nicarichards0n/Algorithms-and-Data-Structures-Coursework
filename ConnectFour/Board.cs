using System;
using System.Collections.Generic;

namespace ConnectFour
{
    class Board
    {
        private IDictionary<string, Slot> _slots = new Dictionary<string, Slot>();
        private int _height;
        private int _width;

        // Initialise empty board with custom height and width values (up to 26x26)
        public void InitBoard(int height, int width)
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
                    _slots.Add(slot.XCoordinate.ToString() + slot.YCoordinate, slot);
                }
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

        // Establish whether or not the board is full (i.e. stalemate)
        public bool Stalemate()
        {
            bool stalemate = true;
            foreach(KeyValuePair<string, Slot> slot in _slots)
            {
                if (slot.Value.Content == 0)
                {
                    stalemate = false;
                    return stalemate;
                }
            }
            return stalemate;
        }

        public int Height
        {
            get => _height;
        }

        public int Width
        {
            get => _width;
        }

        public IDictionary<string, Slot> Slots
        {
            get => _slots;
        }
    }
}
