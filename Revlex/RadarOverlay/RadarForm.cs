using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Drawing.Drawing2D;

namespace Revlex.RadarOverlay
{
    public delegate void RadarFormPosition(int x, int y);
    public delegate void RadarFormSize(int sX, int sY);
    public delegate void RadarFormBorder(bool b);
    public partial class RadarForm : Form
    {




        RECT rect;
        private int Xpos = 0;
        private int Ypos = 0;
        private int RadarSize = 0;
        private WowHelpers WowHelperObj;
        private string Filepath = Application.StartupPath + "\\icons";
        private Bitmap Herb_5, Herb_7, Triangle_4, Triangle_8, Circle_6, Circle_8, Cross_5, Cross_8, Caro_5, Caro_7, Dot_6, Dot_4 , playerA;
        private bool BorderState;

        public const string WINDOW_NAME = "World of Warcraft";
        IntPtr handle = FindWindow(null, WINDOW_NAME);
        Random rnd = new Random();
        private List<RadarEntity> RadarEnemyUnits = new List<RadarEntity>();
        private List<RadarEntity> RadarMiningUnits = new List<RadarEntity>();
        private List<RadarEntity> RadarHerbUnits = new List<RadarEntity>();


        public struct RECT
        {
            public int left, top, right, bottom;
        }

        public GfxPanel panel1 = new GfxPanel();
        public Form1 ParentForm;



        public Graphics GfxLayer1;

        private Pen RedPen = new Pen(Color.Red);
        SolidBrush myBrush1 = new SolidBrush(Color.Green);
        SolidBrush myBrush2 = new SolidBrush(Color.Blue);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hwnd, out RECT IpRect);

        public RadarForm(int _xpos, int _ypos, int _radarSize, Form1 _parentForm)
        {
            Xpos = _xpos;
            Ypos = _ypos;
            RadarSize = _radarSize;
            ParentForm = _parentForm;
            
            InitializeComponent();
            //for (int i = 0; i < 500; i++)
            //{
            //    int x = rnd.Next(0, 200 - 4);
            //    int y = rnd.Next(0, 200 - 4);
            //    RadarUnit[i] = new RadarEntity(x, y);
            //}
            InitializeCustomComponent();
           

        }

        public void ChangePosition(int x, int y)
        {
            panel1.Location = new System.Drawing.Point(x, y);
        }
        public void ChangeSize(int sX, int sY)
        {
            panel1.Size = new System.Drawing.Size(sX, sY);
        }

        public void SwitchBorderState(bool b)
        {
            BorderState = b;
        }


        public void DrawBorder()
        {
            GfxLayer1.DrawLine(RedPen, new Point(0, 0), new Point(panel1.Size.Width - 1, 0));
            GfxLayer1.DrawLine(RedPen, new Point(1, 1), new Point(panel1.Size.Width - 2, 1));
            GfxLayer1.DrawLine(RedPen, new Point(panel1.Size.Width - 1, 0), new Point(panel1.Size.Width - 1, panel1.Size.Height - 1));
            GfxLayer1.DrawLine(RedPen, new Point(panel1.Size.Width - 2, 1), new Point(panel1.Size.Width - 2, panel1.Size.Height - 2));
            GfxLayer1.DrawLine(RedPen, new Point(panel1.Size.Width - 1, panel1.Size.Height - 1), new Point(0, panel1.Size.Height - 1));
            GfxLayer1.DrawLine(RedPen, new Point(panel1.Size.Width - 2, panel1.Size.Height - 2), new Point(1, panel1.Size.Height - 2));
            GfxLayer1.DrawLine(RedPen, new Point(0, panel1.Size.Height - 1), new Point(0, 0));
            GfxLayer1.DrawLine(RedPen, new Point(1, panel1.Size.Height - 2), new Point(1, 1));
        }

