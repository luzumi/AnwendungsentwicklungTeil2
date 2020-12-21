using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

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

            //drumlessTrack = new MediaPlayer();
            //drumlessTrack.Open(new Uri(@"808Samples/Prison_SongDrumless.mp3", UriKind.Relative));
            //drumlessTrack.Play();
        }


        void PlaySound(object sender, EventArgs e)
        {
            //aus dem buttonName wird die letzte Zahl extrahiert und als zähler verwendet
            Int32.TryParse((sender as Button).Name.Substring(3), out int number);

            if (number == 8) loadSoundSet(setSetter);

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


        /// <summary>
        /// Listeeneinträge mit Blankoeinträgen in den Soundsetzt werden erstellt 
        /// </summary>
        /// <param name="pSender"></param>
        /// <param name="pE"></param>
        private void BtNewSamples_OnClick(object pSender, RoutedEventArgs pE)
        {
            var soundSampleStrings = new List<string[]>();
            string[] input = Directory.GetFiles(@"808Samples");

            int counter = 0;
            int line = -1;
            string patter = "";
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i].Contains("mp3"))
                {
                    continue;
                }

                if (counter == soundSampleStrings.Count)
                {
                    soundSampleStrings.Add(new[] {" ", " ", " ", " ", " ", " ", " ", " ", " "});
                }

                if (!patter.Equals(input[i].Substring(11, 3)))
                {
                    patter = input[i].Substring(11, 3);
                    counter = 0;
                    line++;
                }

                string pattern;
                Match match;
                int zahl;
                switch (input[i].Substring(11, 3))
                {
                    case "bas":
                        soundSampleStrings[counter][line] = input[i];
                        counter++;
                        break;

                    case "cla":
                        soundSampleStrings[counter][line] = input[i];
                        counter++;
                        break;

                    case "cym":
                        soundSampleStrings[counter][line] = input[i];
                        counter++;
                        break;

                    case "hih":
                        pattern = "([0-9]+)";
                        match = Regex.Match(input[i].Substring(5), pattern);
                        zahl = Int32.Parse(match.Value);

                        if (zahl % 2 == 0)
                        {
                            soundSampleStrings[counter][line] = input[i];
                        }
                        else
                        {
                            soundSampleStrings[counter][line + 1] = input[i];
                            counter++;
                        }

                        break;

                    case "kic":
                        soundSampleStrings[counter][line + 1] = input[i];
                        counter++;
                        break;

                    case "sna":
                        pattern = "([0-9]+)";
                        match = Regex.Match(input[i].Substring(5), pattern);
                        zahl = Int32.Parse(match.Value);

                        if (zahl % 2 == 0)
                        {
                            soundSampleStrings[counter][line] = input[i];
                        }
                        else
                        {
                            soundSampleStrings[counter][line + 1] = input[i];
                            counter++;
                        }

                        break;

                    case "tom":
                        soundSampleStrings[counter][line + 1] = input[i];
                        counter++;
                        break;
                }
            }

            DB(soundSampleStrings);
        }

        private static void DB(List<string[]> soundSampleStrings)
        {
            SQLiteCommand command = new SQLiteCommand();
            SQLiteConnectionStringBuilder builder = new SQLiteConnectionStringBuilder();
            builder.DataSource = Directory.GetParent(Environment.CommandLine)?.FullName + @"\samples.db";
            builder.Version = 3;

            int id = 1;
            using (SQLiteConnection connection = new SQLiteConnection(builder.ToString()))
            {
                connection.Open();
                //TODO statement setzen
                if (id == 1)
                {
                    connect(soundSampleStrings, connection);
                    id++;
                }

                //FillDataBase(soundSampleStrings, connection, command);
            }
        }

        private static void FillDataBase(List<string[]> soundSampleStrings, SQLiteConnection connection,
            SQLiteCommand command)
        {
            int id = 1;

            for (int i = 0; i < soundSampleStrings.Count - 1; i++)
            {
                command = connection.CreateCommand();
                //command.Parameters.Add(new SQLiteParameter("ID",    id));
                command.CommandText =
                    "INSERT INTO 'SoundSamples' (ID, bass, clap, cymbal, hihat, hihat2, kick, snare, snare2, tom)" +
                    "Values (@id, @bass, @clap, @cymbal, @hihat, " +
                    "@hihat2, @kick, @snare, @snare2, @tom); ";
                command.Parameters.Add(new SQLiteParameter("ID", id));
                command.Parameters.Add(new SQLiteParameter("bass", soundSampleStrings[i][0]));
                command.Parameters.Add(new SQLiteParameter("clap", soundSampleStrings[i][1]));
                command.Parameters.Add(new SQLiteParameter("cymbal", soundSampleStrings[i][2]));
                command.Parameters.Add(new SQLiteParameter("hihat", soundSampleStrings[i][3]));
                command.Parameters.Add(new SQLiteParameter("hihat2", soundSampleStrings[i][4]));
                command.Parameters.Add(new SQLiteParameter("kick", soundSampleStrings[i][5]));
                command.Parameters.Add(new SQLiteParameter("snare", soundSampleStrings[i][6]));
                command.Parameters.Add(new SQLiteParameter("snare2", soundSampleStrings[i][7]));
                command.Parameters.Add(new SQLiteParameter("tom", soundSampleStrings[i][8]));

                command.ExecuteReader();

                id++;
            }
        }


        public static void connect(List<string[]> soundSampleStrings, SQLiteConnection connection)
        {
            SQLiteCommand sqlite_cmd = new SQLiteCommand();
            SQLiteDataReader sqlite_datareader;
            
            //FillDataBase(soundSampleStrings, connection, sqlite_cmd);
            
            sqlite_cmd.CommandText = "SELECT * FROM 'SoundSamples'";
            sqlite_cmd.Connection = connection;
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            
            while (sqlite_datareader.Read()) // Read() returns true if there is still a result line to read
            {
                string myreader = sqlite_datareader.GetString(0);
                MessageBox.Show(myreader);
            }

            
        }
    }
}