// corat::WPFFIrstSteps.ChatMessages.MessageViewAllClients.cs::022021

using System;
using System.Collections.Generic;
using Microsoft.VisualBasic;

namespace ChatMessages
{
    /// <summary>
    /// Fordert eine Übersicht der verbundenen User an
    /// - erbt von <see cref="Message"></see>
    /// </summary>
    public class MessageViewAllClients : Message
    {
        public byte[] Data;
        public List<string> UserList;
        public string TargetName;

        /// <summary>
        /// Fordert eine Übersicht der verbundenen User an
        /// </summary>
        public MessageViewAllClients()
        {
            MessageType = MessageTypes.ViewAllClients;
            UserList = new();
        }

        /// <summary>
        /// Fordert eine Übersicht der verbundenen User an
        /// </summary>
        /// <param name="pArray"> DatenStream</param>
        /// <exception cref="ArgumentNullException"> DatenStream Ist <c>null</c> </exception>
        public MessageViewAllClients(byte[] pArray)
        {
            if (pArray == null)
            {
                throw new ArgumentNullException(nameof(pArray));
            }

            UserList = new(pArray[1..^1].ToString()?.Split(";") ?? Array.Empty<string>());
        }

        /// <summary>
        /// Zählt die Anzahl der verbundenen User
        /// </summary>
        /// <returns> VerbundeneUserAnzahl</returns>
        public override int GetSize()
        {
            int count = 1;
            foreach (var item in UserList)
            {
                count += item.Length + 1;
            }

            return count;
        }

        /// <summary>
        /// Wandelt Nachricht in NachrichtenStream um
        /// </summary>
        /// <returns>NachrichtenStream</returns>
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
        /// <param name="pArray"> NachrichtenStream</param>
        /// <returns> gibt eine Direktnachricht zurück</returns>
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
