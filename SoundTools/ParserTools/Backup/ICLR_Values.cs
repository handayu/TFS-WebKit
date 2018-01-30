using System;
using System.Collections.Generic;
using System.Text;

namespace ParserTools
{
    public interface ICLR_Values:IDisposable
    {
        void Add(double Value);
		int GetMaxMin(int Start,int Count,ref double Max,ref double Min);
		void Clear();
		int Count {get;}
        double this[int Index]
        {
            get;
            set;
        }
        void BeginUpdate();
        void EndUpdate();
        string Name { get; }
        object Tag { get; set; }
        void SetCount(int Count);
        void Insert(int Index, IEnumerable<double> value);
        bool IsSingle { get; }
    }
}
