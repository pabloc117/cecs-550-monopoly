/* The following class must not be modified in its functionality.
 * The implementation of methods may change, but passed arguments
 * and returns must remain constant.
 * Events may be added, but not removed or modified.
 */
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Networking
{
    public sealed class Communicator
    {
        private static Communicator instance = null;
        private static readonly object padlock = new object();
        private ROLE _UserRole = ROLE.UNKNOWN;
        private Socket s;
        private TcpListener myList; //used for server communication
        private TcpClient tcpclnt;  //used for client communication
        private bool _IsConnected = false;
        private Guid _ComputerID = new Guid(); //Guid to represent our computer.  Used to differentiate between endpoints.

        public enum ROLE
        {
            UNKNOWN,
            CLIENT,
            SERVER
        }

        private Communicator()
        { 
        }

        /// <summary>
        /// Returns the instance of Communicator.
        /// </summary>
        public static Communicator Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Communicator();
                    }
                    return instance;
                }
            }
        }

        /// <summary>
        /// Returns the connection status.
        /// </summary>
        public bool IsConnected
        {
            get { return _IsConnected; }
            set { _IsConnected = value; }
        }

        /// <summary>
        /// Returns the role of the User, whether it be Server, Client, or UnKnown.
        /// </summary>
        public ROLE UserRole
        {
            get
            {
                return _UserRole;
            }
            set
            {
                if (value != ROLE.UNKNOWN && _UserRole == ROLE.UNKNOWN)
                {
                    if (value == ROLE.SERVER)
                        myList = new TcpListener(GetMyIpAddr(), 8001);
                    else
                        tcpclnt = new TcpClient();
                    _UserRole = value;
                }
            }
        }

        /// <summary>
        /// Starts a connection with the desired endpoint.
        /// </summary>
        /// <param name="ipAddr">The desired endpoint for connection. ex: "192.168.0.101"</param>
        public void StartClient(string ipAddr)
        {
            try
            {
                tcpclnt.Connect(ipAddr, 8001);
            }
            catch (SocketException)
            {   //TODO KDog - Create custom exception.
                throw new SocketException();
                //MessageBox.Show("That was not a valid server IP.");
            }
            s = tcpclnt.Client;
            onConnect();
            OnConnectionStatusChanged(new ConnectionStatusChangedEventArgs(s.Connected));
        }

        /// <summary>
        /// Gets the local endpoints
        /// </summary>
        /// <returns>Endpoint for which remote users should use to connect.</returns>
        public IPAddress GetMyIpAddr()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }
            return null;
        }

        /// <summary>
        /// Sends data across the socket.
        /// </summary>
        /// <param name="data">Data to be sent.</param>
        public void Send(byte[] data)
        {
            try
            {
                s.Send(data);
            }
            catch (ObjectDisposedException)
            { }
        }

        /// <summary>
        /// Starts the Server.
        /// </summary>
        public void StartServer()
        {
            ThreadStart ts = new ThreadStart(StartServerWork);
            Thread ServerThread = new Thread(ts);
            ServerThread.IsBackground = true;
            ServerThread.Start();
        }

        private void StartServerWork()
        {
            myList.Start();
            while (!myList.Pending()) { };
            s = myList.AcceptSocket();
            if (s.Connected)
                onConnect();
            OnConnectionStatusChanged(new ConnectionStatusChangedEventArgs(s.Connected));
        }

        private void onConnect()
        {
            System.Console.WriteLine("Connected");
            ThreadStart ts = new ThreadStart(Receive);
            Thread ReceiveThread = new Thread(ts);
            ReceiveThread.IsBackground = true;
            ReceiveThread.Start();
        }

        private void Receive()
        {
            while (true)
            {
                try
                {
                    while (s.Available == 0 && s.Connected) { };
                    byte[] data = new byte[s.Available];
                    s.Receive(data);
                    OnDataRecieved(new DataReceivedEventArgs(data));
                }
                catch (Exception)
                {
                    OnConnectionStatusChanged(new ConnectionStatusChangedEventArgs(s.Connected));
                    break;
                }
            }
        }

        public void Close()
        {
            s.Disconnect(false);
            s.Close();
        }

        /// <summary>
        /// Event triggered when data is received from the socket stream.
        /// </summary>
        public event EventHandler<DataReceivedEventArgs> DataRecieved;
        private void OnDataRecieved(DataReceivedEventArgs e)
        {
            DataRecieved(this, e);
        }
        /// <summary>
        /// Event triggered when there is a change in Connection Status.
        /// </summary>
        public event EventHandler<ConnectionStatusChangedEventArgs> ConnectionStatusChanged;
        private void OnConnectionStatusChanged(ConnectionStatusChangedEventArgs e)
        {
            this._IsConnected = e.Connected;
            ConnectionStatusChanged(this, e);
        }
    }
    public class ConnectionStatusChangedEventArgs : EventArgs
    {
        public readonly bool Connected;
        public ConnectionStatusChangedEventArgs(bool Connected)
        {
            this.Connected = Connected;
        }
    }

    public class DataReceivedEventArgs : EventArgs
    {
        public readonly byte[] DataReceived;
        public DataReceivedEventArgs(byte[] DataReceived)
        {
            this.DataReceived = DataReceived;
        }
    }


}