        private void InitializeCustomComponent()
        {
            Console.WriteLine("init2");

            // load radar images
            try
            {
                Herb_5 = new Bitmap(Filepath + "\\" + "herb_8bit_5.png");
                Herb_7 = new Bitmap(Filepath + "\\" + "herb_8bit_5.png");
                Triangle_4 = new Bitmap(Filepath + "\\" + "triangle_8bit_4.png");
                Triangle_8 = new Bitmap(Filepath + "\\" + "triangle_8bit_8.png");
                Circle_6 = new Bitmap(Filepath + "\\" + "circle_8bit_6.png");
                Circle_8 = new Bitmap(Filepath + "\\" + "circle_8bit_8.png");
                Cross_5 = new Bitmap(Filepath + "\\" + "cross_8bit_5.png");
                Cross_8 = new Bitmap(Filepath + "\\" + "cross_8bit_8.png");
                Caro_5 = new Bitmap(Filepath + "\\" + "caro_8bit_5.png");
                Caro_7 = new Bitmap(Filepath + "\\" + "caro_8bit_7.png");
                Dot_4 = new Bitmap(Filepath + "\\" + "dot_8bit_4.png");
                Dot_6 = new Bitmap(Filepath + "\\" + "dot_8bit_6.png");
                playerA = new Bitmap(Filepath + "\\" + "player_8bit_8.png");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error loading icons: " + e.ToString());
            }
            // 
            // panel1
            // 
            panel1.Location = new System.Drawing.Point(Xpos, Ypos);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(RadarSize, RadarSize);
            panel1.TabIndex = 0;
            panel1.BackColor = Color.Transparent;
            Controls.Add(panel1);
            panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.MyPaintEventHandler);


