using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;
using ChatMessages;

namespace ChatServerLogic
{
    /// <summary>
    /// Logic des ChatServers
    /// </summary>
    public class ChatServer
    {
        private readonly TcpListener _listener;
        private readonly LinkedList<ConnectionInfo> _connectionList;
        public List<Room> RoomList;

        private readonly Action<string> _onMessageReceived;
        readonly CancellationTokenSource ctsListener;
        readonly CancellationTokenSource ctsReader;

        public bool IsListenerRunning { get => _isListenerRunning; }
        private bool _isListenerRunning;

        public bool IsReaderRunning { get; } //TODO:???????

        /// <summary>
        /// Logic des Servers
        /// </summary>
        /// <param name="ReceiveMethod"> Methode die von einem Chatserver ausgeführt wird </param>
        public ChatServer(Action<string> ReceiveMethod)
        {
            _onMessageReceived = ReceiveMethod;
            _listener = new(System.Net.IPAddress.Any, 1337);
            ctsListener = new();
            ctsReader = new();
            _connectionList = new();
            RoomList = new();
            RoomList.Add(new Room("Lobby"));
        }

        /// <summary>
        /// startet einen Listener Asynchron
        /// </summary>
        public async void StartListenerAsync()
        {
            if (_isListenerRunning) return;

            _isListenerRunning = true;
            _onMessageReceived.Invoke("starte Listener");
            _listener.Start();

            while (!ctsListener.IsCancellationRequested)
            {
                TcpClient newClient = await Task.Run(() => _listener.AcceptTcpClient());
                ConnectionInfo connInfo = new();
                connInfo.ReaderTask = Task.Run(() => Receive(connInfo), ctsReader.Token);
                connInfo.Tcp = newClient;
                _connectionList.AddLast(connInfo);
                _onMessageReceived.Invoke($"Client {newClient.Client.RemoteEndPoint} verbindet sich");
                RoomList = new();
                RoomList.Add(new Room("Lobby"){RoomMember = _connectionList});
            }

            _onMessageReceived.Invoke("stoppe Listener");
            _listener.Stop();
            _isListenerRunning = false;
        }

        /// <summary>
        /// stoppt den Listener
        /// </summary>
        public void StopListener()
        {
            _onMessageReceived.Invoke("Listener wird beendet");
            ctsListener.Cancel();
        }

        /// <summary>
        /// stoppt den Reader
        /// </summary>
        public void StopReader()
        {
            _onMessageReceived.Invoke("Reader wird beendet");
            ctsReader.Cancel();
            Kick();
        }

        /// <summary>
        /// sendet Nachricht an alle Clients
        /// </summary>
        /// <param name="pMessage"> die zu sendende Nachricht</param>
        public void SendBroadcastMessage(string pMessage)
        {
            MessageBroadCast mbc = new();
            mbc.ContentType = DataType.Text;
            mbc.userName = "ServerMessage";
            mbc.Content = (mbc.userName + ": " + pMessage).ConvertToArray();
            foreach (var client in _connectionList)
            {
                if (client.Tcp.Connected) client.Tcp.GetStream().Write(mbc.ToArray(), 0, mbc.GetSize());
            }
        }

        /// <summary>
        /// eine Nachricht wird empfangen
        /// </summary>
        /// <param name="pClient"></param>
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
                    HandleIncomingMessage(pClient, data, recievedBytes);
                }
                catch (Exception)
                {
                    _onMessageReceived.Invoke("eine Exception wurde ausgelöst");
                    return;
                }

