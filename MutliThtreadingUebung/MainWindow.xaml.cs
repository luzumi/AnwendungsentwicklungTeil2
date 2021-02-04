using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


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
        private Task<long>[] _threads;
        private Task[] _threadsWithoutReturn;
        int _progressPerMill;
        Object _sync = new();
        private int _core = 7;
        private Updater[] updater;


        public MainWindow()
        {
            InitializeComponent();
            updater = new Updater[_core];
            _sumProgress = new(UpdateProgressBar);
        }

        private void UpdateProgressBar(int pNewValue)
        {
            ProgressBar.Value = pNewValue;
        }

        private async void btnStart_Click(object pSender, RoutedEventArgs pE)
        {
            _randomArray = new byte[100_000_000];
            _result = 0;
            _progressPerMill = 0;
            _threads = new Task<long>[_core];
            _threadsWithoutReturn = new Task[_core];
            _sumCancelSource = new CancellationTokenSource();

            await Task.Run(CreateArraySegmentedAsync)
                .ConfigureAwait(true);

            for (int i = 0; i < _core; i++)
            {
                CreateProgressBar(i);
            }

            _result = await Task.Run(SumPartitionalArrayWithThreadsAsync).ConfigureAwait(true);

            Label.Content = _result.ToString();
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
        }

        private long SumPartitionalArrayWithThreadsAsync()
        {
            int segmentLenght;
            int segmentBase;
            for (int i = 0; i < _threads.Length; i++)
            {
                segmentLenght = _randomArray.Length / (_core) + _randomArray.Length % (_core);
                segmentBase = _randomArray.Length / (_core);
                int count = i;
                var arrayS = new ArraySegment<byte>(_randomArray,
                    count == 0 ? 0 : (segmentLenght + (count - 1) * segmentBase),
                    count == 0 ? segmentLenght : segmentBase);

                _threads[count] = new Task<long>(() =>
                    SumPartArray(updater[count].SumProgress, _sumCancelSource.Token, arrayS));
            }

            foreach (var item in _threads)
            {
                item?.Start();
            }

            Task.WhenAll(_threads);
            long result = 0;
            foreach (var thread in _threads)
            {
                // ReSharper disable once AsyncConverter.AsyncWait
                result += thread.Result;
            }

            return result;
        }

        private void CreateProgressBar(int count)
        {
            updater[count] = new Updater();
            Grid.SetColumn(updater[count].ProgressBar, count); // button in spalte positionieren
            Grid.SetRow(updater[count].ProgressBar, 1); // button in zeile positionieren
            updater[count].ProgressBar.Margin = new Thickness(3, 0, 0, 0);
            Progressbars.Children.Add(updater[count].ProgressBar);
        }
        
        private async Task SumRandomArray(int sliderVaule, int partSize)
        {
            _result = 0;
            _sumCancelSource = new CancellationTokenSource();

            for (int i = 0; i < sliderVaule; i++)
            {
                var arrayPart = i;
                _threads[i] = new Task<long>(
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

        private void CreateRandomArray(IProgress<int> pSumProgress, CancellationToken pToken, ArraySegment<byte> pArrayS)
        {
            Random rndGen = new();
            for (int position = 0; position < pArrayS.Count; position++)
            {
                pArrayS[position] = (byte)rndGen.Next(256);
                if (position % (_randomArray.Length / 1000) == 0)
                {
                    _progressPerMill += 1;
                    pSumProgress.Report(++_progressPerMill);

                    if (pToken.IsCancellationRequested)
                    {
                        return;
                    }
                }
            }

        }

        private void CreateArraySegmentedAsync()
        {
            int segmentLenght;
            int segmentBase;
            for (int i = 0; i < _threads.Length; i++)
            {
                int count = i;
                segmentLenght = _randomArray.Length / (_core) + _randomArray.Length % (_core);
                segmentBase = _randomArray.Length / (_core);
               
                var arrayS = new ArraySegment<byte>(_randomArray,
                    count == 0 ? 0 : (segmentLenght + (count - 1) * segmentBase),
                    count == 0 ? segmentLenght : segmentBase);

                _threadsWithoutReturn[count] = new Task(() => CreateRandomArray(_sumProgress, _sumCancelSource.Token, arrayS));

            }

            foreach (var item in _threadsWithoutReturn)
            {
                item?.Start();
            }
        }

        private long SumPartArray(IProgress<int> pSumProgress, CancellationToken pToken,
            ArraySegment<byte> pArray)
        {
            long sum = 0;
            _progressPerMill = 0;
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
                        return sum;
                    }
                }
            }

            return sum;
        }

        private long SumRandomArray(IProgress<int> pSumProgress, CancellationToken pToken, int pStart, int pEnd)
        {
            long sum = 0;

            for (int position = pStart; position < pEnd; position++)
            {
                sum += _randomArray[position];
                if (position % (_randomArray.Length / 1000) == 0)
                {
                    if (pToken.IsCancellationRequested)
                    {
                        return sum;
                    }

                    lock (_sync)
                    {
                        _progressPerMill += 1;
                        pSumProgress.Report(_progressPerMill);
                    }
                }
            }

            return sum;
        }
    }
}
