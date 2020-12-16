using System.Windows;
using System.Windows.Controls;

namespace WPFFirstSteps
{
    /// <summary>
    ///     Interaktionslogik für Anzeige.xaml
    /// </summary>
    public partial class PageAnzeige : Page
    {
        public PageAnzeige()
        {
            InitializeComponent();
        }

        private void CheckBoxNachtmodus_Checked(object sender, RoutedEventArgs e)
        {
            lblNachtmodus.Content = CheckBoxNachtmodus.IsChecked.Value ? "AN" : "AUS";
        }
    }
}