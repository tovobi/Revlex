using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Globalization;
using System.ComponentModel;
using System.Media;



namespace Revlex
{
	public static class ThreadWatcher
	{
		public static bool StopThread { get; set; }
	}
	public class MemRevlex
	{
		public static String GetTimestamp(DateTime value) { return value.ToString("mm:ss:ffff"); }
		private static Process proc;
		private static byte[] bit2search1;
		private static int start;
		private static int end;
		private static IniFile config;
		private static uint lastOffset;
		private static uint lastPid;
		private static uint skip;
		private static int scanRangeStart;
		private static int rangeSize;
		private static IntPtr p;
		private static ArrayList memoryRangesMinMax;
		private static Process[] procs;
		private static bool foundOffset;
		private static uint possibleOffset;
		private static RichTextBox richDebug;
		private static byte[] pattern;
		private static int percent;
		private static int lastPercent = 0;
		private static ProgressBarConstruct progressBarMemoryScan;
		private static Form1 parentForm;
		private static BackgroundWorker backgroundWorkerScanMemory;

		[DllImport("kernel32.dll", SetLastError = true)]
		static extern bool ReadProcessMemory(
		  IntPtr hProcess,
		  IntPtr lpBaseAddress,
		  [Out] byte[] lpBuffer,
		  int dwSize,
		  out int lpNumberOfBytesRead
		);

		[DllImport("kernel32.dll")]
		public static extern bool ReadProcessMemory(
			int hProcess, 
			int lpBaseAddress, 
			byte[] lpBuffer, 
			int dwSize, 
			ref int lpNumberOfBytesRead
		);

		[DllImport("kernel32.dll")]
		public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool CloseHandle(IntPtr hObject);

		[DllImport("kernel32.dll")]
		static extern void GetSystemInfo(out SYSTEM_INFO lpSystemInfo);

		[DllImport("kernel32.dll", SetLastError = true)]
		static extern int VirtualQueryEx(IntPtr hProcess,
		IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);




		// REQUIRED CONSTS

		const int PROCESS_QUERY_INFORMATION = 0x0400;
		const int MEM_COMMIT = 0x00001000;
		const int PAGE_READWRITE = 0x04;
		const int PROCESS_WM_READ = 0x0010;





		public struct MEMORY_BASIC_INFORMATION
		{
			public int BaseAddress;
			public int AllocationBase;
			public int AllocationProtect;
			public int RegionSize;
			public int State;
			public int Protect;
			public int lType;
		}

		public struct SYSTEM_INFO
		{
			public ushort processorArchitecture;
			ushort reserved;
			public uint pageSize;
			public IntPtr minimumApplicationAddress;
			public IntPtr maximumApplicationAddress;
			public IntPtr activeProcessorMask;
			public uint numberOfProcessors;
			public uint processorType;
			public uint allocationGranularity;
			public ushort processorLevel;
			public ushort processorRevision;
		}
		public struct ScanBlocks
		{
			public int minRange;
			public int maxRange;
			public ScanBlocks(int minRange, int maxRange)
			{
				this.minRange = minRange;
				this.maxRange = maxRange;
			}
		}


		public static ArrayList GetScanRange(Process proc)
		{
			// getting minimum & maximum address
			//StreamWriter sw = new StreamWriter("scanBlock.txt");
			ArrayList memoryRangesMinMax = new ArrayList();
			
			SYSTEM_INFO sys_info = new SYSTEM_INFO();
			GetSystemInfo(out sys_info);

			IntPtr proc_min_address = sys_info.minimumApplicationAddress;
			IntPtr proc_max_address = sys_info.maximumApplicationAddress;

			// saving the values as long ints so I won't have to do a lot of casts later
			long proc_min_address_l = (long)proc_min_address;
			long proc_max_address_l = (long)proc_max_address;



			// opening the process with desired access level
			IntPtr processHandle =
			OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_WM_READ, false, proc.Id);

