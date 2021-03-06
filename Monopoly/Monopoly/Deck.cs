﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopoly
{
    //This class takes care of how we handle the decks of Chance and Community Chest cards.
    //It implement the intialization of the cards, shuffling, and drawing and sort the cards.
    class Deck
    {
        ImplementCards imp_Cards = new ImplementCards();

        List<int> chance = new List<int>();
        List<int> communityChest = new List<int>();

        public void InitChance()
        {
            for (int i = 0; i < chance.Count; i++)
            {
                chance.Add(i + 1);
            }
            Shuffle(chance, 1);
        }
        public void InitCommunityChest()
        {
            for (int i = 0; i < communityChest.Count; i++)
            {
                communityChest.Add(i + 21);
            }
            Shuffle(communityChest, 2);
        }
        private void Shuffle(List<int> cards, int type)
        {
            int temp = 0;
            int count = 0;
            int min, max;
            Random r = new Random();
            List<int> tempCards = new List<int>();

            if (type == 1)
            {
                min = 1;
                max = 17;
            }
            else
            {
                min = 21;
                max = 37;
            }

            //loop while the deck of card is still less than 16
            for (int i = 0; i < cards.Count; i++)
            {
                //generate a random number of min <= n < max
                temp = r.Next(min, max);
                //if there is something in the deck, we need to check to make sure
                //the random generated number isn't the same as the one in the deck
                if (tempCards.Count > 0)
                {
                    for (int j = i - 1; j >= 0; j--)
                    {
                        if (temp == tempCards[j])
                        {
                            count++;
                        }
                    }
                    //number already exist in deck
                    if (count > 0)
                    {
                        i--;
                        count = 0;
                    }
                    //number is not in deck yet, so add to deck
                    else
                    {
                        tempCards.Add(temp);
                    }
                }
                //empty deck, add the generated number to deck
                else
                    tempCards.Add(temp);
            }
            for (int i = 0; i < cards.Count; i++)
            {
                cards[i] = tempCards[i];
            }
        }
        public void drawCard(int type)
        {
            int card = 0; //card 0 = Community Chest, 1 = Chance

            //Gets a copy of the card
            //then add that to the bottom, and then remove it from top
            if (type == 1)
            {
                card = chance[0];
                chance.Add(chance[0]);
                chance.RemoveAt(0);
            }
            else
            {
                card = communityChest[0];
                communityChest.Add(communityChest[0]);
                communityChest.RemoveAt(0);
            }
            imp_Cards.getCardInfo(card);
        }
    }
}
