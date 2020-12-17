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
        private MediaPlayer[] mediaPlayers = new MediaPlayer[9];
        List<string[]> soundSets;

        public MainWindow()
        {
            InitializeComponent();
            for (int i = 0; i < mediaPlayers.Length; i++)
            {
                mediaPlayers[i] = new MediaPlayer();
            }

            soundSets = new List<string[]>();
            string[] firstSet = new string[9];

            //Drumsamples from https://hiphopmakers.com/free-808-drum-kit-227-samples
            firstSet[0] = @"808Samples/cymbal.wav";
            firstSet[1] = @"808Samples/cymbal(1).wav";
            firstSet[2] = @"808Samples/hihat(5).wav";
            firstSet[3] = @"808Samples/tom(5).wav";
            firstSet[4] = @"808Samples/snare(5).wav";
            firstSet[5] = @"808Samples/snare.wav";
            firstSet[6] = @"808Samples/snare(7).wav";
            firstSet[7] = @"808Samples/bass(2).wav";
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
                    E0.Fill = Brushes.Red;
                    mediaPlayers[count].MediaEnded += (o, e) => E0.Fill = Brushes.Green;
                    break;

                case 1:
                    E1.Fill = Brushes.Red;
                    mediaPlayers[count].MediaEnded += (o, e) => E1.Fill = Brushes.Green;
                    break;

                case 2:
                    E2.Fill = Brushes.Red;
                    mediaPlayers[count].MediaEnded += (o, e) => E2.Fill = Brushes.Green;
                    break;

                case 3:
                    E3.Fill = Brushes.Red;
                    mediaPlayers[count].MediaEnded += (o, e) => E3.Fill = Brushes.Green;
                    break;

                case 4:
                    E4.Fill = Brushes.Red;
                    mediaPlayers[count].MediaEnded += (o, e) => E4.Fill = Brushes.Green;
                    break;

                case 5:
                    E5.Fill = Brushes.Red;
                    mediaPlayers[count].MediaEnded += (o, e) => E5.Fill = Brushes.Green;
                    break;

                case 6:
                    E6.Fill = Brushes.Red;
                    mediaPlayers[count].MediaEnded += (o, e) => E6.Fill = Brushes.Green;
                    break;

                case 7:
                    E7.Fill = Brushes.Red;
                    mediaPlayers[count].MediaEnded += (o, e) => E7.Fill = Brushes.Green;
                    break;

                case 8:
                    E8.Fill = Brushes.Red;
                    mediaPlayers[count].MediaEnded += (o, e) => E8.Fill = Brushes.Green;
                    break;
            }
        }


        private void playSound(int count)
        {
            mediaPlayers[count].Position = TimeSpan.Zero;
            mediaPlayers[count].Play();
        }

        void loadSoundSet(int pSoundSet)
        {
            for (int i = 0; i < mediaPlayers.Length; i++)
            {
                mediaPlayers[i].Open(new Uri(soundSets[pSoundSet][i], UriKind.Relative));
            }
        }
    }
}