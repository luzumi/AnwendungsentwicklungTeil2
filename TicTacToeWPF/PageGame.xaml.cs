﻿using System;
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
        private SoundPlayer validSoundX;
        private SoundPlayer validSoundO;
        private SoundPlayer invalidSound;
        private SoundPlayer winSound;
        private SoundPlayer tieSound;

        public PageGame()
        {
            InitializeComponent();
            game = new Spielfeld();
            //Sounds from https://soundimage.org/
            validSoundX =  new SoundPlayer(@"Sounds\custom_20.wav");
            validSoundO =  new SoundPlayer(@"Sounds\custom_21.wav");
            invalidSound = new SoundPlayer(@"Sounds\light-switch-pull-chain-daniel_simon.wav");
            tieSound =     new SoundPlayer(@"Sounds\LOOP24_172BPM.wav");
            winSound =     new SoundPlayer(@"Sounds\LOOP15_140BPM.wav");
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
                    invalidSound.Play();
                    return;
            }

            (pSender as Button).Content =
                game.Board[coordinates.X, coordinates.Y] == FieldState.X ? "X" : "O";

            (pSender as Button).Foreground = (pSender as Button).Content == "X" ? Brushes.Red : Brushes.Green;

            if (game.GetPlayerID()) { validSoundX.Play(); }
            else { validSoundO.Play(); }

            switch (tr)
            {
                case TurnResult.Tie:
                    imageBtTie.Visibility = Visibility.Visible;
                    imageTie.Visibility = Visibility.Visible;
                    tieSound.Play();
                    break;

                case TurnResult.Win:
                    imageBtWin.Visibility = Visibility.Visible;
                    imageWin.Visibility = Visibility.Visible;
                    winSound.Play();
                    break;
            }
        }

        private void Reset_onClick(object pSender, RoutedEventArgs pE)
        {
            game = new Spielfeld();
            imageBtWin.Visibility = Visibility.Hidden;
            imageBtTie.Visibility = Visibility.Hidden;
            bt00.Content = "";
            bt01.Content = "";
            bt02.Content = "";
            bt10.Content = "";
            bt11.Content = "";
            bt12.Content = "";
            bt20.Content = "";
            bt21.Content = "";
            bt22.Content = "";
        }
    }
}