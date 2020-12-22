using System;
using System.Collections.Generic;
using System.Data.SQLite;
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

namespace Memory
{
    /// <summary>
    /// Interaktionslogik für ResultPage.xaml
    /// </summary>
    public partial class ResultPage : Page
    {
        private Pair player;

        public ResultPage()
        {
            InitializeComponent();
        }

        public ResultPage(Pair player)
        {
            this.player = player;
            InitializeComponent();

            ReadDatabase(player, getActualPlayerInfos(player.Name));
            lblRank.Content = "Rank: " + player.Rank;
            lblResultPoints.Content = "Points: " + player.Points;
            lblSolvedDate.Content = "Gespielt am: \n" + player.solveDate;
            lblSolvedTime.Content = "benötigte Zeit: " + Math.Round(player.SolveTime / 1000, 3);
            lblTileNumbers.Content = "Anzahl Bilder: " + player.tileNumber/2;
        }
        
        public void ReadDatabase(Pair actPair, string SqlStatement)
        {

            // entweder builder nutzen oder auf https://www.connectionstrings.com/ nachschauen
            SQLiteConnectionStringBuilder builder = new();
            builder.DataSource = "./memory.db";
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
                command.CommandText = SqlStatement;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // angenommende Tabelle:
                        // ID, Name, Points, SolveTime, TileNumber, SolveDate
                        // 1 , Hans,    20,     12.09,  12,         21.12.2020
                        // 2 , Lisa,    18,     16.34,  14          22.12.2020

                        actPair.Points = reader.GetInt32(2);
                        actPair.Rank = reader.FieldCount;
                        actPair.SolveTime = reader.GetDouble(3);
                        actPair.tileNumber = reader.GetInt32(4);
                        actPair.solveDate = reader.GetDateTime(5);
                        
                        highScore.Add(actPair);
                    }
                }
            }
        }

        string getActualPlayerInfos(string name)
        {
            return $"Select * from Scores Where Name = '{name}' Order by SolveTime desc";
        }

        
    }
}
