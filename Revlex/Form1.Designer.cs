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
            this.tabPageRadar = new System.Windows.Forms.TabPage();
            this.pnlRadarCfg = new System.Windows.Forms.Panel();
            this.btnRadarAccept = new System.Windows.Forms.Button();
            this.pnlRadarCfgValue = new System.Windows.Forms.Panel();
            this.txtRadarSizeY = new System.Windows.Forms.TextBox();
            this.cbxEnableBorder = new System.Windows.Forms.CheckBox();
            this.txtRadarSizeX = new System.Windows.Forms.TextBox();
            this.txtRadarYPos = new System.Windows.Forms.TextBox();
            this.txtRadarXPos = new System.Windows.Forms.TextBox();
            this.cbxEnableRadar = new System.Windows.Forms.CheckBox();
            this.pnlRadarCfgKey = new System.Windows.Forms.Panel();
            this.lblRadarBorder = new System.Windows.Forms.Label();
            this.lblRadarSize = new System.Windows.Forms.Label();
            this.lblRadarYPos = new System.Windows.Forms.Label();
            this.lblRadarXPos = new System.Windows.Forms.Label();
            this.lblEnableRadar = new System.Windows.Forms.Label();
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
            this.cbxRadarHerbs = new System.Windows.Forms.CheckBox();
            this.cbxRadarVeins = new System.Windows.Forms.CheckBox();
            this.lblRadarHerbs = new System.Windows.Forms.Label();
            this.lblRadarVeins = new System.Windows.Forms.Label();
            this.lblRadarFriendlyPlayer = new System.Windows.Forms.Label();
            this.lblRadarEnemyPlayer = new System.Windows.Forms.Label();
            this.cbxRadarEnemyPlayer = new System.Windows.Forms.CheckBox();
            this.cbxRadarFriendlyPlayer = new System.Windows.Forms.CheckBox();
            this.lblSectionDisplay = new System.Windows.Forms.Label();
            this.lblRadarSound = new System.Windows.Forms.Label();
            this.cbxSoundFriendlyPlayer = new System.Windows.Forms.CheckBox();
            this.cbxSoundEnemyPlayer = new System.Windows.Forms.CheckBox();
            this.cbxSoundHerbs = new System.Windows.Forms.CheckBox();
            this.cbxSoundVeins = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPageObjects.SuspendLayout();
            this.tabPageRotation.SuspendLayout();
            this.grpBoxBindings.SuspendLayout();
            this.tabPageRadar.SuspendLayout();
            this.pnlRadarCfg.SuspendLayout();
            this.pnlRadarCfgValue.SuspendLayout();
            this.pnlRadarCfgKey.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdRefreshUi
            // 
            this.cmdRefreshUi.Location = new System.Drawing.Point(118, 11);
            this.cmdRefreshUi.Name = "cmdRefreshUi";
            this.cmdRefreshUi.Size = new System.Drawing.Size(74, 23);
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
            this.tabControl1.Controls.Add(this.tabPageRadar);
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
            // tabPageRadar
            // 
            this.tabPageRadar.Controls.Add(this.pnlRadarCfg);
            this.tabPageRadar.Location = new System.Drawing.Point(4, 22);
            this.tabPageRadar.Name = "tabPageRadar";
            this.tabPageRadar.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRadar.Size = new System.Drawing.Size(771, 405);
            this.tabPageRadar.TabIndex = 3;
            this.tabPageRadar.Text = "Radar";
            this.tabPageRadar.UseVisualStyleBackColor = true;
            // 
            // pnlRadarCfg
            // 
            this.pnlRadarCfg.Controls.Add(this.btnRadarAccept);
            this.pnlRadarCfg.Controls.Add(this.pnlRadarCfgValue);
            this.pnlRadarCfg.Controls.Add(this.pnlRadarCfgKey);
            this.pnlRadarCfg.Location = new System.Drawing.Point(6, 7);
            this.pnlRadarCfg.Name = "pnlRadarCfg";
            this.pnlRadarCfg.Size = new System.Drawing.Size(322, 392);
            this.pnlRadarCfg.TabIndex = 2;
            // 
            // btnRadarAccept
            // 
            this.btnRadarAccept.Location = new System.Drawing.Point(4, 368);
            this.btnRadarAccept.Name = "btnRadarAccept";
            this.btnRadarAccept.Size = new System.Drawing.Size(75, 23);
            this.btnRadarAccept.TabIndex = 3;
            this.btnRadarAccept.Text = "Accept";
            this.btnRadarAccept.UseVisualStyleBackColor = true;
            this.btnRadarAccept.Click += new System.EventHandler(this.btnRadarAccept_Click);
            // 
            // pnlRadarCfgValue
            // 
            this.pnlRadarCfgValue.Controls.Add(this.cbxSoundVeins);
            this.pnlRadarCfgValue.Controls.Add(this.cbxSoundHerbs);
            this.pnlRadarCfgValue.Controls.Add(this.cbxSoundEnemyPlayer);
            this.pnlRadarCfgValue.Controls.Add(this.cbxSoundFriendlyPlayer);
            this.pnlRadarCfgValue.Controls.Add(this.lblRadarSound);
            this.pnlRadarCfgValue.Controls.Add(this.lblSectionDisplay);
            this.pnlRadarCfgValue.Controls.Add(this.cbxRadarFriendlyPlayer);
            this.pnlRadarCfgValue.Controls.Add(this.cbxRadarEnemyPlayer);
            this.pnlRadarCfgValue.Controls.Add(this.cbxRadarVeins);
            this.pnlRadarCfgValue.Controls.Add(this.cbxRadarHerbs);
            this.pnlRadarCfgValue.Controls.Add(this.txtRadarSizeY);
            this.pnlRadarCfgValue.Controls.Add(this.cbxEnableBorder);
            this.pnlRadarCfgValue.Controls.Add(this.txtRadarSizeX);
            this.pnlRadarCfgValue.Controls.Add(this.txtRadarYPos);
            this.pnlRadarCfgValue.Controls.Add(this.txtRadarXPos);
            this.pnlRadarCfgValue.Controls.Add(this.cbxEnableRadar);
            this.pnlRadarCfgValue.Location = new System.Drawing.Point(91, 0);
            this.pnlRadarCfgValue.Name = "pnlRadarCfgValue";
            this.pnlRadarCfgValue.Size = new System.Drawing.Size(126, 353);
            this.pnlRadarCfgValue.TabIndex = 2;
            // 
            // txtRadarSizeY
            // 
            this.txtRadarSizeY.Location = new System.Drawing.Point(57, 84);
            this.txtRadarSizeY.Name = "txtRadarSizeY";
            this.txtRadarSizeY.Size = new System.Drawing.Size(46, 20);
            this.txtRadarSizeY.TabIndex = 5;
            this.txtRadarSizeY.Text = "165";
            // 
            // cbxEnableBorder
            // 
            this.cbxEnableBorder.AutoSize = true;
            this.cbxEnableBorder.Location = new System.Drawing.Point(4, 111);
            this.cbxEnableBorder.Name = "cbxEnableBorder";
            this.cbxEnableBorder.Size = new System.Drawing.Size(15, 14);
            this.cbxEnableBorder.TabIndex = 4;
            this.cbxEnableBorder.UseVisualStyleBackColor = true;
            this.cbxEnableBorder.CheckedChanged += new System.EventHandler(this.cbxEnableBorder_CheckedChanged);
            // 
            // txtRadarSizeX
            // 
            this.txtRadarSizeX.Location = new System.Drawing.Point(4, 84);
            this.txtRadarSizeX.Name = "txtRadarSizeX";
            this.txtRadarSizeX.Size = new System.Drawing.Size(46, 20);
            this.txtRadarSizeX.TabIndex = 3;
            this.txtRadarSizeX.Text = "167";
            // 
            // txtRadarYPos
            // 
            this.txtRadarYPos.Location = new System.Drawing.Point(4, 57);
            this.txtRadarYPos.Name = "txtRadarYPos";
            this.txtRadarYPos.Size = new System.Drawing.Size(100, 20);
            this.txtRadarYPos.TabIndex = 2;
            this.txtRadarYPos.Text = "0";
            // 
            // txtRadarXPos
            // 
            this.txtRadarXPos.Location = new System.Drawing.Point(4, 30);
            this.txtRadarXPos.Name = "txtRadarXPos";
            this.txtRadarXPos.Size = new System.Drawing.Size(100, 20);
            this.txtRadarXPos.TabIndex = 1;
            this.txtRadarXPos.Text = "1737";
            // 
            // cbxEnableRadar
            // 
            this.cbxEnableRadar.AutoSize = true;
            this.cbxEnableRadar.Location = new System.Drawing.Point(4, 7);
            this.cbxEnableRadar.Name = "cbxEnableRadar";
            this.cbxEnableRadar.Size = new System.Drawing.Size(15, 14);
            this.cbxEnableRadar.TabIndex = 0;
            this.cbxEnableRadar.UseVisualStyleBackColor = true;
            this.cbxEnableRadar.CheckedChanged += new System.EventHandler(this.cbxEnableRadar_CheckedChanged);
            // 
            // pnlRadarCfgKey
            // 
            this.pnlRadarCfgKey.Controls.Add(this.lblRadarEnemyPlayer);
            this.pnlRadarCfgKey.Controls.Add(this.lblRadarFriendlyPlayer);
            this.pnlRadarCfgKey.Controls.Add(this.lblRadarVeins);
            this.pnlRadarCfgKey.Controls.Add(this.lblRadarHerbs);
            this.pnlRadarCfgKey.Controls.Add(this.lblRadarBorder);
            this.pnlRadarCfgKey.Controls.Add(this.lblRadarSize);
            this.pnlRadarCfgKey.Controls.Add(this.lblRadarYPos);
            this.pnlRadarCfgKey.Controls.Add(this.lblRadarXPos);
            this.pnlRadarCfgKey.Controls.Add(this.lblEnableRadar);
            this.pnlRadarCfgKey.Location = new System.Drawing.Point(0, 0);
            this.pnlRadarCfgKey.Name = "pnlRadarCfgKey";
            this.pnlRadarCfgKey.Size = new System.Drawing.Size(90, 353);
            this.pnlRadarCfgKey.TabIndex = 1;
            // 
            // lblRadarBorder
            // 
            this.lblRadarBorder.AutoSize = true;
            this.lblRadarBorder.Location = new System.Drawing.Point(0, 115);
            this.lblRadarBorder.Name = "lblRadarBorder";
            this.lblRadarBorder.Size = new System.Drawing.Size(74, 13);
            this.lblRadarBorder.TabIndex = 4;
            this.lblRadarBorder.Text = "Enable Border";
            // 
            // lblRadarSize
            // 
            this.lblRadarSize.AutoSize = true;
            this.lblRadarSize.Location = new System.Drawing.Point(0, 88);
            this.lblRadarSize.Name = "lblRadarSize";
            this.lblRadarSize.Size = new System.Drawing.Size(28, 13);
            this.lblRadarSize.TabIndex = 3;
            this.lblRadarSize.Text = "size:";
            // 
            // lblRadarYPos
            // 
            this.lblRadarYPos.AutoSize = true;
            this.lblRadarYPos.Location = new System.Drawing.Point(1, 61);
            this.lblRadarYPos.Name = "lblRadarYPos";
            this.lblRadarYPos.Size = new System.Drawing.Size(54, 13);
            this.lblRadarYPos.TabIndex = 2;
            this.lblRadarYPos.Text = "y position:";
            // 
            // lblRadarXPos
            // 
            this.lblRadarXPos.AutoSize = true;
            this.lblRadarXPos.Location = new System.Drawing.Point(1, 34);
            this.lblRadarXPos.Name = "lblRadarXPos";
            this.lblRadarXPos.Size = new System.Drawing.Size(54, 13);
            this.lblRadarXPos.TabIndex = 1;
            this.lblRadarXPos.Text = "x position:";
            // 
            // lblEnableRadar
            // 
            this.lblEnableRadar.AutoSize = true;
            this.lblEnableRadar.Location = new System.Drawing.Point(0, 7);
            this.lblEnableRadar.Name = "lblEnableRadar";
            this.lblEnableRadar.Size = new System.Drawing.Size(75, 13);
            this.lblEnableRadar.TabIndex = 0;
            this.lblEnableRadar.Text = "Enable Radar:";
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
            this.richDebug.Size = new System.Drawing.Size(779, 319);
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
            this.cmdInternalUpdate.Location = new System.Drawing.Point(199, 11);
            this.cmdInternalUpdate.Name = "cmdInternalUpdate";
            this.cmdInternalUpdate.Size = new System.Drawing.Size(60, 23);
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
            // cbxRadarHerbs
            // 
            this.cbxRadarHerbs.AutoSize = true;
            this.cbxRadarHerbs.Checked = true;
            this.cbxRadarHerbs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxRadarHerbs.Location = new System.Drawing.Point(4, 231);
            this.cbxRadarHerbs.Name = "cbxRadarHerbs";
            this.cbxRadarHerbs.Size = new System.Drawing.Size(15, 14);
            this.cbxRadarHerbs.TabIndex = 6;
            this.cbxRadarHerbs.UseVisualStyleBackColor = true;
            // 
            // cbxRadarVeins
            // 
            this.cbxRadarVeins.AutoSize = true;
            this.cbxRadarVeins.Location = new System.Drawing.Point(4, 258);
            this.cbxRadarVeins.Name = "cbxRadarVeins";
            this.cbxRadarVeins.Size = new System.Drawing.Size(15, 14);
            this.cbxRadarVeins.TabIndex = 7;
            this.cbxRadarVeins.UseVisualStyleBackColor = true;
            // 
            // lblRadarHerbs
            // 
            this.lblRadarHerbs.AutoSize = true;
            this.lblRadarHerbs.Location = new System.Drawing.Point(0, 231);
            this.lblRadarHerbs.Name = "lblRadarHerbs";
            this.lblRadarHerbs.Size = new System.Drawing.Size(38, 13);
            this.lblRadarHerbs.TabIndex = 5;
            this.lblRadarHerbs.Text = "Herbs:";
            // 
            // lblRadarVeins
            // 
            this.lblRadarVeins.AutoSize = true;
            this.lblRadarVeins.Location = new System.Drawing.Point(0, 258);
            this.lblRadarVeins.Name = "lblRadarVeins";
            this.lblRadarVeins.Size = new System.Drawing.Size(36, 13);
            this.lblRadarVeins.TabIndex = 6;
            this.lblRadarVeins.Text = "Veins:";
            // 
            // lblRadarFriendlyPlayer
            // 
            this.lblRadarFriendlyPlayer.AutoSize = true;
            this.lblRadarFriendlyPlayer.Location = new System.Drawing.Point(0, 177);
            this.lblRadarFriendlyPlayer.Name = "lblRadarFriendlyPlayer";
            this.lblRadarFriendlyPlayer.Size = new System.Drawing.Size(78, 13);
            this.lblRadarFriendlyPlayer.TabIndex = 7;
            this.lblRadarFriendlyPlayer.Text = "Friendly Player:";
            // 
            // lblRadarEnemyPlayer
            // 
            this.lblRadarEnemyPlayer.AutoSize = true;
            this.lblRadarEnemyPlayer.Location = new System.Drawing.Point(0, 204);
            this.lblRadarEnemyPlayer.Name = "lblRadarEnemyPlayer";
            this.lblRadarEnemyPlayer.Size = new System.Drawing.Size(74, 13);
            this.lblRadarEnemyPlayer.TabIndex = 8;
            this.lblRadarEnemyPlayer.Text = "Enemy Player:";
            // 
            // cbxRadarEnemyPlayer
            // 
            this.cbxRadarEnemyPlayer.AutoSize = true;
            this.cbxRadarEnemyPlayer.Checked = true;
            this.cbxRadarEnemyPlayer.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxRadarEnemyPlayer.Location = new System.Drawing.Point(4, 204);
            this.cbxRadarEnemyPlayer.Name = "cbxRadarEnemyPlayer";
            this.cbxRadarEnemyPlayer.Size = new System.Drawing.Size(15, 14);
            this.cbxRadarEnemyPlayer.TabIndex = 8;
            this.cbxRadarEnemyPlayer.UseVisualStyleBackColor = true;
            // 
            // cbxRadarFriendlyPlayer
            // 
            this.cbxRadarFriendlyPlayer.AutoSize = true;
            this.cbxRadarFriendlyPlayer.Location = new System.Drawing.Point(4, 177);
            this.cbxRadarFriendlyPlayer.Name = "cbxRadarFriendlyPlayer";
            this.cbxRadarFriendlyPlayer.Size = new System.Drawing.Size(15, 14);
            this.cbxRadarFriendlyPlayer.TabIndex = 9;
            this.cbxRadarFriendlyPlayer.UseVisualStyleBackColor = true;
            // 
            // lblSectionDisplay
            // 
            this.lblSectionDisplay.AutoSize = true;
            this.lblSectionDisplay.Location = new System.Drawing.Point(0, 150);
            this.lblSectionDisplay.Name = "lblSectionDisplay";
            this.lblSectionDisplay.Size = new System.Drawing.Size(41, 13);
            this.lblSectionDisplay.TabIndex = 10;
            this.lblSectionDisplay.Text = "Display";
            // 
            // lblRadarSound
            // 
            this.lblRadarSound.AutoSize = true;
            this.lblRadarSound.Location = new System.Drawing.Point(57, 150);
            this.lblRadarSound.Name = "lblRadarSound";
            this.lblRadarSound.Size = new System.Drawing.Size(38, 13);
            this.lblRadarSound.TabIndex = 11;
            this.lblRadarSound.Text = "Sound";
            // 
            // cbxSoundFriendlyPlayer
            // 
            this.cbxSoundFriendlyPlayer.AutoSize = true;
            this.cbxSoundFriendlyPlayer.Location = new System.Drawing.Point(61, 177);
            this.cbxSoundFriendlyPlayer.Name = "cbxSoundFriendlyPlayer";
            this.cbxSoundFriendlyPlayer.Size = new System.Drawing.Size(15, 14);
            this.cbxSoundFriendlyPlayer.TabIndex = 12;
            this.cbxSoundFriendlyPlayer.UseVisualStyleBackColor = true;
            // 
            // cbxSoundEnemyPlayer
            // 
            this.cbxSoundEnemyPlayer.AutoSize = true;
            this.cbxSoundEnemyPlayer.Checked = true;
            this.cbxSoundEnemyPlayer.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxSoundEnemyPlayer.Location = new System.Drawing.Point(61, 204);
            this.cbxSoundEnemyPlayer.Name = "cbxSoundEnemyPlayer";
            this.cbxSoundEnemyPlayer.Size = new System.Drawing.Size(15, 14);
            this.cbxSoundEnemyPlayer.TabIndex = 13;
            this.cbxSoundEnemyPlayer.UseVisualStyleBackColor = true;
            // 
            // cbxSoundHerbs
            // 
            this.cbxSoundHerbs.AutoSize = true;
            this.cbxSoundHerbs.Checked = true;
            this.cbxSoundHerbs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxSoundHerbs.Location = new System.Drawing.Point(61, 231);
            this.cbxSoundHerbs.Name = "cbxSoundHerbs";
            this.cbxSoundHerbs.Size = new System.Drawing.Size(15, 14);
            this.cbxSoundHerbs.TabIndex = 14;
            this.cbxSoundHerbs.UseVisualStyleBackColor = true;
            // 
            // cbxSoundVeins
            // 
            this.cbxSoundVeins.AutoSize = true;
            this.cbxSoundVeins.Location = new System.Drawing.Point(61, 258);
            this.cbxSoundVeins.Name = "cbxSoundVeins";
            this.cbxSoundVeins.Size = new System.Drawing.Size(15, 14);
            this.cbxSoundVeins.TabIndex = 15;
            this.cbxSoundVeins.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(797, 876);
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
            this.tabPageRadar.ResumeLayout(false);
            this.pnlRadarCfg.ResumeLayout(false);
            this.pnlRadarCfgValue.ResumeLayout(false);
            this.pnlRadarCfgValue.PerformLayout();
            this.pnlRadarCfgKey.ResumeLayout(false);
            this.pnlRadarCfgKey.PerformLayout();
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
        private System.Windows.Forms.TabPage tabPageRadar;
        private System.Windows.Forms.Panel pnlRadarCfg;
        private System.Windows.Forms.Panel pnlRadarCfgValue;
        private System.Windows.Forms.TextBox txtRadarSizeX;
        private System.Windows.Forms.TextBox txtRadarYPos;
        private System.Windows.Forms.TextBox txtRadarXPos;
        private System.Windows.Forms.CheckBox cbxEnableRadar;
        private System.Windows.Forms.Panel pnlRadarCfgKey;
        private System.Windows.Forms.Label lblRadarSize;
        private System.Windows.Forms.Label lblRadarYPos;
        private System.Windows.Forms.Label lblRadarXPos;
        private System.Windows.Forms.Label lblEnableRadar;
        private System.Windows.Forms.Button btnRadarAccept;
        private System.Windows.Forms.CheckBox cbxEnableBorder;
        private System.Windows.Forms.Label lblRadarBorder;
        private System.Windows.Forms.TextBox txtRadarSizeY;
        private System.Windows.Forms.CheckBox cbxSoundVeins;
        private System.Windows.Forms.CheckBox cbxSoundHerbs;
        private System.Windows.Forms.CheckBox cbxSoundEnemyPlayer;
        private System.Windows.Forms.CheckBox cbxSoundFriendlyPlayer;
        private System.Windows.Forms.Label lblRadarSound;
        private System.Windows.Forms.Label lblSectionDisplay;
        private System.Windows.Forms.CheckBox cbxRadarFriendlyPlayer;
        private System.Windows.Forms.CheckBox cbxRadarEnemyPlayer;
        private System.Windows.Forms.CheckBox cbxRadarVeins;
        private System.Windows.Forms.CheckBox cbxRadarHerbs;
        private System.Windows.Forms.Label lblRadarEnemyPlayer;
        private System.Windows.Forms.Label lblRadarFriendlyPlayer;
        private System.Windows.Forms.Label lblRadarVeins;
        private System.Windows.Forms.Label lblRadarHerbs;
    }
}

