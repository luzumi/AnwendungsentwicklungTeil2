// corat::WPFFIrstSteps.ChatMessages.MessageDirect.cs::022021

using System;
using System.ComponentModel.DataAnnotations;

namespace ChatMessages
{
    /// <summary>
    /// Direktnachricht an einen Client
    /// - erbt von <see cref="Message"></see>
    /// </summary>
    public class MessageDirect : Message
    {
        public byte[] _data;
        public string SenderName;
        public string TargetName;
        public Datatypes DataType;

        /// <summary>
        /// Direktnachricht an einen Client
        /// </summary>
        public MessageDirect()
        {
            MessageType = MessageTypes.DirectMessage;
        }

        /// <summary>
        /// Direktnachricht an einen Client
        /// </summary>
        /// <param name="Data"> zu sendender NachrichtenStream</param>
        public MessageDirect(byte[] Data)
        {
            MessageType = MessageTypes.DirectMessage;
            DataType = (Datatypes)Data[1];
            SenderName = Data[4..(Data[2] + 4)].ConvertToString();
            TargetName = Data[(4 + SenderName.Length)..(4 + SenderName.Length + Data[3])].ConvertToString();
            _data = Data[(4 + SenderName.Length + Data[3])..];
        }

        public override int GetSize()
        {
            return 4 + _data.Length;
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

        /// <summary>
        /// zusammensetzen eines empfangenen Pakets
        /// </summary>
        /// <param name="pArray"></param>
        /// <returns></returns>
        public static MessageDirect FromArray(byte[] pArray)
        {
            if (pArray is null || pArray.Length > 2) throw new ArgumentException("Error MessageDirect fromArray");

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
