using System.ComponentModel.DataAnnotations;
using System.IO;
using Microsoft.VisualBasic;

namespace ChatMessages
{
    /// <summary>
    /// Ein Client wird vom Server geworfen
    /// - erbt von <see cref="Message"></see>
    /// </summary>
    public class MessageClientKick : Message
    {
        /// <summary>
        /// Ein Client wird vom Server geworfen
        /// </summary>
        public MessageClientKick()
        {
            MessageType = MessageTypes.ClientRemove;
        }

        public override int GetSize()
        {
            return 1;
        }

        public override byte[] ToArray()
        {
            byte[] data = new byte[1];
            data[0] = (byte)MessageType;
            return data;
        }
    }
}
