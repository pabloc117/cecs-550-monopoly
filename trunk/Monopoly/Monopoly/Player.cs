using System;
using System.Collections.Generic;

namespace Monopoly
{
    public class Player
    {
        public int PlayerId;
        public int _Money = 0;
        private Dictionary<int, PropertyListing> _Properties;
        public String PlayerGUID;

        public static string ConvertPlayerID(Dictionary<string,Player> p, int id)
        {
            string ret = "";
            foreach (Player t in p.Values)
            {
                if(id == t.PlayerId)
                    ret = t.PlayerGUID;
            }
            return ret;
        }

        public int Money
        {
            get 
            {
                return _Money;
            }
            set 
            {
                _Money = value;
                OnPlayerUpdate(new PlayerUpdateEventArgs());
            }
        }

        public void AddProperty(int key, PropertyListing value)
        {
            _Properties.Add(key, value);
            OnPlayerUpdate(new PlayerUpdateEventArgs());
        }

        public void RemoveProperty(int key)
        {
            _Properties.Remove(key);
            OnPlayerUpdate(new PlayerUpdateEventArgs());
        }

        public Dictionary<int, PropertyListing>.ValueCollection GetProperties()
        {
            return _Properties.Values;
        }

        public Player(int PlayerId, string PlayerGUID)
        {
            this.PlayerId = PlayerId;
            this.PlayerGUID = PlayerGUID;
            this.Money = 1500;
        }

        public event EventHandler<PlayerUpdateEventArgs> PlayerUpdate;
        private void OnPlayerUpdate(PlayerUpdateEventArgs e)
        {
            if (PlayerUpdate != null)
                PlayerUpdate(this, e);
        }
    }
    public class PlayerUpdateEventArgs : EventArgs
    {
        public PlayerUpdateEventArgs()
        {
        }
    }
}
