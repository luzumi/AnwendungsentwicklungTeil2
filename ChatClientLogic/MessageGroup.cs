// corat::WPFFIrstSteps.ChatClientLogic.MessageGroup.cs::022021

using System;
using ChatMessages;

namespace ChatClientLogic
{
    public class MessageGroup : Message
    {
        string RoomID;
        public string SenderName;
        public DataType ContentType;
        public byte[] _data;

        public MessageGroup()
        {
            MessageType = MessageTypes.GroupMessage;
            RoomID = string.Empty;
            SenderName = string.Empty;
        }

        public MessageGroup(byte[] Data)
        {
            MessageType = MessageTypes.GroupMessage;
            ContentType = (DataType)Data[1];
            RoomID = "";
            SenderName = Data[(3 + RoomID.Length)..(3 + RoomID.Length + Data[2])].ConvertToString();
            _data = Data[(4 + RoomID.Length + SenderName.Length)..];
        }

        public override int GetSize()
        {
            return 4 + RoomID.Length + SenderName.Length + _data.Length;
        }

        public override byte[] ToArray()
        {
            byte[] data = new byte[GetSize()];
            data[0] = (byte)MessageType;
            data[1] = (byte)ContentType;
            data[2] = (byte)RoomID.Length;
            data[3] = (byte)SenderName.Length;
            byte[] buffer = RoomID.ConvertToArray();
            Array.Copy(buffer, 0, data, 4, buffer.Length);
            buffer = SenderName.ConvertToArray();
            Array.Copy(buffer, 0, data, 4 + RoomID.Length, buffer.Length);
            Array.Copy(_data, 0, data, 4 + RoomID.Length + buffer.Length, _data.Length);
            return data;
        }
    }
}
