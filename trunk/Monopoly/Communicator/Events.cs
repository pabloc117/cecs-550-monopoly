using System;
using System.Net;

namespace Networking
{
    public class ConnectionStatusChangedEventArgs : EventArgs
    {
        public readonly bool Connected;
        public readonly Guid RemoteGUID;
        public ConnectionStatusChangedEventArgs(bool Connected, Guid RemoteGUID)
        {
            this.Connected = Connected;
            this.RemoteGUID = RemoteGUID;
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
