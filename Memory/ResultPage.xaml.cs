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

            lblResultPoints.Content = player.Points.ToString();
        }
    }
}
