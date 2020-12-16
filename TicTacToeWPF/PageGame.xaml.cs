using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TicTacToe;
using Button = System.Windows.Controls.Button;
using Point = TicTacToe.Point;

namespace TicTacToeWPF
{
    /// <summary>
    ///     Interaktionslogik für PageGame.xaml
    /// </summary>
    public partial class PageGame
    {
        private Spielfeld _game;
        private readonly SoundPlayer _invalidSound;
        private readonly SoundPlayer _tieSound;
        private readonly SoundPlayer _validSoundO;
        private readonly SoundPlayer _validSoundX;
        private readonly SoundPlayer _winSound;

        public PageGame()
        {
            InitializeComponent();
            _game = new Spielfeld();
            //Sounds from https://soundimage.org/
            _validSoundX = new SoundPlayer(@"Sounds\custom_20.wav");
            _validSoundO = new SoundPlayer(@"Sounds\custom_21.wav");
            _invalidSound = new SoundPlayer(@"Sounds\light-switch-pull-chain-daniel_simon.wav");
            _tieSound = new SoundPlayer(@"Sounds\LOOP24_172BPM.wav");
            _winSound = new SoundPlayer(@"Sounds\LOOP15_140BPM.wav");
        }


        private void Bt_OnClick(object pSender, RoutedEventArgs pE)
        {
            Point coordinates;

            coordinates = ((Button) pSender).Name switch
            {
                "Bt00" => new Point(0, 0),
                "Bt01" => new Point(0, 1),
                "Bt02" => new Point(0, 2),
                "Bt10" => new Point(1, 0),
                "Bt11" => new Point(1, 1),
                "Bt12" => new Point(1, 2),
                "Bt20" => new Point(2, 0),
                "Bt21" => new Point(2, 1),
                _ => new Point(2, 2)
            };

            var tr = _game.Turn(coordinates);

            switch (tr)
            {
                case TurnResult.Tie:
                    _game.Board[coordinates.X, coordinates.Y] =
                        !_game.GetPlayerID() ? FieldState.X : FieldState.O;
                    break;

                case TurnResult.Win:
                    _game.Board[coordinates.X, coordinates.Y] =
                        !_game.GetPlayerID() ? FieldState.X : FieldState.O;
                    break;

                case TurnResult.Valid:
                    _game.Board[coordinates.X, coordinates.Y] =
                        _game.GetPlayerID() ? FieldState.X : FieldState.O;
                    break;

                case TurnResult.Invalid:
                    _invalidSound.Play();
                    return;
            }

            (((pSender as Button).Content as Grid).Children[1] as Label).Content =
                _game.Board[coordinates.X, coordinates.Y] == FieldState.X ? "X" : "O";

            (((pSender as Button).Content as Grid).Children[1] as Label).Foreground =
                (((pSender as Button).Content as Grid).Children[1] as Label).Content.ToString() == "X"
                    ? Brushes.Red
                    : Brushes.Green;

            if (_game.GetPlayerID())
                _validSoundX.Play();
            else
                _validSoundO.Play();

            switch (tr)
            {
                case TurnResult.Tie:
                    ImageBtTie.Visibility = Visibility.Visible;
                    ImageTie.Visibility = Visibility.Visible;
                    _tieSound.Play();
                    break;

                case TurnResult.Win:
                    ImageBtWin.Visibility = Visibility.Visible;
                    ImageWin.Visibility = Visibility.Visible;
                    _winSound.Play();
                    break;
            }
        }

        private void Reset_onClick(object pSender, RoutedEventArgs pE)
        {
            _game = new Spielfeld();
            ImageBtWin.Visibility = Visibility.Hidden;
            ImageBtTie.Visibility = Visibility.Hidden;
            ((Bt00.Content as Grid).Children[1] as Label).Content = "";
            ((Bt01.Content as Grid).Children[1] as Label).Content = "";
            ((Bt02.Content as Grid).Children[1] as Label).Content = "";
            ((Bt10.Content as Grid).Children[1] as Label).Content = "";
            ((Bt11.Content as Grid).Children[1] as Label).Content = "";
            ((Bt12.Content as Grid).Children[1] as Label).Content = "";
            ((Bt20.Content as Grid).Children[1] as Label).Content = "";
            ((Bt21.Content as Grid).Children[1] as Label).Content = "";
            ((Bt22.Content as Grid).Children[1] as Label).Content = "";
        }
    }
}