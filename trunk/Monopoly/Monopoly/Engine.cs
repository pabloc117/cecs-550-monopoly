using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Monopoly
{
    class Engine
    {
        private int maxPlayerIndex = 0;
        private int currentPlayerIndex = 0;
        private bool waiting = false;
        public int CurrentPlayerIndex
        {
            get { return currentPlayerIndex; }
        }

        public void StartGame(int numPlayers)
        {
            maxPlayerIndex = numPlayers - 1;
            OnPlayerTurn(new PlayerTurnEventArgs(-1, currentPlayerIndex));
        }

        public void TurnEnded()
        {
            int previousPlayerIndex = currentPlayerIndex;
            if (currentPlayerIndex == maxPlayerIndex)
                currentPlayerIndex = 0;
            else currentPlayerIndex++;
            OnPlayerTurn(new PlayerTurnEventArgs(previousPlayerIndex, currentPlayerIndex));
        }

        public event EventHandler<PlayerTurnEventArgs> PlayerTurn;
        private void OnPlayerTurn(PlayerTurnEventArgs e)
        {
            PlayerTurn(this, e);
        }
    }
}
