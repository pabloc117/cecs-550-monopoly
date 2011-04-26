using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Monopoly
{
    public class MessageHandler
    {
        List<Message> messages = new List<Message>();
        private Thread messageThread;

        public MessageHandler()
        {
        }

        private void HandleMessage(object omsg)
        {
            Message msg = omsg as Message;
            if (msg == null)
                return;
            string[] attr;
            string data = msg.Data == null ? "" : Encoding.UTF8.GetString(msg.Data);
            switch (msg.MessageType)
            {
                case Message.Type.Chat:
                    attr = data.Split(new String[] { Message.DELIMETER }, StringSplitOptions.None);
                    OnNewIncomingMessage(new NewIncomingMessageEventArgs(attr[0], attr[1]));
                    break;
                case Message.Type.Roll:
                    attr = data.Split(new String[] { Message.DELIMETER }, StringSplitOptions.None);
                    OnRollMessage(new RollMessageEventArgs(Int32.Parse(attr[0]), Int32.Parse(attr[1])));
                    break;
                case Message.Type.Trade:
                    break;
                case Message.Type.Unknown:
                    break;
                case Message.Type.IdInit:
                    OnPlayerInitMessage(new PlayerInitPacketEventArgs(data));
                    break;
                case Message.Type.Turn:
                    string[] pTurn = data.Split(new String[] { Message.DELIMETER }, StringSplitOptions.None);
                    OnPlayerTurnMessage(new PlayerTurnEventArgs(Int32.Parse(pTurn[0]), Int32.Parse(pTurn[1])));
                    break;
                case Message.Type.EndTurn:
                    OnEndTurnMessage(new EndTurnMessageEventArgs());
                    break;
                default:
                    break;
            }
        }

        public void QueueMessage(byte[] msg)
        {
            int type = msg[0];
            byte[] data = null;
            if (msg.Length > 1)
            {
                data = new byte[msg.Length - 1];
                Array.Copy(msg, 1, data, 0, data.Length);
            }
            ParameterizedThreadStart start = new ParameterizedThreadStart(HandleMessage);
            Thread messageThread = new Thread(start);
            messageThread.IsBackground = true;
            messageThread.Name = "Message Thread";
            messageThread.Start(new Message(Message.GetType(type), data));
        }

        /// <summary>
        /// Event triggered when there is an incoming chat message.
        /// </summary>
        public event EventHandler<NewIncomingMessageEventArgs> NewIncomingMessage;
        private void OnNewIncomingMessage(NewIncomingMessageEventArgs e)
        {
            NewIncomingMessage(this, e);
        }

        public event EventHandler<PlayerInitPacketEventArgs> PlayerInitMessage;
        private void OnPlayerInitMessage(PlayerInitPacketEventArgs e)
        {
            PlayerInitMessage(this, e);
        }
        public event EventHandler<RollMessageEventArgs> RollMessage;
        private void OnRollMessage(RollMessageEventArgs e)
        {
            RollMessage(this, e);
        }
        
        public event EventHandler<PlayerTurnEventArgs> PlayerTurnMessage;
        private void OnPlayerTurnMessage(PlayerTurnEventArgs e)
        {
            PlayerTurnMessage(this, e);
        }

        public event EventHandler<EndTurnMessageEventArgs> EndTurnMessage;
        private void OnEndTurnMessage(EndTurnMessageEventArgs e)
        {
            EndTurnMessage(this, e);
        }
    }
    public class PlayerTurnEventArgs : EventArgs
    {
        public readonly int EndTurnId;
        public readonly int StartTurnId;
        public PlayerTurnEventArgs(int EndTurnId, int StartTurnId)
        {
            this.EndTurnId = EndTurnId;
            this.StartTurnId = StartTurnId;
        }
    }
    public class PlayerInitPacketEventArgs : EventArgs
    {
        public readonly string PlayerPacket;
        public PlayerInitPacketEventArgs(string PlayerPacket)
        {
            this.PlayerPacket = PlayerPacket;
        }
    }
    public class NewIncomingMessageEventArgs : EventArgs
    {
        public readonly string Sender;
        public readonly string Message;
        public NewIncomingMessageEventArgs(string sender, string message)
        {
            this.Sender = sender;
            this.Message = message;
        }
    }
    public class RollMessageEventArgs : EventArgs
    {
        public int Seed = 0;
        public int PlayerID = 0;
        public RollMessageEventArgs(int playerID, int seed)
        {
            this.PlayerID = playerID;
            this.Seed = seed;
        }
    }

    public class EndTurnMessageEventArgs : EventArgs
    {
        public EndTurnMessageEventArgs()
        { }
    }

    public class Message
    {

        public enum Type
        {
            
            Unknown = -1,
            Chat = 0,
            Trade = 1,
            Roll = 2,
            IdInit = 3,
            Turn = 4,
            EndTurn = 5
        }

        public static string DELIMETER = "<@>";

        public static Type GetType(int type)
        {
            switch(type)
            {
                case (int)Type.Chat:
                    return Type.Chat;
                case (int)Type.Roll:
                    return Type.Roll;
                case (int)Type.Trade:
                    return Type.Trade;
                case (int)Type.IdInit:
                    return Type.IdInit;
                case (int)Type.Turn:
                    return Type.Turn;
                case (int)Type.EndTurn:
                    return Type.EndTurn;
                default:
                    return Type.Unknown;
            }
        }

        private byte[] data;
        private Type type;

        public byte[] Data
        {
            get { return data; }
        }

        public Type MessageType
        {
            get { return type; }
        }

        public Message(Message.Type type, byte[] data)
        {
            this.type = type;
            this.data = data;
        }

        public byte[] ToBytes()
        {
            byte[] data = new byte[this.data.Length + 1];

            data[0] = (byte)((int)this.type);
            Array.Copy(this.data, 0, data, 1, this.data.Length);

            return data;
        }
    }
}
