using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Magic;
using System.Runtime.InteropServices;
using System.Threading;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.IO;




//namespace System.Windows.Forms
//{
//	public delegate void CbxIndexChangedEventHandler(MyComboBox cbx);
//	public class MyComboBox : ComboBox
//	{
//		[SRCategoryAttribute("CatBehavior")]
//		[SRDescriptionAttribute("selectedIndexChangedEventDescr")]
//		new public event CbxIndexChangedEventHandler SelectedIndexChanged;
//	}
//}


namespace Revlex
{


	public delegate void AttachedEventHandler();
	public delegate void GotBasicDataEventHandler();
	//public delegate void ScanSuccessfullEventHandler(uint offset);
	public delegate void RefreshUiDataEventHandler();
	public delegate void InternalUpdateEventHandler();
	public delegate void LoadCustomClassEventHandler();
	public delegate void ActionButtonDataGridFilledEventHandler();
	public delegate void PrerequisitesLoadedEventHandler();






	public partial class Form1 : Form
	{
		// check for focus windows
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		private static extern IntPtr GetForegroundWindow();

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);
















		public Form1()
		{

			InitializeComponent();
			InitializeAdditionalComponent();
			WowHelperObj = new WowHelpers(LocalPlayer, CurrentTarget,richDebug);
			AttachedEvent += new AttachedEventHandler(OnAttached);
			//ScanSuccessfullEvent += new ScanSuccessfullEventHandler(OnScanSuccessfull);
			RefreshUiDataEvent += new RefreshUiDataEventHandler(OnRefreshUiData);
			InternalUpdateEvent += new InternalUpdateEventHandler(OnInternalUpdate);
			GotBasicDataEvent += new GotBasicDataEventHandler(OnGotBasicData);
			LoadCustomClassEvent += new LoadCustomClassEventHandler(OnLoadCustomClass);
			ActionButtonDataGridFilledEvent += new ActionButtonDataGridFilledEventHandler(OnActionButtonDataGridFilled);
			PrerequisitesLoadedEvent += new PrerequisitesLoadedEventHandler(OnPrerequisitesLoaded);



		}














		//protected override void WndProc(ref Message m)
		//{
		//	if (m.Msg == 0x0312 && m.WParam.ToInt32() == MYACTION_HOTKEY_ID)
		//	{
		//		Log.Print("Key:" + MYACTION_HOTKEY_ID.ToString());
		//	}
		//	base.WndProc(ref m);
		//}

		string[] configData;
		Dictionary<string, Keys> SpecialKeyDic = new Dictionary<string, Keys>();
		globalKeyboardHook gkh = new globalKeyboardHook();
		private BindingList<WowAccCharacter> accountDirectories = new BindingList<WowAccCharacter>();
		private BindingList<string> transferAccNames = new BindingList<string>();
		public string lastDirDialogDir;
		public bool FormLoaded = false;
		public KeyPress SpellKeyPress = new KeyPress();
		private CodeDomProvider codeProvider = new CSharpCodeProvider();
		private CompilerParameters compilerParams = new CompilerParameters();
		public CustomRotation Rotation = new CustomRotation();
		private KeybindCombo[] cbxKeybindingConfigData = new KeybindCombo[11];
		private Dictionary<int, Action> RotationFuntcionAssignment = new Dictionary<int, Action>();
		private List<WowListObject> RadarObjList;
		private List<Spells> SpellsList;
		private List<SpellsOnCooldown> SpellsOnCdList;
		private List<Auras> PlayerBuffList;
		private List<Auras> PlayerDebuffList;
		private List<Auras> TargetBuffList;
		private List<Auras> TargetDebuffList;
		private bool FirstInternalUpdate = true;
		private bool FirstRefreshUi = true;
		private int LastCvarOffsetFromConfig;
		private int LastPidFromConfig;
		public event AttachedEventHandler AttachedEvent;
		//public event ScanSuccessfullEventHandler ScanSuccessfullEvent;
		public event RefreshUiDataEventHandler RefreshUiDataEvent;
		public event InternalUpdateEventHandler InternalUpdateEvent;
		public event GotBasicDataEventHandler GotBasicDataEvent;
		public event LoadCustomClassEventHandler LoadCustomClassEvent;
		public event ActionButtonDataGridFilledEventHandler ActionButtonDataGridFilledEvent;
		public event PrerequisitesLoadedEventHandler PrerequisitesLoadedEvent;






		public bool Attached = false;
		public bool GotBasicData = false;
		//public bool ScanSuccessfull = false;
		private WowObject LocalPlayer = new WowObject();
		private WowObject CurrentTarget = new WowObject();
		public WowHelpers WowHelperObj;


		private int uiRefreshTimer = 0;
		private int internalUpdateTimer = 0;
		public uint CvarOffset = 0;
		/*
		public List<CvarData>[] CvarDataList = {
			new List<CvarData>(),	// Spells
			new List<CvarData>(),	// Player Buffs
			new List<CvarData>(),	// Player Debuffs
			new List<CvarData>(),	// Target Buffs
			new List<CvarData>()	// Target Debuffs
		};
		*/
		//List<CvarData>[] CvarDataList;
		public uint spellCvarOffset = 0;
		public WowUnitList RadarList;

		public System.Windows.Forms.DataGridView dataGridWowObjList;
		public System.Windows.Forms.DataGridView[] dataGridCombatCvars = new System.Windows.Forms.DataGridView[6];
		public System.Windows.Forms.Panel[] pnlCombatCvars = new System.Windows.Forms.Panel[6];
		public System.Windows.Forms.Label[] lblCombatCvars = new System.Windows.Forms.Label[6];
		public System.Windows.Forms.DataGridViewTextBoxColumn[] cvarName = new System.Windows.Forms.DataGridViewTextBoxColumn[6];
		public System.Windows.Forms.DataGridViewTextBoxColumn[] cvarCd = new System.Windows.Forms.DataGridViewTextBoxColumn[6];
		public System.Windows.Forms.DataGridViewTextBoxColumn[] cvarId = new System.Windows.Forms.DataGridViewTextBoxColumn[6];

		public System.Windows.Forms.ComboBox[] cbxSelectKeybindings = new System.Windows.Forms.ComboBox[11];
		public System.Windows.Forms.ComboBox[] cbxSelectKeybindingsMod = new System.Windows.Forms.ComboBox[11];
		public System.Windows.Forms.Label[] lblKeybindings = new System.Windows.Forms.Label[11];

		public System.Windows.Forms.Panel pnlActionButtons = new System.Windows.Forms.Panel();
		public System.Windows.Forms.Label lblActionButtons = new System.Windows.Forms.Label();
		public System.Windows.Forms.DataGridView dataGridActionButtons = new System.Windows.Forms.DataGridView();
		private System.Windows.Forms.Button cmdRefreshActionButtonList = new Button();
		//public System.Windows.Forms.DataGridViewTextBoxColumn actionButtonSpellName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		//public System.Windows.Forms.DataGridViewTextBoxColumn actionButtonKeybind = new System.Windows.Forms.DataGridViewTextBoxColumn();
		//public System.Windows.Forms.DataGridViewTextBoxColumn actionButtonSpellId = new System.Windows.Forms.DataGridViewTextBoxColumn();
		//public System.Windows.Forms.DataGridViewTextBoxColumn actionButtonName = new System.Windows.Forms.DataGridViewTextBoxColumn();






