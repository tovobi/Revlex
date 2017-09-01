using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;
using NLog.Targets;

namespace Revlex
{

	
	public static class Log
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		/// <summary>
		/// 
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="logLevel">log level: 0 = Info, 1 = Debug, 2 = Info, 3 = Warn, 4 = Error, 5 = Fatal, 6 = Off</param>
		/// <param name="cr">carriage return: 0 = none, 1 = "\r\n"</param>
		public static void Print(string msg, int logLevel = 1, int cr = 1)
		{
			string[] carriageReturnType = new string[] { "", "\r\n" };
			LogLevel[] logLevelType = new LogLevel[] { LogLevel.Info, LogLevel.Debug, LogLevel.Info, LogLevel.Warn, LogLevel.Error, LogLevel.Fatal, LogLevel.Off };
			logger.Log(LogLevel.Info, msg + carriageReturnType[cr]);
		}
		public static void Br()
		{
			logger.Log(LogLevel.Info, "\r\n");
		}


	}
}
