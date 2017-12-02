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
using System.Media;

namespace Revlex.RadarOverlay
{
    //public delegate void RadarFormPosition(int x, int y);
    //public delegate void RadarFormSize(int sX, int sY);
    //public delegate void RadarFormBorder(bool b);
    public delegate void RadarSettings(
        int posX,
        int posY,
        int sizeX,
        int sizeY,
        bool border,
        bool displayEnemyPlayer,
        bool soundEnemyPlayer,
        bool displayFriendlyPLayer,
        bool soundFriendlyPlayer,
        bool displayVeins,
        bool soundVeins,
        bool displayHerbs,
        bool soundHerbs,
        string displaySearchObj,
        bool soundSearch
        );
    public partial class RadarForm : Form
    {


        RECT rect;
        private int Xpos = 0;
        private int Ypos = 0;
        private int RadarSize = 0;
        private WowHelpers WowHelperObj;
        private string FilepathIcons = Application.StartupPath + "\\icons";
        private string FilepathSounds = Application.StartupPath + "\\sounds";
        private Bitmap Herb_5, Herb_7, Triangle_4, Triangle_8, Circle_6, Circle_8, Cross_5, Cross_6, Cross_8, Caro_5, Caro_7, Dot_6, Dot_4, playerA;
        //private SoundPlayer SfxEnemy, SfxEnemyTargetMe, SfxVein, SfxHerb, SfxSearch;
        private bool BorderState;
        private bool DisplayVeins;
        private bool SoundVeins;
        private bool DisplayHerbs;
        private bool SoundHerbs;
        private bool DisplayEnemyPlayer;
        private bool SoundEnemyPlayer;
        private bool DisplayFriendlyPlayer;
        private bool SoundFriendlyPlayer;
        private string DisplaySearchObj = "";
        private bool SoundSearch;
        private float PlayerDotRotation = 0f;
        private bool EnemyDetected = false;
        private bool EnemyTargetsMe = false;
        private bool VeinDetected = false;
        private bool HerbDetected = false;
        private bool SearchDetected = false;

        private bool EnemyDetectedBeep = false;
        private bool EnemyTargetsMeBeep = false;
        private bool VeinDetectedBeep = false;
        private bool HerbDetectedBeep = false;
        private bool SearchDetectedBeep = false;
        private double LastVeinDetectBeep = 0;
        private double LastHerbDetectBeep = 0;
        private double LastEnemyDetectBeep = 0;
        private double LastSearchDetectBeep = 0;
        private long BeepIntervall = 3000;


        public const string WINDOW_NAME = "World of Warcraft";
        IntPtr handle = FindWindow(null, WINDOW_NAME);
        Random rnd = new Random();
        private List<RadarEntity> RadarEnemyUnits = new List<RadarEntity>();
        private List<RadarEntity> RadarFriendlyUnits = new List<RadarEntity>();
        private List<RadarEntity> RadarMiningUnits = new List<RadarEntity>();
        private List<RadarEntity> RadarHerbUnits = new List<RadarEntity>();
        private List<RadarEntity> RadarSearchUnits = new List<RadarEntity>();


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
            InitializeCustomComponent();


        }