		public void InitializeAdditionalComponent()
		{


			// InitializeAdditionalComponent() will executed BEFORE Form1_Load
			// Datasource for account name combobox
			// to obtain a auto refresh of datasource, use bindinglist	
			transferAccNames = new BindingList<string>(accountDirectories.Select(C => C.Name).ToList());
			cbxAccountName.DataSource = transferAccNames;

			// accountDirectories includes the WowAccCharacter class which includes "charname - servername" and the location of the bindingcache.wtf
			accountDirectories.Add(new WowAccCharacter("None",""));


			// to load the wow-class class on runtime
			compilerParams.CompilerOptions = "/target:library /optimize";
			compilerParams.GenerateExecutable = false;
			compilerParams.GenerateInMemory = true;
			compilerParams.IncludeDebugInformation = false;
			compilerParams.ReferencedAssemblies.Add("mscorlib.dll");
			compilerParams.ReferencedAssemblies.Add("System.dll");

			



			// data list for UI-Update
			List<ComboBoxTimeData> cbxRefreshSelectionData = new List<ComboBoxTimeData> {
				new ComboBoxTimeData("once per click",9999999),
				new ComboBoxTimeData("0.1 seconds",100),
				new ComboBoxTimeData("0.2 seconds",200),
				new ComboBoxTimeData("0.33 seconds",333),
				new ComboBoxTimeData("0.5 seconds",500),
				new ComboBoxTimeData("0.66 seconds",666),
				new ComboBoxTimeData("1 seconds",1000),
				new ComboBoxTimeData("2 seconds",2000),
				new ComboBoxTimeData("3 seconds",3000),
				new ComboBoxTimeData("5 seconds",5000),
				new ComboBoxTimeData("10 seconds",10000)
			};
			// data list for internal refresh
			List<ComboBoxTimeData> cbxInternalUpdateData = new List<ComboBoxTimeData> {
				new ComboBoxTimeData("once per click",9999999),
				new ComboBoxTimeData("0.1 seconds",100),
				new ComboBoxTimeData("0.2 seconds",200),
				new ComboBoxTimeData("0.33 seconds",333),
				new ComboBoxTimeData("0.5 seconds",500),
				new ComboBoxTimeData("0.66 seconds",666),
				new ComboBoxTimeData("0.75 seconds",75),
				new ComboBoxTimeData("1 seconds",1000),
				new ComboBoxTimeData("1.5 seconds",1500),
				new ComboBoxTimeData("2 seconds",2000),
				new ComboBoxTimeData("3 seconds",3000),
				new ComboBoxTimeData("5 seconds",5000)
			};


			// fill dictionary for special keys
			SpecialKeyDic.Add("´", Keys.Oem6);
			SpecialKeyDic.Add("ä", Keys.Oem7);
			SpecialKeyDic.Add("ö", Keys.Oemtilde);
			SpecialKeyDic.Add("ü", Keys.Oem1);
			SpecialKeyDic.Add("#", Keys.OemQuestion);
			SpecialKeyDic.Add("^", Keys.Oem5);
			SpecialKeyDic.Add("<", Keys.OemBackslash);
			SpecialKeyDic.Add("ß", Keys.OemOpenBrackets);
			SpecialKeyDic.Add(",", Keys.Oemcomma);
			SpecialKeyDic.Add(".", Keys.OemPeriod);
			SpecialKeyDic.Add("-", Keys.OemMinus);
			SpecialKeyDic.Add("+", Keys.Oemplus);


			// populate keybindingstuff with data
			// Aha! - we must array-ize the datasource too, when we want to fill our comboboxes with them
			List<string>[] cbxKeybindingsData = new List<string>[11];
			List<string>[] cbxSelectKeybindingsModData = new List<string>[11];

			for (int u = 0; u < 11; u++)
			{
				// data list for keybindings
				cbxKeybindingsData[u] = new List<string>();
				cbxKeybindingsData[u].Add("None");
				char c;
				for (int i = 0x30; i < 0x3a; i++)
				{
					c = (char)i;
					string s = c.ToString();
					cbxKeybindingsData[u].Add(s);
				}
				for (int i = 0x41; i < 0x5b; i++)
				{
					c = (char)i;
					string s = c.ToString();
					cbxKeybindingsData[u].Add(s);
				}
				cbxKeybindingsData[u].Add("Ä");
				cbxKeybindingsData[u].Add("Ö");
				cbxKeybindingsData[u].Add("Ü");
				cbxKeybindingsData[u].Add("ß");
				cbxKeybindingsData[u].Add("<");
				cbxKeybindingsData[u].Add("+");
				cbxKeybindingsData[u].Add("#");
				cbxKeybindingsData[u].Add(",");
				cbxKeybindingsData[u].Add(".");
				cbxKeybindingsData[u].Add("-");
				cbxKeybindingsData[u].Add("^");

				// Data list for keybindings modifier
				cbxSelectKeybindingsModData[u] = new List<string>();
				cbxSelectKeybindingsModData[u].Add("None");
				cbxSelectKeybindingsModData[u].Add("Alt");
				cbxSelectKeybindingsModData[u].Add("Shift");
				cbxSelectKeybindingsModData[u].Add("Control");
			}




			dataGridWowObjList = new System.Windows.Forms.DataGridView();
			//((System.ComponentModel.ISupportInitialize)(dataGridWowObjList)).BeginInit();
			// 
			// dataGridWowObjList
			// 
			dataGridWowObjList.AllowUserToDeleteRows = true;
			dataGridWowObjList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			dataGridWowObjList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
			dataGridWowObjList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridWowObjList.Location = new System.Drawing.Point(12, 42);
			dataGridWowObjList.Name = "dataGridWowObjList";
			dataGridWowObjList.ReadOnly = true;
			dataGridWowObjList.RowHeadersVisible = false;
			dataGridWowObjList.Size = new System.Drawing.Size(745, 350);
			dataGridWowObjList.TabIndex = 8;
			dataGridWowObjList.DataSource = RadarObjList;


			tabPageObjects.Controls.Add(dataGridWowObjList);
			//((System.ComponentModel.ISupportInitialize)(this.dataGridWowObjList)).EndInit();
			
			tabControl1.SelectedTab = tabPageRotation;
			cbxRefreshUi.DataSource = cbxRefreshSelectionData;
			cbxRefreshUi.DisplayMember = "name";
			cbxRefreshUi.ValueMember = "seconds";
			cbxRefreshUi.SelectedIndex = 3;
			cbxInternalUpdate.DataSource = cbxInternalUpdateData;
			cbxInternalUpdate.DisplayMember = "name";
			cbxInternalUpdate.ValueMember = "seconds";
			cbxInternalUpdate.SelectedIndex = 3;
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyleCombatCvars = new System.Windows.Forms.DataGridViewCellStyle();


			
			
			//
			// Datagrid for action buttons
			//
			// panel for data grids action buttons
			this.tabPageRotation.Controls.Add(this.pnlActionButtons);
			this.pnlActionButtons.SuspendLayout();
			this.pnlActionButtons.Controls.Add(this.dataGridActionButtons);
			this.pnlActionButtons.Name = "pnlActionButtons";
			this.pnlActionButtons.Location = new System.Drawing.Point(212, 11);
			this.pnlActionButtons.Size = new System.Drawing.Size(424, 387);


			// label for data grids action buttons
			this.pnlActionButtons.Controls.Add(this.lblActionButtons);
			this.lblActionButtons.AutoSize = true;
			this.lblActionButtons.Margin = new System.Windows.Forms.Padding(0);
			this.lblActionButtons.Name = "lblActionButtons";
			this.lblActionButtons.Location = new System.Drawing.Point(-3, 0);
			this.lblActionButtons.Size = new System.Drawing.Size(97, 16);
			this.lblActionButtons.Text = "Action Buttons";


			// data grid for combat action buttons	
			this.dataGridActionButtons.AllowUserToDeleteRows = false;
			this.dataGridActionButtons.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.dataGridActionButtons.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None;
			this.dataGridActionButtons.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.None;
			this.dataGridActionButtons.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
			this.dataGridActionButtons.Location = new System.Drawing.Point(0, 26);
			this.dataGridActionButtons.Size = new System.Drawing.Size(424, 355);
			this.dataGridActionButtons.ColumnHeadersHeight = 21;

			// cmdRefreshActionButtonList
			this.pnlActionButtons.Controls.Add(this.cmdRefreshActionButtonList);
			this.cmdRefreshActionButtonList.Cursor = System.Windows.Forms.Cursors.Hand;
			this.cmdRefreshActionButtonList.FlatAppearance.BorderSize = 0;
			this.cmdRefreshActionButtonList.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
			this.cmdRefreshActionButtonList.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
			this.cmdRefreshActionButtonList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.cmdRefreshActionButtonList.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.cmdRefreshActionButtonList.Location = new System.Drawing.Point(pnlActionButtons.Size.Width - cmdRefreshActionButtonList.Size.Width + 9, -4);
			this.cmdRefreshActionButtonList.Name = "cmdRefreshActionButtonList";
			this.cmdRefreshActionButtonList.Size = new System.Drawing.Size(75, 25);
			this.cmdRefreshActionButtonList.TabIndex = 4;
			this.cmdRefreshActionButtonList.Text = "⭮ Refresh";
			this.cmdRefreshActionButtonList.Enabled = false;
			this.cmdRefreshActionButtonList.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.cmdRefreshActionButtonList.UseVisualStyleBackColor = true;
			this.cmdRefreshActionButtonList.Click += new System.EventHandler(this.cmdRefreshActionButtonList_Click);
			this.cmdRefreshActionButtonList.MouseEnter += new System.EventHandler(this.cmdRefreshActionButtonList_MouseEnter);
			this.cmdRefreshActionButtonList.MouseLeave += new System.EventHandler(this.cmdRefreshActionButtonList_MouseLeave);
			this.cmdRefreshActionButtonList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cmdRefreshActionButtonList_MouseDown);
			this.cmdRefreshActionButtonList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cmdRefreshActionButtonList_MouseUp);



			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyleActionButtons = new System.Windows.Forms.DataGridViewCellStyle();
			dataGridViewCellStyleActionButtons.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyleActionButtons.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyleActionButtons.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
			dataGridViewCellStyleActionButtons.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyleActionButtons.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyleActionButtons.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyleActionButtons.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridActionButtons.DefaultCellStyle = dataGridViewCellStyleActionButtons;
			this.dataGridActionButtons.Dock = System.Windows.Forms.DockStyle.None;
			this.dataGridActionButtons.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.dataGridActionButtons.Name = "dataGridActionButtons";
			this.dataGridActionButtons.ReadOnly = true;
			this.dataGridActionButtons.RowHeadersVisible = false;
			this.dataGridActionButtons.RowTemplate.Height = 18;
			this.dataGridActionButtons.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;




			//
			// Draw & initialize the keybinding ui
			//
			for (int i = 0; i < 11; i++)
			{
				this.lblKeybindings[i] = new System.Windows.Forms.Label();
				this.cbxSelectKeybindings[i] = new ComboBox();
				this.cbxSelectKeybindingsMod[i] = new System.Windows.Forms.ComboBox();

				// label for lblKeybindings
				//this.tabPageRotation.Controls.Add(this.lblKeybindings[i]);
				this.grpBoxBindings.Controls.Add(this.lblKeybindings[i]);

				this.lblKeybindings[i].AutoSize = true;
				this.lblKeybindings[i].Margin = new System.Windows.Forms.Padding(0);
				this.lblKeybindings[i].Name = "lblKeybindings" + (i + 1).ToString();
				this.lblKeybindings[i].Location = new System.Drawing.Point(10, 51 + (i * 30));
				this.lblKeybindings[i].Size = new System.Drawing.Size(97, 16);
				this.lblKeybindings[i].TabIndex = 2 + (i * 10);
				this.lblKeybindings[i].Text = "#" + (i + 1) + ":";

				// 
				// cbxSelectKeybindings
				// 
				//this.tabPageRotation.Controls.Add(this.cbxSelectKeybindings[i]);
				this.grpBoxBindings.Controls.Add(this.cbxSelectKeybindings[i]);
				this.cbxSelectKeybindings[i].DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
				this.cbxSelectKeybindings[i].FormattingEnabled = true;
				this.cbxSelectKeybindings[i].Location = new System.Drawing.Point(40, 47 + (i * 30));
				this.cbxSelectKeybindings[i].Name = "cbxSelectKeybindings" + (i + 1).ToString();				
				this.cbxSelectKeybindings[i].Size = new System.Drawing.Size(60, 21);
				this.cbxSelectKeybindings[i].TabIndex = 3 + (i * 10);
				this.cbxSelectKeybindings[i].SelectedIndexChanged += new System.EventHandler(this.cbxSelectKeybindings_CheckForUniqueness);
				this.cbxSelectKeybindings[i].DataSource = cbxKeybindingsData[i];
				if (cbxSelectKeybindings[i].SelectedIndex == 0)
				{
					lblKeybindings[i].ForeColor = Color.FromArgb(175, 175, 175);
				}

				// 
				// cbxSelectKeybindingsModifier
				// 
				this.tabPageRotation.Controls.Add(this.cbxSelectKeybindingsMod[i]);
				this.grpBoxBindings.Controls.Add(this.cbxSelectKeybindingsMod[i]);
				this.cbxSelectKeybindingsMod[i].DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
				this.cbxSelectKeybindingsMod[i].FormattingEnabled = true;
				this.cbxSelectKeybindingsMod[i].Location = new System.Drawing.Point(112, 47 + (i * 30));
				this.cbxSelectKeybindingsMod[i].Name = "cbxSelectKeybindingsMod" + (i + 1).ToString();
				this.cbxSelectKeybindingsMod[i].Size = new System.Drawing.Size(60, 21);
				this.cbxSelectKeybindingsMod[i].TabIndex = 4 + (i * 10);
				this.cbxSelectKeybindingsMod[i].SelectedIndexChanged += new System.EventHandler(this.cbxSelectKeybindings_CheckForUniqueness);
				this.cbxSelectKeybindingsMod[i].DataSource = cbxSelectKeybindingsModData[i];
			}

			//
			// Draw & initialize the additional data grids for combat cvars
			//
			for (int i = 0; i < 6; i++)
			{
				// init the form elements for the combat tab
				this.cvarName[i] = new System.Windows.Forms.DataGridViewTextBoxColumn();
				this.cvarCd[i] = new System.Windows.Forms.DataGridViewTextBoxColumn();
				this.cvarId[i] = new System.Windows.Forms.DataGridViewTextBoxColumn();
				this.lblCombatCvars[i] = new System.Windows.Forms.Label();
				this.pnlCombatCvars[i] = new System.Windows.Forms.Panel(); // init
				this.dataGridCombatCvars[i] = new System.Windows.Forms.DataGridView(); // init
				

				// panel for data grids in combat
				this.tabPageCombat.Controls.Add(this.pnlCombatCvars[i]);
				this.pnlCombatCvars[i].SuspendLayout();
				this.pnlCombatCvars[i].Controls.Add(this.dataGridCombatCvars[i]);
				this.pnlCombatCvars[i].Name = "pnlCombatCvars" + i.ToString();
				this.pnlCombatCvars[i].TabIndex = 1 + (i * 10);

				// label for data grids in combat
				this.pnlCombatCvars[i].Controls.Add(this.lblCombatCvars[i]);
				this.lblCombatCvars[i].AutoSize = true;
				this.lblCombatCvars[i].Margin = new System.Windows.Forms.Padding(0);
				this.lblCombatCvars[i].Name = "lblCombatCvars" + i.ToString();
				this.lblCombatCvars[i].Location = new System.Drawing.Point(-4, 0);
				this.lblCombatCvars[i].Size = new System.Drawing.Size(97, 16);
				this.lblCombatCvars[i].TabIndex = 2 + (i * 10);

				// data grid for combat cvars	
				this.dataGridCombatCvars[i].AllowUserToDeleteRows = false;
				this.dataGridCombatCvars[i].ScrollBars = System.Windows.Forms.ScrollBars.Both;
				this.dataGridCombatCvars[i].AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None;
				this.dataGridCombatCvars[i].AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.None;
				this.dataGridCombatCvars[i].CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
				this.dataGridCombatCvars[i].Location = new System.Drawing.Point(0, 26);
				this.dataGridCombatCvars[i].ColumnHeadersHeight = 21;

				dataGridViewCellStyleCombatCvars.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
				dataGridViewCellStyleCombatCvars.BackColor = System.Drawing.SystemColors.Window;
				dataGridViewCellStyleCombatCvars.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
				dataGridViewCellStyleCombatCvars.ForeColor = System.Drawing.SystemColors.ControlText;
				dataGridViewCellStyleCombatCvars.SelectionBackColor = System.Drawing.SystemColors.Highlight;
				dataGridViewCellStyleCombatCvars.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
				dataGridViewCellStyleCombatCvars.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
				this.dataGridCombatCvars[i].DefaultCellStyle = dataGridViewCellStyleCombatCvars;
				this.dataGridCombatCvars[i].Dock = System.Windows.Forms.DockStyle.None;
				this.dataGridCombatCvars[i].Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
				this.dataGridCombatCvars[i].Name = "dataGridCombatCvars" + i.ToString();
				this.dataGridCombatCvars[i].ReadOnly = true;
				this.dataGridCombatCvars[i].RowHeadersVisible = false;
				this.dataGridCombatCvars[i].RowTemplate.Height = 18;
				this.dataGridCombatCvars[i].RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
				this.dataGridCombatCvars[i].TabIndex = 3 + (i * 10);
			}
			// data grid for combat cvars	
			this.dataGridCombatCvars[0].DataSource = SpellsList;
			this.dataGridCombatCvars[1].DataSource = PlayerBuffList;
			this.dataGridCombatCvars[2].DataSource = PlayerDebuffList;
			this.dataGridCombatCvars[1].DataSource = TargetBuffList;
			this.dataGridCombatCvars[2].DataSource = TargetDebuffList;
			/*
			for (int i = 3; i < 6; i++)
			{
				this.dataGridCombatCvars[i].Columns.AddRange
				(new System.Windows.Forms.DataGridViewColumn[]
					{
					this.cvarName[i],
					this.cvarCd[i],
					this.cvarId[i]
					}
				);

				// data grid rows
				this.cvarName[i].AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
				//this.cvarName[i].FillWeight = 110F;
				this.cvarName[i].Width = 123;
				this.cvarName[i].HeaderText = "Name";
				this.cvarName[i].Name = "cvarName" + i.ToString();
				this.cvarName[i].ReadOnly = true;

				this.cvarCd[i].AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
				this.cvarCd[i].HeaderText = "Cd";
				this.cvarCd[i].Name = "cvarCd" + i.ToString();
				this.cvarCd[i].ReadOnly = true;
				this.cvarCd[i].Width = 52;
				this.cvarCd[i].ValueType = typeof(double);

				this.cvarId[i].AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
				this.cvarId[i].HeaderText = "Id";
				this.cvarId[i].Name = "cvarId" + i.ToString();
				this.cvarId[i].ReadOnly = true;
				this.cvarId[i].Width = 60;
				this.cvarCd[i].ValueType = typeof(int);
			}
			*/

			int tmpLilBoxHeight = 179;

			// individual design data for combat tab elements
			// spell cvar aka Array 0
			this.pnlCombatCvars[0].Location = new System.Drawing.Point(12, 11);
			this.pnlCombatCvars[0].Size = new System.Drawing.Size(175, 383);
			this.lblCombatCvars[0].Text = "Spell Cooldown";
			this.dataGridCombatCvars[0].Size = new System.Drawing.Size(175, 355);

			// Player Buffs aka Array 1
			this.pnlCombatCvars[1].Location = new System.Drawing.Point(202, 11);
			this.pnlCombatCvars[1].Size = new System.Drawing.Size(175, tmpLilBoxHeight);
			this.lblCombatCvars[1].Text = "Buffs Player";
			this.dataGridCombatCvars[1].Size = new System.Drawing.Size(175, tmpLilBoxHeight - 26);

			// Player Debuffs aka Array 2
			this.pnlCombatCvars[2].Location = new System.Drawing.Point(202, tmpLilBoxHeight + 34);
			this.pnlCombatCvars[2].Size = new System.Drawing.Size(175, tmpLilBoxHeight);
			this.lblCombatCvars[2].Text = "Debuffs Player";
			this.dataGridCombatCvars[2].Size = new System.Drawing.Size(175, tmpLilBoxHeight - 26);

			// Target Buffs aka Array 3
			this.pnlCombatCvars[3].Location = new System.Drawing.Point(392, 11);
			this.pnlCombatCvars[3].Size = new System.Drawing.Size(175, tmpLilBoxHeight);
			this.lblCombatCvars[3].Text = "Buffs Target";
			this.dataGridCombatCvars[3].Size = new System.Drawing.Size(175, tmpLilBoxHeight - 26);

			// Target Debuffs aka Array 4
			this.pnlCombatCvars[4].Location = new System.Drawing.Point(392, tmpLilBoxHeight + 34);
			this.pnlCombatCvars[4].Size = new System.Drawing.Size(175, tmpLilBoxHeight);
			this.lblCombatCvars[4].Text = "Debuffs Target";
			this.dataGridCombatCvars[4].Size = new System.Drawing.Size(175, tmpLilBoxHeight - 26);

			// Other Data aka Array 5
			this.pnlCombatCvars[5].Location = new System.Drawing.Point(582, 11);
			this.pnlCombatCvars[5].Size = new System.Drawing.Size(175, 383);
			this.lblCombatCvars[5].Text = "Other Data";
			this.dataGridCombatCvars[5].Size = new System.Drawing.Size(175, 355);

			for (int i = 0; i < 6; i++)
			{
				this.pnlCombatCvars[i].ResumeLayout(false);
			}
		}



		private void txtSearchUnit_TextChanged(object sender, EventArgs e)
		{

		}

		private void cmdAttach_Click(object sender, EventArgs e)
		{
			tryAttaching();
		}
		private void tryAttaching()
		{
			RvxButtonState(cmdAttach, 2, false);
			if (WowHelperObj.Init())
			{
				Attached = true;
				AttachedEvent();
			}
			else
			{
				Attached = false;
				RvxButtonState(cmdAttach, 0, true);
			}
		}
		public void OnAttached()
		{
			Log.Print("OnAttached()");
			RvxButtonState(cmdAttach, 3, false);
			if (WowHelperObj.GetBasicWowData())
			{
				FirstInternalUpdateCheck();
				// Load hostile Factions from cfg, based on own Faction and when Localplayer is defined				
				if (Constants.GetPlayerFaction(WowHelperObj.LocalPlayer.FactionTemplate) == "Alliance")
				{
					CalculateHostileFactions(configData[17]);
					Log.Print("Load Hostiles for A");
				}
				else if (Constants.GetPlayerFaction(WowHelperObj.LocalPlayer.FactionTemplate) == "Horde")
				{
					CalculateHostileFactions(configData[18]);
					Log.Print("Load Hostiles for H");
				}
				GotBasicData = true;
				GotBasicDataEvent();
			}
			else
			{
				Attached = false;
				GotBasicData = false;
				RvxButtonState(cmdAttach, 0, true);
			}
		}

		//// old Cvar-Scan here
		//public void OnGotBasicData()
		//{
		//	Log.Print("OnGotBasicDataEvent()");
		//	RvxButtonState(cmdAttach, 4, false);
		//	ScanCvar.Scanner(WowHelperObj.wowMem, config, LastCvarOffsetFromConfig, LastPidFromConfig, richDebug, this);
		//}

		public void OnPrerequisitesLoaded()
		{

		}

		private void button1_Click(object sender, EventArgs e)
		{
			SpellKeyPress.KeyUp(WowHelperObj.wowMem);
		}

		public void OnGotBasicData()
		{
			Log.Print("OnGotBasicDataEvent()");
			if (cbxSelectWowClassfile.Items.Count != 0)			{
				cbxSelectWowClassfile.Enabled = false;
				WowAccCharacter x = accountDirectories.Where(C => C.Name == cbxAccountName.SelectedItem.ToString()).First();
				FillActionButtonDataGrid(dataGridActionButtons, x.BindingFile);
				ActionButtonDataGridFilledEvent();
			}
			else
			{
				Log.Print("No class file loaded!", 4);
				MessageBox.Show("No class file loaded!");
				RvxButtonState(cmdAttach, 0, true);
				RvxButtonState(cmdRefreshUi, 0, false);
				RvxButtonState(cmdInternalUpdate, 0, false);
				cbxSelectWowClassfile.Enabled = true;
				Attached = false;
				GotBasicData = false;
			}

			//LoadCustomClassEvent();
		}

		public void OnActionButtonDataGridFilled()
		{
			Log.Print("OnActionButtonDataGridFilled()");
			if (WowHelperObj.ActionButtonAndSpellList.Count != 0)
			{
				RvxButtonState(cmdAttach, 0, false);
				RvxButtonState(cmdRefreshUi, 0, true);
				RvxButtonState(cmdInternalUpdate, 0, true);
				RvxButtonState(cmdRefreshActionButtonList, 9, true);
			}
			else
			{
				Log.Print("No actions buttons loaded!", 4);
				MessageBox.Show("No actions buttons file loaded!");
				RvxButtonState(cmdAttach, 0, true);
				RvxButtonState(cmdRefreshUi, 0, false);
				RvxButtonState(cmdInternalUpdate, 0, false);
				RvxButtonState(cmdRefreshActionButtonList, 9, false);
				Attached = false;
				GotBasicData = false;
			}
		}

		// Not used anymore
		//public void OnScanSuccessfull(uint offset)
		//{
		//	if (offset != 0)
		//	{
		//		ScanSuccessfull = true;
		//		Log.Print("OnScanSuccessfull()");
		//		RvxButtonState(cmdAttach, 0, false);
		//		RvxButtonState(cmdRefreshUi, 0, true);
		//		RvxButtonState(cmdInternalUpdate, 0, true);
		//	}
		//	else
		//	{
		//		Attached = false;
		//		GotBasicData = false;
		//		ScanSuccessfull = false;
		//		RvxButtonState(cmdAttach, 0, true);
		//		RvxButtonState(cmdRefreshUi, 0, false);
		//		RvxButtonState(cmdInternalUpdate, 0, false);
		//	}
		//}

		public void OnLoadCustomClass()
		{
			Log.Print("OnLoadCustomClass()");

			//StreamReader streamReader = new StreamReader("scanBlock.txt");
			// compile the code
			//CompilerResults results = codeProvider.CompileAssemblyFromSource(compilerParams, sourceCode);



		}

		public void OnRefreshUiData()
		{
			Log.Print("OnRefreshUiData()");
			refreshUiThings();
			clearLog();
		}
		public void OnInternalUpdate()
		{
			Log.Print("OnInternalUpdate()");
			if (!internalUpdateThings())
			{
				timerInternalUpdate.Stop();
				timerRefreshUi.Stop();
				tryAttaching();
			}
			clearLog();
		}
		private void clearLog()
		{
			while (richDebug.Lines.Length > 1570)
			{
				richDebug.Select(0, richDebug.GetFirstCharIndexFromLine((richDebug.Lines.Length-30))); // select the first line
				richDebug.SelectedText = "";
			}
			//if (richDebug.TextLength > 1000)
			//{
			//	richDebug.Clear();
			//	richDebug.Lines.Remove();
			//}
		}



		private void cmdInternalUpdate_Click(object sender, EventArgs e)
		{
			Log.Print("\\\\\\\\\\\\\\");
			// Internal Update
			if (cbxInternalUpdate.SelectedIndex == 0)
			{
				InternalUpdateEvent();
			}
			else
			{
				timerInternalUpdate.Interval = internalUpdateTimer;
				if (timerInternalUpdate.Enabled == false)
				{
					timerInternalUpdate.Start();
					RvxButtonState(cmdInternalUpdate, 8, true);
				}
				else
				{
					timerInternalUpdate.Stop();
					RvxButtonState(cmdInternalUpdate, 0, true);
				}
			}

		}
		private void cmdRefreshUi_Click(object sender, EventArgs e)
		{
			Log.Print("///////");
			// UI Refresh
			if (cbxRefreshUi.SelectedIndex == 0)
			{
				RefreshUiDataEvent();
			}
			else
			{
				timerRefreshUi.Interval = uiRefreshTimer;
				if (timerRefreshUi.Enabled == false)
				{
					timerRefreshUi.Start();
					RvxButtonState(cmdRefreshUi, 8, true);
				}
				else
				{
					timerRefreshUi.Stop();
					RvxButtonState(cmdRefreshUi, 0, true);
				}
			}
		}


		// sets the color of any cmd-button
		public void RvxButtonState(Button btn, int btnColorId, bool enabled)
		{
			Color[] btnColor = new Color[10] {
				Color.FromKnownColor(KnownColor.Control),
				Color.FromArgb(255, 64, 0),
				Color.FromArgb(255, 128, 0),
				Color.FromArgb(255, 192, 0),
				Color.FromArgb(255, 255, 0),
				Color.FromArgb(64, 192, 64),
				Color.FromArgb(0, 192, 192),
				Color.FromArgb(64, 64, 192),
				Color.FromArgb(128, 255, 128),
				Color.FromArgb(255, 255, 255)
			};
			btn.Enabled = enabled;
			btn.BackColor = btnColor[btnColorId];
		}

		private void timerRefreshUi_Tick(object sender, EventArgs e)
		{
			RefreshUiDataEvent();
		}

		private void timerInternalUpdate_Tick(object sender, EventArgs e)
		{
			InternalUpdateEvent();
		}


		private bool checkWowLinking()
		{
			if (WowHelperObj.GetLocalPlayerGuid() != 0)
			{
				return true;
			}
			else
			{
				Log.Print("Not in world.", 4);
				if (!WowHelperObj.Init(false))
				{
					Log.Print("No longer attached to a wow process.", 4);
				}
				RvxButtonState(cmdAttach, 0, true);
				RvxButtonState(cmdRefreshUi, 0, false);
				RvxButtonState(cmdInternalUpdate, 0, false);
				RvxButtonState(cmdRefreshActionButtonList, 9, false);
				FirstRefreshUi = true;
				FirstInternalUpdate = true;
				Attached = false;
				GotBasicData = false;
				Log.Print("Reset Buttons, set FirstInternalUpdate 'true', set FirstRefreshUi 'true', set Attached 'false', set GotBasicData 'false'");
				return false;
			}
		}

		public void FirstInternalUpdateCheck()
		{
			if (!WowHelperObj.CheckConnection())
			{
				// if Connection failed, we reset FirstInternalUpdate
				FirstInternalUpdate = true;
			}
			if (FirstInternalUpdate)
			{
				LocalPlayer = WowHelperObj.LocalPlayer;
				Log.Print("CHECK!");
				SpellsList = WowHelperObj.GetPlayerSpells();
				foreach (Spells obj in SpellsList)
				{
					obj.Cd = WowHelperObj.GetSpellCooldown(obj.Id);
				}
				FirstInternalUpdate = false;
				Log.Print("playerName: " + (WowHelperObj.wowMem.ReadASCIIString(((uint)WowHelperObj.wowMem.MainModule.BaseAddress + (uint)0x827D88), 0x8)).ToString());
				Log.Print("Base: " + WowHelperObj.wowMem.MainModule.BaseAddress.ToString() + "#: " + WowHelperObj.wowMem.MainModule.BaseAddress.ToString("x"));
				Log.Print("set FirstInternalUpdate 'false'");
			}
		}

		private bool internalUpdateThings()
		{
			if (checkWowLinking())
			{
				FirstInternalUpdateCheck();

				/* // vorerst deaktiviert 30.7.2017, siehe Ende von form1.cs 
				 */
				RadarObjList = RadarList.ScanObj();
				CurrentTarget = WowHelperObj.GetCurrentTarget();
				PlayerBuffList = WowHelperObj.GetUnitBuffs(LocalPlayer);
				PlayerDebuffList = WowHelperObj.GetUnitDebuffs(LocalPlayer);
				if (CurrentTarget != null && CurrentTarget.Guid != 0)
				{
					TargetBuffList = WowHelperObj.GetUnitBuffs(CurrentTarget);
					TargetDebuffList = WowHelperObj.GetUnitDebuffs(CurrentTarget);
				}
				SpellsOnCdList = WowHelperObj.GetPlayerSpellsOnCooldown();
	
			}
			else
			{
				return false;
			}
			return true;
		}


		private bool refreshUiThings()
		{
			string lblExtensionPlayerBuffs = "";
			string lblExtensionPlayerDebuffs = "";
			string lblExtensionTargetBuffs = "";
			string lblExtensionTargetDebuffs = "";

			if (FirstRefreshUi)
			{
				if (SpellsList != null)
				{
					ScanCvar.GenerateCompleteSpellList(dataGridCombatCvars[0], SpellsList);
					FirstRefreshUi = false;
				}
			}
			SpellsList = ScanCvar.UpdateSpellList(dataGridCombatCvars[0], SpellsList, SpellsOnCdList, WowHelperObj);
			dataGridCombatCvars[0].Refresh();

			if (LocalPlayer != null && LocalPlayer.Guid != 0)
			{
				dataGridCombatCvars[1].DataSource = PlayerBuffList;
				dataGridCombatCvars[2].DataSource = PlayerDebuffList;
				lblExtensionPlayerBuffs = ("Buffs Player - " + LocalPlayer.Name + "                    ").Substring(0, 33);
				lblExtensionPlayerDebuffs = ("Debuffs Player - " + LocalPlayer.Name + "                    ").Substring(0, 33);
			}
			lblCombatCvars[1].Text = lblExtensionPlayerBuffs;
			lblCombatCvars[2].Text = lblExtensionPlayerDebuffs;

			if (CurrentTarget != null && CurrentTarget.Guid != 0)
			{
				dataGridCombatCvars[3].DataSource = TargetBuffList;
				dataGridCombatCvars[4].DataSource = TargetDebuffList;
				lblExtensionTargetBuffs = ("Buffs Target - " + CurrentTarget.Name + "                    ").Substring(0, 33);
				lblExtensionTargetDebuffs = ("Debuffs Target - " + CurrentTarget.Name + "                    ").Substring(0, 33);
			}
			else
			{
				if (TargetBuffList != null) { TargetBuffList.Clear(); }
				if (TargetDebuffList != null) { TargetDebuffList.Clear(); }
				dataGridCombatCvars[3].DataSource = null;
				dataGridCombatCvars[4].DataSource = null;
				lblExtensionTargetBuffs = "Buffs Target";
				lblExtensionTargetDebuffs = "Debuffs Target";
			}
			lblCombatCvars[3].Text = lblExtensionTargetBuffs;
			lblCombatCvars[4].Text = lblExtensionTargetDebuffs;
			dataGridCombatCvars[1].Refresh();
			dataGridCombatCvars[2].Refresh();
			dataGridCombatCvars[3].Refresh();
			dataGridCombatCvars[4].Refresh();

			// refresh object 
			if (RadarList != null)
			{
				RadarList.RefreshList(RadarObjList);
			}
			else
			{
				return false;
			}
			return true;

		}

		/*
		public void ShowSpellList()
		{
			//dataGridCombatCvars[1].Rows.Cast<DataGridViewRow>().Where(x => Int32.TryParse(x.Cells[1]) > 0).
			if (dataGridCombatCvars[1] != null && dataGridCombatCvars[0].Rows != null)
			{
				foreach (DataGridViewRow row in dataGridCombatCvars[0].Rows)
				{
					if (row.Cells[0].Value != null)
						Log.Print(row.Cells[1].Value.ToString());
				}
			}
			
		}
		*/
		public void CalculateHostileFactions(string s)
		{
			int tempHostFac;
			string[] tempStrArr = s.Split(',');
			foreach (string str in tempStrArr)
			{
				Int32.TryParse(str, out tempHostFac);
				WowHelperObj.HostileFaction[tempHostFac] = true;
				Log.Print("Hostile: " + tempHostFac + " " + WowHelperObj.HostileFaction[tempHostFac]);
			}
		}

		public void Form1_Load(object sender, EventArgs e)
		{
			// Form1_Load() will executed AFTER InitializeAdditionalComponent() 
			timerCheckHook.Start();
			RvxButtonState(cmdRefreshUi, 0, false);
			RvxButtonState(cmdInternalUpdate, 0, false);
			RadarList = new WowUnitList(txtSearchUnit, 400, dataGridWowObjList, WowHelperObj);
			configData = Prerequisites.LoadConfig();
			cbxSelectWowClassfile.DataSource = Prerequisites.GetAllWowClassFiles();
			//cbxAccountName.DataSource = Prerequisites.GetAccountNames();
			Int32.TryParse(configData[0], out LastCvarOffsetFromConfig);
			Int32.TryParse(configData[1], out LastPidFromConfig);
			ValidateAndSetWowFolder(configData[3]);
			ValidateAndSetAccountName(configData[2]);
			int tempX = 0;
			int tempY = 0;
			Int32.TryParse(configData[4], out tempX);
			Int32.TryParse(configData[5], out tempY);
			this.Location = new Point(tempX, tempY);
			for (int i = 0; i < 11; i++)
			{
				cbxKeybindingConfigData[i] = new KeybindCombo("None", "None",(uint)i+1,Rotation);
				cbxKeybindingConfigData[i].CompoundString = configData[6 + i];
				cbxSelectKeybindings[i].SelectedItem = cbxKeybindingConfigData[i].Hotkey.ToUpper();
				cbxSelectKeybindingsMod[i].SelectedItem = cbxKeybindingConfigData[i].Modifer;
				CheckKeybindColor(cbxSelectKeybindings[i], lblKeybindings[i]);
			}

			SetRotationKeys();

			gkh.KeyDown += new KeyEventHandler(gkh_KeyDown);
			gkh.KeyUp += new KeyEventHandler(gkh_KeyUp);


			// Form was loaded
			FormLoaded = true;
		}

		// writes the hotkeys for the Rotation to the keyboard hook list
		private void SetRotationKeys()
		{
			
			Log.Print("--------SetRotationKeys()");

			gkh.HookedKeys.Clear();
			
			for (int i = 0; i < 11; i++)
			{
				int tmpVK = (cbxKeybindingConfigData[i].Hotkey).ToUpper().ToCharArray().First();
				if (cbxKeybindingConfigData[i].Hotkey != "None" && !SpecialKeyDic.ContainsKey(cbxKeybindingConfigData[i].Hotkey))
				{
					Log.Print(tmpVK.ToString("X") + " - " + ((Keys)tmpVK).ToString());
					gkh.HookedKeys.Add((Keys)tmpVK);
					cbxKeybindingConfigData[i].Realkey = (Keys)tmpVK;
				}
				else if (SpecialKeyDic.ContainsKey(cbxKeybindingConfigData[i].Hotkey))
				{
					Log.Print(tmpVK.ToString("X") + " - " + SpecialKeyDic[cbxKeybindingConfigData[i].Hotkey].ToString());
					gkh.HookedKeys.Add(SpecialKeyDic[cbxKeybindingConfigData[i].Hotkey]);
					cbxKeybindingConfigData[i].Realkey = SpecialKeyDic[cbxKeybindingConfigData[i].Hotkey];
				}
				
			}
			//gkh.HookedKeys.Add(Keys.Alt);
			//gkh.HookedKeys.Add(Keys.Shift);
			//gkh.HookedKeys.Add(Keys.Control);
			//gkh.HookedKeys.Add(Keys.LShiftKey);
			//gkh.HookedKeys.Add(Keys.LControlKey);
			//gkh.HookedKeys.Add(Keys.Control);




			//List<Keys> bla = Enum.GetValues(typeof(Keys)).Cast<Keys>().ToList();
			//foreach (Keys x in bla)
			//{
			//	gkh.HookedKeys.Add(x);
			//}
		}

		void gkh_KeyUp(object sender, KeyEventArgs e)
		{
			//Log.Print("Up\t" + e.KeyCode.ToString() + " " + ApplicationIsActivated() + "\t mod: " + Control.ModifierKeys.ToString());
			//cbxKeybindingConfigData.Select(c => c.Realkey.ToString() == e.KeyCode.ToString()).First();
			//e.Handled = true; // we turned this off, so it diddnt block the keypress from wow




		}

		void gkh_KeyDown(object sender, KeyEventArgs e)
		{
			
			//Log.Print("Down\t" + e.KeyCode.ToString() + " " + ApplicationIsActivated() + "\t mod: "+ Control.ModifierKeys.ToString());
			KeybindCombo tmpCombo = cbxKeybindingConfigData.Where(c => c.Realkey.ToString() == e.KeyCode.ToString()).FirstOrDefault();
			//e.Handled = true; // we turned this off, so it diddnt block the keypress from wow
			if (tmpCombo != null && tmpCombo.Modifer == Control.ModifierKeys.ToString())
			{
				Log.Br();
				Log.Print("------------------------- Keydown");
				WowHelperObj.ScanObj();
				//WowHelperObj.GetObjectBaseByGuid2(WowHelperObj.LocalPlayer.Guid);
				tmpCombo.RotationFunc();
				clearLog();
			}

		}


		private void Form1_FormClosed(object sender, FormClosedEventArgs e)
		{
			//UnhookWindowsHookEx(_hookHandle);
		}


		private void richDebug_TextChanged(object sender, EventArgs e)
		{
			/*
			int closingTimeBracket = richDebug.Text.LastIndexOf("]");
			int openingTimeBracket = richDebug.Text.LastIndexOf("[");
			int textLength = richDebug.Text.Length;
			*/
			richDebug.SelectionStart = richDebug.Text.Length;
			richDebug.ScrollToCaret();
			/*
			richDebug.SelectionStart = openingTimeBracket;
			richDebug.SelectionLength = closingTimeBracket - openingTimeBracket;
			richDebug.SelectionColor = Color.Green;
			richDebug.SelectionStart = closingTimeBracket;
			richDebug.SelectionLength = textLength - closingTimeBracket;
			richDebug.SelectionColor = Color.Red;
			*/

		}

		private void cbxRefreshUi_SelectedIndexChanged(object sender, EventArgs e)
		{
			bool result = Int32.TryParse(cbxRefreshUi.SelectedValue.ToString(), out uiRefreshTimer);
			if (cbxRefreshUi.SelectedIndex > 0)
			{
				timerRefreshUi.Interval = uiRefreshTimer;
				if (timerInternalUpdate.Enabled == true)
				{
					timerRefreshUi.Start();
				}
			}
			else 
			{
				timerRefreshUi.Stop();
			}
		}




		private void cbxInternalUpdate_SelectedIndexChanged(object sender, EventArgs e)
		{
			bool result = Int32.TryParse(cbxInternalUpdate.SelectedValue.ToString(), out internalUpdateTimer);
			if (cbxInternalUpdate.SelectedIndex > 0)
			{
				timerInternalUpdate.Interval = internalUpdateTimer;
				if (timerRefreshUi.Enabled == true)
				{
					timerInternalUpdate.Start();
				}
			}
			else
			{
				timerInternalUpdate.Stop();
			}
		}


		private void cbxSelectKeybindings_CheckForUniqueness(object sender, EventArgs e)
		{
			if (FormLoaded)
			{
				ComboBox current = new ComboBox();
				current = (sender as ComboBox);
				int u = 0;
				foreach (ComboBox c in cbxSelectKeybindings)
				{
					if (c.SelectedIndex != 0 && c.Name != current.Name && c.SelectedIndex == current.SelectedIndex)
					{
						c.SelectedIndex = 0;
					}
					CheckKeybindColor(c, lblKeybindings[u]);
					// add the data from combobox to real data
					cbxKeybindingConfigData[u].Hotkey = (cbxSelectKeybindings[u].SelectedItem).ToString();
					cbxKeybindingConfigData[u].Modifer = (cbxSelectKeybindingsMod[u].SelectedItem).ToString();
					// save the keybind to config
					Prerequisites.SaveConfigKeybind(u, new KeybindCombo((cbxSelectKeybindings[u].SelectedItem).ToString(), (cbxSelectKeybindingsMod[u].SelectedItem).ToString(),(uint)u+1,Rotation));
					u++;
				}
				grpBoxBindings.Focus();
				SetRotationKeys();
			}
		}





		


		private void CheckKeybindColor(ComboBox cbx, Label label)
		{
			if (cbx.SelectedIndex != 0)
			{
				label.ForeColor = Color.FromArgb(0, 0, 0);
			}
			if (cbx.SelectedIndex == 0)
			{
				label.ForeColor = Color.FromArgb(175, 175, 175);
			}

		}

		private void btnSearchWowFolder_Click(object sender, EventArgs e)
		{
			using (var fbd = new FolderBrowserDialog())
			{
				fbd.SelectedPath = lastDirDialogDir;
				if (!Directory.Exists(lastDirDialogDir))
				{
					#region default folders
					if (Directory.Exists("C:\\wow"))
					{
						fbd.SelectedPath = "C:\\wow";
					}
					else if (Directory.Exists("C:\\games\\wow"))
					{
						fbd.SelectedPath = "C:\\games\\wow";
					}
					else if (Directory.Exists("D:\\wow"))
					{
						fbd.SelectedPath = "D:\\wow";
					}
					else if (Directory.Exists("D:\\games\\wow"))
					{
						fbd.SelectedPath = "D:\\games\\wow";
					}
					else if (Directory.Exists("D:\\spiele\\wow"))
					{
						fbd.SelectedPath = "D:\\spiele\\wow";
					}
					else if (Directory.Exists("C:\\"))
					{
						fbd.SelectedPath = "C:\\";
					}
					#endregion default folders
				}
				DialogResult result = fbd.ShowDialog();

				if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
				{
					ValidateAndSetWowFolder(fbd.SelectedPath);
				}
			}
		}
		private void txtWowFolder_Leave(object sender, EventArgs e)
		{
			ValidateAndSetWowFolder(txtWowFolder.Text);
		}

		private bool ValidateAndSetWowFolder(string path)
		{
			string fileLocation = path + "\\wow.exe";
			string tmpGlobalBindingFile;
			string tmpFinalBindingFile;
			accountDirectories.Clear();
			accountDirectories.Add(new WowAccCharacter("None",""));			
			lastDirDialogDir = path;		
			if (File.Exists(fileLocation))
			{
				Log.Print("+ Path: " + fileLocation);
				WowHelperObj.WowFolder = path;
				txtWowFolder.Text = path;
				//list all accounts in wow folder
				List<string> tempAccDirList = WowAccCharacter.GetDirectories(WowHelperObj.WowFolder + "\\WTF\\Account\\");
				if (tempAccDirList != null && tempAccDirList.Count > 0)
				{
					foreach (string s in tempAccDirList)
					{
						string[] z = s.Split('\\');
						Log.Print(z[z.Length - 1]);
						tmpGlobalBindingFile = WowHelperObj.WowFolder + "\\WTF\\Account\\" + z[z.Length - 1] + "\\bindings-cache.wtf";
						// the binding file only is valid if it exists and hase more then 1000 bytes
						// then it will add to the WowAccCharacter class
						if (!File.Exists(tmpGlobalBindingFile) || new System.IO.FileInfo(tmpGlobalBindingFile).Length < 1000)
						{
							tmpGlobalBindingFile = "";
						}
						//list all characters in account folder
						List <string> tempSrvDirList = WowAccCharacter.GetDirectories(WowHelperObj.WowFolder + "\\WTF\\Account\\"+ z[z.Length - 1] + "\\");
						if (tempSrvDirList != null && tempSrvDirList.Count > 0)
						{
							foreach (string w in tempSrvDirList)
							{								
								string[] v = w.Split('\\');
								Log.Print(v[v.Length - 1]);
								if (v[v.Length - 1] != "SavedVariables")
								{
									// Wir setzen das locale binding file generell als tmpFinalBindingFile ein, wenn es nicht valide ist, wird tmpFinalBindingFile = tmpGlobalBindingFile
									tmpFinalBindingFile = WowHelperObj.WowFolder + "\\WTF\\Account\\" + z[z.Length - 1] + "\\" + v[v.Length - 1];
									// the binding file only is valid if it exists and hase more then 1000 bytes
									// then it will add to the WowAccCharacter class
									if (!File.Exists(tmpFinalBindingFile) || new System.IO.FileInfo(tmpFinalBindingFile).Length < 1000)
									{
										tmpFinalBindingFile = tmpGlobalBindingFile;
									}
									accountDirectories.Add(new WowAccCharacter(z[z.Length - 1] + " - " + v[v.Length - 1], tmpFinalBindingFile));
									Log.Print("Bindings: " + tmpFinalBindingFile);
									transferAccNames = new BindingList<string>(accountDirectories.Select(C => C.Name).ToList()); // add all "accountname - servername" to a list and put it as bindinglist in transferAccNames
								}
							}
						}
						
					}
				}
				cbxAccountName.DataSource = transferAccNames;
				Prerequisites.SaveConfigWowFolder(path);
				return true;
			}
			else
			{
				cbxAccountName.DataSource = transferAccNames;
				Log.Print("Not a valid WoW folder!");
				Log.Print("No accounts found!");
				if (FormLoaded)
				{
					MessageBox.Show("Not a valid WoW folder!\n\rNo accounts found!");
				}
				WowHelperObj.WowFolder = "";
				txtWowFolder.Text = "";
				return false;
			}

		}
		private bool ValidateAndSetAccountName(string accsrv)
		{
			Log.Print("B: " + accountDirectories.Count.ToString());
			transferAccNames = new BindingList<string>(accountDirectories.Select(C => C.Name).ToList()); // add all "accountname - servername" to a list and put it as bindinglist in transferAccNames
			cbxAccountName.DataSource = transferAccNames;
			Log.Print("B cbx: " + cbxAccountName.Items.Count.ToString());
			string[] acc = System.Text.RegularExpressions.Regex.Split(accsrv, " - ");
			if (acc[0] != "None" && accountDirectories.Select(C => C.Name).ToList().Contains(accsrv) && Directory.Exists(WowHelperObj.WowFolder + "\\WTF\\Account\\" + acc[0] + "\\" + acc[1]))
			{
				cbxAccountName.SelectedItem = accsrv;
				WowHelperObj.AccountName = accsrv;
				Log.Print(WowHelperObj.AccountName + " " + cbxAccountName.SelectedItem + " " + accountDirectories.Where(C => C.Name == accsrv).First().BindingFile);
				if (!cmdAttach.Enabled)
				{
					FillActionButtonDataGrid(dataGridActionButtons, accountDirectories.Where(C => C.Name == accsrv).First().BindingFile);
				}
				return true;
			}
			if (FormLoaded)
			{
				MessageBox.Show("No accounts found or selected!");
			}
			Log.Print("No accounts found or selected!");
			WowHelperObj.AccountName = "";
			return false;
		}

		private void cbxAccountName_SelectedIndexChanged(object sender, EventArgs e)
		{
			ComboBox current = new ComboBox();
			current = (sender as ComboBox);
			
			if (current.SelectedItem != null)
			{
				Prerequisites.SaveConfigAccountName(current.SelectedItem.ToString());
				WowAccCharacter x = accountDirectories.Where(C => C.Name == current.SelectedItem.ToString()).First();
				if (x.BindingFile != "" && !cmdAttach.Enabled)
				{
					FillActionButtonDataGrid(dataGridActionButtons, x.BindingFile);
				}
			}
		}

		public bool ApplicationIsActivated()
		{
			var activatedHandle = GetForegroundWindow();
			if (activatedHandle == IntPtr.Zero)
			{
				return false;       // No window is currently activated
			}

			var procId = WowHelperObj.wowMem.ProcessId;


			int activeProcId;
			GetWindowThreadProcessId(activatedHandle, out activeProcId);

			return activeProcId == procId;
		}

		private void FillActionButtonDataGrid(DataGridView dgv, string bf)
		{
			if (bf != null && bf != "")
			{
				WowHelperObj.ActionButtonAndSpellList = WowHelperObj.CreateActionButtonData(WowHelperObj.ReadBindingsCache(bf), WowHelperObj.GetActionButtonBindings());
				if (WowHelperObj.ActionButtonAndSpellList != null && dgv != null)
				{
					Log.Print("!!!!");
					dgv.DataSource = WowHelperObj.ActionButtonAndSpellList;
					Rotation.Init(WowHelperObj.ActionButtonAndSpellList, timerCheckHook, gkh, WowHelperObj);
					this.dataGridActionButtons.Columns[0].Width = 110;
					this.dataGridActionButtons.Columns[1].Width = 35;
					this.dataGridActionButtons.Columns[2].Width = 50;
					this.dataGridActionButtons.Columns[3].Width = 20;
					this.dataGridActionButtons.Columns[4].Width = 20;
					this.dataGridActionButtons.Columns[5].Width = 60;
					this.dataGridActionButtons.Columns[6].Width = 170;
				}
				Log.Print("Grid: " + WowHelperObj.ActionButtonAndSpellList.ElementAt(1).SpellName);
				Log.Print("dgv: " + dgv.ToString());
				Log.Print("List: " + WowHelperObj.ActionButtonAndSpellList.Count);
				Log.Print("Grid: " + dataGridActionButtons.Rows.Count);
			}
			else
			{
				Log.Print("No account selected / binding-cache.wtf found!", 4);
				MessageBox.Show("No account selected / binding-cache.wtf found!");
			}
			

			
		}

		private void cmdRefreshActionButtonList_Click(object sender, EventArgs e)
		{
			WowAccCharacter x = accountDirectories.Where(C => C.Name == cbxAccountName.SelectedItem.ToString()).First();
			if (x.BindingFile != "" && !cmdAttach.Enabled)
			{
				FillActionButtonDataGrid(dataGridActionButtons, x.BindingFile);
			}
		}

		private void cmdRefreshActionButtonList_MouseEnter(object sender, EventArgs e)
		{
			cmdRefreshActionButtonList.ForeColor = Color.FromArgb(128, 128, 128);
		}
		private void cmdRefreshActionButtonList_MouseLeave(object sender, EventArgs e)
		{
			cmdRefreshActionButtonList.ForeColor = Color.FromArgb(0, 0, 0);
		}
		private void cmdRefreshActionButtonList_MouseDown(object sender, MouseEventArgs e)
		{
			cmdRefreshActionButtonList.ForeColor = Color.FromArgb(0,196, 0);
		}
		private void cmdRefreshActionButtonList_MouseUp(object sender, MouseEventArgs e)
		{
			cmdRefreshActionButtonList.ForeColor = Color.FromArgb(128, 128, 128);
		}

		private void timerCheckHook_Tick(object sender, EventArgs e)
		{
			if (Attached && ApplicationIsActivated() && !gkh.hooked && WowHelperObj.GetTextCursor() == 0)
			{
				gkh.hook();
				Log.Print("Hook on");
			}
			else if ((!Attached || !ApplicationIsActivated() || WowHelperObj.GetTextCursor() == 1) && gkh.hooked)
			{
				gkh.unhook();
				Log.Print("Hook off");
			}
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			int tempX = Math.Max(0, this.Location.X);
			int tempY = Math.Max(0, this.Location.Y);
			Prerequisites.SaveConfigWindowPositionX(tempX.ToString());
			Prerequisites.SaveConfigWindowPositionY(tempY.ToString());
			if (Constants.GetPlayerFaction(WowHelperObj.LocalPlayer.FactionTemplate) == "Alliance")
			{
				Prerequisites.SaveHostileFactionAlliance(WowHelperObj.HostileFaction);
				Log.Print("Save Hostiles for A");
			}
			else if (Constants.GetPlayerFaction(WowHelperObj.LocalPlayer.FactionTemplate) == "Horde")
			{
				Prerequisites.SaveHostileFactionHorde(WowHelperObj.HostileFaction);
				Log.Print("Save Hostiles for H");
			}
		}









		//public List<string> GetAllAccounts()
		//{
		//	DirectoryInfo pathInfo = new DirectoryInfo(Filepath);
		//	foreach (var x in pathInfo.GetFiles("*.cs"))
		//	{
		//		Log.Print("file: " + x);
		//		WowClassFiles.Add(x.ToString());
		//	}
		//	return WowClassFiles;
		//}
	}
}


//Wozu brauchen wir den permaneten refresh von Auras und spells etc? Das können wir doch beim keypress erledigen