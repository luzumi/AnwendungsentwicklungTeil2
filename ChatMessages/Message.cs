// Daniel Neubieser:WPFFIrstStepsChatMessagesMessage.cs022021

namespace ChatMessages
{
    /// <summary>
    /// BasisKlasse aller NachrichtenTypen
    /// </summary>
    public abstract class Message
    {
        public MessageTypes MessageType;
        public string userName;

        /// <summary>
        /// Nachrichtenlänge wird berechnet
        /// </summary>
        /// <returns> Länge der Nachricht</returns>
        public abstract int GetSize();

        /// <summary>
        /// zusammensetzen eines zu sendenes paketes
        /// </summary>
        /// <returns></returns>
        public abstract byte[] ToArray();
        
    }
}
