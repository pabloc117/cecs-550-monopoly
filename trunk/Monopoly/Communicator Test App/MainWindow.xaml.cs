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
using Networking;

namespace Communicator_Test_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Communicator c = Communicator.Instance;
            //System.Text.UTF8Encoding encoding=new System.Text.UTF8Encoding();
            //byte[] d = encoding.GetBytes("hey there");
            //byte[] b = c.CreatePacket(d);
            c.ConnectionStatusChanged += new EventHandler<ConnectionStatusChangedEventArgs>(c_ConnectionStatusChanged);
        }

        void c_ConnectionStatusChanged(object sender, ConnectionStatusChangedEventArgs e)
        {
            if (e.Connected)
            {
                UpdateStatus("Connection Size = Connected!" + e.RemoteGUID.ToString());
            }
            else
            {
                 UpdateStatus("Connection Size = FAILED!");
            }
        }

        private void UpdateStatus(string statusString)
        {
            if (this.Dispatcher.CheckAccess())
            {
                ConnectionStatus.Text = statusString;
            }
            else this.Dispatcher.BeginInvoke(new Action<string>(UpdateStatus), new object[] { statusString });
        }

        private void Server_Click(object sender, RoutedEventArgs e)
        {
            Communicator.Instance.UserRole = Communicator.ROLE.SERVER;
            Communicator.Instance.StartServer(8001);
            ServerIp.Text = Communicator.Instance.GetMyIpAddr().ToString();
        }

        private void Client_Click(object sender, RoutedEventArgs e)
        {
            Communicator.Instance.UserRole = Communicator.ROLE.CLIENT;
            Console.WriteLine("DesiredIP = " + desiredIp.Text);
            Communicator.Instance.StartClient(desiredIp.Text, 8001);
        }
    }
}
