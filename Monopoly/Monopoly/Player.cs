using System;
using System.Collections.Generic;

namespace Monopoly
{
    public class Player
    {
        public int PlayerId;
        public int Money;
        public Dictionary<int, PropertyListing> Properties;
        public String PlayerGUID;
        List<int> money = new List<int>();

        public Player(int PlayerId, string PlayerGUID)
        {
            this.PlayerId = PlayerId;
            this.PlayerGUID = PlayerGUID;
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
