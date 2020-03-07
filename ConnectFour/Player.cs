using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectFour
{
    class Player
    {
        private string _name;

        public Player (string name)
        {
            _name = name;
        }

        public string Name
        {
            get => _name;
        }

    }
}
