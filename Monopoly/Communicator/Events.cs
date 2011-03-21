using System;
using System.Net;

namespace Networking
{
    public class ConnectionStatusChangedEventArgs : EventArgs
    {
        public readonly bool Connected;
        public readonly EndPoint RemoteEndPoint;
        public ConnectionStatusChangedEventArgs(bool Connected, EndPoint RemoteEndPoint)
        {
            this.Connected = Connected;
            this.RemoteEndPoint = RemoteEndPoint;
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
