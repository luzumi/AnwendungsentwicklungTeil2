// corat::WPFFIrstSteps.ChatServerLogic.ConnectionInfo.cs::022021

using System.Net.Sockets;
using System.Threading.Tasks;

namespace ChatServerLogic
{
    class ConnectionInfo
    {
        public TcpClient Tcp;
        public string UserName;
        public Task ReaderTask;
    }
}