			//StreamWriter sw = new StreamWriter("dump.txt");

			// this will store any information we get from VirtualQueryEx()
			MEMORY_BASIC_INFORMATION mem_basic_info = new MEMORY_BASIC_INFORMATION();

			int bytesRead = 0;  // number of bytes read with ReadProcessMemory

			while (proc_min_address_l < proc_max_address_l)
			{
				// 28 = sizeof(MEMORY_BASIC_INFORMATION)
				VirtualQueryEx(processHandle, proc_min_address, out mem_basic_info, 28);

				// if this memory chunk is accessible
				if (mem_basic_info.Protect ==
				PAGE_READWRITE && mem_basic_info.State == MEM_COMMIT)
				{
					byte[] buffer = new byte[mem_basic_info.RegionSize];

					// read everything in the buffer above
					ReadProcessMemory((int)processHandle,
					mem_basic_info.BaseAddress, buffer, mem_basic_info.RegionSize, ref bytesRead);
					memoryRangesMinMax.Add(new ScanBlocks(mem_basic_info.BaseAddress, mem_basic_info.BaseAddress + mem_basic_info.RegionSize));
					//sw.WriteLine("-------------\r\n" + mem_basic_info.BaseAddress.ToString("X") + " - " + mem_basic_info.RegionSize.ToString("X") + "\r\n");
					// then output this in the file
					//for (int i = 0; i < mem_basic_info.RegionSize; i = i + 0x8)
						//sw.WriteLine("0x{0} : {1}", (mem_basic_info.BaseAddress + i).ToString("X"), (char)buffer[i]);
						//Log.Print("0x" + (mem_basic_info.BaseAddress + i).ToString("X") + " : " + (char)buffer[i]);
				}

				// move to the next memory chunk
				proc_min_address_l += mem_basic_info.RegionSize;
				proc_min_address = new IntPtr(proc_min_address_l);
			}

			
			foreach (ScanBlocks v in memoryRangesMinMax)
			{
				//sw.WriteLine("-------------\r\n" + v.minRange.ToString("X") + " - " + v.maxRange.ToString("X") + "\r\n");
			}

			//sw.Close();
			return memoryRangesMinMax;
			//Console.ReadLine();
		}


		public static void backgroundWorkerScanMemory_DoWork(object sender, DoWorkEventArgs e)
		{
			foundOffset = false;
			possibleOffset = 0;

			//
			//do a scan on the previous offset from config
			//
			if (lastOffset != 0 && lastPid == proc.Id)
			{
				Log.Print("found previous offset1: " + lastOffset.ToString("X") + " / " + lastOffset.ToString() + "    " + lastPid + " : " + proc.Id);
				// Check if scanaddress was succesfull in finding the right Offset, if so, we return possibleOffset)
				// We dont scan for the last succesfull offset, we risk it to take it as proven offset, cuz for some reason it scans to often
				//possibleOffset = lastOffset;
				//foundOffset = true;
				//e.Cancel = true;
				//forceProgressbarAnimation(percent, e.Cancel, foundOffset);
				//Thread.Sleep(50);
				//return;
				
				possibleOffset = 0;
				if (ScanAddress((int)lastOffset, 0x8, out possibleOffset))
				{
					Log.Print("found previous offset2: " + lastOffset.ToString("X") + " / " + lastOffset.ToString() + "    " + lastPid + " : " + proc.Id);
					foundOffset = true;
					e.Cancel = true;
					forceProgressbarAnimation(percent, e.Cancel, foundOffset);
					Thread.Sleep(50);
					return;
				}
				
			}

			//
			// core functions of the backgroundworkerprocess
			// regular scan when the check for previous offsets failed
			//
			if (!foundOffset)
			{
				lastPercent = 0;
				//foreach (ScanBlocks scanBlock in memoryRangesMinMax)

				while (scanRangeStart < end)   //end of memory // u can specify to read less if u know he does not fill it all
				{
					percent = (int)Math.Round((double)(scanRangeStart - start ) / (double)(end - start) * (double)100);
					// Check if scanaddress was succesfull in finding the right Offset, if so, we return possibleOffset)
					possibleOffset = 0;					
					if (ScanAddress(scanRangeStart, rangeSize, out possibleOffset))
					{
						Log.Print("found smt");
						config.Write("lastCvarOffset", possibleOffset.ToString());
						config.Write("lastPid", proc.Id.ToString());
						foundOffset = true;
						e.Cancel = true;
						forceProgressbarAnimation(percent, e.Cancel, foundOffset);
						Thread.Sleep(100);							
						return;
					}
					
					scanRangeStart += rangeSize;

					// if the progress was canceled by user
					if (backgroundWorkerScanMemory.CancellationPending)
					{
						e.Cancel = true;
						return;
					}

					// progress was completed and we did not found the offset
					if (lastPercent > 99 && foundOffset == false)
					{
						forceProgressbarAnimation(percent, e.Cancel, foundOffset);
						backgroundWorkerScanMemory.ReportProgress(100);
						Thread.Sleep(100);
						e.Cancel = true;
						return;
					}
					// we only call ReportProgress when the percentage is changing
					if (percent != lastPercent)
					{
						backgroundWorkerScanMemory.ReportProgress(percent);
						lastPercent = percent;
					}
				}
			}
		}

