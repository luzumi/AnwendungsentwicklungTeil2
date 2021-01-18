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
            PageGame.LoopSound = new MediaPlayer();
            PageGame.LoopSound.Open(new Uri(@".\Sounds\Clippity-Clop_Looping.wav", UriKind.Relative));
            PageGame.LoopSound.Play();
            PageGame.LoopSound.MediaEnded += (o, e) =>
            {
                PageGame.LoopSound.Position = TimeSpan.Zero;
                PageGame.LoopSound.Play();
            };
        }

        private void ButtonBase_OnClick(object pSender, RoutedEventArgs pE)
        {
            FrameContent.Navigate(new PageGame());
        }
    }
}
