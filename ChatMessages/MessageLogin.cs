// Daniel Neubieser:WPFFIrstStepsChatMessagesMessageLogin.cs022021

using System;
using System.Text;

namespace ChatMessages
{
    public class MessageLogin : Message
    {
        public byte[] Password;
        public string UserName;
        public MessageLogin(string pUserName)
        {
            MessageType = MessageTypes.Login;
            userName = pUserName;
        }

        //private const int passwordLength = 32;
        public byte[] _password = {1, 2, 3};
        public byte[] Data;

        public override int GetSize()
        {
            return 3 + _password.Length + userName.Length;
        }

        public override byte[] ToArray()
        {
            Data = new byte[this.GetSize()];
            Data[0] = (byte)MessageType;
            Data[1] = (byte)userName.Length;
            Data[2] = (byte)_password.Length;
            var nameArray = userName.ConvertToArray();
            Array.Copy(nameArray, 0, Data, 3, nameArray.Length);
            Array.Copy(_password, 0, Data, 3 + nameArray.Length, _password.Length);
            return Data;
        }

        /// <summary>
        /// zusammensetzen eines empfangenen Pakets
        /// </summary>
        /// <param name="pArray"></param>
        /// <returns></returns>
        public static MessageLogin FromArray(byte[] pData)
        {
            if (pData is null || pData.Length < 3) throw new ArgumentException("Error MessageLogin fromArray");
            MessageLogin m = new((pData[3.. (3 + pData[1])]).ConvertToString());
            m._password = pData[(3 + pData[1]) .. ];
            return m;
        }
    }
}
