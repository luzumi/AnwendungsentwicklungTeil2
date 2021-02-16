using System.ComponentModel.DataAnnotations;
using System.IO;
using Microsoft.VisualBasic;

namespace ChatMessages
{
    class MessageClientAdd : Message
    {
        public MessageClientAdd()
        {
            MessageType = MessageTypes.ClientAdd;
        }

        public override int GetSize()
        {
            throw new System.NotImplementedException();
        }

        public override byte[] ToArray()
        {
            throw new System.NotImplementedException();
        }

        public static Message FromArray(byte[] pArray)
        {
            throw new System.NotImplementedException();
        }
    }

    class MessageClientRemove : Message
    {
        public MessageClientRemove()
        {
            MessageType = MessageTypes.ClientRemove;
        }

        public override int GetSize()
        {
            throw new System.NotImplementedException();
        }

        public override byte[] ToArray()
        {
            throw new System.NotImplementedException();
        }

        public static Message FromArray(byte[] pArray)
        {
            throw new System.NotImplementedException();
        }
    }

    class MessageViewRequest : Message
    {
        public MessageViewRequest()
        {
            MessageType = MessageTypes.ViewRequest;
        }

        public override int GetSize()
        {
            throw new System.NotImplementedException();
        }

        public override byte[] ToArray()
        {
            throw new System.NotImplementedException();
        }

        public static Message FromArray(byte[] pArray)
        {
            throw new System.NotImplementedException();
        }
    }
}
