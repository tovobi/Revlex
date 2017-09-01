using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revlex
{
	public class ProgressBarConstruct : IDisposable
	{
		public System.Windows.Forms.ProgressBar myProgressBar;
		private System.Windows.Forms.Panel pnlProgress;
		public System.Windows.Forms.Label lblProgressBar;
		private Form1 parentForm;
		public static string progressBarText = "";
		public bool visible
		{
			get
			{
				return myProgressBar.Visible;
			}
			set
			{
				myProgressBar.Visible = value;
				pnlProgress.Visible = value;
				lblProgressBar.Visible = value;
			}
		}
		public int progress
		{
			get
			{
				return myProgressBar.Value;
			}
			set
			{
				if (value >= 0 && value <= myProgressBar.Maximum)
				{
					myProgressBar.Value = value;
				}
			}
		}


		public ProgressBarConstruct(string name, Form1 _parentForm)
		{
			parentForm = _parentForm;
			myProgressBar = new System.Windows.Forms.ProgressBar();
			pnlProgress = new System.Windows.Forms.Panel();
			lblProgressBar = new System.Windows.Forms.Label();
			pnlProgress.SuspendLayout();

			// 
			// myProgressBar
			// 
			myProgressBar.Anchor = System.Windows.Forms.AnchorStyles.Top;
			myProgressBar.BackColor = System.Drawing.SystemColors.Control;
			myProgressBar.Location = new System.Drawing.Point(10, 10);
			myProgressBar.Name = "progressBar" + name;
			myProgressBar.Size = new System.Drawing.Size(480, 30);
			myProgressBar.TabIndex = 17;
			myProgressBar.UseWaitCursor = true;
			myProgressBar.Value = 0;
			// 
			// pnlProgress
			// 
			pnlProgress.Anchor = System.Windows.Forms.AnchorStyles.Top;
			pnlProgress.BackColor = System.Drawing.SystemColors.Control;
			pnlProgress.Controls.Add(lblProgressBar);
			pnlProgress.Controls.Add(myProgressBar);
			pnlProgress.Location = new System.Drawing.Point(151, 192);
			pnlProgress.Name = "pnlProgress" + name;
			pnlProgress.Size = new System.Drawing.Size(500, 68);
			pnlProgress.TabIndex = 18;


			// 
			// lblProgressBar
			// 
			lblProgressBar.AutoSize = true;
			lblProgressBar.Location = new System.Drawing.Point(7, 47);
			lblProgressBar.Name = "lblProgressBar" + name;
			lblProgressBar.Size = new System.Drawing.Size(350, 13);
			lblProgressBar.TabIndex = 18;
			lblProgressBar.Text = "processing ...";
			parentForm.Controls.Add(pnlProgress);
			pnlProgress.ResumeLayout(false);
			pnlProgress.PerformLayout();
			pnlProgress.BringToFront();
		}


		bool disposed = false;
		public void Dispose()
		{
			// wird nur beim ersten Aufruf ausgeführt
			if (!disposed)
			{
				Dispose(true);
				GC.SuppressFinalize(this);
				disposed = true;
			}
		}
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				// Freigabe verwalteter Objekte
				myProgressBar = null;
				pnlProgress = null;
				lblProgressBar = null;
				parentForm = null;
			}
			// Freigabe von Fremdressourcen
		}
		// Destruktor
		~ProgressBarConstruct()
		{
			Dispose(false);
		}
	}
}
