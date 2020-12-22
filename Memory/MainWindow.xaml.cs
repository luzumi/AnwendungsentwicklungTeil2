using System;
using System.Collections.Generic;
using System.Data.SQLite;
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

        List<BitmapImage> bitmaps;
        private bool clickCount;
        private Button firstSelectedButton = new();
        private Button secondSelectedButton = new();
        (string firstPic, string secondPic) pairTuple = new();
        private int absolutePairs;
        private int rounds = 0;

        public MainWindow()
        {
            InitializeComponent();


            bitmaps = new List<BitmapImage>();

            // alle bilder laden
            foreach (var fileName in Directory.GetFiles("Images"))
            {
                BitmapImage tempBitmap = new(); // neues Bild erstellen
                tempBitmap.BeginInit();// füllen des Bildes starten
                tempBitmap.UriSource = new Uri( Directory.GetParent(Environment.CommandLine).FullName + @"\" + fileName);// bildinhalt aus datei laden
                tempBitmap.EndInit();// füllen des Bildes finalisieren
                bitmaps.Add(tempBitmap);
            }

        }

        void CreateGame(int columns, int rows)
        {
            // clear

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
                    Image tempImage = new Image
                    {
                        Stretch = Stretch.UniformToFill
                    };
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
                
                pairTuple.firstPic = (firstSelectedButton.Content as Image).Source.ToString() ;
                
                (button.Content as Image).Opacity = 1;
                
                clickCount = !clickCount;
            }
            else
            {
                secondSelectedButton = button;
                pairTuple.secondPic = (button.Content as Image).Source.ToString();
                (button.Content as Image).Opacity = 1;
                
                //TODO: vergleich 

                if (pairTuple.firstPic == pairTuple.secondPic)
                {
                    firstSelectedButton = null;
                    secondSelectedButton = null;
                    absolutePairs--;
                    rounds++;
                    
                    if (absolutePairs == 0) {} //TODO Resultscreen
                }
                else
                {
                    rounds++;
                }
                
                clickCount = !clickCount;
            }
        }


        private async Task delayHideAsync(Button buttonA, Button buttonB)
        {
            await Task.Delay(2000);
            (buttonA.Content as Image).Opacity = 0;
            (buttonB.Content as Image).Opacity = 0;
            
        }

        private void ShuffleButtons(int columns, int rows, Random rndGen, List<int> availableBitmaps, List<Image> images)
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


        void ReadDatabase()
        {

            // entweder builder nutzen oder auf https://www.connectionstrings.com/ nachschauen
            SQLiteConnectionStringBuilder builder = new();
            builder.DataSource = "./highscore.db";
            builder.Version = 3;
            /*
            MySqlConnectionStringBuilder connectionStringBuilder = new MySqlConnectionStringBuilder(); // Der Builder kann uns den passenden Connectionstring zusammensetzen sodass Syntaxfehler minimiert werden
            connectionStringBuilder.Server = "192.168.2.2"; // IP Adresse des servers, DNS name, Localhost und . funktioniert auch
            connectionStringBuilder.UserID = "MusicDBUser"; // Benutzername innerhalb des DBMS, dieser nutzer sollte so wenig rechte wie möglich bekommen
            connectionStringBuilder.Password = "MusicDBPass"; // Passwort zu dem Benutzernamen
            connectionStringBuilder.Database = "musicdb"; // Datenbankname mit der sich verbunden werden soll, alle SQL statements sind dann relativ zu dieser Datenbank (siehe USE )
            connectionStringBuilder.SslMode = MySqlSslMode.None; // None ist ok für testumgebungen, im Internet immer verschlüsseln. Benötigt extra CPU-Leistung
            */

            List<Pair> highScore = new();

            using (SQLiteConnection connection = new SQLiteConnection(builder.ToString()))
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "select rowid, Name, Points from Scores order by Points desc top 10;";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // angenommende Tabelle:
                        // ID, Name, Points
                        // 1 , Hans, 20
                        // 2 , Lisa, 18
                        Pair temp = new();
                        temp.Rank = reader.GetInt32(0);
                        temp.Name = reader.GetString(1);
                        temp.Points = reader.GetInt32(2);
                        highScore.Add(temp);
                    }
                }
            }

            void AddEntryToDatabase(int points, string playerName)
            {
                // entweder builder nutzen oder auf https://www.connectionstrings.com/ nachschauen
                SQLiteConnectionStringBuilder builder = new();
                builder.DataSource = "./highscore.db";
                builder.Version = 3;

                using (SQLiteConnection connection = new SQLiteConnection(builder.ToString()))
                {
                    connection.Open();

                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandText = "insert into Scores (Name, Points) values (@name, @points);";
                    command.Parameters.AddWithValue("name", playerName);
                    command.Parameters.AddWithValue("points", points);

                    if (command.ExecuteNonQuery() == 0)
                        throw new Exception();
                }
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