// Daniel Neubieser:WPFFIrstStepsChatMessagesMessageLogin.cs022021

using System;
using System.Text;

namespace ChatMessages
{
    class MessageLogin : Message
    {
        public MessageLogin()
        {
            MessageType = MessageTypes.Login;
        }

        private const int passwordLenght = 32;
        public string _userName;
        public byte[] _passwort;
        public override int GetSize()
        {
            return 1 + _passwort.Length + (_userName.Length * 2);
        }

        public override byte[] ToArray()
        {
            byte[] data = new byte[this.GetSize()];
            data[0] = (byte)MessageType;
            Array.Copy(_passwort, 0, data, 1, _passwort.Length);
            var nameArray = _userName.ConvertToArray();
            Array.Copy(nameArray, 0, data, 1 + _passwort.Length, nameArray.Length);
            return data;
        }

        public static MessageLogin FromArray(byte[] pData)
        {
            if (pData is null || pData.Length != 1) throw new ArgumentException();
            MessageLogin m = new();
            m.MessageType = (MessageTypes)pData[0];
            m._passwort = pData[0..passwordLenght];
            m._userName = pData[34..].ConvertToString();
            return m;
        }
    }
}
