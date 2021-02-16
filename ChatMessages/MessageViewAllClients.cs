// corat::WPFFIrstSteps.ChatMessages.MessageViewAllClients.cs::022021

using System;

namespace ChatMessages
{
    public class MessageViewAllClients : Message
    {
        public byte[] _data;
        public string SenderName;
        public string TargetName;
        public Datatypes DataType;
        public MessageViewAllClients()
        {
            MessageType = MessageTypes.ViewAllClients;
            DataType = Datatypes.Text;
        }

        public override int GetSize()
        {
            throw new System.NotImplementedException();
        }

        public override byte[] ToArray()
        {
            byte[] result = new byte[this.GetSize()];
            result[0] = (byte)MessageType;
            result[1] = (byte)DataType;
            result[2] = (byte)SenderName.Length;

            var sender = SenderName.ConvertToArray();
            Array.Copy(sender, 0, result, 4, sender.Length);

            return result;
        }

        public static Message FromArray(byte[] pArray)
        {
            if (pArray is null || pArray.Length > 2) throw new ArgumentException();

            MessageDirect m = new MessageDirect();
            int lenghtSender = pArray[2];
            int lenghtTarget = pArray[3];
            m.SenderName = pArray[4..(4 + lenghtSender)].ConvertToString();
            m.TargetName = pArray[(4 + lenghtSender + 1)..(4 + lenghtSender + 1 + lenghtTarget)].ConvertToString();
            m._data = pArray[(4 + lenghtSender + 1 + lenghtTarget + 1)..];

            return m;
        }
    }
}
