using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopoly
{
    class Shuffle
    {
        private int[] shuffle(int[] cards)
        {
            int temp = 0;
            Random r = new Random();
            for (int i = 0; i < cards.Length; i++)
            {
                temp = r.Next(1, 16);
                for (int j = i; j > 0; j--)
                {
                    
                }
            }
            return cards;
        }
    }
}
