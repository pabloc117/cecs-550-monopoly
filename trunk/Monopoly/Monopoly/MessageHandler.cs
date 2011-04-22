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

        public void Start()
        {
            if (messageThread == null)
            {
                ThreadStart start = new ThreadStart(HandleMessages);
                messageThread = new Thread(start);
                messageThread.IsBackground = true;
                messageThread.Name = "Message Thread";
                messageThread.Start();
            }
        }

        public void Stop()
        {
            if (messageThread != null)
            {
                messageThread.Interrupt();
                messageThread = null;
            }
        }

        private void HandleMessages()
        { 
            while(true)
            {
                if (messages.Count > 0)
                {
                    Message msg = messages.First();
                    switch (msg.MessageType)
                    {
                        case Message.Type.Chat:
                            string data = Encoding.UTF8.GetString(msg.Data);
                            string[] attr = data.Split(new String[]{Message.DELIMETER}, StringSplitOptions.None);
                            OnNewIncomingMessage(new NewIncomingMessageEventArgs(attr[0], attr[1]));
                            break;
                        case Message.Type.Move:
                            break;
                        case Message.Type.Trade:
                            break;
                        case Message.Type.Unknown:
                            break;
                        case Message.Type.IdInit:
                            OnPlayerInitMessage(new PlayerInitPacketEventArgs(Encoding.UTF8.GetString(msg.Data)));
                            break;
                        case Message.Type.Turn:
                            //TODO Add this

                            break;
                        default:
                            break;
                    }
                    messages.Remove(msg);
                }
                else
                { }
            }
        }

        public void QueueMessage(byte[] msg)
        {
            int type = msg[0];
            byte[] data = new byte[msg.Length - 1];
            Array.Copy(msg, 1, data, 0, data.Length);
            messages.Add(new Message(Message.GetType(type), data));
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
        
        public event EventHandler<PlayerTurnEventArgs> PlayerTurnMessage;
        private void OnPlayerTurnMessage(PlayerTurnEventArgs e)
        {
            PlayerTurnMessage(this, e);
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

    public class Message
    {

        public enum Type
        {
            
            Unknown = -1,
            Chat = 0,
            Trade = 1,
            Move = 2,
            IdInit = 3,
            Turn = 4
        }

        public static string DELIMETER = "<@>";

        public static Type GetType(int type)
        {
            switch(type)
            {
                case (int)Type.Chat:
                    return Type.Chat;
                case (int)Type.Move:
                    return Type.Move;
                case (int)Type.Trade:
                    return Type.Trade;
                case (int)Type.IdInit:
                    return Type.IdInit;
                case (int)Type.Turn:
                    return Type.Turn;
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
