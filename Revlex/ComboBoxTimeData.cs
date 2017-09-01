using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revlex
{
	public class ComboBoxTimeData
	{
		public string name { get; set; } = "";
		public double seconds { get; set; } = 0;

		public ComboBoxTimeData(string _name, double _seconds)
		{
			name = _name;
			seconds = _seconds;
		}
	}
}
