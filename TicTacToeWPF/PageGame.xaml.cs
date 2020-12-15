using System;
using System.Collections.Generic;
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
using TicTacToe;
using Button = System.Windows.Controls.Button;
using Point = TicTacToe.Point;

namespace TicTacToeWPF
{
    /// <summary>
    /// Interaktionslogik für PageGame.xaml
    /// </summary>
    public partial class PageGame : Page
    {
        private Spielfeld game;

        public PageGame()
        {
            InitializeComponent();
            game = new Spielfeld();
        }


        private void Bt_OnClick(object pSender, RoutedEventArgs pE)
        {
            Point coordinates;

            coordinates = (pSender as Button).Name switch
            {
                "bt00" => new Point(0, 0),
                "bt01" => new Point(0, 1),
                "bt02" => new Point(0, 2),
                "bt10" => new Point(1, 0),
                "bt11" => new Point(1, 1),
                "bt12" => new Point(1, 2),
                "bt20" => new Point(2, 0),
                "bt21" => new Point(2, 1),
                "bt22" => new Point(2, 2)
            };

            TurnResult tr = game.Turn(coordinates);

            switch (tr)
            {
                case TurnResult.Tie:
                    game.Board[coordinates.X, coordinates.Y] = 
                        !game.GetPlayerID() ? FieldState.X : FieldState.O;
                    break;
                case TurnResult.Win:
                    game.Board[coordinates.X, coordinates.Y] = 
                        !game.GetPlayerID() ? FieldState.X : FieldState.O;
                    break;
                case TurnResult.Valid:
                    game.Board[coordinates.X, coordinates.Y] = 
                        game.GetPlayerID() ? FieldState.X : FieldState.O;
                    break;
                case TurnResult.Invalid:
                    return;
            }

            (pSender as Button).Content = 
                game.Board[coordinates.X, coordinates.Y] == FieldState.X ? "X" : "O";
            
            switch (tr)
            {
                case TurnResult.Tie:
                    imageTie.Visibility = Visibility.Visible;
                    break;
                case TurnResult.Win:
                    imageWin.Visibility = Visibility.Visible;
                    break;
            }
        }
    }
}