namespace Revlex
{
	public partial class Form1
	{
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Verwendete Ressourcen bereinigen.
		/// </summary>
		/// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Vom Windows Form-Designer generierter Code

		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung.
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.cmdRefreshUi = new System.Windows.Forms.Button();
			this.cmdAttach = new System.Windows.Forms.Button();
			this.lblSearchObj = new System.Windows.Forms.Label();
			this.txtSearchUnit = new System.Windows.Forms.TextBox();
			this.timerRefreshUi = new System.Windows.Forms.Timer(this.components);
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPageObjects = new System.Windows.Forms.TabPage();
			this.tabPageCombat = new System.Windows.Forms.TabPage();
			this.tabPageRotation = new System.Windows.Forms.TabPage();
			this.grpBoxBindings = new System.Windows.Forms.GroupBox();
			this.lblModifier = new System.Windows.Forms.Label();
			this.lblKey = new System.Windows.Forms.Label();
			this.lblSelectWowClassFile = new System.Windows.Forms.Label();
			this.cbxSelectWowClassfile = new System.Windows.Forms.ComboBox();
			this.richDebug = new System.Windows.Forms.RichTextBox();
			this.cbxRefreshUi = new System.Windows.Forms.ComboBox();
			this.lblInternalUpdate = new System.Windows.Forms.Label();
			this.cbxInternalUpdate = new System.Windows.Forms.ComboBox();
			this.timerInternalUpdate = new System.Windows.Forms.Timer(this.components);
			this.lblRefreshUi = new System.Windows.Forms.Label();
			this.cmdInternalUpdate = new System.Windows.Forms.Button();
			this.lblAccountName = new System.Windows.Forms.Label();
			this.cbxAccountName = new System.Windows.Forms.ComboBox();
			this.txtWowFolder = new System.Windows.Forms.TextBox();
			this.lblWowFolder = new System.Windows.Forms.Label();
			this.btnSearchWowFolder = new System.Windows.Forms.Button();
			this.timerCheckHook = new System.Windows.Forms.Timer(this.components);
			this.tabControl1.SuspendLayout();
			this.tabPageObjects.SuspendLayout();
			this.tabPageRotation.SuspendLayout();
			this.grpBoxBindings.SuspendLayout();
			this.SuspendLayout();
			// 
			// cmdRefreshUi
			// 
			this.cmdRefreshUi.Location = new System.Drawing.Point(118, 11);
			this.cmdRefreshUi.Name = "cmdRefreshUi";
			this.cmdRefreshUi.Size = new System.Drawing.Size(100, 23);
			this.cmdRefreshUi.TabIndex = 9;
			this.cmdRefreshUi.Text = "Refresh UI";
			this.cmdRefreshUi.UseVisualStyleBackColor = true;
			this.cmdRefreshUi.Click += new System.EventHandler(this.cmdRefreshUi_Click);
			// 
			// cmdAttach
			// 
			this.cmdAttach.Location = new System.Drawing.Point(10, 11);
			this.cmdAttach.Name = "cmdAttach";
			this.cmdAttach.Size = new System.Drawing.Size(100, 23);
			this.cmdAttach.TabIndex = 10;
			this.cmdAttach.Text = "Attach";
			this.cmdAttach.UseVisualStyleBackColor = true;
			this.cmdAttach.Click += new System.EventHandler(this.cmdAttach_Click);
			// 
			// lblSearchObj
			// 
			this.lblSearchObj.AutoSize = true;
			this.lblSearchObj.Location = new System.Drawing.Point(6, 14);
			this.lblSearchObj.Name = "lblSearchObj";
			this.lblSearchObj.Size = new System.Drawing.Size(66, 13);
			this.lblSearchObj.TabIndex = 11;
			this.lblSearchObj.Text = "Search Unit:";
			// 
			// txtSearchUnit
			// 
			this.txtSearchUnit.Location = new System.Drawing.Point(95, 12);
			this.txtSearchUnit.Name = "txtSearchUnit";
			this.txtSearchUnit.Size = new System.Drawing.Size(662, 20);
			this.txtSearchUnit.TabIndex = 12;
			this.txtSearchUnit.TextChanged += new System.EventHandler(this.txtSearchUnit_TextChanged);
			// 
			// timerRefreshUi
			// 
			this.timerRefreshUi.Tick += new System.EventHandler(this.timerRefreshUi_Tick);
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPageObjects);
			this.tabControl1.Controls.Add(this.tabPageCombat);
			this.tabControl1.Controls.Add(this.tabPageRotation);
			this.tabControl1.Location = new System.Drawing.Point(10, 95);
			this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(779, 431);
			this.tabControl1.TabIndex = 15;
			// 
			// tabPageObjects
			// 
			this.tabPageObjects.Controls.Add(this.lblSearchObj);
			this.tabPageObjects.Controls.Add(this.txtSearchUnit);
			this.tabPageObjects.Location = new System.Drawing.Point(4, 22);
			this.tabPageObjects.Margin = new System.Windows.Forms.Padding(2);
			this.tabPageObjects.Name = "tabPageObjects";
			this.tabPageObjects.Padding = new System.Windows.Forms.Padding(2);
			this.tabPageObjects.Size = new System.Drawing.Size(771, 405);
			this.tabPageObjects.TabIndex = 0;
			this.tabPageObjects.Text = "Objects";
			this.tabPageObjects.UseVisualStyleBackColor = true;
			// 
			// tabPageCombat
			// 
			this.tabPageCombat.Location = new System.Drawing.Point(4, 22);
			this.tabPageCombat.Margin = new System.Windows.Forms.Padding(2);
			this.tabPageCombat.Name = "tabPageCombat";
			this.tabPageCombat.Padding = new System.Windows.Forms.Padding(2);
			this.tabPageCombat.Size = new System.Drawing.Size(771, 405);
			this.tabPageCombat.TabIndex = 1;
			this.tabPageCombat.Text = "Combat Data";
			this.tabPageCombat.UseVisualStyleBackColor = true;
			// 
			// tabPageRotation
			// 
			this.tabPageRotation.Controls.Add(this.grpBoxBindings);
			this.tabPageRotation.Location = new System.Drawing.Point(4, 22);
			this.tabPageRotation.Margin = new System.Windows.Forms.Padding(2);
			this.tabPageRotation.Name = "tabPageRotation";
			this.tabPageRotation.Padding = new System.Windows.Forms.Padding(2);
			this.tabPageRotation.Size = new System.Drawing.Size(771, 405);
			this.tabPageRotation.TabIndex = 2;
			this.tabPageRotation.Text = "Rotation";
			this.tabPageRotation.UseVisualStyleBackColor = true;
			// 
			// grpBoxBindings
			// 
			this.grpBoxBindings.Controls.Add(this.lblModifier);
			this.grpBoxBindings.Controls.Add(this.lblKey);
			this.grpBoxBindings.Location = new System.Drawing.Point(12, 11);
			this.grpBoxBindings.Name = "grpBoxBindings";
			this.grpBoxBindings.Size = new System.Drawing.Size(185, 382);
			this.grpBoxBindings.TabIndex = 3;
			this.grpBoxBindings.TabStop = false;
			this.grpBoxBindings.Text = "Keybindings for rotations";
			// 
			// lblModifier
			// 
			this.lblModifier.AutoSize = true;
			this.lblModifier.Location = new System.Drawing.Point(112, 24);
			this.lblModifier.Name = "lblModifier";
			this.lblModifier.Size = new System.Drawing.Size(44, 13);
			this.lblModifier.TabIndex = 1;
			this.lblModifier.Text = "Modifier";
			// 
			// lblKey
			// 
			this.lblKey.AutoSize = true;
			this.lblKey.Location = new System.Drawing.Point(36, 24);
			this.lblKey.Name = "lblKey";
			this.lblKey.Size = new System.Drawing.Size(25, 13);
			this.lblKey.TabIndex = 0;
			this.lblKey.Text = "Key";
			// 
			// lblSelectWowClassFile
			// 
			this.lblSelectWowClassFile.AutoSize = true;
			this.lblSelectWowClassFile.Location = new System.Drawing.Point(362, 57);
			this.lblSelectWowClassFile.Name = "lblSelectWowClassFile";
			this.lblSelectWowClassFile.Size = new System.Drawing.Size(50, 13);
			this.lblSelectWowClassFile.TabIndex = 1;
			this.lblSelectWowClassFile.Text = "class file:";
			// 
			// cbxSelectWowClassfile
			// 
			this.cbxSelectWowClassfile.BackColor = System.Drawing.SystemColors.Window;
			this.cbxSelectWowClassfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxSelectWowClassfile.ForeColor = System.Drawing.SystemColors.WindowText;
			this.cbxSelectWowClassfile.FormattingEnabled = true;
			this.cbxSelectWowClassfile.Location = new System.Drawing.Point(412, 53);
			this.cbxSelectWowClassfile.Name = "cbxSelectWowClassfile";
			this.cbxSelectWowClassfile.Size = new System.Drawing.Size(130, 21);
			this.cbxSelectWowClassfile.TabIndex = 0;
			// 
			// richDebug
			// 
			this.richDebug.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.richDebug.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.richDebug.Font = new System.Drawing.Font("Lucida Console", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.richDebug.Location = new System.Drawing.Point(9, 547);
			this.richDebug.Margin = new System.Windows.Forms.Padding(2);
			this.richDebug.Name = "richDebug";
			this.richDebug.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
			this.richDebug.Size = new System.Drawing.Size(779, 219);
			this.richDebug.TabIndex = 16;
			this.richDebug.Text = "";
			this.richDebug.WordWrap = false;
			this.richDebug.TextChanged += new System.EventHandler(this.richDebug_TextChanged);
			// 
			// cbxRefreshUi
			// 
			this.cbxRefreshUi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxRefreshUi.FormattingEnabled = true;
			this.cbxRefreshUi.Items.AddRange(new object[] {
            "no repeat",
            "0.1 sec",
            "0.25 sec",
            "0.5 sec",
            "1 sec",
            "2 sec",
            "5 sec",
            "10 sec"});
			this.cbxRefreshUi.Location = new System.Drawing.Point(412, 12);
			this.cbxRefreshUi.Name = "cbxRefreshUi";
			this.cbxRefreshUi.Size = new System.Drawing.Size(130, 21);
			this.cbxRefreshUi.TabIndex = 17;
			this.cbxRefreshUi.SelectedIndexChanged += new System.EventHandler(this.cbxRefreshUi_SelectedIndexChanged);
			// 
			// lblInternalUpdate
			// 
			this.lblInternalUpdate.AutoSize = true;
			this.lblInternalUpdate.Location = new System.Drawing.Point(572, 16);
			this.lblInternalUpdate.Name = "lblInternalUpdate";
			this.lblInternalUpdate.Size = new System.Drawing.Size(80, 13);
			this.lblInternalUpdate.TabIndex = 19;
			this.lblInternalUpdate.Text = "internal update:";
			// 
			// cbxInternalUpdate
			// 
			this.cbxInternalUpdate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxInternalUpdate.FormattingEnabled = true;
			this.cbxInternalUpdate.Items.AddRange(new object[] {
            "no repeat",
            "0.1 sec",
            "0.25 sec",
            "0.5 sec",
            "1 sec",
            "2 sec",
            "5 sec",
            "10 sec"});
			this.cbxInternalUpdate.Location = new System.Drawing.Point(652, 12);
			this.cbxInternalUpdate.Name = "cbxInternalUpdate";
			this.cbxInternalUpdate.Size = new System.Drawing.Size(130, 21);
			this.cbxInternalUpdate.TabIndex = 20;
			this.cbxInternalUpdate.SelectedIndexChanged += new System.EventHandler(this.cbxInternalUpdate_SelectedIndexChanged);
			// 
			// timerInternalUpdate
			// 
			this.timerInternalUpdate.Tick += new System.EventHandler(this.timerInternalUpdate_Tick);
			// 
			// lblRefreshUi
			// 
			this.lblRefreshUi.AutoSize = true;
			this.lblRefreshUi.Location = new System.Drawing.Point(356, 16);
			this.lblRefreshUi.Name = "lblRefreshUi";
			this.lblRefreshUi.Size = new System.Drawing.Size(56, 13);
			this.lblRefreshUi.TabIndex = 22;
			this.lblRefreshUi.Text = "refresh UI:";
			// 
			// cmdInternalUpdate
			// 
			this.cmdInternalUpdate.Location = new System.Drawing.Point(226, 11);
			this.cmdInternalUpdate.Name = "cmdInternalUpdate";
			this.cmdInternalUpdate.Size = new System.Drawing.Size(100, 23);
			this.cmdInternalUpdate.TabIndex = 23;
			this.cmdInternalUpdate.Text = "Start";
			this.cmdInternalUpdate.UseVisualStyleBackColor = true;
			this.cmdInternalUpdate.Click += new System.EventHandler(this.cmdInternalUpdate_Click);
			// 
			// lblAccountName
			// 
			this.lblAccountName.AutoSize = true;
			this.lblAccountName.Location = new System.Drawing.Point(574, 57);
			this.lblAccountName.Name = "lblAccountName";
			this.lblAccountName.Size = new System.Drawing.Size(78, 13);
			this.lblAccountName.TabIndex = 24;
			this.lblAccountName.Text = "account name:";
			// 
			// cbxAccountName
			// 
			this.cbxAccountName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxAccountName.FormattingEnabled = true;
			this.cbxAccountName.Location = new System.Drawing.Point(652, 53);
			this.cbxAccountName.Name = "cbxAccountName";
			this.cbxAccountName.Size = new System.Drawing.Size(130, 21);
			this.cbxAccountName.TabIndex = 25;
			this.cbxAccountName.SelectedIndexChanged += new System.EventHandler(this.cbxAccountName_SelectedIndexChanged);
			// 
			// txtWowFolder
			// 
			this.txtWowFolder.Location = new System.Drawing.Point(119, 53);
			this.txtWowFolder.MaximumSize = new System.Drawing.Size(206, 21);
			this.txtWowFolder.MinimumSize = new System.Drawing.Size(100, 21);
			this.txtWowFolder.Name = "txtWowFolder";
			this.txtWowFolder.Size = new System.Drawing.Size(159, 21);
			this.txtWowFolder.TabIndex = 26;
			this.txtWowFolder.Leave += new System.EventHandler(this.txtWowFolder_Leave);
			// 
			// lblWowFolder
			// 
			this.lblWowFolder.AutoSize = true;
			this.lblWowFolder.Location = new System.Drawing.Point(8, 57);
			this.lblWowFolder.Name = "lblWowFolder";
			this.lblWowFolder.Size = new System.Drawing.Size(108, 13);
			this.lblWowFolder.TabIndex = 27;
			this.lblWowFolder.Text = "Location of wow.exe:";
			// 
			// btnSearchWowFolder
			// 
			this.btnSearchWowFolder.Location = new System.Drawing.Point(279, 52);
			this.btnSearchWowFolder.Margin = new System.Windows.Forms.Padding(0);
			this.btnSearchWowFolder.Name = "btnSearchWowFolder";
			this.btnSearchWowFolder.Size = new System.Drawing.Size(47, 23);
			this.btnSearchWowFolder.TabIndex = 28;
			this.btnSearchWowFolder.Text = "search";
			this.btnSearchWowFolder.UseVisualStyleBackColor = true;
			this.btnSearchWowFolder.Click += new System.EventHandler(this.btnSearchWowFolder_Click);
			// 
			// timerCheckHook
			// 
			this.timerCheckHook.Tick += new System.EventHandler(this.timerCheckHook_Tick);
			// 
			// Form1
			// 
			this.AllowDrop = true;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(797, 776);
			this.Controls.Add(this.btnSearchWowFolder);
			this.Controls.Add(this.lblWowFolder);
			this.Controls.Add(this.txtWowFolder);
			this.Controls.Add(this.cbxAccountName);
			this.Controls.Add(this.lblAccountName);
			this.Controls.Add(this.cmdInternalUpdate);
			this.Controls.Add(this.cbxSelectWowClassfile);
			this.Controls.Add(this.lblSelectWowClassFile);
			this.Controls.Add(this.lblRefreshUi);
			this.Controls.Add(this.cbxInternalUpdate);
			this.Controls.Add(this.lblInternalUpdate);
			this.Controls.Add(this.cbxRefreshUi);
			this.Controls.Add(this.richDebug);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.cmdAttach);
			this.Controls.Add(this.cmdRefreshUi);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Form1";
			this.Text = "Revlex";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.tabControl1.ResumeLayout(false);
			this.tabPageObjects.ResumeLayout(false);
			this.tabPageObjects.PerformLayout();
			this.tabPageRotation.ResumeLayout(false);
			this.grpBoxBindings.ResumeLayout(false);
			this.grpBoxBindings.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		public System.Windows.Forms.Button cmdRefreshUi;
		public System.Windows.Forms.Button cmdAttach;
		private System.Windows.Forms.Label lblSearchObj;
		private System.Windows.Forms.TextBox txtSearchUnit;
		private System.Windows.Forms.Timer timerRefreshUi;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPageObjects;
		private System.Windows.Forms.TabPage tabPageCombat;
		private System.Windows.Forms.TabPage tabPageRotation;
		private System.Windows.Forms.RichTextBox richDebug;
		private System.Windows.Forms.ComboBox cbxRefreshUi;
		private System.Windows.Forms.Label lblInternalUpdate;
		private System.Windows.Forms.ComboBox cbxInternalUpdate;
		private System.Windows.Forms.Timer timerInternalUpdate;
		private System.Windows.Forms.Label lblRefreshUi;
		public System.Windows.Forms.Button cmdInternalUpdate;
		private System.Windows.Forms.Label lblSelectWowClassFile;
		private System.Windows.Forms.ComboBox cbxSelectWowClassfile;
		private System.Windows.Forms.GroupBox grpBoxBindings;
		private System.Windows.Forms.Label lblModifier;
		private System.Windows.Forms.Label lblKey;
		private System.Windows.Forms.Label lblAccountName;
		private System.Windows.Forms.ComboBox cbxAccountName;
		private System.Windows.Forms.TextBox txtWowFolder;
		private System.Windows.Forms.Label lblWowFolder;
		private System.Windows.Forms.Button btnSearchWowFolder;
		private System.Windows.Forms.Timer timerCheckHook;
	}
}

