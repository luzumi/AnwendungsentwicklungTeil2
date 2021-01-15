using System.Windows;
using System.Windows.Controls;

namespace Binding
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

        private void loadBindingIntro(object pSender, RoutedEventArgs pE)
        {
            frmContent.Navigate(new IntroBinding());
        }

        private void loadBindingIntroExcersice(object pSender, RoutedEventArgs pE)
        {
            frmContent.Navigate(new BindingIntroExercise());
        }

        private void loadBindingDirection(object pSender, RoutedEventArgs pE)
        {
            frmContent.Navigate(new BindingDirection());
        }

        private void loadBindingExternal(object pSender, RoutedEventArgs pE)
        {
            Page pageToShow = new BindingExternal();
            frmContent.Navigate(new BindingExternal(this));
        }

        private void loadBindingFormatAndConvert(object pSender, RoutedEventArgs pE)
        {
            frmContent.Navigate(new BindingFormatAndConvert());
        }

        private void LoadBindingPropertys(object pSender, RoutedEventArgs pE)
        {
            frmContent.Navigate(new BindingPropertys());
        }

        private void LoadBindingCommands(object pSender, RoutedEventArgs pE)
        {
            frmContent.Navigate(new BindingCommands());
        }
    }
}
