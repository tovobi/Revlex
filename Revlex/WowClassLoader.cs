using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Revlex
{
	class WowClassLoader
	{

		private ComboBox Cbx;
		private List<string> WowClassFiles = new List<string>();
		private string Filepath = Application.StartupPath + "\\classroutines";

		public  WowClassLoader(ComboBox _cbx)
		{
			Cbx = _cbx;
		}
		public List<string> GetAllFiles()
		{
			//WowClassFiles = Directory.GetFiles(Filepath, "*.cs", SearchOption.AllDirectories);
			DirectoryInfo pathInfo = new DirectoryInfo(Filepath);
			foreach (var x in pathInfo.GetFiles("*.cs"))
			{
				Log.Print("file: " + x);
				WowClassFiles.Add(x.ToString());
			}
			return WowClassFiles;
		}
	
	}
}
