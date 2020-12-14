using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFFirstSteps
{
    /// <summary>
    /// Interaktionslogik für Anzeige.xaml
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
