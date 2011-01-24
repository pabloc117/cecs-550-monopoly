using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopoly
{
    //This class only contains the string content of each Chance and Community chest cards.
    class CardsContent
    {        
        public string ChanceCards(int card)
        {
            string content = "";
            switch (card)
            {
                case 1:
                    content = "Advance to Go.\nCollect $200";
                    break;
                case 2:
                    content = "Advance to Illinois Ave.";
                    break;
                case 3:
                    content = "Advance token to nearest Utility. If unowned, you may buy it from the Bank. If owned, throw dice and pay owner a total ten times the amount thrown.";
                    break;
                case 4:
                    content = "Advance token to the nearest Railroad and pay owner twice the rental to which he/she is otherwise entitled. If Railroad is unowned, you may buy it from the Bank.";
                    break;
                case 5:
                    content = "You have won a game competition.\nCollect $100";
                    break;
                case 6:
                    content = "Advance to St. Charles Place.\nIf you pass Go, collect $200.";
                    break;
                case 7:
                    content = "Bank pays you dividend of $50";
                    break;
                case 8:
                    content = "Get out of Jail free";
                    break;
                case 9:
                    content = "Go back 3 spaces";
                    break;
                case 10:
                    content = "Go directly to Jail – do not pass Go, do not collect $200";
                    break;
                case 11:
                    content = "Make general repairs on all your property.\nFor each house pay $25 \nFor each hotel $100";
                    break;
                case 12:
                    content = "Speeding Fine $15";
                    break;
                case 13:
                    content = "Take a trip to Reading Railroad.\nIf you pass Go collect $200";
                    break;
                case 14:
                    content = "Advance to Boardwalk";
                    break;
                case 15:
                    content = "You have been elected chairman of the board.\nPay each player $50 ";
                    break;
                case 16:
                    content = "Your building and loan matures.\nCollect $150";
                    break;
            }
            return content;
        }
        public string CommunityCards(int card)
        {
            string content = "";
            switch (card)
            {
                case 1:
                    content = "In community";
                        break;
            }
            return content;
        }
    }
}
