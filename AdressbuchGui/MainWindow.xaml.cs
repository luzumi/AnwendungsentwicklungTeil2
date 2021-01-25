using System.Runtime.CompilerServices;
using System.Windows;
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
        WebContentPage _page = new WebContentPage(null);

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShowContactDetails(object pSender, DataTransferEventArgs pE)
        {
            FrmContent.Navigate(_page);
        }
    }
}
