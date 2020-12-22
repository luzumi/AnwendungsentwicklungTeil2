using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Memory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private Pair player;
        List<BitmapImage> bitmaps;
        private bool clickCount;
        private Button firstSelectedButton = new();
        private Button secondSelectedButton = new();
        (string firstPic, string secondPic) pairTuple = new();
        private int absolutePairs;
        private int rounds = 0;
        public DateTime startTime;
        public static (int widht, int height) coord = new();


        public DateTime StartTime
        {
            get => startTime;
            set => startTime = value;
        }

        public int Rounds
        {
            get => rounds;
            set
            {
                rounds = value;
                lblPoints.Content = "Turns: " + rounds.ToString();
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            bitmaps = new List<BitmapImage>();

            // alle bilder laden
            foreach (var fileName in Directory.GetFiles("Images"))
            {
                BitmapImage tempBitmap = new(); // neues Bild erstellen
                tempBitmap.BeginInit(); // füllen des Bildes starten
                tempBitmap.UriSource =
                    new Uri(Directory.GetParent(Environment.CommandLine).FullName + @"\" +
                            fileName); // bildinhalt aus datei laden
                tempBitmap.EndInit(); // füllen des Bildes finalisieren
                bitmaps.Add(tempBitmap);
            }
        }

        void CreateGame(int columns, int rows)
        {
            // clear
            coord = (columns, rows);
            player = new();
            Rounds = 0;
            FrameContent.Visibility = Visibility.Collapsed;
            Spielfeld.Children.Clear();
            Spielfeld.ColumnDefinitions.Clear();
            Spielfeld.RowDefinitions.Clear();

            // Linkedliste mit erlaubten image ziffern erstellen
            List<int> availableBitmaps = new List<int>();
            for (int counter = 0; counter < bitmaps.Count; counter++)
                availableBitmaps.Add(counter);

            List<Image> images = new List<Image>();
            // recreate
            DefineRowCol(columns, rows);

            var rndGen = CreateButtonsRandom(columns, rows, images);

            ShuffleButtons(columns, rows, rndGen, availableBitmaps, images);

            player.Name = tfName.Text;
            player.Points = 0;
            tfName.SelectAll();

            StartTime = DateTime.Now;
        }


        /// <summary>
        /// Erstellt variable die Oberfläche des Spielfelds
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="rows"></param>
        private void DefineRowCol(int columns, int rows)
        {
            var gridElementSize = new GridLength(80);
            absolutePairs = columns * rows / 2;

            for (int counter = 0; counter < columns; counter++)
            {
                ColumnDefinition colDef = new();
                colDef.Width = gridElementSize;
                Spielfeld.ColumnDefinitions.Add(colDef);
            }

            for (int counter = 0; counter < rows; counter++)
            {
                RowDefinition rowDef = new();
                rowDef.Height = gridElementSize;
                Spielfeld.RowDefinitions.Add(rowDef);
            }
        }

        /// <summary>
        /// weist jedem Button ein Bild und einen zweiten button mit dem gleichen bild zu
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="rows"></param>
        /// <param name="images"></param>
        /// <returns></returns>
        private Random CreateButtonsRandom(int columns, int rows, List<Image> images)
        {
            Random rndGen = new();
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    // Image tag erstellen
                    Image tempImage = new Image {Stretch = Stretch.UniformToFill};
                    images.Add(tempImage);
                    // Button erstellen und füllen
                    Button temp = new();
                    temp.Click += btn_Click;
                    temp.Style = FindResource("FieldButton") as Style;
                    temp.Content = tempImage; // button mit Image füllen

                    // Im Grid eintragen
                    Grid.SetColumn(temp, col); // button in spalte positionieren
                    Grid.SetRow(temp, row); // button in zeile positionieren
                    Spielfeld.Children.Add(temp);
                }
            }

            return rndGen;
        }


        private void btn_Click(object sender, RoutedEventArgs e)
        {
            var button = (sender as Button);

            if (!clickCount)
            {
                if (secondSelectedButton != null && firstSelectedButton != null)
                {
                    if (firstSelectedButton.Content != null && secondSelectedButton.Content != null)
                    {
                        (firstSelectedButton.Content as Image).Opacity = 0;
                        (secondSelectedButton.Content as Image).Opacity = 0;
                    }
                }

                firstSelectedButton = button;

                pairTuple.firstPic = (firstSelectedButton.Content as Image).Source.ToString();

                (button.Content as Image).Opacity = 1;

                Rounds++;
                
                clickCount = !clickCount;
            }
            else
            {
                secondSelectedButton = button;
                pairTuple.secondPic = (button.Content as Image).Source.ToString();
                (button.Content as Image).Opacity = 1;

                if (pairTuple.firstPic == pairTuple.secondPic)
                {
                    firstSelectedButton = null;
                    secondSelectedButton = null;

                    absolutePairs--;
                    Rounds++;

                    if (absolutePairs == 0)
                    {
                        TimeSpan timeSpan = DateTime.Now - StartTime;
                        FrameContent.Visibility = Visibility.Visible;
                        FrameContent.Navigate(new ResultPage(player));
                        AddEntryToDatabase(Rounds, player.Name, timeSpan);
                    } //TODO Resultscreen
                    player.Points = (int)(Rounds * Int32.Parse(tbWidth.Text) * Int32.Parse(tbHeight.Text) /
                                     player.SolveTime / 100);
                    lblPoints.Content = player.Points;
                }
                else
                {
                    Rounds++;
                    player.Points = Rounds;
                }

                clickCount = !clickCount;
            }
        }


        private void ShuffleButtons(int columns, int rows, Random rndGen, List<int> availableBitmaps,
            List<Image> images)
        {
            for (int counter = 0; counter < columns * rows / 2; counter++)
            {
                int choosenBitmap = rndGen.Next(availableBitmaps.Count);
                int chosenImage;
                chosenImage = rndGen.Next(images.Count);
                images[chosenImage].Source = bitmaps[availableBitmaps[choosenBitmap]];
                images[chosenImage].Opacity = 0;
                images.RemoveAt(chosenImage);

                chosenImage = rndGen.Next(images.Count);
                images[chosenImage].Source = bitmaps[availableBitmaps[choosenBitmap]];
                images[chosenImage].Opacity = 0;
                images.RemoveAt(chosenImage);

                availableBitmaps.RemoveAt(choosenBitmap);
            }
        }


        void AddEntryToDatabase(int points, string playerName, TimeSpan timeSpan)
        {
            // entweder builder nutzen oder auf https://www.connectionstrings.com/ nachschauen
            SQLiteConnectionStringBuilder builder = new();
            builder.DataSource = "./memory.db";
            builder.Version = 3;
            builder.FailIfMissing = true;

            using (SQLiteConnection connection = new SQLiteConnection(builder.ToString()))
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText =
                    "insert into 'Scores' (Name, Points, SolveTime, TileNumber) values (@name, @points, @solveTime, @tileNumber);";
                command.Parameters.AddWithValue("name", playerName);
                command.Parameters.AddWithValue("points", points);
                command.Parameters.AddWithValue("solveTime", timeSpan.TotalMilliseconds);
                command.Parameters.AddWithValue("tileNumber", Int32.Parse(tbHeight.Text) * Int32.Parse(tbWidth.Text));

                if (command.ExecuteNonQuery() == 0)
                    throw new Exception();
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(tbWidth.Text, out int widht))
            {
                var button = (sender as Button);

                button.Background = Brushes.Red;
            }

            if (int.TryParse(tbHeight.Text, out int height))
            {
                var button = (sender as Button);

                button.Background = Brushes.Red;
            }

            if (widht * height < 67 && widht > 0 && height > 0 && (widht * height) % 2 == 0)
            {
                CreateGame(widht, height);
            }
        }
    }
}
