using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace ChatClientGUI
{
    class ChatViewModel : INotifyPropertyChanged
    {
        public ICommand Command_Connect { get; init; }
        public ICommand Command_Send { get; init; }
        public event PropertyChangedEventHandler PropertyChanged;
        public Brush ConnectionColor
        {
            get => IsConnected ? Brushes.Green : Brushes.Red;
        }

        public string ButtonText => IsConnected ? " Disconnect" : " Connect";

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

        public Action ScrollDownMethod;
        private readonly ChatClientLogic.ClientLogic logic;
        public bool IsConnected { get => logic.IsConnected; }
        public Dispatcher UiDispatcher { get; internal set; }

        public ChatViewModel()
        {
            Command_Connect = new GenericCommand(Connect);
            Command_Send = new GenericCommand(SendNewMessage, () => IsConnected);
            logic = new(DisplayReceivedMessage);
            logic.OnConnectionStatus = ConnectionStatusChange;
            Messages = string.Empty;
            NewMessage = string.Empty;
        }

        private void SendNewMessage()
        {
            logic.SendMessage(_newMessage);
            NewMessage = string.Empty;
            ScrollDownMethod?.Invoke();
        }

        private void DisplayReceivedMessage(string ReceivedMessage)
        {
            Messages += Environment.NewLine + "Other> " + ReceivedMessage;
            UiDispatcher.Invoke(() => ScrollDownMethod?.Invoke());
        }
        private void Connect()
        {
            if (IsConnected)
                logic.Stop();
            else
                logic.Start();

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
