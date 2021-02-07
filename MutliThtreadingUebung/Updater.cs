using System;
using System.Windows.Controls;

namespace MutliThreadingUebung
{
    class ArrayPackage
    {
        public ProgressBar ProgressBar;
        public Progress<int> SumProgress;

        public ArrayPackage()
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
