using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Monopoly
{
    /// <summary>
    /// Interaction logic for PlayerInfo.xaml
    /// </summary>
    public partial class PlayerInfo : UserControl
    {
        Player LocalPlayer;
        public PlayerInfo()
        {
            InitializeComponent();
        }

        public void InitPlayer(Player player)
        {
            if (this.Dispatcher.CheckAccess())
            {
                LocalPlayer = player;
                PlayerID.Text = "Player " + (player.PlayerId + 1);
                MoneyDisplay.Text = "Cash: $" + player.Money;
                player.PlayerUpdate += new EventHandler<PlayerUpdateEventArgs>(player_PlayerUpdate);
            }
            else this.Dispatcher.BeginInvoke(new Action<Player>(InitPlayer), new object[] {player });
        }

        void player_PlayerUpdate(object sender, PlayerUpdateEventArgs e)
        {
            if (this.Dispatcher.CheckAccess())
            {
                MoneyDisplay.Text = "Cash: $" + LocalPlayer.Money;
                PropertyDisplay.Text = String.Empty;
                Dictionary<int, PropertyListing>.ValueCollection Dict = LocalPlayer.GetProperties();
                foreach(PropertyListing pl in Dict)
                {
                    PropertyDisplay.Inlines.Add(new Run(pl.Name));
                    PropertyDisplay.Inlines.Add(new LineBreak());
                }
                if (Dict == null || Dict.Count == 0)
                {
                    PropertyDisplay.Inlines.Add(new Run("No Properties Owned"));
                    PropertyDisplay.Inlines.Add(new LineBreak());
                }
            }
            else this.Dispatcher.BeginInvoke(new Action<object, PlayerUpdateEventArgs>(player_PlayerUpdate), new object[] { sender, e });
        }
    }
}