		// finalize the progressbar animation if the offset was found before 100% is reached
		public static void forceProgressbarAnimation(int currentPercent, bool cancel, bool _foundOffset)
		{
			if (cancel && _foundOffset)
			{
				for (int i = currentPercent; i <= 100; i++)
				{
					Thread.Sleep(2);
					backgroundWorkerScanMemory.ReportProgress(i);
				}
			}
		}

		public static void backgroundWorkerScanMemory_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			progressBarMemoryScan.myProgressBar.Invoke(new Action(() => progressBarMemoryScan.myProgressBar.Value = e.ProgressPercentage));
			if (foundOffset && e.ProgressPercentage == 100)
			{
				progressBarMemoryScan.lblProgressBar.Invoke(new Action(() => progressBarMemoryScan.lblProgressBar.Text = "Completed: " + progressBarMemoryScan.myProgressBar.Value.ToString() + "%     (offset at: 0x" + scanRangeStart.ToString("X") + ")"));
				progressBarMemoryScan.myProgressBar.Invoke(new Action(() => progressBarMemoryScan.myProgressBar.ForeColor = System.Drawing.Color.Green));
			}
			else
			{
				progressBarMemoryScan.lblProgressBar.Invoke(new Action(() => progressBarMemoryScan.lblProgressBar.Text = "Processing: " + progressBarMemoryScan.myProgressBar.Value.ToString() + "%     (mem range: 0x" + scanRangeStart.ToString("X") + ")"));
			}
		}

		static void backgroundWorkerScanMemory_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			//richDebug.Invoke(new Action(() => richDebug.Text += "mll: " + possibleOffset.ToString() + "\r\n"));
			parentForm.CvarOffset = possibleOffset;

			// if the progress was 100% complete and offset was found
			if (e.Cancelled && foundOffset)
			{
				progressBarMemoryScan.lblProgressBar.Invoke(new Action(() => progressBarMemoryScan.lblProgressBar.Text = "Completed: " + progressBarMemoryScan.myProgressBar.Value.ToString() + "%     (offset at: 0x" + scanRangeStart.ToString("X") + ")"));
				progressBarMemoryScan.myProgressBar.Invoke(new Action(() => progressBarMemoryScan.myProgressBar.ForeColor = System.Drawing.Color.Green));
			}
			// if the progress was cancelt or no offset was found
			else if (e.Cancelled && !foundOffset)
			{
				progressBarMemoryScan.lblProgressBar.Invoke(new Action(() => progressBarMemoryScan.lblProgressBar.Text = "No offset found: " + progressBarMemoryScan.myProgressBar.Value.ToString("X") + "%"));
				progressBarMemoryScan.myProgressBar.Invoke(new Action(() => progressBarMemoryScan.myProgressBar.ForeColor = System.Drawing.Color.Red));
				//parentForm.cmdAttach.Enabled = true;
			}
			// if an error occured
			else if (e.Error != null)
			{
				progressBarMemoryScan.lblProgressBar.Invoke(new Action(() => progressBarMemoryScan.lblProgressBar.Text = "Error"));
				progressBarMemoryScan.myProgressBar.Invoke(new Action(() => progressBarMemoryScan.myProgressBar.ForeColor = System.Drawing.Color.Red));
			}
			// wait 2seconds at the end of the scan, because we wont that the progressbar disapear so fast
			Thread.Sleep(200);
			//Change the status of the buttons on the UI accordingly
			//parentForm.cmdData.Enabled = true;
			//hide progressbar and destroy obj of progressbar and background worker task
			progressBarMemoryScan.visible = false;			
			progressBarMemoryScan.Dispose();
			backgroundWorkerScanMemory.CancelAsync();
			backgroundWorkerScanMemory.Dispose();
			//parentForm.OnScanSuccessfull(possibleOffset); // thats not an event, just a function call //Haben wir auskommentiert, da wir den Scanevent am 25.7. rausgenommen (an dem Tag haben wir actionbutton data geschrieben)

		}
		public static void MemoryScanner(Process _proc, byte[] _bit2search1, int _start, int _end, IniFile _config, uint _lastOffset, uint _lastPid, RichTextBox _richDebug, Form1 _parentForm, uint _skip = 0x0)
		{

			// //// Wir müssen wieder eine vorauswahl der speicherranges machen
			// How we get the final offset to the form1.cs? We didnt, the final offset is located in the static variable "possibleOffset" which we access only in MemRevlex.cs
			// But later we add an Event which push the possibleOffset to form1.cs (OnScanSuccessfullEvent)
			parentForm = _parentForm;
			//parentForm.cmdAttach.Enabled = false;
			//parentForm.cmdData.Enabled = false;
			proc = _proc;
			pattern = _bit2search1;
			start = _start;
			end = _end;
			config = _config;
			lastOffset = _lastOffset;
			lastPid = _lastPid;
			skip = _skip;
			scanRangeStart = start;
			rangeSize = 0xFF;
			richDebug = _richDebug;
			p = OpenProcess(0x10 | 0x20, true, proc.Id);
			procs = Process.GetProcessesByName("WoW");
			foundOffset = false;
			possibleOffset = 0;
			memoryRangesMinMax = GetScanRange(proc);

			if (proc == null)  //proces not found
			{
				return; 
			}

			progressBarMemoryScan = new ProgressBarConstruct("MemoryScan", parentForm);
			progressBarMemoryScan.visible = true;

			// initialize the background´worker task for the scan
			backgroundWorkerScanMemory = new System.ComponentModel.BackgroundWorker();
			backgroundWorkerScanMemory.DoWork += new DoWorkEventHandler(backgroundWorkerScanMemory_DoWork);
			backgroundWorkerScanMemory.ProgressChanged += new ProgressChangedEventHandler(backgroundWorkerScanMemory_ProgressChanged);
			backgroundWorkerScanMemory.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorkerScanMemory_RunWorkerCompleted);
			backgroundWorkerScanMemory.WorkerReportsProgress = true;
			backgroundWorkerScanMemory.WorkerSupportsCancellation = true;
			backgroundWorkerScanMemory.RunWorkerAsync();

			foundOffset = false;
		}


		private static bool ScanAddress(int startScan, int dumpSize, out uint possibleOffset)
		{
			//StreamWriter sw = new StreamWriter("dump2.txt");
			
			byte[] timeIdentifier1 = new byte[2];    //your array lenght;
			byte[] timeIdentifier2 = new byte[2];    //your array lenght;
			byte[] timeIdentifier3 = new byte[2];    //your array lenght;
			int numberOfBytesReaded;

			SigScan scan = new SigScan(procs.FirstOrDefault(), (IntPtr)startScan, dumpSize);
			IntPtr ptr = scan.FindPattern(pattern, "xxxxxxxx", 0);
			if ((int)ptr != 0)
			{
				ReadProcessMemory(p, (IntPtr)(ptr + 0x9), timeIdentifier1, 2, out numberOfBytesReaded);
				Log.Print("A: " + ptr.ToString("X"));
				Thread.Sleep(220); // we need a little time, so we can check if the cvar on the current address is changing
				ReadProcessMemory(p, (IntPtr)(ptr + 0x9), timeIdentifier2, 2, out numberOfBytesReaded);
				if (timeIdentifier1[0] != timeIdentifier2[0] || timeIdentifier1[1] != timeIdentifier2[1])
				{
					Log.Print("BB: " + ptr.ToString("X"));
					Thread.Sleep(445); // we need a little time, so we can check if the cvar on the current address is changing
					ReadProcessMemory(p, (IntPtr)(ptr + 0x9), timeIdentifier3, 2, out numberOfBytesReaded);
					if (timeIdentifier2[0] != timeIdentifier3[0] || timeIdentifier2[1] != timeIdentifier3[1])
					{
						Log.Print("CCC: " + ptr.ToString("X"));
						Log.Print("Found: " + ptr.ToString("X"));
						possibleOffset = (uint)ptr;
						scan = null; // destroy obj
						return true;
					}
				}
			}
			scan = null; // destroy obj
			possibleOffset = 0;
			if (dumpSize == 0x8)
			{
				Log.Print("startScan: " + startScan.ToString("X") + "  dumpSize: " + dumpSize.ToString("X") + "   possibleOff: " + possibleOffset.ToString("X") + "   ptr: " + ptr.ToString("X"));
			}
			return false;
		}

		public static List<CvarData>[] GetCvarData(Process proc, uint offset)
		{
			//StreamWriter sw = new StreamWriter("dump3.txt");
			byte[] readedData = new byte[1400];    //your array lenght;
			int bytesReaded;

			string[][] stringData = new string[5][];

			stringData[0] = new string[] { };
			stringData[1] = new string[] { };
			stringData[2] = new string[] { };
			stringData[3] = new string[] { };
			stringData[4] = new string[] { };

			List<CvarData>[] data = {
				new List<CvarData>(),	// Spells
				new List<CvarData>(),	// Player Buffs
				new List<CvarData>(),	// Player Debuffs
				new List<CvarData>(),	// Target Buffs
				new List<CvarData>()	// Target Debuffs
			};
			if (proc == null)
			{
				return null; //can replace with exit nag(message)+exit;
			}
			IntPtr p = OpenProcess(0x10 | 0x20, true, proc.Id); //0x10-read 0x20-write
			ReadProcessMemory(p, (IntPtr)offset, readedData, 1400, out bytesReaded);
			stringData[0] = Encoding.ASCII.GetString(readedData).Replace("\n#", "#").Replace("\n^", "^").Split('#');
			stringData[1] = stringData[0][0].Split('!');
			stringData[2] = stringData[1][2].Substring(2).Split('^');
			int u = 0;
			foreach (string v1 in stringData[2])
			{
				stringData[3] = v1.Split('\n');
				int o = 0;
				foreach (string v2 in stringData[3])
				{
					o++;
					stringData[4] = v2.Split('~');
					string tmpstr1 = stringData[4][0];
					string tmpstr2 = stringData[4][1];
					data[u].Add(new CvarData(stringData[4][0], float.Parse(stringData[4][1], CultureInfo.InvariantCulture.NumberFormat),0));
					//data[u].Add(new CvarData(stringData[4][0], float.Parse(stringData[4][1], CultureInfo.InvariantCulture.NumberFormat)));
				}
				u++;
			}
			return data;
		}
	}
}
