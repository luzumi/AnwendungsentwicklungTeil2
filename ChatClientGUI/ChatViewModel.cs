using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using ChatClientLogic;

namespace ChatClientGUI
{
    class ChatViewModel : INotifyPropertyChanged
    {
        public ICommand CommandConnect { get; }
        public ICommand CommandSend { get; }
        public ICommand CommandViewUserList { get; }

        public event PropertyChangedEventHandler PropertyChanged;
        public Action ScrollDownMethod;
        private readonly ClientLogic _logic;


        public Dispatcher UiDispatcher { get; internal set; }
        public Brush ConnectionColor => IsConnected ? Brushes.Green : Brushes.Red;
        public bool IsConnected { get => _logic.IsConnected; }
        public bool IsNotConnected { get => !_logic.IsConnected; }
        public string VisibilityTextBox => _logic.IsConnected ? "Hidden" : "Visible";
        public string VisibilityNoTextBox => !_logic.IsConnected ? "Hidden" : "Visible";
        public string ButtonText => IsConnected ? " Disconnect" : " Connect";
        public bool CbTimeStamp { get; set; }
        public bool ButtonViewUserList { get; set; }
        public string ViewUserListVisibility => !ButtonViewUserList ? "Hidden" : "Visible";
        public string SelectedTarget { get; set; }
        public ObservableCollection<string> UserList { get; init; }

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


        private void SendNewMessage()
        {
            _logic.SendMessage(_newMessage, UserName);
            NewMessage = string.Empty;
        }

        private void DisplayReceivedMessage(string receivedMessage)
        {
            Messages += Environment.NewLine +
                        (CbTimeStamp ? DateTime.Now.ToShortTimeString() + " " + receivedMessage : receivedMessage);
            UiDispatcher.Invoke(ScrollDownMethod);
        }

        private void ChangeViewUserListVisibility()
        {
            ButtonViewUserList = !ButtonViewUserList;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ButtonViewUserList)));
        }

        private void DisplayUserList(List<string> receivedUserList)
        {
            ChangeViewUserListVisibility();
            UiDispatcher.Invoke(() =>
            {
                UserList.Clear();
                foreach (var user in receivedUserList)
                    UserList.Add(user);
            });
        }

        private void Connect(PasswordBox pPasswordBox)
        {
            if (IsConnected)
                _logic.Stop();
            else
            {
                using (MD5 hash = MD5.Create())
                    _logic.Start(UserName, hash.ComputeHash(Encoding.ASCII.GetBytes(pPasswordBox.Password)));
                UserList.Add(UserName);
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsConnected)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsNotConnected)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ConnectionColor)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ButtonText)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VisibilityTextBox)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VisibilityNoTextBox)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ButtonViewUserList)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ViewUserListVisibility)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserList)));
            (CommandSend as GenericCommand)?.RaiseCanExecuteChanged();
        }


        private bool CanConnect(PasswordBox pPasswordBox)
        {
            if (IsNotConnected)
                return true;

            return UserName.Length > 0 && pPasswordBox.Password.Length > 0; //TODO: bessere tests
        }

        private void ConnectionStatusChange()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsConnected)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsNotConnected)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ConnectionColor)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ButtonText)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VisibilityTextBox)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VisibilityNoTextBox)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ButtonViewUserList)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ViewUserListVisibility)));
            UiDispatcher?.Invoke(((GenericCommand)CommandSend).RaiseCanExecuteChanged);
            UiDispatcher?.Invoke(((GenericCommand)CommandViewUserList).RaiseCanExecuteChanged);
        }
    }
}
