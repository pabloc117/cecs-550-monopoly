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
using System.Windows.Shapes;

namespace Monopoly
{
    /// <summary>
    /// Interaction logic for Chance.xaml
    /// </summary>
    public partial class Chance : Window
    {
        List<int> chanceCards = new List<int>(new int[16]);
        public Chance()
        {
            InitializeComponent();
            //shuffle(chanceCards);
        }

        private void Assign_Cards(int card)
        {
            switch (card)
            {
                case 1:
                    txtBlock.Text = "Advance to Go.\nCollect $200";
                    break;
                case 2:
                    txtBlock.Text = "Advance to Illinois Ave.";
                    break;
                case 3:
                    txtBlock.Text = "Advance token to nearest Utility. If unowned, you may buy it from the Bank. If owned, throw dice and pay owner a total ten times the amount thrown.";
                    break;
                case 4:
                    txtBlock.Text = "Advance token to the nearest Railroad and pay owner twice the rental to which he/she is otherwise entitled. If Railroad is unowned, you may buy it from the Bank.";
                    break;
                case 5:
                    txtBlock.Text = "You have won a game competition.\nCollect $100";
                    break;
                case 6:
                    txtBlock.Text = "Advance to St. Charles Place.\nIf you pass Go, collect $200.";
                    break;
                case 7:
                    txtBlock.Text = "Bank pays you dividend of $50";
                    break;
                case 8:
                    txtBlock.Text = "Get out of Jail free";
                    break;
                case 9:
                    txtBlock.Text = "Go back 3 spaces";
                    break;
                case 10:
                    txtBlock.Text = "Go directly to Jail – do not pass Go, do not collect $200";
                    break;
                case 11:
                    txtBlock.Text = "Make general repairs on all your property.\nFor each house pay $25 \nFor each hotel $100";
                    break;
                case 12:
                    txtBlock.Text = "Speeding Fine $15";
                    break;
                case 13:
                    txtBlock.Text = "Take a trip to Reading Railroad.\nIf you pass Go collect $200";
                    break;
                case 14:
                    txtBlock.Text = "Advance to Boardwalk";
                    break;
                case 15:
                    txtBlock.Text = "You have been elected chairman of the board.\nPay each player $50 ";
                    break;
                case 16:
                    txtBlock.Text = "Your building and loan matures.\nCollect $150";
                    break;
            }

        }
    }
}
