using System;
using System.Windows.Controls;

namespace MutliThreadingUebung
{
    class Updater
    {
        public ProgressBar ProgressBar;
        public Progress<int> SumProgress;

        public Updater()
        {
            ProgressBar = new ProgressBar();
            SumProgress = new(Report);
        }

        public void Report(int pNewValue)
        {
            ProgressBar.Value = pNewValue;
        }
    }
}
