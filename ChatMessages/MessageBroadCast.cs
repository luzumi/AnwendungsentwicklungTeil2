// corat::WPFFIrstSteps.ChatMessages.MessageBroadCast.cs::022021

using System;
using System.Text;

namespace ChatMessages
{
    /// <summary>
    /// Nachricht an alle Clients
    /// </summary>
    public class MessageBroadCast : Message
    {
        public DataType ContentType;
        public byte[] Content;

        /// <summary>
        /// Nachricht an alle Clients
        /// </summary>
        public MessageBroadCast()
        {
            MessageType = MessageTypes.Broadcast;
        }


        public override int GetSize()
        {
            return 3 + userName.Length + Content.Length;
        }


        public override byte[] ToArray()
        {
            byte[] data = new byte[GetSize()];
            data[0] = (byte)MessageType;
            data[1] = (byte)ContentType;
            data[2] = (byte)userName.Length;
            var arr = userName.ConvertToArray();
            Array.Copy(arr, 0, data, 3, arr.Length);
            Array.Copy(Content, 0, data, 3 + arr.Length, Content.Length);
            return data;
        }


        /// <summary>
        /// zusammensetzen eines empfangenen Pakets
        /// </summary>
        /// <param name="pArray"></param>
        /// <returns></returns>
        public MessageBroadCast(byte[] pArray)
        {
            if (pArray is null || pArray.Length < 3) throw new ArgumentException("Error MBC 040");
            MessageType = (MessageTypes)pArray[0];
            ContentType = (DataType)pArray[1];
            int lengthSender = pArray[2];
            userName = pArray[3..(3 + lengthSender)].ConvertToString();
            Content = pArray[(3 + lengthSender)..];
        }
    }
}
