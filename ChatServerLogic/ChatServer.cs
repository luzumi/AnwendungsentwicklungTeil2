using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Text;
using System.Threading;
using ChatMessages;

namespace ChatServerLogic
{
    public class ChatServer
    {
        private readonly TcpListener _listener;
        private readonly LinkedList<ConnectionInfo> _connectionList;


        private readonly Action<string> _onMessageReceived;
        readonly CancellationTokenSource ctsListener;
        readonly CancellationTokenSource ctsReader;

        public bool IsListenerRunning { get => _isListenerRunning; }
        private bool _isListenerRunning;

        public bool IsReaderRunning { get; } //TODO:???????


        public ChatServer(Action<string> RecieveMethod)
        {
            _onMessageReceived = RecieveMethod;
            _listener = new(System.Net.IPAddress.Any, 1337);
            ctsListener = new();
            ctsReader = new();
            _connectionList = new();
        }

        public async void StartListenerAsync()
        {
            if (_isListenerRunning) return;

            _isListenerRunning = true;
            _onMessageReceived.Invoke("starte Listener");
            _listener.Start();

            while (!ctsListener.IsCancellationRequested)
            {
                TcpClient newClient = await Task.Run(() => _listener.AcceptTcpClient());
                ConnectionInfo ci = new();
                ci.ReaderTask = Task.Run(() => Receive(ci), ctsReader.Token);
                ci.Tcp = newClient;
                ci.UserName = "afdkag";     //TODO Namen einbauen
                _connectionList.AddLast(ci);
                _onMessageReceived.Invoke("Client verbindet sich");
            }

            _onMessageReceived.Invoke("stoppe Listener");
            _listener.Stop();
            _isListenerRunning = false;
        }

        public void StopListener()
        {
            _onMessageReceived.Invoke("Listener wird beendet");
            ctsListener.Cancel();
        }

        public void StopReader()
        {
            _onMessageReceived.Invoke("Reader wird beendet");
            ctsReader.Cancel();
            Kick();
        }

        public void SendMessage(string Message)
        {
            var data = Message.ConvertToArray();
            foreach (var client in _connectionList)
            {
                if (client.Tcp.Connected) client.Tcp.GetStream().Write(data, 0, data.Length);
            }
        }

        private async void Receive(ConnectionInfo pClient)
        {
            byte[] data = new byte[1024];
            int recievedBytes = 0;

            while (true)
            {
                try
                {
                    recievedBytes =
                        await pClient.Tcp.GetStream().ReadAsync(data.AsMemory(0, data.Length), ctsListener.Token);
                }
                catch (Exception)
                {
                    _onMessageReceived.Invoke("eine Exception wurde ausgelöst");
                    return;
                }

                if (recievedBytes < 1)
                {
                    _onMessageReceived.Invoke("Weniger als 1 byte empfangen, beende verbindung");
                    break;
                }

                switch ((MessageTypes)data[0])
                {
                    case MessageTypes.Login:
                        break;
                    case MessageTypes.Logout:
                        break;
                    case MessageTypes.Broadcast:
                        foreach (var client in _connectionList)
                        {
                            client.Tcp.GetStream().Write(data, 0, recievedBytes);
                        }

                        if ((Datatypes)data[1] == Datatypes.Text)
                            _onMessageReceived(MessageBroadCast.FromArray(data[..recievedBytes]).Data
                                .ConvertToString());
                        break;
                    case MessageTypes.DirectMessage:
                        var messageDirect = MessageDirect.FromArray(data[..recievedBytes]);
                        foreach (var client in _connectionList)
                        {
                            if (client.UserName == messageDirect.TargetName)
                            {
                                client.Tcp.GetStream().Write(data, 0, recievedBytes);
                                break;
                            }
                        }
                        if ((Datatypes)data[1] == Datatypes.Text)
                            _onMessageReceived(messageDirect._data.ConvertToString());
                        break;
                    case MessageTypes.JoinGroup:
                        break;
                    case MessageTypes.LeaveGroup:
                        break;
                    case MessageTypes.GroupMessage:
                        break;
                    case MessageTypes.ClientAdd:
                        break;
                    case MessageTypes.ClientRemove:
                        break;
                    case MessageTypes.ViewRequest:
                        break;
                    case MessageTypes.ViewAllClients:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            pClient.Tcp.Close();
            _connectionList.Remove(pClient);
        }

        public List<string> GetConnectionStatus()
        {
            List<string> resultList = new();
            resultList.Add("Status des Servers");
            resultList.Add($"Listener : {IsListenerRunning}");
            resultList.Add($"Reader : {IsReaderRunning}");

            int counter = 0;
            foreach (var client in _connectionList)
            {
                resultList.Add($"Client: {counter++} {client.Tcp.Connected} " +
                               $"IP: {client.Tcp.Client.RemoteEndPoint}");
            }

            return resultList;
        }


        public void Kick()
        {
            foreach (var client in _connectionList)
            {
                client.Tcp.Close();
            }

            _connectionList.Clear();
        }
    }
}
