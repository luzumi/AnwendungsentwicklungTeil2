// Daniel Neubieser:WPFFIrstStepsChatMessagesMessageLogOut.cs022021

namespace ChatMessages
{
    /// <summary>
    /// LogOutMessage
    /// - erbt von <see cref="MessageLoginStatus"></see>
    /// </summary>
    class MessageLogOut : MessageLoginStatus
    {
        /// <summary>
        /// LogOutMessage
        /// </summary>
        public MessageLogOut()
        {
            MessageType = MessageTypes.Logout;
        }
    }
}
