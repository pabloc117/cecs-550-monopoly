using System;
using System.Collections.Generic;

namespace Monopoly
{
    class Player
    {
        public int PlayerId;
        public int Money;
        public Dictionary<int, PropertyListing> Properties;
        public String PlayerEndPoint;

        public Player(int PlayerId, string PlayerEndPoint)
        {
            this.PlayerId = PlayerId;
            this.PlayerEndPoint = PlayerEndPoint;
            this.Money = 1500;
        }
    }
}
