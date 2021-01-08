using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
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
        (string firstPic, string secondPic) pairTuple;
        private int absolutePairs;
        private int turns;
        public DateTime startTime;
        public static (int widht, int height) coord;

        
        public DateTime StartTime
        {
            get => startTime;
            set => startTime = value;
        }

        public int Turns
        {
            get => turns;
            set
            {
                turns = value;
                lblPoints.Content = "Turns: " + turns.ToString();
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            bitmaps = new List<BitmapImage>();
            
            LoadAllPictures();
        }

        /// <summary>
        /// Lädt alle MemorieBilder aus dem Verzeichnis "Images"
        /// </summary>
        private void LoadAllPictures()
        {
            foreach (var fileName in Directory.GetFiles("Images"))
            {
                BitmapImage tempBitmap = new(); // neues Bild erstellen
                tempBitmap.BeginInit(); // füllen des Bildes starten
                tempBitmap.UriSource =
                    new Uri(Directory.GetParent(Environment.CommandLine)?.FullName + @"\" +
                            fileName); // bildinhalt aus datei laden
                tempBitmap.EndInit(); // füllen des Bildes finalisieren
                bitmaps.Add(tempBitmap);
            }
        }

        /// <summary>
        /// Erstellt ein neues befülltes Spielfeld
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="rows"></param>
        void CreateGame(int columns, int rows)
        {
            // clear
            coord = (columns, rows);
            player = new();
            Turns = 0;
            FrameContent.Visibility = Visibility.Collapsed;
            Spielfeld.Children.Clear();
            Spielfeld.ColumnDefinitions.Clear();
            Spielfeld.RowDefinitions.Clear();

            // Linkedliste mit erlaubten image ziffern erstellen
            CreateListOfAllAllowedNumbers(columns, rows);

            player.Name = tfName.Text;
            player.Points = 0;
            tfName.SelectAll();

            StartTime = DateTime.Now;
        }

        /// <summary>
        /// Fügt Ausgewählte Bilder zufällig in die entsprecheden Buttons
        /// </summary>
        /// <param name="columns">SpielfeldSpalte</param>
        /// <param name="rows">SpielfeldZeile</param>
        private void CreateListOfAllAllowedNumbers(int columns, int rows)
        {
            List<int> availableBitmaps = new List<int>();
            for (int counter = 0; counter < bitmaps.Count; counter++)
                availableBitmaps.Add(counter);

            List<Image> images = new List<Image>();
            // recreate
            DefineRowCol(columns, rows);

            var rndGen = CreateButtonsRandom(columns, rows, images);

            ShuffleButtons(columns, rows, rndGen, availableBitmaps, images);
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

        /// <summary>
        /// entsprechend des gedrückten Buttons wird der Button ausgewertet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Click(object sender, RoutedEventArgs e)
        {
            var button = (sender as Button);

            if (!clickCount)
            {
                SetOpacityIfTwoButtonsSelected();

                firstSelectedButton = button;

                ChangeFirstSelectedButton(button);

                Turns++;
                
                clickCount = !clickCount;
            }
            else
            {
                ChangeSecondSelectedButton(button);

                CoupleIsFound();

                clickCount = !clickCount;
            }
        }

        /// <summary>
        /// zeigt den ausgewählten zweiten Button korrekt an
        /// </summary>
        /// <param name="button"></param>
        private void ChangeSecondSelectedButton(Button button)
        {
            secondSelectedButton = button;
            if (button != null)
            {
                pairTuple.secondPic = (button.Content as Image)?.Source.ToString();
                ((Image) button.Content).Opacity = 1;
            }
        }

        
        /// <summary>
        /// Auswertung bei einem gefundenen Paar
        /// </summary>
        private void CoupleIsFound()
        {
            //TODO: doppeltes Klicken des gleichen Buttons ausschliessen
            if (pairTuple.firstPic == pairTuple.secondPic)
            {
                firstSelectedButton = null;
                secondSelectedButton = null;

                absolutePairs--;
                Turns++;

                CheckAllCouplesAreFoundet();
                //TODO Resultscreen

                lblPoints.Content = player.Points;
            }
            else
            {
                Turns++;
            }
        }

        /// <summary>
        /// auslösen der Resultpage
        /// </summary>
        private void CheckAllCouplesAreFoundet()
        {
            if (absolutePairs == 0)
            {
                TimeSpan timeSpan = DateTime.Now - StartTime;
                FrameContent.Visibility = Visibility.Visible;
                AddEntryToDatabase(Turns, player.Name, timeSpan);
                FrameContent.Navigate(new ResultPage(player));
            }
        }

        /// <summary>
        /// erster Button ist geklickt und wird korrekt dargestellt
        /// </summary>
        /// <param name="button"></param>
        private void ChangeFirstSelectedButton(Button button)
        {
            if (firstSelectedButton != null)
            {
                pairTuple.firstPic = (firstSelectedButton.Content as Image)?.Source.ToString();
            }

            if (button != null)
            {
                ((Image) button.Content).Opacity = 1;
            }
        }

        /// <summary>
        /// sind beide Buttons gesetzt, werden sie Opacity = 0 gesetzt
        /// </summary>
        private void SetOpacityIfTwoButtonsSelected()
        {
            if (secondSelectedButton != null && firstSelectedButton != null)
            {
                if (firstSelectedButton.Content != null && secondSelectedButton.Content != null)
                {
                    ((Image) firstSelectedButton.Content).Opacity = 0;
                    ((Image) secondSelectedButton.Content).Opacity = 0;
                }
            }
        }

        /// <summary>
        /// setzt versteckt die möglichen Bilder auf die Buttons
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="rows"></param>
        /// <param name="rndGen"></param>
        /// <param name="availableBitmaps"></param>
        /// <param name="images"></param>
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

        /// <summary>
        /// schreibt gewonnenes Spiel nach Punkteberechnung in die Datenbank
        /// </summary>
        /// <param name="points"></param>
        /// <param name="playerName"></param>
        /// <param name="timeSpan"></param>
        private void AddEntryToDatabase(int points, string playerName, TimeSpan timeSpan)
        {
            // entweder builder nutzen oder auf https://www.connectionstrings.com/ nachschauen
            SQLiteConnectionStringBuilder builder = new();
            builder.DataSource = "./memory.db";
            builder.Version = 3;
            builder.FailIfMissing = true;
            
            //TODO: calculate points
            points = (int)(MainWindow.coord.widht * (double)MainWindow.coord.height / points * 100) ;
            
            

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

                if (button != null)
                {
                    button.Background = Brushes.Red;
                }
            }

            if (int.TryParse(tbHeight.Text, out int height))
            {
                var button = (sender as Button);

                if (button != null)
                {
                    button.Background = Brushes.Red;
                }
            }

            if (widht * height < 67 && widht > 0 && height > 0 && (widht * height) % 2 == 0)
            {
                CreateGame(widht, height);
            }
        }
    }
}
