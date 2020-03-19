using System;

namespace ConnectFour
{
    class Slot
    {
        private char _xCoordinate;
        private int _yCoordinate;
        private int _content;

        public Slot (char xCoordinate, int yCoordinate)
        {
            _xCoordinate = xCoordinate;
            _yCoordinate = yCoordinate;
        }
        public Slot(char xCoordinate, int yCoordinate, int content)
        {
            _xCoordinate = xCoordinate;
            _yCoordinate = yCoordinate;
            _content = content;
        }

        public char XCoordinate
        {
            get => _xCoordinate;
        }

        public int YCoordinate
        {
            get => _yCoordinate;
        }

        public int Content
        {
            get => _content;
            set
            {
                if (value == 0 || value == 1 || value == 2)
                {
                    _content = value;
                }
                else
                {
                    throw new Exception("Invalid slot content encountered.");
                }
            }
        }
    }
}
