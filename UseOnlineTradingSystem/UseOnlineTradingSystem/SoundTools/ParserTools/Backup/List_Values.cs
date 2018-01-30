using System;
using System.Collections.Generic;
using System.Threading;
namespace ParserTools
{
    public class List_Values : ICLR_Values
    {
        List<double> list = new List<double>();
        ReaderWriterLock mLock = new ReaderWriterLock();
        public List_Values(string Name)
        {
            mName = Name;
        }
        bool mUpdating = false;
        #region ICLR_Values 成员
        public void BeginUpdate()
        {
            mUpdating = true;
        }
        public bool IsSingle
        {
            get
            {
                return false;
            }
        }
        public void SetCount(int Count)
        {
            list.Clear();
            for (int i = 0; i < Count; i++)
            {
                list.Add(ParserHelper.Invalidation);
            }
        }
        public void EndUpdate()
        {
            mUpdating = false;
        }
        public void Add(double Value)
        {
            mLock.AcquireWriterLock(-1);
            try
            {
                list.Add(Value);
            }
            finally
            {
                mLock.ReleaseWriterLock();
            }
        }

        public int GetMaxMin(int Start, int Count, ref double Max, ref double Min)
        {
            if (mUpdating)
            {
                Max = 0;
                Min = 0;
                return 0;
            }
            mLock.AcquireReaderLock(-1);
            try
            {
                Max = 0;
                Min = 0;
                if (Start < 0)
                {
                    Start = 0;
                }

                if (Count <= 0)
                {
                    return 0;
                }
                int maxcount = list.Count;
                if (maxcount <= 0) return 0;
                int p = maxcount - Start - 1;
                if (p < 0) return 0;
                double tMax = double.MinValue;
                double tMin = double.MaxValue;
                int c = 0;
                while (c < Count)
                {
                    double v = list[p];
                    if (v != UInt32.MaxValue)
                    {
                        if (v > tMax) tMax = v;
                        if (v < tMin) tMin = v;
                    }
                    p--;
                    c++;
                    if (p < 0) break;
                }

                if (tMax != double.MinValue)
                    Max = tMax;
                if (tMin != double.MaxValue)
                    Min = tMin;
                return c;
            }
            finally
            {
                mLock.ReleaseReaderLock();
            }
        }

        public void Clear()
        {
            mLock.AcquireWriterLock(-1);
            try
            {
                list.Clear();
            }
            finally
            {
                mLock.ReleaseWriterLock();
            }

        }

        public int Count
        {
            get
            {
                if (mUpdating) return 0;
                mLock.AcquireReaderLock(-1);
                try
                {
                    return list.Count;
                }
                finally
                {
                    mLock.ReleaseReaderLock();
                }
            }
        }

        public double this[int Index]
        {
            get
            {
                if (mUpdating) return ParserTools.ParserHelper.Invalidation;
                mLock.AcquireReaderLock(-1);
                try
                {
                    return list[Index];
                }
                finally
                {
                    mLock.ReleaseReaderLock();
                }
            }

            set
            {
                mLock.AcquireWriterLock(-1);
                try
                {
                    list[Index] = value;
                }
                finally
                {
                    mLock.ReleaseWriterLock();
                }
            }
        }
        string mName = string.Empty;
        public string Name
        {
            get
            {
                return mName;
            }
        }
        Object mTag = null;
        public object Tag
        {
            get
            {
                return mTag;
            }
            set
            {
                mTag = value;
            }
        }

        public void Insert(int Index, IEnumerable<double> value)
        {
            mLock.AcquireWriterLock(-1);
            try
            {
                list.InsertRange(Index, value);
            }
            finally
            {
                mLock.ReleaseWriterLock();
            }
        }



        public void Dispose()
        {
            Clear();
        }

        #endregion
    }
}
