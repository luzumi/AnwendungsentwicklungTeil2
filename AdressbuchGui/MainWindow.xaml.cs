using System.Windows;
using System.Windows.Controls;


namespace AdressbuchGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WebContentPage _page;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShowContactDetails(object pSender, SelectionChangedEventArgs pSelectionChangedEventArgs)
        {
            _page = new WebContentPage { DataContext = this.DataContext };
            FrmContent.Navigate(_page);
        }


    }
}