            panel1.BringToFront();


        }

        private void MyPaintEventHandler(object sender, System.Windows.Forms.PaintEventArgs args)
        {
            GfxLayer1 = args.Graphics;
            GfxLayer1.SmoothingMode = SmoothingMode.None;
            GfxLayer1.InterpolationMode = InterpolationMode.NearestNeighbor;
            GfxLayer1.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            SolidBrush friendlyBrush = new SolidBrush(Color.FromArgb(255,0,128,0));
            SolidBrush playerBrush = new SolidBrush(Color.FromArgb(255, 255, 255, 255));
            SolidBrush blackBrush = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
            SolidBrush enemyBrush = new SolidBrush(Color.FromArgb(255, 128, 0, 0));


            //Bitmap image = ImageEffect.ColorTint(new Bitmap(@"D:\mbxEye_lock_closed.png"), 0, 0, 222);
            //draw units
            foreach (RadarEntity k in RadarEnemyUnits)
            {

                GfxLayer1.DrawImage(ImageEffect.ColorTint(k.IconType,k.IconColor.R, k.IconColor.G, k.IconColor.B), k.X-2, k.Y-4, 6, 6);
            }
            foreach (RadarEntity k in RadarMiningUnits)
            {

                GfxLayer1.DrawImage(ImageEffect.ColorTint(k.IconType, k.IconColor.R, k.IconColor.G, k.IconColor.B), k.X-2, k.Y-6, 7, 8);
            }
            foreach (RadarEntity k in RadarHerbUnits)
            {

                GfxLayer1.DrawImage(ImageEffect.ColorTint(k.IconType, k.IconColor.R, k.IconColor.G, k.IconColor.B), k.X-1, k.Y-5, 7, 6);
            }
            // draw player
            //GfxLayer1.FillEllipse(playerBrush, (panel1.Size.Width / 2)-1, (panel1.Size.Height / 2)-5, 5, 5);
            //GfxLayer1.DrawImage(playerA, (panel1.Size.Width / 2) - 2, (panel1.Size.Height / 2) - 6, 8, 10);
            int panelCenterX = panel1.Width / 2;
            int panelCenterY = panel1.Height / 2;
            Point[] p = new Point[] { new Point(panelCenterX - 5, panelCenterY - 5), new Point(panelCenterX + 5, panelCenterY - 0), new Point(panelCenterX - 0, panelCenterY + 5) };
            GfxLayer1.FillPolygon(playerBrush, p);
            if (BorderState)
            {
                DrawBorder();
            }

            


        }



        private void RadarForm_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.Wheat;
            this.TransparencyKey = Color.Wheat;
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;

            int initialStyle = GetWindowLong(this.Handle, -20);
            SetWindowLong(this.Handle, -20, initialStyle | 0x80000 | 0x20);

            GetWindowRect(handle, out rect);
            this.Size = new Size(rect.right - rect.left, rect.bottom - rect.top);
            this.Top = rect.top;
            this.Left = rect.left;
            this.DoubleBuffered = true;

            Console.WriteLine("load");

            timerDraw.Interval = 30;
            timerDraw.Start();
            timerCalcRadarUnits.Interval = 30;
            timerCalcRadarUnits.Start();

        }


        private void timerDraw_Tick(object sender, EventArgs e)
        {
            panel1.Invalidate();
            panel1.Refresh();

        }

        private void timerCalcRadarUnits_Tick(object sender, EventArgs e)
        {
            WowVector3d relativeVector2d;

            // get all WowObject from mainthread
            object pulledWowHelperObj = ParentForm.Invoke(new GetWowObjDataDelegate(ParentForm.GetWowObjData));
            WowHelperObj = (WowHelpers)pulledWowHelperObj;
            WowHelperObj.ScanObj();
            SolidBrush enemyBrush = new SolidBrush(Color.FromArgb(255, 128, 0, 0));


            //foreach (RadarEntity k in RadarUnit)
            //{
            //    int x = rnd.Next(0, 3) - 1;
            //    int y = rnd.Next(0, 3) - 1;
            //    k.X = k.X + x;
            //    k.Y = k.Y + y;
            //}

            //prepare player enemies for drawing
            RadarEnemyUnits.Clear();
            int u = 0;
            foreach (WowObject item in WowHelperObj.GetNearPlayerEnemies())
            {
                relativeVector2d = item.GetRadarPosition((uint)panel1.Size.Width, (uint)panel1.Size.Height, 230, WowHelperObj.LocalPlayer.vector3d.x, WowHelperObj.LocalPlayer.vector3d.y);
                int enemyShiftColor = 100 + (10 * ((int)WowHelperObj.LocalPlayer.Level - (int)item.Level));
                enemyShiftColor = Math.Max(Math.Min(160, enemyShiftColor), 50);
                float shiftColor2 = 80f + ((((float)WowHelperObj.LocalPlayer.Level - (float)item.Level) * 1) * 5.333f);
                shiftColor2 = Math.Max(Math.Max(30, shiftColor2),120);
                Color iconColor = Color.FromArgb(255, (int)Math.Round(shiftColor2), (int)Math.Round(shiftColor2));
                RadarEnemyUnits.Add(new RadarEntity((int)relativeVector2d.x, (int)relativeVector2d.y, iconColor, Dot_6));
                u++;
            }

            //prepare mining veins for drawing
            RadarMiningUnits.Clear();
            u = 0;
            foreach (WowObject item in WowHelperObj.GetMiningNodes())
            {
                relativeVector2d = item.GetRadarPosition((uint)panel1.Size.Width, (uint)panel1.Size.Height, 230, WowHelperObj.LocalPlayer.vector3d.x, WowHelperObj.LocalPlayer.vector3d.y);
                Color iconColor = Color.FromArgb(255, 196, 0);
                RadarMiningUnits.Add(new RadarEntity((int)relativeVector2d.x, (int)relativeVector2d.y, iconColor, Triangle_8));
                u++;
            }

            //prepare herbs for drawing
            RadarHerbUnits.Clear();
            u = 0;
            foreach (WowObject item in WowHelperObj.GetHerbNodes())
            {
                relativeVector2d = item.GetRadarPosition((uint)panel1.Size.Width, (uint)panel1.Size.Height, 230, WowHelperObj.LocalPlayer.vector3d.x, WowHelperObj.LocalPlayer.vector3d.y);
                Color iconColor = Color.FromArgb(128, 255, 64);
                RadarHerbUnits.Add(new RadarEntity((int)relativeVector2d.x, (int)relativeVector2d.y, iconColor, Herb_7));
                u++;
            }

        }




    }
}
