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
using System.Text.RegularExpressions;

namespace Monopoly
{
    /// <summary>
    /// Interaction logic for IPRequest.xaml
    /// </summary>
    public partial class IPRequest : Window
    {
        public IPRequest()
        {
            InitializeComponent();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            //check valid input
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string ip = ip1.Text + "." + ip2.Text + "." + ip3.Text + "." + ip4.Text;
            OnIPAccept(new ConnectClickedEventArgs(ip));
            this.Close();
        }

        public event EventHandler<ConnectClickedEventArgs> IPAccept;
        private void OnIPAccept(ConnectClickedEventArgs e)
        {
            IPAccept(this, e);
        }

        private void ip_GotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }
    }

    public class ConnectClickedEventArgs : EventArgs
    {
        public string IP = "";
        public ConnectClickedEventArgs(string IP)
        { 
            this.IP = IP;
        }
    }
}
