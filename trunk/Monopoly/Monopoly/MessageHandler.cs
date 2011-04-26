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
            OnNewIncomingMessage(new NewIncomingMessageEventArgs(omsg as Message));
        }

        public static string[] SplitString(string data)
        {
            return data.Split(new String[] { Message.DELIMETER }, StringSplitOptions.None);
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
    }
    public class NewIncomingMessageEventArgs : EventArgs
    {
        public readonly Message Message;
        public NewIncomingMessageEventArgs(Message message)
        {
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
            Roll = 2,
            IdInit = 3,
            Turn = 4,
            EndTurn = 5,
            Buy = 6,
            Rent = 7
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
                case (int)Type.Buy:
                    return Type.Buy;
                case (int)Type.Rent:
                    return Type.Rent;
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
