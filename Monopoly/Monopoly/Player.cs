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
        List<int> money = new List<int>();

        public Player(int PlayerId, string PlayerEndPoint)
        {
            this.PlayerId = PlayerId;
            this.PlayerEndPoint = PlayerEndPoint;
            this.Money = 1500;
        }
        public Player()
        {
            for (int i = 0; i < 4; i++)
            {
                money.Add(2000);
            }
        }
        public void Collect(int player, int amount)
        {
            money[player] = money[player] + amount;
        }
        public void pay(int player, int amount)
        {
            money[player] = money[player] - amount;
        }
    }
}
