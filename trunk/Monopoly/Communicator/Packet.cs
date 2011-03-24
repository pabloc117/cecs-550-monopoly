using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Networking
{
    public class Packet
    {
        public enum PACKET_FLAG
        {
            SYSTEM_READ = 0,
            USER_READ = 1
        }

        private PACKET_FLAG _DestinationFlag;
        private static int GUID_LENGTH = 16;
        public static int HEADER_LENGTH = GUID_LENGTH + 5;
        private Guid _SenderGuid;
        private byte[] _Message;
        private int _MessageLength;

        public PACKET_FLAG DestinationFlag
        {
            get { return _DestinationFlag; }
        }

        public Guid SenderGuid
        {
            get { return _SenderGuid; }
        }

        public byte[] Message
        {
            get { return _Message; }
            set
            {
                if (value.Length == _Message.Length)
                    _Message = value;
            }
        }

        public int MessageLength
        {
            get { return _MessageLength; }
        }

        public Packet(PACKET_FLAG DestinationFlag, Guid SenderGuid, byte[] Message)
        {
            this._DestinationFlag = DestinationFlag;
            this._SenderGuid = SenderGuid;
            this._Message = Message;
        }

        public Packet(byte[] HeaderArray)
        {
            this._DestinationFlag = HeaderArray[0] == 0 ? PACKET_FLAG.SYSTEM_READ : PACKET_FLAG.USER_READ;
            byte[] RemoteGuidArray = new byte[GUID_LENGTH];
            Buffer.BlockCopy(HeaderArray, 1, RemoteGuidArray, 0, GUID_LENGTH);
            this._SenderGuid = new Guid(RemoteGuidArray);
            byte[] MessageLength = new byte[4];
            Buffer.BlockCopy(HeaderArray, 1 + GUID_LENGTH, MessageLength, 0, 4);
            _MessageLength = BitConverter.ToInt32(MessageLength, 0);
            this._Message = new byte[_MessageLength];
        }

        public byte[] ToBytes()
        {
            byte[] bytePacket = new byte[HEADER_LENGTH + _Message.Length];
            Buffer.BlockCopy(new byte[]{(byte) _DestinationFlag}, 0, bytePacket, 0, 1);
            Buffer.BlockCopy(_SenderGuid.ToByteArray(), 0, bytePacket, 1, _SenderGuid.ToByteArray().Length);
            Buffer.BlockCopy(BitConverter.GetBytes(_Message.Length), 0, bytePacket, _SenderGuid.ToByteArray().Length + 1, 4);
            Buffer.BlockCopy(_Message, 0, bytePacket, _SenderGuid.ToByteArray().Length + 1 + 4, _Message.Length);
            return bytePacket;
        }
    }
}
