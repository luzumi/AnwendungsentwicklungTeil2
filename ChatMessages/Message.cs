// Daniel Neubieser:WPFFIrstStepsChatMessagesMessage.cs022021

namespace ChatMessages
{
    public abstract class Message
    {
        public MessageTypes MessageType;
        public string userName;

        public abstract int GetSize();

        /// <summary>
        /// zusammensetzen eines zu sendenes paketes
        /// </summary>
        /// <returns></returns>
        public abstract byte[] ToArray();
        
    }
}
