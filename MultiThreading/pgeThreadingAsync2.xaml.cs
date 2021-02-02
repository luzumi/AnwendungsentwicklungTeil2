﻿using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MultiThreading
{
    /// <summary>
    /// Interaction logic for pgeThreadingAsync2.xaml
    /// </summary>
    public partial class pgeThreadingAsync2 : Page
    {
        public pgeThreadingAsync2()
        {
            InitializeComponent();
        }

        private async void btnAStart_Click(object sender, RoutedEventArgs e)
        {
            tblAusgabeA.Text = "running";
            tblAusgabeA.Text = await Task.Run(() => Wait5SecForString());
            tblAusgabeA.Text = await Task.Run(() => Wait5SecForString2());
        }
        private async void btnBStart_Click(object sender, RoutedEventArgs e)
        {
            tblAusgabeB.Text = "running";
            tblAusgabeB.Text = await Task.Run(() => Wait5SecForString());
            tblAusgabeB.Text = await Task.Run(() => Wait5SecForString2());
        }

        public static string Wait5SecForString()
        {
            Thread.Sleep(5000);
            return "Done";
        }
        public static string Wait5SecForString2()
        {
            Thread.Sleep(5000);
            return "Done 2";
        }
    }
}
