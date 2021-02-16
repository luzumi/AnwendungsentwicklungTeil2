using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Threading;
using ChatClientLogic;

namespace ChatClientGUI
{
    class ChatViewModel : INotifyPropertyChanged
    {
        public ICommand Command_Connect { get; }
        public ICommand Command_Send { get; }
        public event PropertyChangedEventHandler PropertyChanged;
        public Action ScrollDownMethod;
        private readonly ClientLogic _logic;


        public Dispatcher UiDispatcher { get; internal set; }
        public Brush ConnectionColor => IsConnected ? Brushes.Green : Brushes.Red;
        public bool IsConnected { get => _logic.IsConnected; }
        public bool IsNotConnected { get => !_logic.IsConnected; }
        public string VisibilityTextBox => _logic.IsConnected? "Hidden" : "Visible";
        public string VisibilityNoTextBox => !_logic.IsConnected? "Hidden" : "Visible";
        public string ButtonText => IsConnected ? " Disconnect" : " Connect";
        public bool CbTimeStamp { get; set; }
        


        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
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
                    _newMessage =  value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NewMessage)));
                }
            }
        }
        private string _newMessage;

        

        public ChatViewModel()
        {
            Command_Connect = new GenericCommand(Connect);
            Command_Send = new GenericCommand(SendNewMessage, () => IsConnected);
            _logic = new ClientLogic(DisplayReceivedMessage) {OnConnectionStatus = ConnectionStatusChange};
            Messages = string.Empty;
            NewMessage = string.Empty;
        }

        private void SendNewMessage()
        {
            string message = CbTimeStamp
                ? DateTime.Now.ToShortTimeString() + ": " + UserName + ": " + _newMessage
                : UserName + ": " + _newMessage;
            _logic.SendMessage(message);
            NewMessage = string.Empty;
            ScrollDownMethod?.Invoke();
        }

        private void DisplayReceivedMessage(string receivedMessage)
        {
            Messages += Environment.NewLine + receivedMessage;
            UiDispatcher.Invoke(() => ScrollDownMethod?.Invoke());
        }
        private void Connect()
        {
            if (IsConnected)
                _logic.Stop();
            else
            {
                _logic.Start(UserName);
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsConnected)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsNotConnected)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ConnectionColor)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ButtonText)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VisibilityTextBox)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VisibilityNoTextBox)));
            (Command_Send as GenericCommand)?.RaiseCanExecuteChanged();
        }

        private void ConnectionStatusChange()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsConnected)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsNotConnected)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ConnectionColor)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ButtonText)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VisibilityTextBox)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VisibilityNoTextBox)));
            UiDispatcher?.Invoke(((GenericCommand) Command_Send).RaiseCanExecuteChanged);
        }
    }
}
