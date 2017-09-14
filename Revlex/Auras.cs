using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revlex
{
	public class Auras
	{
		public string Name { get; set; }
		public uint Id { get; set; }
		public float Duration { get; set; }
		public uint Stacks { get; set; }
        public long TimeApplied { get; set; }
        public static long GetTime()
        {
            var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)timeSpan.TotalMilliseconds;
        }
        public Auras(string _name, uint _id, float _duration = 1, uint _stacks = 1, long _timeApplied = 0)
		{
			Name = _name;
			Id = _id;
			Duration = _duration;
			Stacks = _stacks;
            if (_timeApplied == 0)
            {
                TimeApplied = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
            }
            else
            {
                TimeApplied = _timeApplied;
            }

        }
	}
}
