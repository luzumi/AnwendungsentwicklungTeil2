using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Windows.Controls;
using Microsoft.VisualBasic.CompilerServices;

namespace MutliThreadingUebung
{
    public class ArrayPackage<TReturnType, TArrayType>
    {

        public TReturnType Result
        {
            get => _result;
            set => _result = value;
        }
        private TReturnType _result;

        public ProgressBar ProgressBar;
        public IProgress<int> Progress;
        public ArraySegment<TArrayType> Segment;

        public ArrayPackage(ArraySegment<TArrayType> arraySegments)
        {
            Segment = arraySegments;
            ProgressBar = new ProgressBar();
            Progress = new Progress<int>(Report);
        }

        private void Report(int newValue)
        {
            ProgressBar.Value = newValue;
        }

        public long SumSegment(object other)
        {
            return LongType.FromObject(other);
        }

        public TReturnType GetResult(object pResult)
        {
            return (TReturnType)pResult;
        }
    }
}
