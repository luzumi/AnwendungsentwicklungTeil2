using System;
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
        byte[] _randomArray;
        private long _result;
        private Task<long>[] _threads;
        private Task[] _threadsWithoutReturn;
        int _progressPerMill;
        Object _sync = new();
        private int _core = 7;
        private ArrayPackage<long, byte>[] _updater;
        const int parts = 100;
        private int _refreshRate;
        public int RefreshRate {
            get                         //1_000_000.
                                        //10000
            {
                _refreshRate = _randomArray.Length / parts / 50;
                return _refreshRate;
            }
            set => _refreshRate = value;
        }

        public int WorkPackageNumber { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void UpdateProgressBar(byte pNewValue)
        {
            ProgressBar.Value = pNewValue;
        }

        private async void btnStart_Click(object pSender, RoutedEventArgs pE)
        {
            _randomArray = new byte[1_000_000];
            _result = 0;
            _progressPerMill = 0;
            _threads = new Task<long>[_core];
            _threadsWithoutReturn = new Task[_core];
            _sumCancelSource = new CancellationTokenSource();
            _updater = new ArrayPackage<long, byte>[parts]; //_core];

            CreateProgressBars(parts);


            await Task.Run(() => CreateArraySegmentedAsync(_updater, _sumCancelSource.Token))
                .ConfigureAwait(true);

            _result += await Task.Run(() => GetArrayResult(_updater, _sumCancelSource.Token))
                .ConfigureAwait(true);


            Label.Content = _result.ToString();
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
        }

        private void btnStop_Click(object pSender, RoutedEventArgs pE)
        {
            _sumCancelSource.Cancel();
        }


        private void CreateProgressBars(int pParts)
        {
            int segmentLength = _randomArray.Length / pParts;
            for (int i = 0; i < pParts; i++)
            {
                int ii = i;
                int offset = (ii == 0)
                    ? 0
                    : (ii - 1) * segmentLength + (segmentLength + _randomArray.Length % pParts);
                int count = segmentLength + _randomArray.Length % pParts;
                _updater[ii] = new ArrayPackage<long, byte>(new ArraySegment<byte>(_randomArray, offset, count));
                Progressbars.Children.Add(_updater[ii].ProgressBar);
            }
        }

        private void CreateArraySegmentedAsync(ArrayPackage<long, byte>[] pPackage, CancellationToken pToken)
        {
            for (int i = 0; i < pPackage.Length; i++)
            {
                var partIndex = i;
                if (_threads != null)
                {
                    _threadsWithoutReturn[partIndex % _core] = new Task(() => FillPackage(pPackage, pToken, partIndex: partIndex),
                        _sumCancelSource.Token);

                    _threadsWithoutReturn[partIndex % _core].Start();
                }

                if (pToken.IsCancellationRequested)
                    return;

                if (partIndex % _core == _core - 1)
                {
                    Task.WhenAny(_threadsWithoutReturn!);
                }

                //FillPackage(pPackage, pToken, partIndex);
            }
        }

        private void FillPackage(ArrayPackage<long, byte>[] pPackage, CancellationToken pToken, int partIndex)
        {
            _progressPerMill = 0;
            Random rndGen = new();
            int length = pPackage[partIndex].Segment.Count;
            for (int i = 0; i < length; i++)
            {
                pPackage[partIndex].Segment[i] = 1; //(byte)rndGen.Next(256);
                
                if (i % RefreshRate == 0) pPackage[partIndex].Progress.Report(++_progressPerMill);

                if (pToken.IsCancellationRequested)
                {
                    return;
                }
            }
        }

        private long GetArrayResult(ArrayPackage<long, byte>[] pSegmentArray, CancellationToken pToken)
        {
            long sum = 0;
            for (int i = 0; i < parts; i++)
            {
                var partIndex = i;
                if (_threads != null)
                {
                    _threads[partIndex % _core] = new Task<long>(() => (SumRandomArray(_updater[partIndex], pToken)),
                        _sumCancelSource.Token);

                    _threads[partIndex % _core].Start();
                }

                if (pToken.IsCancellationRequested)
                    return pSegmentArray[partIndex].Result;

                if (partIndex % _core == _core - 1)
                {
                    Task.WhenAll(_threads!);
                    foreach (var item in _threads)
                    {
                        sum += item.Result;
                    }
                }
            }

            return sum;
        }

        private long SumRandomArray(ArrayPackage<long, byte> pPackage, CancellationToken pToken)
        {
            _sumCancelSource = new CancellationTokenSource();

            for (int i = 0; i < pPackage.Segment.Count; i++)
            {
                var arrayIndex = i;
                pPackage.Result += pPackage.Segment[arrayIndex];

                if (pToken.IsCancellationRequested)
                    return pPackage.Result;
                if (i % RefreshRate == 0) pPackage.Progress.Report(++_progressPerMill);
            }

            return pPackage.Result;
        }

        #region OldMethods

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

        long SumPartitionalArrayWithThreadsAsync(int pPackage, CancellationToken pToken)
        {
            long sumThread = 0;
            int package = pPackage;

            if (_threads != null)
            {
                _threads[package % _core] = new Task<long>(() => GetArrayResult(_updater, pToken),
                    _sumCancelSource.Token);

                _threads[package % _core].Start();
            }

            if (pToken.IsCancellationRequested)
                return sumThread;

            if (package % _core == _core - 1)
            {
                if (_threads != null)
                {
                    Task.WhenAll(_threads);
                    var index = 0;
                    for (; index < _threads.Length; index++)
                    {
                        var b = _threads[index];
                        sumThread += b.Result;
                    }
                }
            }


            if (package % 2 == 0)
                _updater[package].Progress.Report(++_progressPerMill);

            return sumThread;
        }

        private long SumPartArray(IProgress<int> pSumProgress, CancellationToken pToken, ArraySegment<byte> pArray)
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

        private void CreateRandomArray(IProgress<int> pSumProgress, CancellationToken pToken,
            ArraySegment<byte> pArrayS)
        {
            Random rndGen = new();
            for (int position = 0; position < pArrayS.Count; position++)
            {
                pArrayS[position] = (byte)rndGen.Next(256);
                if (position % (_randomArray.Length / 100) == 0)
                {
                    pSumProgress.Report(++_progressPerMill);

                    if (pToken.IsCancellationRequested)
                    {
                        return;
                    }
                }
            }
        }

        private void CreateProgressBar(int count)
        {
            _updater[count] = new ArrayPackage<long, byte>(_randomArray);
            Progressbars.Children.Add(_updater[count].ProgressBar);
        }

        #endregion
    }
}
