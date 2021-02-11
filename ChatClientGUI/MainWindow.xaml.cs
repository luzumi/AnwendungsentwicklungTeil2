using System.Windows;

namespace ChatClientGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
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
