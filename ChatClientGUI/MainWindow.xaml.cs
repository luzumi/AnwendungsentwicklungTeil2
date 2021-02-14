using System.Windows;

namespace ChatClientGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            ChatViewModel vm = new();
            vm.ScrollDownMethod = SvMessages.ScrollToBottom;
            vm.UiDispatcher = Dispatcher;
            DataContext = vm;
        }
    }
}
