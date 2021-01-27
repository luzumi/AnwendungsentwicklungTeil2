using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


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
            _page = new WebContentPage();
            _page.DataContext = this.DataContext;
            FrmContent.Navigate(_page);
        }

        
    }
}
