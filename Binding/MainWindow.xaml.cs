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
            FrameContent.Navigate(new IntroBinding());
        }

        private void loadBindingIntroExcersice(object pSender, RoutedEventArgs pE)
        {
            FrameContent.Navigate(new BindingIntroExercise());
        }

        private void loadBindingDirection(object pSender, RoutedEventArgs pE)
        {
            FrameContent.Navigate(new BindingDirection());
        }

        private void loadBindingExternal(object pSender, RoutedEventArgs pE)
        {
            Page pageToShow = new BindingExternal();
            FrameContent.Navigate(new BindingExternal(this));
        }
    }
}
