using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revlex
{
    class ErrorData
    {
        public long Time;
        public string Name;
        public uint Index;
        public static uint Count;
        public ErrorData(long _time, string _name)
        {
            Count++;
            Time = _time;
            Name = _name;
            Index = Count;            
        }
    }
}
