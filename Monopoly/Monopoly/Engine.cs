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
            ThreadStart workStart = new ThreadStart(Work);
            Thread workThread = new Thread(workStart);
            workThread.Name = "EngineWorkThread";
            workThread.IsBackground = true;

            this.maxPlayerIndex = numPlayers - 1;
        }

        private void Work()
        {
            while (true)
            {
                waiting = true;
                if (currentPlayerIndex == maxPlayerIndex)
                    currentPlayerIndex = 0;
                else currentPlayerIndex++;
                OnPlayerTurn(new PlayerTurnEventArgs(currentPlayerIndex));
                while (waiting)
                { }
            }
        }

        public void TurnEnded()
        {
            waiting = false;
        }

        public event EventHandler<PlayerTurnEventArgs> PlayerTurn;
        private void OnPlayerTurn(PlayerTurnEventArgs e)
        {
            PlayerTurn(this, e);
        }
    }

    public class PlayerTurnEventArgs : EventArgs
    {
        public int ID = 0;
        public PlayerTurnEventArgs(int ID)
        {
            this.ID = ID;
        }
    }
}