        public void SetRadarSettingsVars(
            int posX,
            int posY,
            int sizeX,
            int sizeY,
            bool border,
            bool displayEnemyPlayer,
            bool soundEnemyPlayer,
            bool displayFriendlyPLayer,
            bool soundFriendlyPlayer,
            bool displayVeins,
            bool soundVeins,
            bool displayHerbs,
            bool soundHerbs,
            string displaySearchObj,
            bool soundSearch
        )
        {
            panel1.Size = new System.Drawing.Size(sizeX, sizeY);
            panel1.Location = new System.Drawing.Point(posX, posY);
            BorderState = border;
            DisplayVeins = displayVeins;
            SoundVeins = soundVeins;
            DisplayHerbs = displayHerbs;
            SoundHerbs = soundHerbs;
            DisplayEnemyPlayer = displayEnemyPlayer;
            SoundEnemyPlayer = soundEnemyPlayer;
            DisplayFriendlyPlayer = displayFriendlyPLayer;
            SoundFriendlyPlayer = soundFriendlyPlayer;
            DisplaySearchObj = displaySearchObj;
            SoundSearch = soundSearch;
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

            //// load radar sounds
            //try
            //{
            //    SfxEnemy = new SoundPlayer(FilepathSounds + "\\" + "nmy.wav");
            //    SfxEnemyTargetMe = new SoundPlayer(FilepathSounds + "\\" + "nmy_tarme.wav");
            //    SfxVein = new SoundPlayer(FilepathSounds + "\\" + "vein.wav");
            //    SfxHerb = new SoundPlayer(FilepathSounds + "\\" + "herb.wav");
            //    SfxSearch = new SoundPlayer(FilepathSounds + "\\" + "search.wav");
            //}
            //catch (Exception e)
            //{
            //    Log.Print("Error loading sounds: \r\n" + e.Message, 4);
            //}
            // load radar images
            try
            {
                Herb_5 = new Bitmap(FilepathIcons + "\\" + "herb_8bit_5.png");
                Herb_7 = new Bitmap(FilepathIcons + "\\" + "herb_8bit_5.png");
                Triangle_4 = new Bitmap(FilepathIcons + "\\" + "triangle_8bit_4.png");
                Triangle_8 = new Bitmap(FilepathIcons + "\\" + "triangle_8bit_8.png");
                Circle_6 = new Bitmap(FilepathIcons + "\\" + "circle_8bit_6.png");
                Circle_8 = new Bitmap(FilepathIcons + "\\" + "circle_8bit_8.png");
                Cross_5 = new Bitmap(FilepathIcons + "\\" + "cross_8bit_5.png");
                Cross_6 = new Bitmap(FilepathIcons + "\\" + "cross_8bit_6.png");
                Cross_8 = new Bitmap(FilepathIcons + "\\" + "cross_8bit_8.png");
                Caro_5 = new Bitmap(FilepathIcons + "\\" + "caro_8bit_5.png");
                Caro_7 = new Bitmap(FilepathIcons + "\\" + "caro_8bit_7.png");
                Dot_4 = new Bitmap(FilepathIcons + "\\" + "dot_8bit_4.png");
                Dot_6 = new Bitmap(FilepathIcons + "\\" + "dot_8bit_6.png");
                playerA = new Bitmap(FilepathIcons + "\\" + "player_8bit_8.png");
            }
            catch (Exception e)
            {
                Log.Print("Error loading Icons: \r\n" + e.Message, 4);
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
            SolidBrush friendlyBrush = new SolidBrush(Color.FromArgb(255, 0, 128, 0));
            SolidBrush playerBrush = new SolidBrush(Color.FromArgb(255, 255, 255, 255));
            SolidBrush blackBrush = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
            SolidBrush enemyBrush = new SolidBrush(Color.FromArgb(255, 128, 0, 0));


            //Bitmap image = ImageEffect.ColorTint(new Bitmap(@"D:\mbxEye_lock_closed.png"), 0, 0, 222);
            //draw units
            if (DisplayFriendlyPlayer)
            {
                foreach (RadarEntity k in RadarFriendlyUnits)
                {
                    GfxLayer1.DrawImage(ImageEffect.ColorTint(k.IconType, k.IconColor.R, k.IconColor.G, k.IconColor.B), k.X, k.Y, k.Width, k.Height);
                }
            }

            // draw player
            DrawPlayerDot(playerA, (double)(PlayerDotRotation - (Math.PI * 2)) * -1, new Point((panel1.Size.Width / 2) + 2, (panel1.Size.Height / 2) - 3), 10);

            if (DisplaySearchObj != "")
            {
                foreach (RadarEntity k in RadarSearchUnits)
                {
                    GfxLayer1.DrawImage(ImageEffect.ColorTint(k.IconType, k.IconColor.R, k.IconColor.G, k.IconColor.B), k.X, k.Y, k.Width, k.Height);
                }
            }
            if (DisplayEnemyPlayer)
            {
                foreach (RadarEntity k in RadarEnemyUnits)
                {
                    GfxLayer1.DrawImage(ImageEffect.ColorTint(k.IconType, k.IconColor.R, k.IconColor.G, k.IconColor.B), k.X, k.Y, k.Width, k.Height);
                }
            }
            if (DisplayVeins)
            {
                foreach (RadarEntity k in RadarMiningUnits)
                {
                    GfxLayer1.DrawImage(ImageEffect.ColorTint(k.IconType, k.IconColor.R, k.IconColor.G, k.IconColor.B), k.X, k.Y, k.Width, k.Height);
                }
            }
            if (DisplayHerbs)
            {
                foreach (RadarEntity k in RadarHerbUnits)
                {
                    GfxLayer1.DrawImage(ImageEffect.ColorTint(k.IconType, k.IconColor.R, k.IconColor.G, k.IconColor.B), k.X, k.Y, k.Width, k.Height);
                }
            }
            if (DisplaySearchObj != "")
            {
                foreach (RadarEntity k in RadarSearchUnits)
                {
                    GfxLayer1.DrawImage(ImageEffect.ColorTint(k.IconType, k.IconColor.R, k.IconColor.G, k.IconColor.B), k.X, k.Y, k.Width, k.Height);
                }
            }
            // draw player dot border
            DrawPlayerDotBorder(playerA, (double)(PlayerDotRotation - (Math.PI * 2)) * -1, new Point((panel1.Size.Width / 2) + 2, (panel1.Size.Height / 2) - 3), 10);

            if (BorderState)
            {
                DrawBorder();
            }


        }

        private void DrawPlayerDot(Bitmap player, double Rotation, Point center, int size)
        {
            Pen BlackPen = new Pen(Color.Black);
            SolidBrush playerBrush = new SolidBrush(Color.FromArgb(255, 255, 255, 255));
            double tailDegree = Math.PI / 4;
            double lengthNeedle = size;
            double lengthTail = size / 2;
            double lengthWing = size / 2;
            double hypWing = Math.Sqrt(Math.Pow(lengthTail, 2) + Math.Pow(lengthWing, 2));
            Point[] p = new Point[] {
                new Point(center.X + (int)Math.Round(Math.Sin(Rotation) * lengthNeedle,0), center.Y + (int)Math.Round(Math.Cos(Rotation) * lengthNeedle,0)*-1),
                new Point(center.X + (int)Math.Round(Math.Sin(Rotation + Math.PI + tailDegree) * hypWing,0),  center.Y + ((int)Math.Round(Math.Cos(Rotation + Math.PI + tailDegree) * hypWing,0)*-1) ),
                new Point(center.X + (int)Math.Round(Math.Sin(Rotation + Math.PI - tailDegree) * hypWing,0),  center.Y + ((int)Math.Round(Math.Cos(Rotation + Math.PI - tailDegree) * hypWing,0)*-1) )
            };
            GfxLayer1.FillPolygon(playerBrush, p);
        }
        private void DrawPlayerDotBorder(Bitmap player, double Rotation, Point center, int size)
        {
            Pen BlackPen = new Pen(Color.Black);
            SolidBrush playerBrush = new SolidBrush(Color.FromArgb(255, 255, 255, 255));
            double tailDegree = Math.PI / 4;
            double lengthNeedle = size;
            double lengthTail = size / 2;
            double lengthWing = size / 2;
            double hypWing = Math.Sqrt(Math.Pow(lengthTail, 2) + Math.Pow(lengthWing, 2));
            Point[] p = new Point[] {
                new Point(center.X + (int)Math.Round(Math.Sin(Rotation) * lengthNeedle,0), center.Y + (int)Math.Round(Math.Cos(Rotation) * lengthNeedle,0)*-1),
                new Point(center.X + (int)Math.Round(Math.Sin(Rotation + Math.PI + tailDegree) * hypWing,0),  center.Y + ((int)Math.Round(Math.Cos(Rotation + Math.PI + tailDegree) * hypWing,0)*-1) ),
                new Point(center.X + (int)Math.Round(Math.Sin(Rotation + Math.PI - tailDegree) * hypWing,0),  center.Y + ((int)Math.Round(Math.Cos(Rotation + Math.PI - tailDegree) * hypWing,0)*-1) )
            };
            GfxLayer1.DrawPolygon(BlackPen, p);
        }

        private double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
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
            EnemyDetected = false;
            EnemyTargetsMe = false;
            VeinDetected = false;
            HerbDetected = false;
            SearchDetected = false;

            WowVector3d relativeVector2d;
            // get all WowObject from mainthread
            object pulledWowHelperObj = ParentForm.Invoke(new GetWowObjDataDelegate(ParentForm.GetWowObjData));
            WowHelperObj = (WowHelpers)pulledWowHelperObj;
            WowHelperObj.ScanObj();
            SolidBrush enemyBrush = new SolidBrush(Color.FromArgb(255, 128, 0, 0));



            int u;

            //prepare friendly player for drawing
            if (DisplayFriendlyPlayer)
            {
                RadarFriendlyUnits.Clear();
                u = 0;
                foreach (WowObject item in WowHelperObj.GetNearPlayerFriends())
                {
                    relativeVector2d = item.GetRadarPosition((uint)panel1.Size.Width, (uint)panel1.Size.Height, 230, WowHelperObj.LocalPlayer.vector3d.x, WowHelperObj.LocalPlayer.vector3d.y);
                    Color iconColor = Color.FromArgb(64, 190, 64);
                    RadarFriendlyUnits.Add(new RadarEntity((int)relativeVector2d.x-2, (int)relativeVector2d.y-4,6,6, iconColor, Dot_6));
                    u++;
                }
            }

            //prepare player enemies for drawing
            if (DisplayEnemyPlayer)
            {
                RadarEnemyUnits.Clear();
                u = 0;
                foreach (WowObject item in WowHelperObj.GetNearPlayerEnemies())
                {

                    relativeVector2d = item.GetRadarPosition((uint)panel1.Size.Width, (uint)panel1.Size.Height, 230, WowHelperObj.LocalPlayer.vector3d.x, WowHelperObj.LocalPlayer.vector3d.y);
                    float shiftColor2 = 80f + ((((float)WowHelperObj.LocalPlayer.Level - (float)item.Level) * 1) * 5.333f);
                    shiftColor2 = Math.Max(Math.Max(30, shiftColor2), 120);
                    Color iconColor = Color.FromArgb(255, (int)Math.Round(shiftColor2), (int)Math.Round(shiftColor2));
                    if (item.TargetGuid == WowHelperObj.LocalPlayer.Guid)
                    {
                        RadarEnemyUnits.Add(new RadarEntity((int)relativeVector2d.x - 2, (int)relativeVector2d.y - 5, 8, 8, iconColor, Cross_8));
                        EnemyTargetsMe = true;
                    }
                    else
                    {
                        RadarEnemyUnits.Add(new RadarEntity((int)relativeVector2d.x - 2, (int)relativeVector2d.y - 4, 6, 6, iconColor, Dot_6));
                    }

                    EnemyDetected = true;
                    EnemyDetectedBeep = true;
                    u++;
                }
            }

            //prepare mining veins for drawing
            if (DisplayVeins)
            {
                RadarMiningUnits.Clear();
                u = 0;
                foreach (WowObject item in WowHelperObj.GetMiningNodes())
                {
                    relativeVector2d = item.GetRadarPosition((uint)panel1.Size.Width, (uint)panel1.Size.Height, 230, WowHelperObj.LocalPlayer.vector3d.x, WowHelperObj.LocalPlayer.vector3d.y);
                    Color iconColor = Color.FromArgb(255, 196, 0);
                    RadarMiningUnits.Add(new RadarEntity((int)relativeVector2d.x-2, (int)relativeVector2d.y-6, 7, 8, iconColor, Triangle_8));

                    VeinDetected = true;
                    VeinDetectedBeep = true;
                    u++;
                }
            }

            if (DisplayHerbs)
            {
                //prepare herbs for drawing
                RadarHerbUnits.Clear();
                u = 0;
                foreach (WowObject item in WowHelperObj.GetHerbNodes())
                {
                    relativeVector2d = item.GetRadarPosition((uint)panel1.Size.Width, (uint)panel1.Size.Height, 230, WowHelperObj.LocalPlayer.vector3d.x, WowHelperObj.LocalPlayer.vector3d.y);
                    Color iconColor = Color.FromArgb(128, 255, 64);
                    RadarHerbUnits.Add(new RadarEntity((int)relativeVector2d.x-1, (int)relativeVector2d.y-5, 7, 6, iconColor, Herb_7));

                    HerbDetected = true;
                    HerbDetectedBeep = true;
                    u++;
                }
            }

            if (DisplaySearchObj != "")
            {
                List<string> splittedSearchObj = new List<string>();
                splittedSearchObj = DisplaySearchObj.Split(',').ToList();
                //prepare herbs for drawing
                RadarSearchUnits.Clear();
                u = 0;
                foreach (WowObject item in WowHelperObj.GetSearchObj(splittedSearchObj))
                {
                    relativeVector2d = item.GetRadarPosition((uint)panel1.Size.Width, (uint)panel1.Size.Height, 230, WowHelperObj.LocalPlayer.vector3d.x, WowHelperObj.LocalPlayer.vector3d.y);
                    Color iconColor = Color.FromArgb(200, 128, 255);
                    RadarHerbUnits.Add(new RadarEntity((int)relativeVector2d.x-2, (int)relativeVector2d.y-5, 8, 8, iconColor, Circle_8));

                    SearchDetected = true;
                    SearchDetectedBeep = true;
                    u++;
                }
            }

            PlayerDotRotation = WowHelperObj.LocalPlayer.Rotation;
            PlaySounds();
        }

