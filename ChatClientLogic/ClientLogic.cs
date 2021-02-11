using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatClientLogic
{
    public class ClientLogic
    {
        private TcpClient connection;
        private readonly Action<string> onNewMessage;
        CancellationTokenSource cts;
        public Action OnConnectionStatus;

        public bool IsConnected => connection != null && connection.Connected;
        public ClientLogic(Action<string> onNewMessage) => this.onNewMessage = onNewMessage;

        public bool Start()
        {
            connection = new TcpClient();

            try
            {
                connection.Connect("127.0.0.1", 1337);
            }
            catch (Exception)
            {
                return false;
            }

            cts = new();
            _ = Task.Run(Receive, cts.Token);
            return true;
        }

        public void SendMessage(string Message)
        {
            if (connection == null || !connection.Connected) return;
            byte[] data;
            data = Encoding.ASCII.GetBytes(Message);
            connection.GetStream().Write(data, 0, data.Length);
        }

        public void Stop()
        {
            cts.Cancel();
            connection?.Close();
        }

        private async void Receive()
        {
            string message;
            byte[] data = new byte[1024];
            int receivedBytes;

            while (true)
            {
                try
                {
                    receivedBytes = await connection.GetStream().ReadAsync(data.AsMemory(0, data.Length), cts.Token);
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
                message = Encoding.ASCII.GetString(data, 0, receivedBytes);
                onNewMessage.Invoke(message);
            }

            connection.Close();
            OnConnectionStatus?.Invoke();
        }
    }
}
