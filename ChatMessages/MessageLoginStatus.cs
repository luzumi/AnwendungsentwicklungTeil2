// Daniel Neubieser:WPFFIrstStepsChatMessagesMessageLoginSuccessfully.cs022021

using System;

namespace ChatMessages
{
    /// <summary>
    /// Anfrage des Status eines Clients
    /// - erbt von <see cref="Message"></see>
    /// </summary>
    public class MessageLoginStatus : Message
    {

        public MessageTypes loginState;

        /// <summary>
        /// Anfrage des Status eines Client
        /// </summary>
        public MessageLoginStatus()
        {
            
        }

        public override int GetSize()
        {
            return 2;
        }

        public override byte[] ToArray()
        {
            byte[] data = new byte[2];
            data[0] = (byte)MessageType;
            data[1] = (byte)loginState;

            return data;
        }

        /// <summary>
        /// zusammensetzen eines empfangenen Pakets
        /// </summary>
        /// <param name="pArray"></param>
        /// <returns> Status der Client-Connection</returns>
        public static MessageLoginStatus FromArray(byte[] pArray)
        {
            if (pArray is null || pArray.Length != 1) throw new ArgumentException("Error MLS 33");
            MessageLoginStatus m = new();
            m.MessageType = (MessageTypes)pArray[0];
            m.loginState = (MessageTypes)pArray[1];
            
            return m;
        }
    }
}
