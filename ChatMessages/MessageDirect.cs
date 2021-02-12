// corat::WPFFIrstSteps.ChatMessages.MessageDirect.cs::022021

using System;
using System.Text;

namespace ChatMessages
{
    public class MessageDirect : Message
    {
        public byte[] _data;
        public string SenderName;
        public string TargetName;
        public MessageDirect()
        {
            MessageType = MessageTypes.DirectMessage;
        }

        public override int GetSize()
        {
            return 2 + _data.Length;
        }

        public override byte[] ToArray()
        {
            byte[] result = new byte[this.GetSize()];
            result[0] = (byte)MessageType;
            result[1] = (byte)DataType;
            result[2] = (byte)SenderName.Length;
            result[3] = (byte)TargetName.Length;

            var sender = SenderName.ConvertToArray();
            Array.Copy(sender, 0, result, 4, sender.Length);
            
            var target = TargetName.ConvertToArray();
            Array.Copy(target, 0, result, 4 + sender.Length, target.Length);

            Array.Copy(_data, 0, result, 4 + sender.Length + target.Length, _data.Length);
            return result;
        }

        public Datatypes DataType;

        public static MessageDirect FromArray(byte[] pArray)
        {
            if (pArray is null || pArray.Length > 2) throw new ArgumentException();

            MessageDirect m = new MessageDirect();
            int lenghtSender = pArray[2];
            int lenghtTarget = pArray[3];
            m.SenderName = pArray[4..(4+ lenghtSender)].ConvertToString(); 
            m.TargetName = pArray[(4 + lenghtSender+1)..(4 + lenghtSender + 1 + lenghtTarget)].ConvertToString(); 
            m._data = pArray[(4 + lenghtSender + 1 + lenghtTarget+1)..];                       

            return m;
        }
    }
}
