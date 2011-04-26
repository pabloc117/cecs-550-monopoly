using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopoly
{
    class ImplementCards
    {
        Player Player;
        MainWindow MW;
        private Dictionary<int, GameCard> _gameCards;
        public void getCardInfo(int cid)
        {
            int jump = 0, move = 0, collect = 0, pay = 0, id = 0;
            string payto = "", collectFrom = "", text = "", type = "";
            _gameCards = ThemeParser.Getcard();

            foreach (GameCard gcard in _gameCards.Values)
            {
                if (gcard.Id == cid)
                {
                    id = gcard.Id;
                    jump = gcard.Jump;
                    move = gcard.Move;
                    collect = gcard.Collect;
                    pay = gcard.Pay;
                    payto = gcard.PayTo;
                    collectFrom = gcard.CollectFrom;
                    text = gcard.Text;
                    type = gcard.Type;
                    break;
                }
            }
            printText(text);
            if (jump != 0)
            {
                jumpTo(jump);
            }
            else if (move != 0)
            {
                movePlayer(move);
            }
            else if (collect != 0)
            {
                if (collectFrom == "bank")
                {
                    collectMoney(collect);
                }
                else if (collectFrom == "player")
                {

                    collectplayer(collect);

                }
            }
            else if (pay != 0)
            {

            }
        }
        public void jumpTo(int loc)
        {
            //place holders--------
            UserPiece up = new UserPiece();
            int current = 0;
            //------------
            MW.Jump(up, current, loc);
        }
        public void movePlayer(int numSpaces)
        {
            //place holder-----------
            UserPiece up = new UserPiece();
            //---------------
            MW.Move(up, numSpaces);
        }
        public void collectMoney(int amount)
        {
            //TODO WTF?
            //Player.Collect(0, amount);
        }
        public void collectplayer(int amount)
        {
            //get player that drew card
            int tempPlayer = 0; //will change this later, this is just place holder
            for (int i = 0; i < 4; i++)
            {
                if (i != tempPlayer)
                {
                    //TODO WTF?
                    //Player.pay(i, amount);
                }
            }
            //TODO WTF?
            //Player.Collect(tempPlayer, amount);
        }
        public void payBank(int amount)
        {
            //TODO WTF?
            //Player.pay(0, amount);
        }
        public void payPlayer(int amount)
        {
            int tempPlayer = 0; //will change this later, this is just place holder
            for (int i = 0; i < 4; i++)
            {
                if (i != tempPlayer)
                {
                    //TODO WTF?
                    //Player.Collect(i, amount);
                }
            }
            //TODO WTF?
            //Player.pay(tempPlayer, amount);
        }
        public void printText(string text)
        {

        }
    }
}
