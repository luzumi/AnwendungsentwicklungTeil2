using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using ChatClientLogic;

namespace ChatClientGUI
{
    /// <summary>
    /// ViewModel ChatClient
    /// </summary>
    class ChatViewModel : INotifyPropertyChanged
    {
        [DebuggerDisplay("{UserName}")]
        public ICommand CommandConnect { get; }
        public ICommand CommandSend { get; }
        public ICommand CommandViewUserList { get; }

        public event PropertyChangedEventHandler PropertyChanged;
        public Action ScrollDownMethod;
        private readonly ClientLogic _logic;
        public Dispatcher UiDispatcher { get; internal set; }

        /// <summary>
        /// ändert <see cref="Brush"/> anhand des VerbindungStatus
        /// </summary>
        public Brush ConnectionColor => IsConnected ? Brushes.Green : Brushes.Red;

        /// <summary>
        /// <code>Property <see cref="bool"/></code> <c>ChatServerLogic</c>  ist verbunden
        /// </summary>
        public bool IsConnected { get => _logic.IsConnected; }

        /// <summary>
        /// <code>Property <see cref="bool"/></code> <c>ChatServerLogic</c>  ist nicht verbunden
        /// </summary>
        public bool IsNotConnected { get => !_logic.IsConnected; }

        /// <summary>
        /// <code>Property <see cref="string"/> Hidden/Visible</code> stellt Sichtbarkeit einer <seealso cref="TextBox"> TextBox </seealso> um
        /// </summary>
        public string VisibilityTextBox => _logic.IsConnected ? "Hidden" : "Visible";

        /// <summary>
        /// <code>Property <see cref="string"/> Hidden/Visible</code> stellt Sichtbarkeit einer <seealso cref="PasswordBox"> PasswordBox </seealso> um
        /// </summary>
        public string VisibilityPasswordBox => !_logic.IsConnected ? "Hidden" : "Visible";

        /// <summary>
        /// <code>Property <see cref="string"/> Disconnect/Connect</code> legt anhand des VerbingsStatus des Clients die
        /// Beschriftung eines <seealso cref="Button"> Button </seealso> fest
        /// </summary>
        public string ButtonText => IsConnected ? " Disconnect" : " Connect";

        /// <summary>
        /// <code>Property <see cref="bool"/> Timestamp On/off </code> legt fest ob ein Timestamp der nachricht hinzugefügt wird
        /// </summary>
        public bool CbTimeStamp { get; set; }

        /// <summary>
        /// <code>Property <see cref="bool"/> <see cref="UserList"/> ist sichtbar/versteckt </code> 
        /// </summary>
        public bool ButtonViewUserList { get; set; }

        /// <summary>
        /// <code>Property <see cref="string"/> Hidden/Visible</code> stellt Sichtbarkeit einer <seealso cref="ListBox"/> <see cref="UserList"/> um
        /// </summary>
        public string ViewUserListVisibility => !ButtonViewUserList ? "Hidden" : "Visible";

        /// <summary>
        /// <code>Property <see cref="string"/>Client aus <see cref="UserList"/> auswählen</code>
        /// </summary>
        public string SelectedTarget { get; set; }
        public ObservableCollection<string> UserList { get; init; }


