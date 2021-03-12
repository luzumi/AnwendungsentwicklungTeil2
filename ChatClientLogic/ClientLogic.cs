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
        private readonly Action<string> _onNewMessage;
        private readonly Action<List<string>> onNewUserList;
        private TcpClient _connection;
        private CancellationTokenSource _cts;
        public Action OnConnectionStatus;

        /// <summary>
        ///     erstellt eine neue ClientLogic
        /// </summary>
        /// <param name="onNewMessage"></param>
        /// <param name="pOnNewUserList"></param>
        public ClientLogic(Action<string> onNewMessage, Action<List<string>> pOnNewUserList)
        {
            _onNewMessage = onNewMessage;
            onNewUserList = pOnNewUserList;
        }

        /// <summary>
        ///     Prüft Status der Verbindung eines Clients
        /// </summary>
        public bool IsConnected => _connection != null && _connection.Connected;

        /// <summary>
        ///     prüft ob Client erfolgreich eingeloggt ist
        /// </summary>
        public bool IsLoggedIn { get; set; }


        /// <summary>
        ///     Startet einen Client und verbindet diesen zum Server
        /// </summary>
        /// <param name="pUserName"> Username des Clients </param>
        /// <param name="pPassword"> Password des Clients</param>
        /// <returns> Erfolgsstatus des Verbindungsversuchs </returns>
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

            _cts = new CancellationTokenSource();
            _ = Task.Run(Receive, _cts.Token);
            MessageLogin ml = new(pUserName);

            using (var hash = SHA256.Create())
            {
                ml._password = hash.ComputeHash(pPassword);
            }

            ml.userName = pUserName;

            _connection.GetStream().Write(ml.ToArray());
            return true;
        }

        /// <summary>
        ///     Sendet Nachricht an alle Clients weiter
        /// </summary>
        /// <param name="pMessage"> zu sendende Nachricht </param>
        /// <param name="pUserName"> Name des sendenden Users</param>
        public void SendMessage(string pMessage, string pUserName)
        {
            if (_connection == null || !_connection.Connected)
            {
                return;
            }

            var mbc = new MessageBroadCast
            {
                ContentType = DataType.Text, userName = pUserName, Content = pMessage.ConvertToArray()
            };

            _connection.GetStream().Write(mbc.ToArray() /*, 0, mbc.GetSize()*/);
        }

        /// <summary>
        ///     Trennt Verbind des Clients vom Server
        /// </summary>
        public void Stop()
        {
            _cts.Cancel();
            _connection?.Close();
        }

        /// <summary>
        ///     empfängt Nachricht und handelt diese
        /// </summary>
        private async void Receive()
        {
            var data = new byte[1024];
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

                // nachricht empfangen
                HandleMessage(data, receivedBytes);
            }

            OnConnectionStatus?.Invoke();
        }

        /// <summary>
        ///     Prüft empfangene Nachricht auf NachrichtenTyp und handelt diese
        /// </summary>
        /// <param name="data"> NachrichtenStream </param>
        /// <param name="receivedBytes"> Länge des NachrichtenStreams </param>
        private void HandleMessage(byte[] data, int receivedBytes)
        {
            switch ((MessageTypes)data[0])
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
                    MessageDirect md = new(data[..receivedBytes]);
                    if (md.DataType is Datatypes.Text)
                    {
                        _onNewMessage("Direktnachricht " + md.SenderName + "> " + md._data.ConvertToString());
                    }
                    else
                        // todo: bild und datei
                    {
                        throw new NotImplementedException("Bisher nur Textnachrichten unterstützt");
                    }

                    break;

                case MessageTypes.GroupMessage:
                    MessageGroup mr = new(data[..receivedBytes]);
                    if (mr.ContentType == DataType.Text)
                    {
                        _onNewMessage(mr.SenderName + "> " + mr._data.ConvertToString());
                    }
                    else
                        // todo: bild und datei
                    {
                        throw new NotImplementedException("Bisher nur Textnachrichten unterstützt");
                    }

                    break;

                case MessageTypes.Broadcast:
                    MessageBroadCast mb = new(data[..receivedBytes]);
                    if (mb.ContentType == DataType.Text)
                    {
                        _onNewMessage(mb.userName + ": " + mb.Content.ConvertToString());
                    }
                    else
                        // todo: bild und datei
                    {
                        throw new NotImplementedException("Bisher nur Textnachrichten unterstützt");
                    }

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
                        $"Nachrichtenart {(MessageTypes)data[0]} nicht unterstützt");
            }
        }

        /// <summary>
        ///     sendet eine Liste aller Clienten
        /// </summary>
        public void RequestUserList()
        {
            MessageViewAllClients m = new();
            _connection.GetStream().Write(m.ToArray());
        }
    }
}
