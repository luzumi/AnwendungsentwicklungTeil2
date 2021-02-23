// corat::WPFFIrstSteps.ChatMessages.MessageClientAdd.cs::022021

namespace ChatMessages
{
    /// <summary>
    /// Ein neuer Client wird hinzugefügt
    /// - erbt von <see cref="Message"></see>
    /// </summary>
    class MessageClientAdd : Message
    {
        /// <summary>
        /// Ein neuer Client wird hinzugefügt
        /// </summary>
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
        
    }
}
