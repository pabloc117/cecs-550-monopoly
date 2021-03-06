﻿using System;
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
    /// Interaction logic for BuyQuery.xaml
    /// </summary>
    public partial class BuyQuery : Window
    {
        private int index = 0;
        public BuyQuery(Window owner, PropertyListing property)
        {
            InitializeComponent();
            index = property.Location;
            this.Owner = owner;
            PropertyName.Text = property.Name;
            TitleBorder.Background = property.ColorGroup;
            Rent.Text = "$" + property.CalculateRent();
            Rent1.Text = property.House1 == 0 ? "N/A" : "$" + property.House1;
            Rent2.Text = property.House2 == 0 ? "N/A" : "$" + property.House2;
            Rent3.Text = property.House3 == 0 ? "N/A" : "$" + property.House3;
            Rent4.Text = property.House4 == 0 ? "N/A" : "$" + property.House4;
            RentH.Text = property.Hotel == 0 ? "N/A" : "$" + property.Hotel;
            CostH.Text = property.HouseCost == 0 ? "N/A" : "$" + property.HouseCost;
            CostHo.Text = property.HouseCost == 0 ? "N/A" : "$" + property.HouseCost;
            Mortgage.Text = property.Mortgage == 0 ? "N/A" : "$" + property.Mortgage;
            PurchaseText.Text = "Would you like to purchase " + property.Name + " for $" + property.Cost + "?";
        }

        private void Buy_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            OnResult(new BuyPropertyEventArgs(index, true));
        }

        private void Pass_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            OnResult(new BuyPropertyEventArgs(index, false));
        }

        public event EventHandler<BuyPropertyEventArgs> Result;
        private void OnResult(BuyPropertyEventArgs e)
        {
            Result(this, e);
        }
    }
    public class BuyPropertyEventArgs : EventArgs
    {
        public int PropertyIndex = 0;
        public bool Bought = false;
        public BuyPropertyEventArgs(int index, bool bought)
        {
            PropertyIndex = index;
            Bought = bought;
        }
    }
}
