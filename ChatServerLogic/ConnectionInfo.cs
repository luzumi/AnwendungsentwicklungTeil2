// corat::WPFFIrstSteps.ChatServerLogic.ConnectionInfo.cs::022021

using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace ChatServerLogic
{
    public class ConnectionInfo
    {
        public TcpClient Tcp;
        public string ciUserName;
        public Task ReaderTask;
        public Room Room;
    }

    public class Room
    {
        public string _RoomName;
        public LinkedList<ConnectionInfo> _roomMember;

        public Room(string pRoomName)
        {
            _RoomName = pRoomName;
        }
    }
}
