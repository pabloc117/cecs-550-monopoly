/* The following class must not be modified in its functionality.
 * The implementation of methods may change, but passed arguments
 * and returns must remain constant.
 * Events may be added, but not removed or modified.
 */
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Networking
{
    public sealed class Communicator
    {
        private static Communicator instance = null;
        private static readonly object padlock = new object();
        private readonly Guid _ComputerID = Guid.NewGuid(); //Guid to represent our computer.  Used to differentiate between endpoints.
        private ROLE _UserRole = ROLE.UNKNOWN;
        private TcpListener myList;                         //used for server communication
        private TcpClient tcpclnt;                          //used for client communication
        private bool _IsConnected = false;
        private int _NumberClients = 2;
        private Dictionary<Guid, Socket> _ConnectionDict = new Dictionary<Guid, Socket>(); //Use this to store all of the connections we have.

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
                    _UserRole = value;
                }
                else
                {
                    //TODO KDog - Should we throw an exception if the programmer tries to change the Role after it has been changed from UKNOWN?
                }
            }
        }

        /// <summary>
        /// Starts a connection with the desired endpoint.
        /// </summary>
        /// <param name="ipAddr">The desired endpoint for connection. ex: "192.168.0.101"</param>
        /// <param name="portNumber">The desired port number to be used for connection.</param>
        public void StartClient(string ipAddr, int portNumber)
        {
            tcpclnt = new TcpClient();
            try
            {
                CheckPort(portNumber);
                tcpclnt.Connect(ipAddr, portNumber);
            }
            catch (SocketException e)
            {   //TODO KDog - Create custom exception?
                throw new SocketException();
                //MessageBox.Show("That was not a valid server IP.");
            }
            Socket s = tcpclnt.Client;
            onConnect(s);
            OnConnectionStatusChanged(new ConnectionStatusChangedEventArgs(s.Connected, s.RemoteEndPoint));
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
        /// Check portNumber to make sure it is an allowed port value. Min is 1, Max is 65535.
        /// </summary>
        /// <param name="portNumber">Port number being checked for usability.</param>
        private void CheckPort(int portNumber)
        {
            if (System.Net.IPEndPoint.MinPort >= portNumber || System.Net.IPEndPoint.MaxPort < portNumber)
                throw new ArgumentOutOfRangeException("portNumber", "The portNumber was either too large or too small. Must be between 1 and 65535 inclusively.");
        }

        /// <summary>
        /// Sends data across the socket.
        /// </summary>
        /// <param name="data">Data to be sent.</param>
        public void Send(byte[] data)
        {
            try
            {
                Packet tempPacket = new Packet(Packet.PACKET_FLAG.USER_READ, this._ComputerID, data);
                this._Send(tempPacket);
            }
            catch (ObjectDisposedException)
            {
                //TODO KDog - Maybe we throw an exception alerting a connection was dropped? Or is this handled with the connection status changed event?
            }
        }

        /// <summary>
        /// Sends data across the socket.
        /// </summary>
        /// <param name="data">Data to be sent.</param>
        private void _Send(Packet packetToSend)
        {
            try
            {
                foreach (KeyValuePair<Guid, Socket> kvp in _ConnectionDict)
                {
                    if (kvp.Key.CompareTo(packetToSend.SenderGuid) != 0)
                    {
                        kvp.Value.Send(packetToSend.ToBytes());
                    }
                }
            }
            catch (ObjectDisposedException)
            {
                //TODO KDog - Maybe we throw an exception alerting a connection was dropped? Or is this handled with the connection status changed event?
            }
        }

        /// <summary>
        /// Starts the Server.
        /// </summary>
        /// <param name="portNumber">The desired port number to be used for connection.</param>
        public void StartServer(int portNumber)
        {
            CheckPort(portNumber);
            myList = new TcpListener(GetMyIpAddr(), portNumber);
            ThreadStart ts = new ThreadStart(StartServerWork);
            Thread ServerThread = new Thread(ts);
            ServerThread.IsBackground = true;
            ServerThread.Start();
        }

        private void StartServerWork()
        {
            for (int i = 0; i < _NumberClients; i++)
            {
                myList.Start();
                while (!myList.Pending()) { };
                Socket s = myList.AcceptSocket();
                if (s.Connected)
                    onConnect(s);
                OnConnectionStatusChanged(new ConnectionStatusChangedEventArgs(s.Connected, s.RemoteEndPoint));
            }
        }

        private void onConnect(Socket socket)
        {
            System.Console.WriteLine("Connected");
            ParameterizedThreadStart ts = new ParameterizedThreadStart(Receive);
            Thread ReceiveThread = new Thread(ts);
            ReceiveThread.IsBackground = true;
            ReceiveThread.Start(socket as object);
            SendHandshake(socket);
        }

        private void Receive(object socket)
        {
            Socket s = socket as Socket;
            while (true)
            {
                try
                {
                    while (s.Available == 0 && s.Connected) { };
                    byte[] data = new byte[s.Available];
                    s.Receive(data);
                    //TODO KDog - We need to convert the received data to a Packet, then process from there.
                    Thread processDataThread = new Thread(new ParameterizedThreadStart(ProcessData));
                    processDataThread.IsBackground = true;
                    processDataThread.Start(new object[] { socket, data });
                }
                catch (Exception)
                {
                    OnConnectionStatusChanged(new ConnectionStatusChangedEventArgs(s.Connected, s.RemoteEndPoint));
                    break;
                }
            }
        }

        private void SendHandshake(Socket socket)
        {
            Packet handshake = new Packet(Packet.PACKET_FLAG.SYSTEM_READ, this._ComputerID, new byte[] { 0 });
            socket.Send(handshake.ToBytes());
        }

        private void ProcessData(object parameters)
        {
            object[] param = parameters as object[];
            Socket dataSocket = param[0] as Socket;
            Packet receivedPacket = new Packet(param[1] as byte[]);
            if (receivedPacket.DestinationFlag == Packet.PACKET_FLAG.USER_READ)
            {
                OnDataRecieved(new DataReceivedEventArgs(receivedPacket.Message));
                if (_UserRole == ROLE.SERVER)
                {
                    this._Send(receivedPacket);
                }
            }
            else if (receivedPacket.DestinationFlag == Packet.PACKET_FLAG.SYSTEM_READ)
            {
                ParseSystemPacket(dataSocket, receivedPacket);
            }
        }

        private void ParseSystemPacket(Socket dataSocket, Packet receivedPacket)
        {
            switch (receivedPacket.Message[0])
            {
                case 0:     //Add the passed Guid and Socket to the Dict.
                    _ConnectionDict.Add(receivedPacket.SenderGuid, dataSocket);
                    break;
                default:
                    throw new NotImplementedException("Parsed packet value is not yet implemented");
            }
        }

        public void Close()
        {
            foreach (Socket s in _ConnectionDict.Values)
            {
                s.Disconnect(false);
                s.Close();
                s.Dispose();
            }
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
}
