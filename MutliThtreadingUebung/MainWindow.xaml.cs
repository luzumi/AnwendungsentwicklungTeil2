using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;


namespace MutliThreadingUebung
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        CancellationTokenSource _sumCancelSource;
        readonly Progress<int> _sumProgress;
        byte[] _randomArray;
        private long _result;
        private Task[] _threads;
        int _progressPerMill;
        Object _sync = new();


        public MainWindow()
        {
            InitializeComponent();
            _sumProgress = new(UpdateProgressBar);
        }

        private void UpdateProgressBar(int pNewValue)
        {
            ProgressBar.Value = pNewValue;
        }

        private async void btnStart_Click(object pSender, RoutedEventArgs pE)
        {
            _randomArray = new byte[1_000_000];
            _result = 0;
            ProgressBar.Value = 0;
            _progressPerMill = 0;
            _threads = new Task[7];
            _sumCancelSource = new CancellationTokenSource();
            //CreateRandomArray
            //await CreateRandomArray(cores, partSize);
            CreateRandomArray(_sumProgress, _sumCancelSource.Token, 0, _randomArray.Length);

            //solange was in der queue ist, den threads in der Liste einen task mit einem queue geben
            int SegmentLaenge = _randomArray.Length / (7) +
                                _randomArray.Length % (7);

            {
                ArraySegment<byte> arrayS = (new ArraySegment<byte>(_randomArray, 0, SegmentLaenge));

                _threads[0] = new Task(() => SumPartArray(_sumProgress, _sumCancelSource.Token, arrayS));
            }

            int SegmentBase = _randomArray.Length / (7);

            for (int i = 0; i < _threads.Length - 1; i++)
            {
                var arrayS = new ArraySegment<byte>(_randomArray, SegmentLaenge + i * SegmentBase, SegmentBase);
                _threads[i + 1] = new Task(() => SumPartArray(_sumProgress, _sumCancelSource.Token, arrayS));
            }

            foreach (var item in _threads)
            {
                item.Start();
            }

            await Task.WhenAll(_threads);

            //SumRandomArray
            //await SumRandomArray(cores, partSize);

            //writeLabel
            Label.Content = _result.ToString();
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
        }

        
        private async Task SumRandomArray(int sliderVaule, int partSize)
        {
            _result = 0;
            _sumCancelSource = new CancellationTokenSource();

            for (int i = 0; i < sliderVaule; i++)
            {
                var arrayPart = i;
                _threads[i] = new Task(
                    () => SumRandomArray(_sumProgress, _sumCancelSource.Token, arrayPart * partSize,
                        arrayPart * partSize + partSize), _sumCancelSource.Token);

                _threads[i].Start();
            }

            await Task.WhenAll(_threads);
        }

        private void btnStop_Click(object pSender, RoutedEventArgs pE)
        {
            _sumCancelSource.Cancel();
        }

        private void CreateRandomArray(IProgress<int> pSumProgress, CancellationToken pToken,
            int pStart,
            int pEnd)
        {
            Random rndGen = new();
            for (int position = pStart; position < pEnd; position++)
            {
                _randomArray[position] = (byte)rndGen.Next(256);

                if (position % (_randomArray.Length / 1000) == 0)
                {
                    pSumProgress.Report(++_progressPerMill);

                    if (pToken.IsCancellationRequested)
                    {
                        return;
                    }
                }
            }
        }


        //private void CreateRandomArray(IProgress<int> pSumProgress, int pStart, int pEnd)
        //{
        //    Random rndGen = new();

        //    for (int position = pStart; position < pEnd; position++)
        //    {
        //        _randomArray[position] = (byte)rndGen.Next(256);
        //        if (position % (_randomArray.Length / 10) == 0)
        //        {
        //            pSumProgress.Report(++_progressPerMill);
        //        }
        //    }
        //}

        //private void CreateQueuedArray()
        //{
        //    int partSize = _randomArray.Length / 100;
        //    int endSize = _randomArray.Length / 100 + _randomArray.Length % 100;
        //    for (int i = 0; i < _randomArray.Length; i++)
        //    {
        //        CreateRandomArray(_sumProgress, i * partSize, i * endSize);
        //        if (_sumCancelSource.IsCancellationRequested)
        //        {
        //            return;
        //        }
        //    }
        //}

        private void SumPartArray(IProgress<int> pSumProgress, CancellationToken pToken,
            ArraySegment<byte> pArray)
        {
            long sum = 0;

            if (pArray != null)
            {
                for (int pos = 0; pos < pArray.Count; pos++)
                {
                    sum += pArray[pos];
                    if (pos % (pArray.Count / 10) == 0)
                    {
                        lock (_sync)
                        {
                            //TODO: alle threadanfragen kommen beim ersten anlauf gleichzeitig an
                            _progressPerMill += 1;
                            pSumProgress.Report(_progressPerMill);
                        }
                    }

                    if (pToken.IsCancellationRequested)
                    {
                        return;
                    }
                }
            }

            lock (_sync)
                _result += sum;
        }

        private void SumRandomArray(IProgress<int> pSumProgress, CancellationToken pToken, int pStart, int pEnd)
        {
            long sum = 0;

            for (int position = pStart; position < pEnd; position++)
            {
                sum += _randomArray[position];
                if (position % (_randomArray.Length / 1000) == 0)
                {
                    if (pToken.IsCancellationRequested)
                    {
                        return;
                    }

                    lock (_sync)
                    {
                        _progressPerMill += 1;
                        pSumProgress.Report(_progressPerMill);
                    }
                }
            }

            lock (_sync)
                _result += sum;
        }
    }
}
