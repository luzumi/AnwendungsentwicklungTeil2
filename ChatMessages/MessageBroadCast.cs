// corat::WPFFIrstSteps.ChatMessages.MessageBroadCast.cs::022021

using System;
using System.Text;

namespace ChatMessages
{
    public class MessageBroadCast : Message
    {
        public byte[] Data;
        public string Sender;
        public Datatypes DataType;
        public MessageBroadCast()
        {
            MessageType = MessageTypes.Broadcast;
        }

        public override int GetSize()
        {
            return 3 + Sender.Length + Data.Length;
        }

        public override byte[] ToArray()
        {
            byte[] result = new byte[this.GetSize()];
            result[0] = (byte)MessageType;
            result[1] = (byte)DataType;
            result[2] = (byte)Sender.Length;

            var sender = Sender.ConvertToArray();
            Array.Copy(sender, 0, result, 3, sender.Length);

            
            Array.Copy(Data, 0, result, 3 + sender.Length , Data.Length);
            return result;
        }

        public static MessageBroadCast FromArray(byte[] pArray)
        {
            if (pArray is null || pArray.Length < 3) throw new ArgumentException();

            MessageBroadCast m = new ();
            int lenghtSender = pArray[2];
            m.Sender = pArray[3..(3 + lenghtSender)].ConvertToString();
            m.Data = pArray[(3 + lenghtSender  )..];

            return m;
        }
    }
}
