using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopoly
{
    class Player
    {
        public int PlayerId;
        public int Money;
        public Dictionary<int, PropertyListing> Properties;

        public Player(int PlayerId)
        {
            this.PlayerId = PlayerId;
            this.Money = 1500;
        }
    }
}
