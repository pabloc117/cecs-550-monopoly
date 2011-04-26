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
    /// Interaction logic for ChatComponent.xaml
    /// </summary>
    public partial class ChatComponent : UserControl
    {
        public ChatComponent()
        {
            InitializeComponent();
        }

        public void NewMessage(string sender, string message)
        {
            if (Dispatcher.CheckAccess())
            {
                Chat.Text += "\n<" + sender + ">:  " + message;
            }
            else Dispatcher.BeginInvoke(new Action<string, string>(NewMessage), new object[]{sender, message});
        }

        public void AcceptInput()
        {
            if (Input.Text.CompareTo("") != 0)
            {
                OnNewOutgoingMessage(new NewOutgoingMessageEventArgs(Input.Text));
                NewMessage("Me", Input.Text);
                Input.Text = "";
            }
        }

        private void Input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AcceptInput();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AcceptInput();
        }

        /// <summary>
        /// Event triggered when user sends a chat message.
        /// </summary>
        public event EventHandler<NewOutgoingMessageEventArgs> NewOutgoingMessage;
        private void OnNewOutgoingMessage(NewOutgoingMessageEventArgs e)
        {
            NewOutgoingMessage(this, e);
        }

        private void Chat_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (0 == e.VerticalChange)
            {
                Chat.ScrollToEnd();
            }
        }
    }
    public class NewOutgoingMessageEventArgs : EventArgs
    {
        public readonly string Message;
        public NewOutgoingMessageEventArgs(string message)
        {
            this.Message = message;
        }
    }
}
