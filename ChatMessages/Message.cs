// Daniel Neubieser:WPFFIrstStepsChatMessagesMessage.cs022021

namespace ChatMessages
{
    public abstract class Message
    {
        public MessageTypes MessageType;

        public abstract int GetSize();
        public abstract byte[] ToArray();
        
    }
}
