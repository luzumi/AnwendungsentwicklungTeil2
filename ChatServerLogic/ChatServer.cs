using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;
using ChatMessages;

namespace ChatServerLogic
{
    public class ChatServer
    {
        private readonly TcpListener _listener;
        private readonly LinkedList<ConnectionInfo> _connectionList;
        private List<Room> _roomList;

        private readonly Action<string> _onMessageReceived;
        readonly CancellationTokenSource ctsListener;
        readonly CancellationTokenSource ctsReader;

        public bool IsListenerRunning { get => _isListenerRunning; }
        private bool _isListenerRunning;

        public bool IsReaderRunning { get; } //TODO:???????


        public ChatServer(Action<string> ReceiveMethod)
        {
            _onMessageReceived = ReceiveMethod;
            _listener = new(System.Net.IPAddress.Any, 1337);
            ctsListener = new();
            ctsReader = new();
            _connectionList = new();
            _roomList = new();
            _roomList.Add(new Room("Lobby"));
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
                ConnectionInfo connInfo = new();
                connInfo.ReaderTask = Task.Run(() => Receive(connInfo), ctsReader.Token);
                connInfo.Tcp = newClient;
                _connectionList.AddLast(connInfo);
                _onMessageReceived.Invoke($"Client {newClient.Client.RemoteEndPoint} verbindet sich");
                _roomList[0]._roomMember.AddFirst(connInfo);
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

        public void SendMessage(string pMessage)
        {
            MessageBroadCast mbc = new("ServerMessage");
            mbc.DataType = Datatypes.Text;
            mbc.Data = (mbc.Sender + ": " + pMessage).ConvertToArray();
            foreach (var client in _connectionList)
            {
                if (client.Tcp.Connected) client.Tcp.GetStream().Write(mbc.ToArray(), 0, mbc.GetSize());
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
                    _onMessageReceived.Invoke($"VerbindungsAbbruch von Client {pClient.ciUserName}");
                    break;
                }

                switch ((MessageTypes)data[0])
                {
                    case MessageTypes.Login:
                        var loginMessage = MessageLogin.FromArray(data[..recievedBytes]);
                        bool success = true;
                        foreach (var connectionInfo in _connectionList)
                        {
                            if (connectionInfo.ciUserName == loginMessage.userName)
                            {
                                //TODO: fehlerbehandlung
                                MessageDirect m = new MessageDirect();
                                success = false;
                                break;
                            }
                        }

                        if (success)
                        {
                            MessageLoginStatus ls = new();
                            ls.loginState = LoginStates.LoginSuccessfully;
                        }
                        else
                        {
                            MessageLoginStatus ls = new();
                            ls.loginState = LoginStates.LoginFail;
                        }

                        pClient.ciUserName = loginMessage.userName;
                        _onMessageReceived(loginMessage.userName);
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
                            if (client.ciUserName == messageDirect.TargetName)
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

                        MessageViewAllClients md = new();
                        md.TargetName = data[3..(data[2])].ConvertToString();
                        md.SenderName = "UserList";

                        ConnectionInfo[] ci = new ConnectionInfo[_connectionList.Count];

                        byte[] listBytes = new byte[32];
                        int startIndex = 5;

                        for (int i = 0; i < _connectionList.Count; i++)
                        {
                            _connectionList.CopyTo(ci, i);
                            listBytes = (Environment.NewLine + ci[i].ciUserName).ConvertToArray() ;
                            startIndex += listBytes.Length;
                            Array.Copy(listBytes, 0, md._data, startIndex, listBytes.Length);
                        }
                        
                        foreach (var client in _connectionList)
                        {
                            if (client.ciUserName == md.TargetName)
                            {
                                client.Tcp.GetStream().Write(md._data, 0, md.GetSize());
                                break;
                            }
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(pClient));
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
            }

            _connectionList.Clear();
        }
    }
}
