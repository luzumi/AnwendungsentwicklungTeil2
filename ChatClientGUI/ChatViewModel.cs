using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;
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


        public bool IsConnected { get => _logic.IsConnected; }
        public string ButtonText => IsConnected ? " Disconnect" : " Connect";
        public Dispatcher UiDispatcher { get; internal set; }
        public Brush ConnectionColor => IsConnected ? Brushes.Green : Brushes.Red;


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
            Command_Connect = new GenericCommand(Connect);
            Command_Send = new GenericCommand(SendNewMessage, () => IsConnected);
            _logic = new ClientLogic(DisplayReceivedMessage) {OnConnectionStatus = ConnectionStatusChange};
            Messages = string.Empty;
            NewMessage = string.Empty;
        }

        private void SendNewMessage()
        {
            _logic.SendMessage(_newMessage);
            NewMessage = string.Empty;
            ScrollDownMethod?.Invoke();
        }

        private void DisplayReceivedMessage(string receivedMessage)
        {
            Messages += Environment.NewLine + "Other> " + receivedMessage;
            UiDispatcher.Invoke(() => ScrollDownMethod?.Invoke());
        }
        private void Connect()
        {
            if (IsConnected)
                _logic.Stop();
            else
                _logic.Start();

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsConnected)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ConnectionColor)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ButtonText)));
            (Command_Send as GenericCommand)?.RaiseCanExecuteChanged();
        }

        private void ConnectionStatusChange()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsConnected)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ConnectionColor)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ButtonText)));
            UiDispatcher?.Invoke(((GenericCommand) Command_Send).RaiseCanExecuteChanged);
        }
    }
}
