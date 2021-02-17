// corat::WPFFIrstSteps.ChatMessages.MessageViewAllClients.cs::022021

using System;
using System.Collections.Generic;
using Microsoft.VisualBasic;

namespace ChatMessages
{
    public class MessageViewAllClients : Message
    {
        public byte[] Data;
        public List<string> UserList;
        public string TargetName;

        public MessageViewAllClients()
        {
            MessageType = MessageTypes.ViewAllClients;
            UserList = new();
        }

        public MessageViewAllClients(byte[] pArray)
        {
            if (pArray == null)
            {
                throw new ArgumentNullException(nameof(pArray));
            }

            UserList = new(pArray[1..^1].ToString()?.Split(";") ?? Array.Empty<string>());
        }

        public override int GetSize()
        {
            int count = 1;
            foreach (var item in UserList)
            {
                count += item.Length + 1;
            }

            return count;
        }

        public override byte[] ToArray()
        {
            byte[] data = new byte[GetSize()];
            data[0] = (byte)MessageType;
            int pos = 1;
            for (int i = 0; i < UserList.Count; i++)
            {
                byte[] nameArray = UserList[i].ConvertToArray();
                Array.Copy(nameArray,0,data,pos,nameArray.Length);
                pos += nameArray.Length;
                data[pos] = ";".ConvertToArray()[0];
                pos++;
            }
            return data;
        }

        /// <summary>
        /// zusammensetzen eines empfangenen Pakets
        /// </summary>
        /// <param name="pArray"></param>
        /// <returns></returns>
        public static Message FromArray(byte[] pArray)
        {
            if (pArray is null || pArray.Length > 2) throw new ArgumentException("Error MessageViewAllClients fromArray");

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