        private void PlaySounds()
        {

            // Beep on vein
            if (SoundVeins && VeinDetected && LastVeinDetectBeep + BeepIntervall < WowHelpers.GetTime())
            {
                Wav.Play(FilepathSounds + "\\" + "vein.wav");
                LastVeinDetectBeep = WowHelpers.GetTime();
            }
            // Beep on herb
            if (SoundHerbs && HerbDetected && LastHerbDetectBeep + BeepIntervall < WowHelpers.GetTime())
            {
                Wav.Play(FilepathSounds + "\\" + "herb.wav");
                LastHerbDetectBeep = WowHelpers.GetTime();
            }

            // Beep on Search
            if (SoundSearch && SearchDetected && LastSearchDetectBeep + BeepIntervall < WowHelpers.GetTime())
            {
                Wav.Play(FilepathSounds + "\\" + "search.wav");
                LastSearchDetectBeep = WowHelpers.GetTime();
                //delay low prio sounds
                LastHerbDetectBeep += 300;
                LastVeinDetectBeep += 300;

            }
            // Beep on Enemy
            if (SoundEnemyPlayer && EnemyDetected && LastEnemyDetectBeep + BeepIntervall < WowHelpers.GetTime())
            {
                if (EnemyTargetsMe)
                {
                    Wav.Play(FilepathSounds + "\\" + "nmy_tarme.wav");
                    LastHerbDetectBeep += 380;
                    LastVeinDetectBeep += 380;
                    LastSearchDetectBeep += 380;
                }
                else
                {
                    Wav.Play(FilepathSounds + "\\" + "nmy.wav");
                    LastHerbDetectBeep += 300;
                    LastVeinDetectBeep += 300;
                    LastSearchDetectBeep += 300;
                }
                LastEnemyDetectBeep = WowHelpers.GetTime();
            }

        }




    }
}
