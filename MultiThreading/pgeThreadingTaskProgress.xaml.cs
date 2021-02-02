using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MultiThreading
{
    /// <summary>
    /// Interaction logic for PgeThreadingTaskProgress.xaml
    /// </summary>
    public partial class PgeThreadingTaskProgress
    {
        Task<int> _worker;
        readonly Progress<int> _progressCom;

        public PgeThreadingTaskProgress()
        {
            InitializeComponent();
            _progressCom = new Progress<int>(RefreshProgressBar);
        }


        private async void btnStart_Click(object pSender, RoutedEventArgs pE)
        {
            tbOut.Background = Brushes.Blue;
            RefreshProgressBar(0);
            _worker = new Task<int>(() => waitAndColor(_progressCom));
            _worker.Start();
            await Task.WhenAll(_worker);
            tbOut.Background = Brushes.Green;
            tbOut.Text = _worker.Result.ToString();
        }


        private void RefreshProgressBar(int pReportedProgress)
        {
            pbProgress.Value = pReportedProgress;
        }


        private int waitAndColor(IProgress<int> pProgress)
        {
            int counter = 0;
            while (counter < 1000_000_000)
            {
                counter++;
                if (counter % 1000_000 == 0)
                {
                    pProgress.Report(counter / 1000_000);
                }
            }
            return counter;
        }
    }
}
