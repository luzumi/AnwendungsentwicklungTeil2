using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WPFFirstSteps
{
    /// <summary>
    ///     Interaktionslogik für PageSound.xaml
    /// </summary>
    public partial class PageSound : Page
    {
        public PageSound()
        {
            InitializeComponent();
        }

        private void Button_IsMouseDirectlyOverChanged(object pSender, DependencyPropertyChangedEventArgs pE)
        {
            btnGeräteeigenschaften.Foreground = IsMouseCaptured ? Brushes.Gray : Brushes.DarkGoldenrod;
            btnGeräteeigenschaften.Background = Brushes.Black;
        }

        private void ChangeVolumeLabelIsMouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            lblVolume.Content = volume.Ticks;
        }
    }
}