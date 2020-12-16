using System;
using System.Windows;

namespace WPFFirstSteps
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnAnzeigeClick(object sender, RoutedEventArgs e)
        {
            frameContent.Navigate(new PageAnzeige());
        }

        private void btnSoundClick(object pSender, RoutedEventArgs pE)
        {
            frameContent.Navigate(new PageSound());
        }

        private void btnImages_Click(object pSender, RoutedEventArgs pE)
        {
            throw new NotImplementedException();
        }
    }
}