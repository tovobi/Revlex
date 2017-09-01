using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revlex
{
	public class InGameKeyCombo
	{
		public InGameKeyCombo(string _key, string _modifier)
		{
			Key = _key;
			Modifier = _modifier;
		}
		public string Key;
		public string Modifier;
	}
}
