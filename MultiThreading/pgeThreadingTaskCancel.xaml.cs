using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MultiThreading
{
    /// <summary>
    /// Interaction logic for pgeThreadingTaskCancel.xaml
    /// </summary>
    public partial class pgeThreadingTaskCancel : Page
    {
        Task<int> worker;
        public pgeThreadingTaskCancel()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            keepRunning = true;
            rectOut.Fill = Brushes.Blue;
            worker = new Task<int>(waitAndColor);
            worker.Start();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            keepRunning = false;
        }

        bool keepRunning = true;

        private int waitAndColor()
        {
            int counter = 0;
            while (counter < 1000000000 && keepRunning)
            {
                counter++;
            }
            Dispatcher.Invoke(() => rectOut.Fill = Brushes.Green);
            Dispatcher.Invoke(() => tblOut.Text = counter.ToString());
            return counter;
        }
    }
}
