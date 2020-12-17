using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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

namespace DrumPad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MediaPlayer[] soundPlayers = new MediaPlayer[9];
        List<string[]> soundSets;

        public MainWindow()
        {
            InitializeComponent();
            for (int i = 0; i < soundPlayers.Length; i++)
            {
                soundPlayers[i] = new MediaPlayer();
            }

            soundSets = new List<string[]>();
            string[] firstSet = new string[9];

            //Drumsamples from https://hiphopmakers.com/free-808-drum-kit-227-samples
            firstSet[0] = @"808Samples/bass(1).wav";
            firstSet[1] = @"808Samples/clap.wav";
            firstSet[2] = @"808Samples/cymbal.wav";
            firstSet[3] = @"808Samples/hihat(2).wav";
            firstSet[4] = @"808Samples/kick.wav";
            firstSet[5] = @"808Samples/snare.wav";
            firstSet[6] = @"808Samples/snare(1).wav";
            firstSet[7] = @"808Samples/kick(2).wav";
            firstSet[8] = @"808Samples/tom(4).wav";

            soundSets.Add(firstSet);

            loadSoundSet(0);
        }

        void PlaySound(object sender, EventArgs e)
        {
            switch ((sender as Button).Name)
            {
                case "bt_0":
                    playSound(0);
                    changeButtonColor(0);
                    break;
                case "bt_1":
                    playSound(1);
                    changeButtonColor(1);
                    break;
                case "bt_2":
                    playSound(2);
                    changeButtonColor(2);
                    break;
                case "bt_3":
                    playSound(3);
                    changeButtonColor(3);
                    break;
                case "bt_4":
                    playSound(4);
                    changeButtonColor(4);
                    break;
                case "bt_5":
                    playSound(5);
                    changeButtonColor(5);
                    break;
                case "bt_6":
                    playSound(6);
                    changeButtonColor(6);
                    break;
                case "bt_7":
                    playSound(7);
                    changeButtonColor(7);
                    break;
                case "bt_8":
                    playSound(8);
                    changeButtonColor(8);
                    break;
            }
        }

        void changeButtonColor(int count)
        {
            switch (count)
            {
                case 0:
                    bt_0.Background = Brushes.Red;
                    soundPlayers[count].MediaEnded += (o, e) => bt_0.Background = Brushes.Green;
                    break;
                case 1:
                    bt_1.Background = Brushes.Red;
                    soundPlayers[count].MediaEnded += (o, e) => bt_1.Background = Brushes.Green;
                    break;
                case 2:
                    bt_2.Background = Brushes.Red;
                    soundPlayers[count].MediaEnded += (o, e) => bt_2.Background = Brushes.Green;
                    break;
                case 3:
                    bt_3.Background = Brushes.Red;
                    soundPlayers[count].MediaEnded += (o, e) => bt_3.Background = Brushes.Green;
                    break;
                case 4:
                    bt_4.Background = Brushes.Red;
                    soundPlayers[count].MediaEnded += (o, e) => bt_4.Background = Brushes.Green;
                    break;
                case 5:
                    bt_5.Background = Brushes.Red;
                    soundPlayers[count].MediaEnded += (o, e) => bt_5.Background = Brushes.Green;
                    break;
                case 6:
                    bt_6.Background = Brushes.Red;
                    soundPlayers[count].MediaEnded += (o, e) => bt_6.Background = Brushes.Green;
                    break;
                case 7:
                    bt_7.Background = Brushes.Red;
                    soundPlayers[count].MediaEnded += (o, e) => bt_7.Background = Brushes.Green;
                    break;
                case 8:
                    bt_8.Background = Brushes.Red;
                    soundPlayers[count].MediaEnded += (o, e) => bt_8.Background = Brushes.Green;

                    break;
            }
        }


        private void playSound(int count)
        {
            soundPlayers[count].Position = TimeSpan.Zero;
            soundPlayers[count].Play();
        }

        void loadSoundSet(int pSoundSet)
        {
            for (int i = 0; i < soundPlayers.Length; i++)
            {
                soundPlayers[i].Open(new Uri(soundSets[pSoundSet][i], UriKind.Relative));
            }
        }
    }
}