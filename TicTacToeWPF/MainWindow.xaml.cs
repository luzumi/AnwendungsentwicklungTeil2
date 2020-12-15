using System;
using System.Collections.Generic;
using System.IO;
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
using TicTacToe;

namespace TicTacToeWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            StartLogo.Text =
                File.ReadAllText(
                    @"E:\VisualStudio-workspace\AnwendungsentwicklungTeil1\Kontrollstrukturen\TicTacToe\Logo.txt");
        }

        private void ButtonBase_OnClick(object pSender, RoutedEventArgs pE)
        {
            FrameContent.Navigate(new PageGame());
        }
    }
}
