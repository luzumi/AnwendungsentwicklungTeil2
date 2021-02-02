using System.Windows;

namespace MultiThreading
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnWithout_Click(object sender, RoutedEventArgs e)
        {
            frmContent.Navigate(new pgeWithoutThreading());
        }

        private void btnThreadingAsync_Click(object sender, RoutedEventArgs e)
        {
            frmContent.Navigate(new pgeThreadingAsync());
        }

        private void btnThreadingAsync2_Click(object sender, RoutedEventArgs e)
        {
            frmContent.Navigate(new pgeThreadingAsync2());
        }
        private void btnThreadingTask_Click(object sender, RoutedEventArgs e)
        {
            frmContent.Navigate(new pgeThreadingTask());
        }

        private void btnThreadingTaskCancel_Click(object sender, RoutedEventArgs e)
        {
            frmContent.Navigate(new pgeThreadingTaskCancel());
        }

        private void btnThreadingTaskProgress_Click(object sender, RoutedEventArgs e)
        {
            frmContent.Navigate(new pgeThreadingTaskCancel());
        }
    }
}
