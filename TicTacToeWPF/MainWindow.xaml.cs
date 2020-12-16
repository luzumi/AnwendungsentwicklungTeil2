using System;
using System.IO;
using System.Windows;
using System.Windows.Media;

namespace TicTacToeWPF
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            StartLogo.Text = File.ReadAllText(
                @"E:\VisualStudio-workspace\AnwendungsentwicklungTeil1\Kontrollstrukturen\TicTacToe\Logo.txt");

            //Sound from https://soundimage.org/
            var loopSound = new MediaPlayer();
            loopSound.Open(new Uri(@".\Sounds\Clippity-Clop_Looping.wav", UriKind.Relative));
            loopSound.Play();
        }


        private void ButtonBase_OnClick(object pSender, RoutedEventArgs pE)
        {
            FrameContent.Navigate(new PageGame());
        }
    }
}