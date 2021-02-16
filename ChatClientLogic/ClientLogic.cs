using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChatMessages;

namespace ChatClientLogic
{
    public class ClientLogic
    {
        private TcpClient _connection;
        private readonly Action<string> _onNewMessage;
        CancellationTokenSource _cts;
        public Action OnConnectionStatus;

        public bool IsConnected => _connection != null && _connection.Connected;
        public ClientLogic(Action<string> onNewMessage) => this._onNewMessage = onNewMessage;

        public bool Start(string pUserName)
        {
            _connection = new TcpClient();

            try
            {
                _connection.Connect("127.0.0.1", 1337);
            }
            catch (Exception)
            {
                return false;
            }


            MessageLogin m = new MessageLogin(pUserName);
            _connection.GetStream().Write(m.ToArray());

            _cts = new();
            _ = Task.Run(Receive, _cts.Token);
            return true;
        }

        public void SendMessage(string pMessage)
        {
            if (_connection == null || !_connection.Connected) return;
            MessageBroadCast mbc = new(pSender:"test");
            mbc.DataType = Datatypes.Text;
            mbc.Data = pMessage.ConvertToArray();

            _connection.GetStream().Write(mbc.ToArray(), 0, mbc.GetSize());
        }

        public void Stop()
        {
            _cts.Cancel();
            _connection?.Close();
        }

        private async void Receive()
        {
            byte[] data = new byte[1024];
            int receivedBytes;

            while (true)
            {
                try
                {
                    receivedBytes = await _connection.GetStream().ReadAsync(data.AsMemory(0, data.Length), _cts.Token);
                }
                catch (Exception)
                {
                    // wenn fehler bei der übertragung stattfinden (server down, netzwerk down)
                    break;
                }

                if (receivedBytes < 1)
                {
                    // server hat verbindung regulär getrennt
                    break;
                }

                // nachricht empfangen
                var message = MessageBroadCast.FromArray(data[..receivedBytes]);
                _onNewMessage.Invoke(message.Data.ConvertToString());
            }

            _connection.Close();
            OnConnectionStatus?.Invoke();
        }
    }
}
