using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using ChatMessages;

namespace ChatClientLogic
{
    public class ClientLogic
    {
        private TcpClient _connection;
        private readonly Action<string> _onNewMessage;
        private readonly Action<List<string>> onNewUserList;
        CancellationTokenSource _cts;
        public Action OnConnectionStatus;

        public bool IsConnected => _connection != null && _connection.Connected;
        public bool IsLoggedIn { get; set; }


        public ClientLogic(Action<string> onNewMessage, Action<List<string>> pOnNewUserList)
        {
            this._onNewMessage = onNewMessage;
            this.onNewUserList = pOnNewUserList;
        }


        public bool Start(string pUserName, byte[] pPassword)
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

            _cts = new();
            _ = Task.Run(Receive, _cts.Token);
            MessageLogin ml = new(pUserName);

            using (MD5 hash = MD5.Create())
            {
                ml.Password = hash.ComputeHash(pPassword);
            }

            ml.UserName = pUserName;

            _connection.GetStream().Write(ml.ToArray());
            return true;
        }

        /// <summary>
        /// Sendet Nachricht an alle Clients weiter
        /// </summary>
        /// <param name="pMessage"> zu sendende Nachricht </param>
        /// <param name="pUserName"> Name des sendenden Users</param>
        public void SendMessage(string pMessage, string pUserName)
        {
            if (_connection == null || !_connection.Connected) return;

            MessageBroadCast mbc = new MessageBroadCast()
            {
                ContentType = DataType.Text, userName = pUserName, Content = pMessage.ConvertToArray()
            };

            _connection.GetStream().Write(mbc.ToArray() /*, 0, mbc.GetSize()*/);
        }

        /// <summary>
        /// Trennt Verbind des Clients vom Server
        /// </summary>
        public void Stop()
        {
            _cts.Cancel();
            _connection?.Close();
        }

        /// <summary>
        /// empfängt Nachricht und handelt diese
        /// </summary>
        private async void Receive()
        {
            
            byte[] data = new byte[1024];
            int receivedBytes;

            while (_connection.Connected)
            {
                try
                {
                    receivedBytes = await _connection.GetStream().ReadAsync(data.AsMemory(0, data.Length), _cts.Token);
                }
                catch (Exception)
                {
                    // wenn fehler bei der übertragung stattfinden (server down, netzwerk down)
                    _connection.Close();
                    break;
                }

                if (receivedBytes < 1)
                {
                    // server hat verbindung regulär getrennt
                    _connection.Close();
                    break;
                }
                else
                {
                    // nachricht empfangen
                    HandleMessage(data, receivedBytes);
                }
            }

            OnConnectionStatus?.Invoke();
        }

        /// <summary>
        /// Prüft empfangene Nachricht auf NachrichtenTyp und handelt diese
        /// </summary>
        /// <param name="data"> NachrichtenStream </param>
        /// <param name="receivedBytes"> Länge des NachrichtenStreams </param>
        private void HandleMessage(byte[] data, int receivedBytes)
        {
            switch ((MessageTypes) data[0])
            {
                case MessageTypes.Login:
                    IsLoggedIn = true;
                    _onNewMessage.Invoke("Server: Login erfolgreich");
                    break;
                case MessageTypes.LoginFail:
                    IsLoggedIn = false;
                    _onNewMessage.Invoke("Server: Login abgelehnt"); //todo grund angeben
                    break;
                case MessageTypes.KickFromServer:
                    IsLoggedIn = false;
                    _connection.Close();
                    _onNewMessage.Invoke("Server: Sie wurden vom Server getrennt");
                    break;
                case MessageTypes.ServerShutdown:
                    IsLoggedIn = false;
                    _connection.Close();
                    _onNewMessage("Server: Heruntergefahren");
                    break;
                case MessageTypes.DirectMessage:
                    MessageDirect md = new(data[0..receivedBytes]);
                    if (md.DataType is Datatypes.Text)
                        _onNewMessage("Direktnachricht " + md.SenderName + "> " + md._data.ConvertToString());
                    else
                        // todo: bild und datei-daten
                        throw new NotImplementedException("Bisher nur Textnachrichten unterstützt");
                    break;
                case MessageTypes.GroupMessage:
                    MessageGroup mr = new(data[0..receivedBytes]);
                    if (mr.ContentType == DataType.Text)
                        _onNewMessage(mr.SenderName + "> " + mr._data.ConvertToString());
                    else
                        // todo: bild und datei-daten
                        throw new NotImplementedException("Bisher nur Textnachrichten unterstützt");
                    break;
                case MessageTypes.Broadcast:
                    MessageBroadCast mb = new(data[..receivedBytes]);
                    if (mb.ContentType == DataType.Text)
                        _onNewMessage(mb.userName + ": " + mb.Content.ConvertToString());
                    else
                        // todo: bild und datei-daten
                        throw new NotImplementedException("Bisher nur Textnachrichten unterstützt");
                    break;
                case MessageTypes.BanFromServer:
                    _connection.Close();
                    _onNewMessage.Invoke("Server: Sie wurden vom Server verbannt");
                    break;
                case MessageTypes.ViewAllClients:
                    MessageViewAllClients mul = new(data[..receivedBytes]);
                    onNewUserList(mul.UserList);
                    break;
                default:
                    //todo: mehr nachrichten implementieren
                    throw new NotImplementedException(
                        $"Nachrichtenart {(MessageTypes) data[0]} nicht unterstützt");
            }
        }


        public void RequestUserList()
        {
            MessageViewAllClients m = new();
            _connection.GetStream().Write(m.ToArray());
        }
    }
}
