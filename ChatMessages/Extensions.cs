// Daniel Neubieser:WPFFIrstStepsChatMessagesExtensions.cs022021

using System.Text;

namespace ChatMessages
{
    public static class Extensions
    {
        public static byte[] ConvertToArray(this string str)
        {
            return Encoding.ASCII.GetBytes(str);
        }

        public static string ConvertToString(this byte[] pBytes)
        {
            return Encoding.ASCII.GetString(pBytes);
        }
    }
}
