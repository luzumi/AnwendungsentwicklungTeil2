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
        private MediaPlayer drumlessTrack;
        List<string[]> soundSets;
        private int setSetter = 0;

        public MainWindow()
        {



            for (int i = 0; i < mediaPlayers.Length; i++)
            {
                mediaPlayers[i] = new MediaPlayer();
            }

            soundSets = new List<string[]>();
            string[] firstSet = new string[9];
            string[] secondSet = new string[9];
            string[] thirdSet = new string[9];

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

            secondSet[0] = @"808Samples/cymbal(3).wav";
            secondSet[1] = @"808Samples/cymbal(4).wav";
            secondSet[2] = @"808Samples/hihat(12).wav";
            secondSet[3] = @"808Samples/tom(8).wav";
            secondSet[4] = @"808Samples/snare(15).wav";
            secondSet[5] = @"808Samples/snare(32).wav";
            secondSet[6] = @"808Samples/snare(27).wav";
            secondSet[7] = @"808Samples/bass(12).wav";
            secondSet[8] = @"808Samples/tom(12).wav";

            soundSets.Add(secondSet);

            thirdSet[0] = @"808Samples/cymbal(5).wav";
            thirdSet[1] = @"808Samples/clap(6).wav";
            thirdSet[2] = @"808Samples/hihat(32).wav";
            thirdSet[3] = @"808Samples/tom(7).wav";
            thirdSet[4] = @"808Samples/snare(35).wav";
            thirdSet[5] = @"808Samples/snare(42).wav";
            thirdSet[6] = @"808Samples/snare(48).wav";
            thirdSet[7] = @"808Samples/bass(7).wav";
            thirdSet[8] = @"808Samples/clap(12).wav";

            soundSets.Add(thirdSet);

            InitializeComponent();
            

            drumlessTrack = new MediaPlayer();
            drumlessTrack.Open(new Uri(@"808Samples/Prison_SongDrumless.mp3", UriKind.Relative));
            drumlessTrack.Play();

        }

        void PlaySound(object sender, EventArgs e)
        {
            Int32.TryParse((sender as Button).Name.Substring(3), out int number);

            if(number == 8) loadSoundSet(setSetter);

            mediaPlayers[number].Open(new Uri(soundSets[setSetter][number], UriKind.Relative));

            playSound(number);

            changeButtonColor(number);

        }

        void changeButtonColor(int count)
        {

            switch (count)
            {
                case 0:
                    E0.Fill = Brushes.OrangeRed;
                    mediaPlayers[count].MediaEnded += (o, e) => E0.Fill = Brushes.DarkGoldenrod;
                    break;

                case 1:
                    E1.Fill = Brushes.OrangeRed;
                    mediaPlayers[count].MediaEnded += (o, e) => E1.Fill = Brushes.DarkGoldenrod;
                    break;

                case 2:
                    E2.Fill = Brushes.OrangeRed;
                    mediaPlayers[count].MediaEnded += (o, e) => E2.Fill = Brushes.DarkGoldenrod;
                    break;

                case 3:
                    E3.Fill = Brushes.OrangeRed;
                    mediaPlayers[count].MediaEnded += (o, e) => E3.Fill = Brushes.DarkGoldenrod;
                    break;

                case 4:
                    E4.Fill = Brushes.OrangeRed;
                    mediaPlayers[count].MediaEnded += (o, e) => E4.Fill = Brushes.DarkGoldenrod;
                    break;

                case 5:
                    E5.Fill = Brushes.OrangeRed;
                    mediaPlayers[count].MediaEnded += (o, e) => E5.Fill = Brushes.DarkGoldenrod;
                    break;

                case 6:
                    E6.Fill = Brushes.OrangeRed;
                    mediaPlayers[count].MediaEnded += (o, e) => E6.Fill = Brushes.DarkGoldenrod;
                    break;

                case 7:
                    E7.Fill = Brushes.OrangeRed;
                    mediaPlayers[count].MediaEnded += (o, e) => E7.Fill = Brushes.DarkGoldenrod;
                    break;

                case 8:
                    E8.Fill = Brushes.OrangeRed;
                    mediaPlayers[count].MediaEnded += (o, e) => E8.Fill = Brushes.DarkGoldenrod;
                    break;
            }
        }


        private void playSound(int count)
        {
            mediaPlayers[count].Position = TimeSpan.Zero;
            mediaPlayers[count].Play();
        }

        private void loadSoundSet(int pSoundSet)
        {
            setSetter++;

            if (setSetter == soundSets.Count)
            {
                setSetter = 0;
                pSoundSet = 0;
            }
        }

        private void ToggleButton_OnChecked(object pSender, RoutedEventArgs pE)
        {
            loadSoundSet(setSetter);
        }
    }
}