using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopoly
{    
    class Deck
    {
        Chance chanceDeck = new Chance();
        CommunityChest communityDeck = new CommunityChest();

        List<int> chance = new List<int>(new int[16]);
        List<int> communityChest = new List<int>(new int[16]);

        public void Init_Chance()
        {
            for (int i = 0; i < chance.Count; i++)
            {
                chance.Add(i + 1);
            }
            Shuffle(chance);
        }
        public void Init_CommunityChest()
        {
            for (int i = 0; i < communityChest.Count; i++)
            {
                communityChest.Add(i + 1);
            }
            Shuffle(communityChest);
        }
        public string drawCard(int type)
        {
            string content = "";
            if (type == 1)
            {
                
            }

            return content;
        }
        private void Shuffle(List<int> cards)
        {
            int temp = 0;
            int count = 0;
            Random r = new Random();
            List<int> tempCards = new List<int>();
            
            for (int i = 0; i < cards.Count; i++)
            {
                temp = r.Next(1, 17);
                if (tempCards.Count > 0)
                {
                    for (int j = i - 1; j >= 0; j--)
                    {
                        if (temp == tempCards[j])
                        {
                            count++;
                        }
                    }
                    if (count > 0)
                    {
                        i--;
                        count = 0;
                    }
                    else
                    {
                        tempCards.Add(temp);
                    }
                }
                else
                    tempCards.Add(temp);
            }
            for (int i = 0; i < cards.Count; i++)
            {
                cards[i] = tempCards[i];
            }
        }
    }
}