        /// <summary>
        /// Name des Nutzers
        /// </summary>
        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                (CommandConnect as GenericParameterCommand)?.RaiseCanExecuteChanged();
            }
        }

        private string _userName = "Your Name";


        /// <summary>
        /// die Nachricht in der Chatansicht
        /// </summary>
        public string Messages
        {
            get => _messages;
            set
            {
                if (_messages != value)
                {
                    _messages = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Messages)));
                }
            }
        }

        private string _messages;


        /// <summary>
        /// die zu sendende Nachricht
        /// </summary>
        public string NewMessage
        {
            get => _newMessage;
            set
            {
                if (_newMessage != value)
                {
                    _newMessage = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NewMessage)));
                }
            }
        }

        private string _newMessage;

        /// <summary>
        /// startet ViewModel für den Clienten
        /// </summary>
        public ChatViewModel()
        {
            CommandConnect =
                new GenericParameterCommand((x) => Connect((PasswordBox)x), (x) => CanConnect((PasswordBox)x));
            CommandViewUserList = new GenericCommand(() => _logic.RequestUserList(), () => true);
            CommandSend = new GenericCommand(SendNewMessage, () => IsConnected);

            _logic = new ClientLogic(DisplayReceivedMessage, DisplayUserList);
            _logic.OnConnectionStatus = ConnectionStatusChange;

            Messages = String.Empty;
            NewMessage = String.Empty;

            UserList = new();
        }

        /// <summary>
        /// sendet neue Nachricht 
        /// </summary>
        private void SendNewMessage()
        {
            _logic.SendMessage(_newMessage, UserName);
            NewMessage = string.Empty;
        }

        /// <summary>
        /// switcht Porperty <c>ButtonViewUserList</c>
        /// </summary>
        private void DisplayReceivedMessage(string receivedMessage)
        {
            Messages += Environment.NewLine +
                        (CbTimeStamp ? DateTime.Now.ToShortTimeString() + " " + receivedMessage : receivedMessage);
            UiDispatcher.Invoke(ScrollDownMethod);
        }

        /// <summary>
        /// switcht Porperty <c>ButtonViewUserList</c>
        /// </summary>
        private void ChangeViewUserListVisibility()
        {
            ButtonViewUserList = !ButtonViewUserList;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ButtonViewUserList)));
        }


        /// <summary>
        /// erstellt Userliste an UI
        /// </summary>
        /// <param name="receivedUserList"> anzuzeigende Liste der User</param>
        private void DisplayUserList(List<string> receivedUserList)
        {
            UiDispatcher.Invoke(() =>
            {
                UserList.Clear();
                foreach (var user in receivedUserList)
                    UserList.Add(user);
            });
        }

        /// <summary>
        /// Verbindet Client mit Logic, wenn keine Verbindung besteht - 
        /// Trennt Client von Logic, wenn Verbindung steht
        /// </summary>
        /// <param name="pPasswordBox"></param>
        private void Connect(PasswordBox pPasswordBox)
        {
            if (IsConnected)
                _logic.Stop();
            else
            {
                using (MD5 hash = MD5.Create())
                    _logic.Start(UserName, hash.ComputeHash(Encoding.ASCII.GetBytes(pPasswordBox.Password)));
                //UserList.Add(UserName);
            }
            //TODO in setter IsConnected
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsConnected)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsNotConnected)));
            //TODO propchanges über ViewStyles
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ConnectionColor)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ButtonText)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VisibilityTextBox)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VisibilityPasswordBox)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ButtonViewUserList)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ViewUserListVisibility)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserList)));
            //TODO in setter IsConnected
            (CommandSend as GenericCommand)?.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Prüft ob Verbindsaufbau erlaubt ist
        /// </summary>
        /// <param name="pPasswordBox"></param>
        /// <returns>Wenn nicht verbunden <code>true</code> - Wenn verbunden prüft ob UserName und Password <c>nicht</c> vorhanden <code>false</code></returns>
        private bool CanConnect(PasswordBox pPasswordBox)
        {
            if (IsNotConnected)
                return true;

            return UserName.Length > 0 && pPasswordBox.Password.Length > 0; //TODO: bessere tests
        }

        /// <summary>
        /// meldet Änderungen der Propertys an Gui wenn sich Verbindungstatus ändert
        /// </summary>
        private void ConnectionStatusChange()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsConnected)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsNotConnected)));
            //TODO propchanges über ViewStyles
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ConnectionColor)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ButtonText)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VisibilityTextBox)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VisibilityPasswordBox)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ButtonViewUserList)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ViewUserListVisibility)));
            UiDispatcher?.Invoke(((GenericCommand)CommandSend).RaiseCanExecuteChanged);
            UiDispatcher?.Invoke(((GenericCommand)CommandViewUserList).RaiseCanExecuteChanged);
        }
    }
}