                if (recievedBytes < 1)
                {
                    _onMessageReceived.Invoke($"VerbindungsAbbruch von Client {pClient.ciUserName}");
                    break;
                }

                
            }

            pClient.Tcp.Close();
            _connectionList.Remove(pClient);
        }

        /// <summary>
        /// eingehende Nachricht wird gehandlet
        /// </summary>
        /// <param name="pClient"> Absender der Nachricht</param>
        /// <param name="data"> Datastream der zu empfangenden Nachricht</param>
        /// <param name="recievedBytes">Länge der Nachricht</param>
        private void HandleIncomingMessage(ConnectionInfo pClient, byte[] data, int recievedBytes)
        {
            switch ((MessageTypes) data[0])
            {
                case MessageTypes.Login:
                    var loginMessage = MessageLogin.FromArray(data[..recievedBytes]);
                    bool success = true;
                    foreach (var connectionInfo in _connectionList)
                    {
                        if (connectionInfo.ciUserName == loginMessage.userName)
                        {
                            //TODO: fehlerbehandlung
                            success = false;
                            break;
                        }
                    }

                    if (success)
                    {
                        MessageLoginStatus ls = new();
                        ls.loginState = MessageTypes.LoginSuccessfully;
                        pClient.ciUserName = loginMessage.userName;
                        pClient.Tcp.GetStream().Write(ls.ToArray());

                        MessageViewAllClients mulLogin = new();
                        foreach (var client in _connectionList)
                            mulLogin.UserList.Add(client.ciUserName);

                        var arr = mulLogin.ToArray();

                        foreach (var client in _connectionList)
                        {
                            client.Tcp.GetStream().Write(arr);
                        }
                    }
                    else
                    {
                        MessageLoginStatus ls = new();
                        ls.loginState = MessageTypes.LoginFail;
                        pClient.Tcp.GetStream().Write(ls.ToArray());
                    }

                    pClient.ciUserName = loginMessage.userName;
                    _onMessageReceived(loginMessage.userName);
                    break;

                case MessageTypes.Logout:
                    pClient.Tcp.Close();
                    break;

                case MessageTypes.Broadcast:
                    foreach (var client in _connectionList)
                    {
                        client.Tcp.GetStream().Write(data.AsSpan()[..recievedBytes]);
                    }

                    if ((Datatypes) data[1] == Datatypes.Text)
                        _onMessageReceived(new MessageBroadCast(data[..recievedBytes]).Content
                            .ConvertToString());
                    break;

                case MessageTypes.DirectMessage:
                    var messageDirect = MessageDirect.FromArray(data[..recievedBytes]);
                    foreach (var client in _connectionList)
                    {
                        if (client.ciUserName == messageDirect.TargetName)
                        {
                            client.Tcp.GetStream().Write(data.AsSpan()[0..recievedBytes]);
                            break;
                        }
                    }

                    if ((Datatypes) data[1] == Datatypes.Text)
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

                    MessageViewAllClients messageViewAllClients = new();

                    foreach (var ci in _connectionList)
                    {
                        messageViewAllClients.UserList.Add(ci.ciUserName);
                    }

                    pClient.Tcp.GetStream().Write(messageViewAllClients.ToArray());
                    break;

                default:
                    _onMessageReceived($"Client {pClient.ciUserName ?? "Unangemeldet"}" +
                                       $" an IP {pClient.Tcp.Client.RemoteEndPoint} hat unbekanntes Paket gesendet: " +
                                       $"{data[0]}  trenne Client");
                    MessageClientKick mk = new();
                    pClient.Tcp.GetStream().Write(mk.ToArray());
                    pClient.Tcp.Close();
                    break;
            }
        }

        public List<string> GetConnectionStatus()
        {
            List<string> resultList = new();
            resultList.Add("Status des Servers");
            resultList.Add($"Listener : {IsListenerRunning}");
            resultList.Add($"Reader : {IsReaderRunning}");

            foreach (var client in _connectionList)
            {
                resultList.Add($"Client: {client.ciUserName ?? "noUsername"} {client.Tcp.Connected} " +
                               $"IP: {client.Tcp.Client.RemoteEndPoint}");
            }

            return resultList;
        }


        public void Kick()
        {
            foreach (var client in _connectionList)
            {
                client.Tcp.Close();
                client.ReaderTask.Wait();
            }

            _connectionList.Clear();
        }
    }
}
