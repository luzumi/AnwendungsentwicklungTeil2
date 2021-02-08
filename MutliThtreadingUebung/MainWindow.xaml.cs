using System;
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
        byte[] _randomArray;
        private long _result;
        private List<Task<long>> _threads;
        private Task[] _threadsWithoutReturn;
        int _progressPerMill;
        readonly Object _sync = new();
        private readonly int _core = Environment.ProcessorCount - 1;
        private ArrayPackage<long, byte>[] _updater;
        const int Parts = 300;
        private int _refreshRate;

        public int RefreshRate
        {
            get //1_000_000.
            //10000
            {
                _refreshRate = _randomArray.Length / Parts / 50;
                return _refreshRate;
            }
            set => _refreshRate = value;
        }

        public int WorkPackageNumber { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void btnStart_Click(object pSender, RoutedEventArgs pE)
        {
            _randomArray = new byte[1_000_000_000];
            _result = 0;
            _progressPerMill = 0;
            _threads = new List<Task<long>>(_core);
            _threadsWithoutReturn = new Task[_core];
            _sumCancelSource = new CancellationTokenSource();
            _updater = new ArrayPackage<long, byte>[Parts]; //_core];

            CreateProgressBars(Parts);


            await Task.Run(() => CreateArraySegmentedAsync(_updater, _sumCancelSource.Token))
                .ConfigureAwait(true);

            _result += await Task.Run(() => GetArrayResult(_sumCancelSource.Token), _sumCancelSource.Token)
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
            int offset = 0;
            int segmentLength = _randomArray.Length / pParts;
            for (int i = 0; i < pParts; i++)
            {
                int count;

                switch (i)
                {
                    case 0:
                        offset = 0;
                        count = segmentLength + _randomArray.Length % pParts;
                        break;

                    default:
                        offset = i * segmentLength + (_randomArray.Length % pParts);
                        count = segmentLength;
                        break;
                }

                _updater[i] = new ArrayPackage<long, byte>(new ArraySegment<byte>(_randomArray, offset, count));
                Progressbars.Children.Add(_updater[i].ProgressBar);
            }
        }

        private void CreateArraySegmentedAsync(ArrayPackage<long, byte>[] pPackage, CancellationToken pToken)
        {
            for (int i = 0; i < pPackage.Length; i++)
            {
                var partIndex = i;
                if (i == 250)
                    partIndex = i;
                if (_threadsWithoutReturn != null)
                {
                    var data = pPackage[partIndex];
                    _threadsWithoutReturn[partIndex % _core] = new Task(
                        () => FillPackage(data, pToken),
                        _sumCancelSource.Token);

                    _threadsWithoutReturn[partIndex % _core].Start();
                }

                if (pToken.IsCancellationRequested)
                    return;

                if (partIndex % _core == _core - 1)
                {
                    Task.WhenAll(_threadsWithoutReturn!);
                }

                //FillPackage(pPackage, pToken, partIndex);
            }

            Task.WhenAll(_threadsWithoutReturn!);
        }

        private void FillPackage(ArrayPackage<long, byte> pPackage, CancellationToken pToken)
        {
            _progressPerMill = 0;
            Random rndGen = new();
            int length = pPackage.Segment.Count;
            for (int i = 0; i < length; i++)
            {
                pPackage.Segment[i] = (byte)rndGen.Next(256);

                if (i % RefreshRate == 0) pPackage.Progress.Report(++_progressPerMill);

                if (pToken.IsCancellationRequested)
                {
                    return;
                }
            }
        }

        private long GetArrayResult(CancellationToken pToken)
        {
            long sum = 0;
            for (int i = 0; i < Parts; i++)
            {
                var partIndex = i;
                var data = _updater[partIndex];
                if (_threads != null)
                {
                    _threads.Add(Task.Run(() => (SumRandomArray(data, pToken)),
                        _sumCancelSource.Token));
                }

                if (pToken.IsCancellationRequested)
                    return data.Result;

                if (partIndex % _core == _core - 1)
                {
                    Task.WhenAll(_threads!);
                    foreach (var item in _threads)
                    {
                        sum += item.Result;
                    }

                    _threads.Clear();
                }
            }

            Task.WhenAll(_threads!);

            foreach (var item in _threads)
            {
                sum += item.Result;
            }

            //sum += item.Result; //fehlt
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

        private void UpdateProgressBar(byte pNewValue)
        {
            ProgressBar.Value = pNewValue;
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

        long SumParthhitionalArrayWithThreadsAsync(int pPackage, CancellationToken pToken)
        {
            long sumThread = 0;
            int package = pPackage;

            if (_threads != null)
            {
                _threads[package % _core] = new Task<long>(() => GetArrayResult(pToken),
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
                    for (; index < _threads.Count; index++)
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
