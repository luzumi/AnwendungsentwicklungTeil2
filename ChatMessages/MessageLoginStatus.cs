// Daniel Neubieser:WPFFIrstStepsChatMessagesMessageLoginSuccessfully.cs022021

using System;

namespace ChatMessages
{
    public class MessageLoginStatus : Message
    {
        const int SessionIdLenght = 4;
        private byte[] _sessionId = new byte[SessionIdLenght];
        public LoginStates loginState;

        public MessageLoginStatus()
        {
            
        }

        public override int GetSize()
        {
            return 2;
        }

        public override byte[] ToArray()
        {
            byte[] data = new byte[1];
            data[0] = (byte)MessageType;
            data[1] = (byte)loginState;

            return data;
        }

        public static MessageLoginStatus FromArray(byte[] pArray)
        {
            if (pArray is null || pArray.Length != 1) throw new ArgumentException("Error MLS 33");
            MessageLoginStatus m = new();
            m.MessageType = (MessageTypes)pArray[0];
            m.loginState = (LoginStates)pArray[1];
            
            return m;
        }
    }
}
