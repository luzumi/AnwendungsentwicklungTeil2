using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Text;
using System.Threading;

namespace ChatServerLogic
{
    public class ChatServer
    {
        private readonly TcpListener listener;
        private TcpClient connection;
        private readonly LinkedList<TcpClient> _connectionList;


        private readonly Action<string> onMessageReceived;
        CancellationTokenSource ctsListener;
        CancellationTokenSource ctsReader;

        public bool IsListenerRunning { get => _isListenerRunning; }
        private bool _isListenerRunning;

        public bool IsReaderRunning { get => _isReaderRunning; }
        private bool _isReaderRunning;


        public ChatServer(Action<string> RecieveMethod)
        {
            onMessageReceived = RecieveMethod;
            listener = new(System.Net.IPAddress.Any, 1337);
            ctsListener = new();
            ctsReader = new();
            _connectionList = new();
        }

        public async void StartListenerAsync()
        {
            if (_isListenerRunning) return;

            _isListenerRunning = true;
            onMessageReceived.Invoke("starte Listener");
            listener.Start();

            while (!ctsListener.IsCancellationRequested)
            {
                TcpClient newClient = await Task.Run(() => listener.AcceptTcpClient());
                _ = Task.Run(() => Receive(newClient), ctsReader.Token);
                _connectionList.AddLast(newClient);
                onMessageReceived.Invoke("Client verbindet sich");
            }

            onMessageReceived.Invoke("stoppe Listener");
            listener.Stop();
        }

        public void StopListener()
        {
            onMessageReceived.Invoke("Listener wird beendet");
            ctsListener.Cancel();
        }

        public void StopReader()
        {
            onMessageReceived.Invoke("Reader wird beendet");
            ctsReader.Cancel();
        }


        public void SendMessage(string Message)
        {
            var data = Encoding.ASCII.GetBytes(Message);
            foreach (TcpClient client in _connectionList)
            {
                if (client.Connected) client.GetStream().Write(data, 0, data.Length);
            }
        }

        private async void Receive(TcpClient pClient)
        {
            string message;
            byte[] data = new byte[1024];
            int recievedBytes = 0;

            while (true)
            {
                try
                {
                    recievedBytes =
                        await pClient.GetStream().ReadAsync(data.AsMemory(0, data.Length), ctsListener.Token);
                    message = Encoding.ASCII.GetString(data, 0, recievedBytes);
                    onMessageReceived.Invoke(message);
                    SendMessage(message);
                }
                catch (Exception)
                {
                    onMessageReceived.Invoke("eine Exception wurde ausgelöst");
                    return;
                }

                if (recievedBytes < 1)
                {
                    onMessageReceived.Invoke("Weniger als 1 byte empfangen, beende verbindung");
                    break;
                }
            }

            pClient.Close();
            _connectionList.Remove(pClient);
        }

        public List<string> GetConnectionStatus()
        {
            List<string> resultList = new();
            resultList.Add("Status des Servers");
            resultList.Add($"Listener : {IsListenerRunning}");
            resultList.Add($"Reader : {IsReaderRunning}");

            int counter = 0;
            foreach (TcpClient client in _connectionList)
            {
                resultList.Add($"Client: {counter++} {client.Connected} IP: {client.Client.RemoteEndPoint}");
            }

            return resultList;
        }


        public void Kick()
        {
            foreach (TcpClient client in _connectionList)
            {
                client.Close();
            }
            _connectionList.Clear();
        }
    }
}
