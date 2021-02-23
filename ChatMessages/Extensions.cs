// Daniel Neubieser:WPFFIrstStepsChatMessagesExtensions.cs022021

using System.Text;

namespace ChatMessages
{
    /// <summary>
    /// statische Methoden zum handlen der Nachrichten
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Wandelt einen Text in ein ByteArray um
        /// </summary>
        /// <param name="str"> Der zu sendende Text</param>
        /// <returns> ByteArray zum </returns>
        public static byte[] ConvertToArray(this string str)
        {
            return Encoding.ASCII.GetBytes(str);
        }

        /// <summary>
        /// Wandelt ein ByteArray in einen Text um
        /// </summary>
        /// <param name="pBytes"> Empfangener Datenstream</param>
        /// <returns>Datenstream in String gewandelt</returns>
        public static string ConvertToString(this byte[] pBytes)
        {
            return Encoding.ASCII.GetString(pBytes);
        }
    }
}
