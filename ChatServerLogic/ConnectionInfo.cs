// corat::WPFFIrstSteps.ChatServerLogic.ConnectionInfo.cs::022021

using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace ChatServerLogic
{
    /// <summary>
    /// erweiterung der TcpClientConnection
    /// </summary>
    public class ConnectionInfo
    {
        public TcpClient Tcp;
        public string ciUserName;
        public Task ReaderTask;
        public Room Room;
    }

    /// <summary>
    /// ein ChatRoom
    /// </summary>
    public class Room
    {
        public string RoomName;
        public LinkedList<ConnectionInfo> RoomMember;

        public Room(string pRoomName)
        {
            RoomName = pRoomName;
        }
    }
}
