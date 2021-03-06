using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using ProjektSitzplan.Design;
using ProjektSitzplan.Structures;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace ProjektSitzplan
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Sitzplan Anzeigen / Schüler auf Sitzplätze setzen. 1. Time
        private bool isLoading = true;
        private bool zeigtSitzplanAn = false;
        private bool ZeigtSitzplanAn
        {
            get => zeigtSitzplanAn;
            set
            {
                zeigtSitzplanAn = value;

                if (zeigtSitzplanAn)
                {
                    SKlasseTxbk.Text = $"{AusgewählteKlasse.Name} - {ÜAusgewählterSitzplan.Name}";

                    SetzeSchüler(true, false, false);
                }
                else
                {
                    ClearSitzplätze();
                }
            }
        }

        private void ClearSitzplätze()
        {
            foreach (TextBlock[] labelArrys in Tischblöcke)
                foreach (TextBlock label in labelArrys)
                    label.Text = "";
        }

        private List<TextBlock[]> Tischblöcke;
        private Border[] Tische;
        private TextBlock[] TischTxbks;

        private void SetzteTischblöcke()
        {
            Tische = new Border[] { Tisch1Brd, Tisch2Brd, Tisch3Brd, Tisch4Brd, Tisch5Brd, Tisch6Brd };
            TischTxbks = new TextBlock[] { Tisch1TxBk, Tisch2TxBk, Tisch3TxBk, Tisch4TxBk, Tisch5TxBk, Tisch6TxBk };

            Tischblöcke = new List<TextBlock[]>
            {
                new TextBlock[] { S_T1_P1, S_T1_P2, S_T1_P3, S_T1_P4, S_T1_P5, S_T1_P6, S_T1_P7, S_T1_P8 },
                new TextBlock[] { S_T2_P1, S_T2_P2, S_T2_P3, S_T2_P4, S_T2_P5, S_T2_P6, S_T2_P7, S_T2_P8 },
                new TextBlock[] { S_T3_P1, S_T3_P2, S_T3_P3, S_T3_P4, S_T3_P5, S_T3_P6, S_T3_P7, S_T3_P8 },
                new TextBlock[] { S_T4_P1, S_T4_P2, S_T4_P3, S_T4_P4, S_T4_P5, S_T4_P6, S_T4_P7, S_T4_P8 },
                new TextBlock[] { S_T5_P1, S_T5_P2, S_T5_P3, S_T5_P4, S_T5_P5, S_T5_P6, S_T5_P7, S_T5_P8 },
                new TextBlock[] { S_T6_P1, S_T6_P2, S_T6_P3, S_T6_P4, S_T6_P5, S_T6_P6, S_T6_P7, S_T6_P8 }
            };
        }

        private void SetzeTische(int verfügbareTische)
        {
            for (int i = 0; i < verfügbareTische; i++)
            {
                Tische[i].Visibility = Visibility.Visible;
            }
        }

        private void SetzeSchüler(bool beruf, bool betrieb, bool geschlecht)
        {
            foreach (TextBlock[] tisch in Tischblöcke)
            {
                foreach (TextBlock platz in tisch)
                {
                    platz.IsEnabled = true;
                    platz.Background = Brushes.Transparent;
                }
            }

            SetzeTische(ÜAusgewählterSitzplan.TischAnzahl);

            SBerufCBx.IsChecked = beruf;
            SBetriebCBx.IsChecked = betrieb;
            SGeschlechtCBx.IsChecked = geschlecht;

            for (int tischIndex = 0; tischIndex < ÜAusgewählterSitzplan.Tische.Length; tischIndex++)
            {
                TischBlock tisch = ÜAusgewählterSitzplan.Tische[tischIndex];

                TischTxbks[tischIndex].Text = $"Tisch-{tischIndex + 1} | Plätze: {tisch.MaxSchueler}";

                TextBlock[] tischLabel = Tischblöcke[tischIndex];

                for (int sitzplatzIndex = 0; sitzplatzIndex < 8; sitzplatzIndex++)
                {
                    if (sitzplatzIndex >= tisch.MaxSchueler)
                    {
                        tischLabel[sitzplatzIndex].IsEnabled = false;
                        tischLabel[sitzplatzIndex].Background = PSColors.ContentControlBorder;
                    }
                    else
                    {
                        Schüler schüler;
                        tisch.Sitzplätze.TryGetValue(sitzplatzIndex, out schüler);

                        string name = "";

                        if (schüler != null)
                        {
                            name = $"{schüler.Vorname} {schüler.Nachname}";

                            name += beruf ? $"\n{schüler.BerufString}" : "";
                            name += betrieb ? $"\n{schüler.Betrieb}" : "";
                            name += geschlecht ? $"\n{schüler.Geschlecht}" : "";

                        }

                        tischLabel[sitzplatzIndex].Text = name;
                    }
                }

            }
        }
        #endregion

        private EWindowContent windowContent = EWindowContent.Leer;
        private EWindowContent WindowContent
        {
            get
            {
                return windowContent;
            }
            set
            {
                switch (value)
                {
                    case EWindowContent.Leer:
                        {
                            KlasseHinzufügenGrd.Visibility = Visibility.Hidden;
                            KlasseÜbersichtGrd.Visibility = Visibility.Hidden;

                            LKeineKlasseAusgewähltStkPnl.Visibility = Visibility.Visible;
                            break;
                        }
                    case EWindowContent.KlasseErstellen:
                        {
                            KlasseÜbersichtGrd.Visibility = Visibility.Hidden;
                            KlasseHinzufügenGrd.Visibility = Visibility.Visible;

                            LKeineKlasseAusgewähltStkPnl.Visibility = Visibility.Hidden;

                            break;
                        }
                    case EWindowContent.KlasseÜbersicht:
                        {
                            KlasseHinzufügenGrd.Visibility = Visibility.Hidden;
                            KlasseÜbersichtGrd.Visibility = Visibility.Visible;

                            LKeineKlasseAusgewähltStkPnl.Visibility = Visibility.Hidden;

                            ZeigeKlasseAn();

                            break;
                        }
                }

                windowContent = value;
            }
        }

        private Sitzplan üAusgewählterSitzplan;
        public Sitzplan ÜAusgewählterSitzplan
        {
            get
            {
                return üAusgewählterSitzplan;
            }
            set
            {
                Set(ref üAusgewählterSitzplan, value);
            }
        }

        private SchulKlasse ausgewählteKlasse;
        SchulKlasse AusgewählteKlasse
        {
            get
            {
                return ausgewählteKlasse;
            }
            set
            {
                Set(ref ausgewählteKlasse, value);
            }
        }

        private Schüler üAusgewählterSchüler;
        public Schüler ÜAusgewählterSchüler
        {
            get
            {
                return üAusgewählterSchüler;
            }
            set
            {
                Set(ref üAusgewählterSchüler, value);
            }
        }

        private Schüler keAusgewählterSchüler;
        public Schüler KEAusgewählterSchüler
        {
            get
            {
                return keAusgewählterSchüler;
            }
            set
            {
                Set(ref keAusgewählterSchüler, value);
            }
        }

        private enum EWindowContent
        {
            Leer, //Default
            KlasseErstellen,
            KlasseÜbersicht
        }

        #region Commands
        public static RoutedCommand CommandCreate = new RoutedCommand();
        public static RoutedCommand CommandImport = new RoutedCommand();
        public static RoutedCommand CommandExport = new RoutedCommand();
        public static RoutedCommand CommandSave = new RoutedCommand();
        public static RoutedCommand CommandRefresh = new RoutedCommand();
        public static RoutedCommand CommandUndo = new RoutedCommand();
        public static RoutedCommand CommandRedo = new RoutedCommand();
        public static RoutedCommand CommandFullscreen = new RoutedCommand();

        private void InitCommands()
        {
            CommandCreate.InputGestures.Add(new KeyGesture(Key.N, ModifierKeys.Control));
            CommandImport.InputGestures.Add(new KeyGesture(Key.I, ModifierKeys.Control));
            CommandExport.InputGestures.Add(new KeyGesture(Key.E, ModifierKeys.Control));
            CommandSave.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
            CommandRefresh.InputGestures.Add(new KeyGesture(Key.F5));
            CommandUndo.InputGestures.Add(new KeyGesture(Key.Z, ModifierKeys.Control));
            CommandRedo.InputGestures.Add(new KeyGesture(Key.Z, ModifierKeys.Control | ModifierKeys.Shift));
            CommandRedo.InputGestures.Add(new KeyGesture(Key.Y, ModifierKeys.Control));
            CommandFullscreen.InputGestures.Add(new KeyGesture(Key.F11));



            CommandBindings.Add(new CommandBinding(CommandCreate, MenuKlasseErstellenBtn_Click));
            CommandBindings.Add(new CommandBinding(CommandImport, MenuKlasseImportierenBtn_Click));
            CommandBindings.Add(new CommandBinding(CommandExport, MenuKlasseExportierenBtn_Click));
            CommandBindings.Add(new CommandBinding(CommandSave, MenuKlasseSpeichernBtn_Click));
            CommandBindings.Add(new CommandBinding(CommandRefresh, KlassenAktualisieren));
            CommandBindings.Add(new CommandBinding(CommandFullscreen, DoRestoreStuff));

            #if DEBUG
            // Shortcut for generating test classes
            CommandTest.InputGestures.Add(new KeyGesture(Key.F2));
            CommandBindings.Add(new CommandBinding(CommandTest, TestingEvnt));
            #endif
        }
        #endregion


        #if DEBUG
        public static RoutedCommand CommandTest = new RoutedCommand();

        private void TestingEvnt(object sender = null, RoutedEventArgs e = null)
        {
            Testing.Test();
            KlassenAktualisieren(false);
        }
        #endif


        private Label[] ContentLabels { get; set; }
        private PackIconSet[] ContentPackIconsSets { get; set; }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }
            field = value;
            NotifyPropertyChanged(propertyName);
            return true;
        }
        #endregion  

        public MainWindow()
        {
            InitializeComponent();

            InitCommands();
            
            StateChanged += (s, e) =>
            {
                if (WindowState.Equals(WindowState.Normal))
                {
                    WindowRestoreButton.Content = Utility.GetImage("Restore1B9BBBE");
                }
                else if (WindowState.Equals(WindowState.Maximized))
                {
                    WindowRestoreButton.Content = Utility.GetImage("Restore2B9BBBE");
                }
            };
            SourceInitialized += (s, e) =>
            {
                IntPtr handle = new WindowInteropHelper(this).Handle;
                HwndSource.FromHwnd(handle).AddHook(new HwndSourceHook(WindowProc));
            };

            Directory.CreateDirectory("SchulKlassen");
            DataHandler.LadeSchulKlassen();
        }

        #region WindowProc Handling
        private static int myMinHeigt = 550;
        private static int myMinWidth = 1100;

        private IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case 0x0024:
                    WmGetMinMaxInfo(hwnd, lParam);
                    handled = true;
                    break;
            }
            return (IntPtr)0;
        }

        private void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
            MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));
            int MONITOR_DEFAULTTONEAREST = 0x00000002;
            IntPtr monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);
            if (monitor != IntPtr.Zero)
            {
                MONITORINFO monitorInfo = new MONITORINFO();
                GetMonitorInfo(monitor, monitorInfo);
                RECT rcWorkArea = monitorInfo.rcWork;
                RECT rcMonitorArea = monitorInfo.rcMonitor;
                mmi.ptMaxPosition.x = Math.Abs(rcWorkArea.left - rcMonitorArea.left);
                mmi.ptMaxPosition.y = Math.Abs(rcWorkArea.top - rcMonitorArea.top);
                mmi.ptMaxSize.x = Math.Abs(rcWorkArea.right - rcWorkArea.left);
                mmi.ptMaxSize.y = Math.Abs(rcWorkArea.bottom - rcWorkArea.top);
                mmi.ptMinTrackSize.x = myMinWidth;
                mmi.ptMinTrackSize.y = myMinHeigt;
            }
            Marshal.StructureToPtr(mmi, lParam, true);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            /// <summary>x coordinate of point.</summary>
            public int x;
            /// <summary>y coordinate of point.</summary>
            public int y;
            /// <summary>Construct a point of coordinates (x,y).</summary>
            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MONITORINFO
        {
            public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));
            public RECT rcMonitor = new RECT();
            public RECT rcWork = new RECT();
            public int dwFlags = 0;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
            public static readonly RECT Empty = new RECT();
            public int Width { get { return Math.Abs(right - left); } }
            public int Height { get { return bottom - top; } }
            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }
            public RECT(RECT rcSrc)
            {
                left = rcSrc.left;
                top = rcSrc.top;
                right = rcSrc.right;
                bottom = rcSrc.bottom;
            }
            public bool IsEmpty { get { return left >= right || top >= bottom; } }
            public override string ToString()
            {
                if (this == Empty) { return "RECT {Empty}"; }
                return "RECT { left : " + left + " / top : " + top + " / right : " + right + " / bottom : " + bottom + " }";
            }
            public override bool Equals(object obj)
            {
                if (!(obj is Rect)) { return false; }
                return (this == (RECT)obj);
            }
            /// <summary>Return the HashCode for this struct (not garanteed to be unique)</summary>
            public override int GetHashCode() => left.GetHashCode() + top.GetHashCode() + right.GetHashCode() + bottom.GetHashCode();
            /// <summary> Determine if 2 RECT are equal (deep compare)</summary>
            public static bool operator ==(RECT rect1, RECT rect2) { return (rect1.left == rect2.left && rect1.top == rect2.top && rect1.right == rect2.right && rect1.bottom == rect2.bottom); }
            /// <summary> Determine if 2 RECT are different(deep compare)</summary>
            public static bool operator !=(RECT rect1, RECT rect2) { return !(rect1 == rect2); }
        }

        [DllImport("user32")]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

        [DllImport("User32")]
        internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);
        #endregion

        #region Allgemeine Events
        #region SitzplanAnzeigenCbxses
        private void SAnzeigenCBx_Checked(object sender, RoutedEventArgs e)
        {
            if (isLoading) return;

            SetzeSchüler(SBerufCBx.IsChecked.Value, SBetriebCBx.IsChecked.Value, SGeschlechtCBx.IsChecked.Value);
        }
        #endregion

        #region SitzplanPlätzeTextblöcke
        private bool istAufPlatz = false;
        private bool isDragging = false;
        private TextBlock ausgewählterPlatz = null;
        private Platz? ursprungsPlatz = null;
        private Platz? zielPlatz = null;

        private struct Platz
        {
            public Platz(int tischIndex, int platzIndex)
            {
                TischIndex = tischIndex;
                PlatzIndex = platzIndex;
            }

            public int TischIndex;
            public int PlatzIndex;
        }

        private TextBlock GetTB(object sender)
        {
            return sender as TextBlock;
        }

        private void ResetPlatz()
        {
            if (ausgewählterPlatz is null) return;

            ausgewählterPlatz.Background = Brushes.Transparent;

            EntsperreFenster();

            ursprungsPlatz = null;
            zielPlatz = null;

            isDragging = false;
        }
        private int[] GetIndexes(object sender)
        {
            string[] coordinates = (sender as TextBlock).Uid.Split('_');

            return new int[] { int.Parse(coordinates[0]), int.Parse(coordinates[1]) };
        }

        private void SperreFenster()
        {
            MainMenu.IsEnabled = false;
            MenuKlassenDtGrd.IsEnabled = false;

            MainMenuLbl.IsEnabled = false;
            MMenuKlasseErstellenBtn.IsEnabled = false;
            MMenuKlasseErstellenLbl.IsEnabled = false;
            MMenuKlasseLöschenBtn.IsEnabled = false;
            MMenuKlasseLöschenLbl.IsEnabled = false;

            SZurückBtn.IsEnabled = false;
            SExportBtn.IsEnabled = false;
            SBerufCBx.IsEnabled = false;
            SBetriebCBx.IsEnabled = false;
            SGeschlechtCBx.IsEnabled = false;
            ÜSitzplanVersteckenGrd.IsEnabled = false;
            SNeuGenerierenBtn.IsEnabled = false;
        }

        private void EntsperreFenster()
        {
            MainMenu.IsEnabled = true;
            MenuKlassenDtGrd.IsEnabled = true;

            MainMenuLbl.IsEnabled = true;
            MMenuKlasseErstellenBtn.IsEnabled = true;
            MMenuKlasseErstellenLbl.IsEnabled = true;
            MMenuKlasseLöschenBtn.IsEnabled = true;
            MMenuKlasseLöschenLbl.IsEnabled = true;

            SZurückBtn.IsEnabled = true;
            SExportBtn.IsEnabled = true;
            SBerufCBx.IsEnabled = true;
            SBetriebCBx.IsEnabled = true;
            SGeschlechtCBx.IsEnabled = true;
            ÜSitzplanVersteckenGrd.IsEnabled = true;
            SNeuGenerierenBtn.IsEnabled = true;
        }

        private bool PlatzVerfügbar(TextBlock platz)
        {
            int tischIndex = int.Parse(platz.Uid.Split('_')[0]);
            int platzIndex = int.Parse(platz.Uid.Split('_')[1]);

            if (ÜAusgewählterSitzplan.Tische[tischIndex].MaxSchueler <= platzIndex)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SitzplanPlatzTextblock_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (isDragging) return;

            SperreFenster();
            isDragging = true;

            Cursor = Cursors.Cross;

            ausgewählterPlatz = GetTB(sender);
            ausgewählterPlatz.Background = PSColors.ContentButtonPreviewBackground;

            int[] indexes = GetIndexes(sender);
            ursprungsPlatz = new Platz(indexes[0], indexes[1]);
        }

        private void SitzplanPlatzTextblock_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            ResetPlatz();

            if (!isDragging)
            {
                GetTB(sender).Background = Brushes.Transparent;
            }

            isDragging = false;
            Cursor = Cursors.Arrow;
        }

        private void SitzplanPlatzTextblock_MouseEnter(object sender, MouseEventArgs e)
        {
            istAufPlatz = true;

            if (isDragging)
            {
                if (!GetTB(sender).Equals(ausgewählterPlatz))
                {
                    GetTB(sender).Background = PSColors.ContentBackground;

                    int[] indexes = GetIndexes(sender);
                    zielPlatz = new Platz(indexes[0], indexes[1]);
                }
                else
                {
                    zielPlatz = ursprungsPlatz;
                }

                ÜAusgewählterSitzplan.SchülerTauschen(ursprungsPlatz.Value.TischIndex,
                    ursprungsPlatz.Value.PlatzIndex,
                    zielPlatz.Value.TischIndex,
                    zielPlatz.Value.PlatzIndex);

                SetzeSchüler(SBerufCBx.IsChecked.Value, SBetriebCBx.IsChecked.Value, SGeschlechtCBx.IsChecked.Value);
            }
        }

        private void SitzplanPlatzTextblock_MouseLeave(object sender, MouseEventArgs e)
        {
            istAufPlatz = false;

            if (isDragging)
            {
                if (!GetTB(sender).Equals(ausgewählterPlatz))
                {
                    GetTB(sender).Background = Brushes.Transparent;
                }

                if (zielPlatz != null)
                {
                    ÜAusgewählterSitzplan.SchülerTauschen(zielPlatz.Value.TischIndex,
                        zielPlatz.Value.PlatzIndex,
                        ursprungsPlatz.Value.TischIndex,
                        ursprungsPlatz.Value.PlatzIndex);
                }
            }
        }

        private void MyMainWindow_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!istAufPlatz)
            {
                if (zielPlatz != null)
                {
                    ÜAusgewählterSitzplan.SchülerTauschen(zielPlatz.Value.TischIndex,
                        zielPlatz.Value.PlatzIndex,
                        ursprungsPlatz.Value.TischIndex,
                        ursprungsPlatz.Value.PlatzIndex);
                }

                ResetPlatz();
            }

            Cursor = Cursors.Arrow;
        }

        private void MyMainWindow_MouseLeave(object sender, MouseEventArgs e)
        {
            ResetPlatz();
        }
        #endregion

        #region TopBarButtons
        private void DoRestoreStuff(object sender = null, RoutedEventArgs e = null)
        {
            if (WindowState.Equals(WindowState.Normal))
            {
                WindowState = WindowState.Maximized;
                WindowRestoreButton.ToolTip = new ToolTip()
                {
                    Content = "Verkleinern",
                    Foreground = PSColors.ToolTipForeground,
                    Background = PSColors.ToolTipBackground,
                    FontFamily = new FontFamily("Segoe UI Semibold"),
                    FontSize = 12
                };
            }
            else
            {
                WindowState = WindowState.Normal;
                WindowRestoreButton.ToolTip = new ToolTip()
                {
                    Content = "Maximieren",
                    Foreground = PSColors.ToolTipForeground,
                    Background = PSColors.ToolTipBackground,
                    FontFamily = new FontFamily("Segoe UI Semibold"),
                    FontSize = 12
                };
            }
        }

        private void TopBarButton_Click(object sender, RoutedEventArgs e)
        {
            Button sBtn = Utility.GetButton(sender);

            switch (sBtn.Uid)
            {
                case "0": //Minimize
                    {
                        WindowState = WindowState.Minimized;
                        return;
                    }
                case "1": //Restore
                    {
                        DoRestoreStuff();
                        return;
                    }
                case "2": //Close
                    {
                        Close();
                        return;
                    }
            }

            throw new NotImplementedException();
        }

        private void TopBarButton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Button sBtn = Utility.GetButton(sender);

            sBtn.Background = PSColors.TopBarHoverBackground;
            sBtn.Content = Utility.GetImage($"{Utility.GetTopBarImagePrefix(sBtn, this)}DCDDDE");
        }

        private void TopBarButton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Button sBtn = Utility.GetButton(sender);

            sBtn.Background = Brushes.Transparent;
            sBtn.Content = Utility.GetImage($"{Utility.GetTopBarImagePrefix(sBtn, this)}B9BBBE");
        }

        private void TopBarButton_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Button sBtn = Utility.GetButton(sender);

            sBtn.Background = PSColors.TopBarPreviewBackground;
            sBtn.Content = Utility.GetImage($"{Utility.GetTopBarImagePrefix(sBtn, this)}FFFFFF");
        }

        private void TopBarButton_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Button sBtn = Utility.GetButton(sender);

            sBtn.Background = PSColors.TopBarHoverBackground;
            sBtn.Content = Utility.GetImage($"{Utility.GetTopBarImagePrefix(sBtn, this)}DCDDDE");
        }
        #endregion

        #region MMenuButtons
        private void MMenuBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            Utility.GetButton(sender).Background = PSColors.SubMenuButtonHover;
        }

        private void MMenuBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            Utility.GetButton(sender).Background = PSColors.MenuBackground;
        }
        #endregion

        #region ContentButtons
        private void ContentButton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Button sBtn = Utility.GetButton(sender);

            sBtn.Background = PSColors.ContentButtonHoverBackground;
            ContentLabels[Utility.GetUid(sBtn)]?.SetForeground(PSColors.ContentButtonHoverForeground);
            ContentPackIconsSets[Utility.GetUid(sBtn)]?.HandleColor(PackIconSet.EEventType.Enter);
        }

        private void ContentButton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Button sBtn = Utility.GetButton(sender);

            sBtn.Background = PSColors.ContentButtonBackground;
            ContentLabels[Utility.GetUid(sBtn)]?.SetForeground(PSColors.ContentButtonForeground);
            ContentPackIconsSets[Utility.GetUid(sBtn)]?.HandleColor(PackIconSet.EEventType.Leave);
        }

        private void ContentButton_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Button sBtn = Utility.GetButton(sender);

            sBtn.Background = PSColors.ContentButtonPreviewBackground;
            ContentLabels[Utility.GetUid(sBtn)]?.SetForeground(PSColors.ContentButtonPreviewForeground);
            ContentPackIconsSets[Utility.GetUid(sBtn)]?.HandleColor(PackIconSet.EEventType.PreviewDown);
        }

        private void ContentButton_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Button sBtn = Utility.GetButton(sender);

            sBtn.Background = PSColors.ContentButtonHoverBackground;
            ContentLabels[Utility.GetUid(sBtn)]?.SetForeground(PSColors.ContentButtonHoverForeground);
            ContentPackIconsSets[Utility.GetUid(sBtn)]?.HandleColor(PackIconSet.EEventType.PreviewUp);
        }
        #endregion

        #region ContentTextBox
        private void ContentTextBox_GotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            (sender as TextBox).Foreground = PSColors.ContentTextBoxSelectedForeground;
        }

        private void ContentTextBox_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            (sender as TextBox).Foreground = PSColors.ContentForeground;
        }
        #endregion

        #region ContentComboBox
        private void ContentComboBox_GotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            (sender as ComboBox).Foreground = PSColors.ContentTextBoxSelectedForeground;
        }

        private void ContentComboBox_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            (sender as ComboBox).Foreground = PSColors.ContentForeground;
        }
        #endregion
        #endregion

        #region Private Methods
        private void KlassenAktualisieren(object sender = null, RoutedEventArgs e = null) //Die parameter einach nur für den compiler damit das hier als command klappt
        {
            KlassenAktualisieren(true);
        }

        private void KlassenAktualisieren(bool voll = true)
        {
            Cursor = Cursors.AppStarting;

            if (voll)
            {
                DataHandler.LadeSchulKlassen();
            }

            MenuKlassenDtGrd.ItemsSource = null;
            MenuKlassenDtGrd.ItemsSource = DataHandler.SchulKlassen;


            if (!DataHandler.SchulKlassen.Contains(AusgewählteKlasse))
            {
                WindowContent = EWindowContent.Leer;
            }

            if (DataHandler.HatKlassen())
            {
                LKeineKlasseAusgewähltStkPnl.Visibility = Visibility.Visible;
                KeineKlassenGefundenStkPnl.Visibility = Visibility.Hidden;
                MenuKlasseExportierenBtn.IsEnabled = true;
            }
            else
            {
                LKeineKlasseAusgewähltStkPnl.Visibility = Visibility.Hidden;
                KeineKlassenGefundenStkPnl.Visibility = Visibility.Visible;
                MenuKlasseExportierenBtn.IsEnabled = false;
            }

            Cursor = Cursors.Arrow;
        }

        private void AktualisiereKESchülerDtGrd()
        {
            KESchülerDtGrd.ItemsSource = null;
            KESchülerDtGrd.ItemsSource = KESchülerListe;

            KEAnzahlSchülerTxbk.Text = KESchülerListe.Count.ToString();

            if (KESchülerListe.Count > 0)
            {
                KEKeineSchülerVorhandenLbl.Visibility = Visibility.Hidden;
            }
            else
            {
                KEKeineSchülerVorhandenLbl.Visibility = Visibility.Visible;
            }
        }

        private void ZeigeKlasseAn()
        {
            ÜKlasseNameLbl.Content = $"Übersicht - {AusgewählteKlasse.Name}";
            ÜKlasseAnzahlSchülerLbl.Content = AusgewählteKlasse.AnzahlSchüler;

            ÜSchülerDtGrd.ItemsSource = null;
            ÜSchülerDtGrd.ItemsSource = AusgewählteKlasse.SchuelerListe;
            ÜKeineSchülerVorhandenLbl.Visibility = ÜSchülerDtGrd.Items.Count > 0 ? Visibility.Hidden : Visibility.Visible;

            ÜSitzplanHinzufügenBtn.IsEnabled = AusgewählteKlasse.SchuelerListe.Count > 0 ? true : false;
            ÜSitzpläneDtGrd.ItemsSource = null;
            ÜSitzpläneDtGrd.ItemsSource = AusgewählteKlasse.Sitzplaene;
            ÜKeineSitzpläneVorhandenLbl.Visibility = ÜSitzpläneDtGrd.Items.Count > 0 ? Visibility.Hidden : Visibility.Visible;
        }
        #endregion

        #region Window
        private void MyMainWindow_Closing(object sender, CancelEventArgs e)
        {
            WindowState = WindowState.Minimized;
            ShowInTaskbar = false;

            DataHandler.SpeicherSchulKlassen();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataHandler.HatKlassen())
            {
                LKeineKlasseAusgewähltStkPnl.Visibility = Visibility.Visible;
                MenuKlasseExportierenBtn.IsEnabled = true;
            }
            else
            {
                KeineKlassenGefundenStkPnl.Visibility = Visibility.Visible;
                MenuKlasseExportierenBtn.IsEnabled = false;
            }

            MenuKlassenDtGrd.ItemsSource = DataHandler.SchulKlassen;

            SetzteTischblöcke();

            ContentLabels = new Label[]
            {
                KECSVImportLbl,
                KEKSchülerHinzufügenLbl,
                KESchülerEntfernenLbl,
                KEAbbrechenLbl,
                KEKlasseErstellenLbl,
                null,
                ÜSchülerEntfernenLbl,
                ÜSchülerHinzufügenLbl,
                ÜSitzplanEntfernenLbl,
                ÜSitzplanHinzufügenLbl,
                null,
                ÜSchülerBearbeitenAbbrechenLbl,
                ÜSchülerBearbeitenÜbernehmenLbl,
                ÜSchülerBildLöschenLbl,
                ÜSchülerBildÄndernLbl,
                ÜSitzplanAnzeigen1Lbl,
                ÜSchülerBearbeitenErstellenLbl,
                SZurückLbl,
                SExportLbl,
                SNeuGenerierenLbl
            };
            ContentPackIconsSets = new PackIconSet[]
            {
                new PackIconSet(KECSVImportPckIco, PackIconSet.EIconType.Content, PSColors.ContentButtonHoverForeground, PSColors.ContentButtonPreviewForeground),
                new PackIconSet(KESchülerHinzufügenPkIco, PackIconSet.EIconType.Content, PSColors.IconHoverGreen, PSColors.IconPreviewGreen),
                new PackIconSet(KESchülerEntfernenPkIco, PackIconSet.EIconType.Content, PSColors.IconHoverRed, PSColors.IconPreviewRed),
                new PackIconSet(KEAbbrechenPkIco, PackIconSet.EIconType.Content, PSColors.IconHoverRed, PSColors.IconPreviewRed),
                new PackIconSet(KEKlasseErstellenPkIco, PackIconSet.EIconType.Content, PSColors.IconHoverGreen, PSColors.IconPreviewGreen),
                new PackIconSet(ÜSitzplanAnzeigenPkIco, PackIconSet.EIconType.Content, PSColors.ContentButtonHoverForeground, PSColors.ContentButtonPreviewForeground),
                new PackIconSet(ÜSchülerEntfernenPckIco, PackIconSet.EIconType.Content, PSColors.IconHoverRed, PSColors.IconPreviewRed),
                new PackIconSet(ÜSchülerHinzufügenPckIco, PackIconSet.EIconType.Content, PSColors.IconHoverGreen, PSColors.IconPreviewGreen),
                new PackIconSet(ÜSitzplanEntfernenPckIco, PackIconSet.EIconType.Content, PSColors.IconHoverRed, PSColors.IconPreviewRed),
                new PackIconSet(ÜSitzplanHinzufügenPckIco, PackIconSet.EIconType.Content, PSColors.IconHoverGreen, PSColors.IconPreviewGreen),
                new PackIconSet(ÜSitzplanVersteckenPkIco, PackIconSet.EIconType.Content, PSColors.ContentButtonHoverForeground, PSColors.ContentButtonPreviewForeground),
                new PackIconSet(ÜSchülerBearbeitenAbbrechenPckIco, PackIconSet.EIconType.Content, PSColors.IconHoverRed, PSColors.IconPreviewRed),
                new PackIconSet(ÜSchülerBearbeitenÜbernehmenPckIco, PackIconSet.EIconType.Content, PSColors.IconHoverGreen, PSColors.IconPreviewGreen),
                null,
                null,
                new PackIconSet(ÜSitzplanAnzeigen1PckIco, PackIconSet.EIconType.Content, PSColors.ContentHoverForeground, PSColors.ContentButtonPreviewForeground),
                new PackIconSet(ÜSchülerBearbeitenErstellenPckIco, PackIconSet.EIconType.Content, PSColors.IconHoverGreen, PSColors.IconPreviewGreen),
                null,
                null,
                new PackIconSet(SNeuGenerierenPckIco, PackIconSet.EIconType.Content, PSColors.ContentHoverForeground, PSColors.ContentButtonPreviewForeground)//19
            };

            isLoading = false;
        }
        #endregion

        #region Menu
        #region SitzplanLbl
        private void SitzplanLbl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Process.Start($@"{Environment.CurrentDirectory}\SchulKlassen");
        }
        #endregion

        #region MenuKlasseImportBtn
        private void MenuKlasseImportierenBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Json files (*.json)|*.json";
            openFileDialog.InitialDirectory = $@"{Environment.CurrentDirectory}\SchulKlassen";
            if (openFileDialog.ShowDialog() == true)
            {
                DataHandler.LadeSchulKlasse(openFileDialog.FileName, true);
            }

            KlasseÜbersichtGrd.Visibility = Visibility.Hidden;

            KlassenAktualisieren(false);
        }
        #endregion

        #region MenuKlasseExportierenBtn
        private void MenuKlasseExportierenBtn_Click(object sender, RoutedEventArgs e)
        {
            if (DataHandler.HatKlassen())
            {
                new ExportWindow().ShowDialog();
            }
        }
        #endregion

        #region MenuKlasseSpeichernBtn
        private void MenuKlasseSpeichernBtn_Click(object sender, RoutedEventArgs e)
        {
            DataHandler.SpeicherSchulKlassen();
        }
        #endregion

        #region MenuHilfeBtn
        private void MenuHilfeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists($"{Environment.CurrentDirectory}\\Betriebsanleitung.pdf"))
            {
                Process.Start($"{Environment.CurrentDirectory}\\Betriebsanleitung.pdf");
            }
            else
            {
                new PsMessageBox("Achtung", "Die Betriebsanleitung konnte nicht gefunden werden.", PsMessageBox.EPsMessageBoxButtons.OK).ShowDialog();
            }
        }
        #endregion

        #region Beenden
        private void MenuBeendenBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion

        #region MenuKlassenDtGrd
        private void MenuKlassenDtGrd_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            ÜbersichtMode = EÜbersichtMode.Leer;

            if (!MenuKlassenDtGrd.SelectedIndex.Equals(-1))
            {
                if (ZeigtSitzplanAn)
                {
                    VersteckeSitzplanAnimation(new TimeSpan(10000));
                }

                AusgewählteKlasse = (SchulKlasse)MenuKlassenDtGrd.SelectedItem;

                WindowContent = EWindowContent.KlasseÜbersicht;
            }

            MMenuKlasseLöschenBtn.IsEnabled = MenuKlassenDtGrd.SelectedIndex > -1;
            MMenuKlasseLöschenLbl.IsEnabled = MenuKlassenDtGrd.SelectedIndex > -1;
            MMenuKlasseLöschenBtn.ToolTip = new ToolTip()
            {
                Content = $"Lösche die \"{AusgewählteKlasse.Name}\" Klasse",
                Foreground = PSColors.ToolTipForeground,
                Background = PSColors.ToolTipBackground,
                FontFamily = new FontFamily("Segoe UI Semibold"),
                FontSize = 12
            };
        }

        private void MenuKlassenDtGrd_Sorting(object sender, DataGridSortingEventArgs e)
        {
            if (e.Column.SortDirection.Equals(ListSortDirection.Ascending))
            {
                MenuKlassenSortingPkIco.Kind = PackIconKind.SortAlphabeticalAscending;
            }
            else
            {
                MenuKlassenSortingPkIco.Kind = PackIconKind.SortAlphabeticalDescending;
            }
        }
        #endregion

        #region MMenuKlasseLöschenBtn
        private void MMenuKlasseLöschenBtn_Click(object sender, RoutedEventArgs e)
        {
            PsMessageBox msg = new PsMessageBox("Achtung", $"Möchtest du wirklich \"{AusgewählteKlasse.Name}\" löschen?", PsMessageBox.EPsMessageBoxButtons.YesNo);

            msg.ShowDialog();

            if (msg.Result.Equals(PsMessageBox.EPsMessageBoxResult.Yes))
            {
                DataHandler.LöscheSchulKlasse(AusgewählteKlasse);

                KlassenAktualisieren(false);
                new PsMessageBox("Achtung", $"Die Klasse \"{AusgewählteKlasse.Name}\" wurde gelöscht.", PsMessageBox.EPsMessageBoxButtons.OK).ShowDialog();

                Keyboard.ClearFocus();
                Focus();
            }
        }
        #endregion

        #region KeineKlassenVorhandenKlasseErstellen
        private void MenuKlasseErstellenBtn_Click(object sender, RoutedEventArgs e)
        {
            KEFensterLeeren();

            WindowContent = EWindowContent.KlasseErstellen;
            KeineKlassenGefundenStkPnl.Visibility = Visibility.Hidden;
            MenuKlassenDtGrd.SelectedIndex = -1;



            KESchülerListe.Clear();
        }
        #endregion
        #endregion

        #region Content - Klasse erstellen
        #region KESchülerBildLöschenBtn
        private System.Drawing.Image keSchülerBild = null;

        private void KESchülerBildLöschenBtn_Click(object sender, RoutedEventArgs e)
        {
            keSchülerBild = null;
            KESchülerBildImg.Source = null;
        }
        #endregion

        #region KESchülerBildÄndernBtn
        private void KESchülerBildÄndernBtn_Click(object sender, RoutedEventArgs e)
        {
            keSchülerBild = SchülerHelfer.SchülerBildDialog();
            KESchülerBildImg.Source = Convert(keSchülerBild);
        }
        #endregion

        #region KEUnitsImportBtn
        private void KECSVImportBtn_Click(object sender, RoutedEventArgs e)
        {
            KESchülerListe.AddRange(SchulKlasse.SchülerImportCSV());

            AktualisiereKESchülerDtGrd();
        }

        private void KESchülerFelderLeeren()
        {
            KESchülerNachnameTxbx.Text = "";
            KESchülerVornameTxbx.Text = "";
            KESchülerBetriebTxbx.Text = "";
            KESchülerGeschlechtCb.SelectedIndex = -1;
            KESchülerBerufCb.SelectedIndex = -1;
            KESchülerBildImg.Source = null;
            KESchülerVerkürztCBx.IsChecked = false;
        }
        #endregion

        #region KESchülerHinzufügenBtn
        private void KESchülerHinzufügenBtn_Click(object sender, RoutedEventArgs e)
        {
            string vorname = KESchülerVornameTxbx.Text.Trim();
            string nachname = KESchülerNachnameTxbx.Text.Trim();
            string betrieb = KESchülerBetriebTxbx.Text.Trim();

            if (string.IsNullOrWhiteSpace(vorname) || string.IsNullOrWhiteSpace(nachname) || string.IsNullOrWhiteSpace(betrieb))
            {
                ErrorHandler.ZeigeFehler(ErrorHandler.ERR_SH_PflichtfelderNichtAusgefüllt);
                return;
            }

            Person.EGeschlecht geschlecht;
            Person.EBeruf beruf;
            try
            {
                geschlecht = (Person.EGeschlecht)Enum.Parse(typeof(Person.EGeschlecht), KESchülerGeschlechtCb.Text, true);
                beruf = (Person.EBeruf)Enum.Parse(typeof(Person.EBeruf), KESchülerBerufCb.Text.Replace(" ", ""), true);
            }
            catch (ArgumentException)
            {
                ErrorHandler.ZeigeFehler(ErrorHandler.ERR_SH_PflichtfelderNichtAusgefüllt);
                return;
            }

            bool verkürzt = KESchülerVerkürztCBx.IsChecked.Value;

            Schüler neuerSchüler = new Schüler(new Person(vorname, nachname, geschlecht, beruf), betrieb, verkürzt, keSchülerBild);

            if (KESchülerListe.Count >= SchulKlasse.MaxSchüler)
            {
                ErrorHandler.ZeigeFehler(ErrorHandler.ERR_MaxSchüler);

                return;
            }

            KESchülerListe.Add(neuerSchüler);
            KESchülerDtGrd.ItemsSource = null;
            KESchülerDtGrd.ItemsSource = KESchülerListe;
            keSchülerBild = null;

            KEAnzahlSchülerTxbk.Text = KESchülerListe.Count.ToString();
            AktualisiereKESchülerDtGrd();

            KESchülerFelderLeeren();
        }
        #endregion

        #region KESchülerDtGrd
        private void KESchülerDtGrd_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            KESchülerEntfernenBtn.IsEnabled = !KESchülerDtGrd.SelectedIndex.Equals(-1);
        }
        #endregion

        #region KESchülerEntfernenBtn
        private void KESchülerEntfernenBtn_Click(object sender, RoutedEventArgs e)
        {
            KESchülerListe.Remove(KESchülerDtGrd.SelectedItem as Schüler);

            AktualisiereKESchülerDtGrd();
        }
        #endregion

        #region KEAbbrechenBtn
        private void KEAbbrechenBtn_Click(object sender, RoutedEventArgs e)
        {
            KEFensterLeeren();
        }
        #endregion

        #region KEKlasseErstellenBtn
        private readonly List<Schüler> KESchülerListe = new List<Schüler>();
        private void KEKlasseErstellenBtn_Click(object sender, RoutedEventArgs e)
        {
            string klassenName = KEKlassenNameTxbx.Text.Trim();

            if (klassenName == null || klassenName.Equals(""))
            {
                ErrorHandler.ZeigeFehler(ErrorHandler.ERR_KE_KeinName);
                return;
            }

            if (Path.GetInvalidFileNameChars().Any(klassenName.Contains))
            {
                ErrorHandler.ZeigeFehler(ErrorHandler.ERR_KE_UriUngültig);
                return;
            }

            if (DataHandler.ExistiertKlasseBereits(klassenName))
            {
                ErrorHandler.ZeigeFehler(ErrorHandler.ERR_KE_KlasserExistiertBereits);
                return;
            }

            SchulKlasse neueKlasse = new SchulKlasse(klassenName, new List<Schüler>(KESchülerListe));
            DataHandler.FügeSchulKlasseHinzu(neueKlasse);

            KEFensterLeeren();

            KlassenAktualisieren(false);
        }
        #endregion

        #region KEBereinigen
        private void KEFensterLeeren()
        {
            KESchülerListe.Clear();

            KESchülerFelderLeeren();
            KEKlassenNameTxbx.Text = "";
            KESchülerDtGrd.ItemsSource = null;
            KEAnzahlSchülerTxbk.Text = "0";

            KESchülerFelderLeeren();

            WindowContent = EWindowContent.Leer;
        }
        #endregion
        #endregion

        #region Content - Klasse Übersicht
        #region ÜSitzplanAnzeigenBtn
        private void ÜSitzplanAnzeigenBtn_Click(object sender, RoutedEventArgs e)
        {
            Storyboard storyboard = new Storyboard();
            DoubleAnimation animation = new DoubleAnimation(KlasseÜbersichtGrd.ActualWidth, 0D, new Duration(new TimeSpan(0, 0, 0, 0, 500)), FillBehavior.HoldEnd);
            Storyboard.SetTargetProperty(animation, new PropertyPath("Width"));
            animation.AccelerationRatio = 0.2D;
            animation.DecelerationRatio = 0.8D;
            storyboard.Children.Add(animation);
            ZeigtSitzplanAn = true;

            KlasseÜbersichtSitzplanGrd.Visibility = Visibility.Visible;
            storyboard.Begin(KlasseÜbersichtContentGrd);
        }
        #endregion

        #region ÜSchülerDtGrd
        public BitmapImage Convert(System.Drawing.Image img)
        {
            if (img is null) return null;

            using (var memory = new MemoryStream())
            {
                System.Drawing.Image bildKopie = new System.Drawing.Bitmap(img);
                bildKopie.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);

                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        private enum EÜbersichtMode
        {
            Leer,
            Bearbeiten,
            Erstellen
        }
        private EÜbersichtMode übersichtMode = EÜbersichtMode.Leer;
        private EÜbersichtMode ÜbersichtMode
        {
            get => übersichtMode;
            set
            {
                switch (value)
                {
                    case EÜbersichtMode.Leer:
                        {
                            ÜSchülerÜbersichtGrd.Visibility = Visibility.Hidden;
                            ÜSchülerÜbersichtÜbernehmenGrd.Visibility = Visibility.Hidden;
                            ÜSchülerÜbersichtErstellenGrd.Visibility = Visibility.Hidden;

                            break;
                        }
                    case EÜbersichtMode.Bearbeiten:
                        {
                            ÜSchülerÜbersichtGrd.Visibility = Visibility.Visible;

                            ÜSchülerÜbersichtErstellenGrd.Visibility = Visibility.Hidden;
                            ÜSchülerÜbersichtÜbernehmenGrd.Visibility = Visibility.Visible;

                            ÜSchülerNameLbl.Content = $"Bearbeite - {ÜAusgewählterSchüler.Vorname} {ÜAusgewählterSchüler.Nachname}";
                            ÜSchülerVornameTxbx.Text = ÜAusgewählterSchüler.Vorname;
                            ÜSchülerNachnameTxbx.Text = ÜAusgewählterSchüler.Nachname;
                            ÜSchülerBetriebTxbx.Text = ÜAusgewählterSchüler.Betrieb;
                            ÜSchülerGeschlechtCb.SelectedIndex = (int)ÜAusgewählterSchüler.Geschlecht;
                            ÜSchülerBerufCb.SelectedIndex = (int)ÜAusgewählterSchüler.Beruf;
                            ÜSchülerVerkürztCBx.IsChecked = ÜAusgewählterSchüler.Verkuerzt;
                            ÜSchülerBildImg.Source = Convert(ÜAusgewählterSchüler.Bild);

                            if (ÜAusgewählterSchüler.Bild is null)
                            {
                                ÜSchülerKeinBildVorhandenLbl.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                ÜSchülerKeinBildVorhandenLbl.Visibility = Visibility.Hidden;

                                ÜSchülerBildImg.Source = Convert(ÜAusgewählterSchüler.Bild);
                            }

                            break;
                        }
                    case EÜbersichtMode.Erstellen:
                        {
                            ÜSchülerÜbersichtGrd.Visibility = Visibility.Visible;

                            ÜSchülerÜbersichtÜbernehmenGrd.Visibility = Visibility.Hidden;
                            ÜSchülerÜbersichtErstellenGrd.Visibility = Visibility.Visible;

                            ÜSchülerNameLbl.Content = $"Erstelle neuen Schüler";
                            ÜSchülerVornameTxbx.Text = "";
                            ÜSchülerNachnameTxbx.Text = "";
                            ÜSchülerBetriebTxbx.Text = "";
                            ÜSchülerGeschlechtCb.SelectedIndex = -1;
                            ÜSchülerBerufCb.SelectedIndex = -1;
                            ÜSchülerVerkürztCBx.IsChecked = false;
                            ÜSchülerBildImg.Source = null;
                            ÜSchülerKeinBildVorhandenLbl.Visibility = Visibility.Visible;

                            break;
                        }
                }

                übersichtMode = value;
            }
        }
        private void ÜSchülerDtGrd_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            ÜSchülerEntfernenBtn.IsEnabled = ÜSchülerDtGrd.SelectedIndex > -1;
            if (ÜSchülerDtGrd.SelectedIndex > -1)
            {
                ÜAusgewählterSchüler = (Schüler)ÜSchülerDtGrd.SelectedItem;

                if (ÜAusgewählterSchüler != null)
                {
                    schülerImg = ÜAusgewählterSchüler.Bild;
                }

                ÜbersichtMode = EÜbersichtMode.Bearbeiten;
            }
            else
            {
                ÜbersichtMode = EÜbersichtMode.Leer;
            }
        }
        #endregion

        #region ÜSchülerEntfernenBtn
        private void AktualisiereSchüler()
        {
            AktualisiereÜSchülerDtGrd();
            ÜKlasseAnzahlSchülerLbl.Content = AusgewählteKlasse.SchuelerListe.Count;
        }

        private void ÜSchülerEntfernenBtn_Click(object sender, RoutedEventArgs e)
        {
            AusgewählteKlasse.SchülerEntfernen((Schüler)ÜSchülerDtGrd.SelectedItem);
            AktualisiereSchüler();

            ÜSitzplanHinzufügenBtn.IsEnabled = AusgewählteKlasse.SchuelerListe.Count > 0 ? true : false;
            ÜKeineSchülerVorhandenLbl.Visibility = ÜSchülerDtGrd.Items.Count > 0 ? Visibility.Hidden : Visibility.Visible;
        }
        #endregion

        #region ÜSchülerHinzufügenBtn
        private void ÜSchülerHinzufügenBtn_Click(object sender, RoutedEventArgs e)
        {
            AktualisiereSchüler();

            ÜSitzplanHinzufügenBtn.IsEnabled = AusgewählteKlasse.SchuelerListe.Count > 0 ? true : false;

            ÜbersichtMode = EÜbersichtMode.Erstellen;
        }
        #endregion

        #region ÜSitzpläneDtGrd
        private void ÜSitzpläneDtGrd_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            ÜSitzplanEntfernenBtn.IsEnabled = ÜSitzpläneDtGrd.SelectedIndex > -1;
            ÜSitzplanAnzeigen1Btn.IsEnabled = ÜSitzpläneDtGrd.SelectedIndex > -1;

            if (ÜSitzpläneDtGrd.SelectedIndex > -1)
            {
                ÜSitzplanAnzeigenGrd.Visibility = Visibility.Visible;

                ÜAusgewählterSitzplan = (Sitzplan)ÜSitzpläneDtGrd.SelectedItem;
            }
            else
            {
                ÜSitzplanAnzeigenGrd.Visibility = Visibility.Hidden;
            }
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ÜSitzplanAnzeigenBtn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }
        #endregion

        #region ÜSitzplanEntfernenBtn
        private void AktualisiereSitzpläne()
        {
            ÜSitzpläneDtGrd.ItemsSource = null;
            ÜSitzpläneDtGrd.ItemsSource = AusgewählteKlasse.Sitzplaene;
        }

        private void ÜSitzplanEntfernenBtn_Click(object sender, RoutedEventArgs e)
        {
            AusgewählteKlasse.Sitzplaene.Remove((Sitzplan)ÜSitzpläneDtGrd.SelectedItem);
            AktualisiereSitzpläne();
            ÜKeineSitzpläneVorhandenLbl.Visibility = ÜSchülerDtGrd.Items.Count > 0 ? Visibility.Hidden : Visibility.Visible;
        }
        #endregion

        #region ÜSitzplanHinzufügenBtn
        private void SitzplanGenerierenClick(object sender, RoutedEventArgs e)
        {
            Sitzplan neuerSitzplan = ausgewählteKlasse.ErstelleSitzplanDialog();

            if (!(neuerSitzplan is null))
            {
                AktualisiereSitzpläne();

                ÜKeineSitzpläneVorhandenLbl.Visibility = ÜSitzpläneDtGrd.Items.Count > 0 ? Visibility.Hidden : Visibility.Visible;
            }

        }
        #endregion

        #region ÜSchülerBildLöschenBtn
        private void ÜSchülerBildLöschenBtn_Click(object sender, RoutedEventArgs e)
        {
            ÜSchülerBildImg.Source = null;
            schülerImg = null;

            switch (ÜbersichtMode)
            {
                case EÜbersichtMode.Bearbeiten:
                    {
                        ÜSchülerBildImg.Source = null;

                        break;
                    }
                case EÜbersichtMode.Erstellen:
                    {
                        ÜSchülerKeinBildVorhandenLbl.Visibility = Visibility.Visible;
                        break;
                    }
            }
        }
        #endregion

        #region ÜSchülerBldÄndernBtn
        private System.Drawing.Image schülerImg = null;
        private void ÜSchülerBildÄndernBtn_Click(object sender, RoutedEventArgs e)
        {
            schülerImg = SchülerHelfer.SchülerBildDialog();
            ÜSchülerBildImg.Source = Convert(schülerImg);
        }
        #endregion

        #region ÜSchülerBearbeitenAbbrechenBtn
        private void ÜSchülerBearbeitenAbbrechenBtn_Click(object sender, RoutedEventArgs e)
        {
            switch (ÜbersichtMode)
            {
                case EÜbersichtMode.Bearbeiten:
                    {
                        ÜSchülerDtGrd.SelectedIndex = -1;
                        break;
                    }
                case EÜbersichtMode.Erstellen:
                    {
                        schülerImg = null;
                        ÜSchülerBildImg.Source = null;

                        ÜbersichtMode = EÜbersichtMode.Leer;
                        break;
                    }
            }

        }
        #endregion

        #region ÜSchülerBearbeitenÜbernehmenBtn
        private void AktualisiereÜSchülerDtGrd()
        {
            ÜSchülerDtGrd.ItemsSource = null;
            ÜSchülerDtGrd.ItemsSource = AusgewählteKlasse.SchuelerListe;
        }

        private void ÜSchülerBearbeitenÜbernehmenBtn_Click(object sender, RoutedEventArgs e)
        {
            Schüler schüler = new Schüler((Schüler)ÜSchülerDtGrd.SelectedItem);

            string vorname = ÜSchülerVornameTxbx.Text.Trim();
            string nachname = ÜSchülerNachnameTxbx.Text.Trim();
            string betrieb = ÜSchülerBetriebTxbx.Text.Trim();

            if (string.IsNullOrWhiteSpace(vorname) || string.IsNullOrWhiteSpace(nachname) || string.IsNullOrWhiteSpace(betrieb))
            {
                ErrorHandler.ZeigeFehler(ErrorHandler.ERR_SH_PflichtfelderNichtAusgefüllt);
                return;
            }

            Person.EGeschlecht geschlecht;
            Person.EBeruf beruf;
            try
            {
                geschlecht = (Person.EGeschlecht)Enum.Parse(typeof(Person.EGeschlecht), ÜSchülerGeschlechtCb.Text, true);
                beruf = (Person.EBeruf)Enum.Parse(typeof(Person.EBeruf), ÜSchülerBerufCb.Text.Replace(" ", ""), true);
            }
            catch (ArgumentException)
            {
                ErrorHandler.ZeigeFehler(ErrorHandler.ERR_SH_PflichtfelderNichtAusgefüllt);
                return;
            }


            bool verkürzt = ÜSchülerVerkürztCBx.IsChecked.Value;

            schüler.Vorname = vorname;
            schüler.Nachname = nachname;
            schüler.Betrieb = betrieb;
            schüler.Geschlecht = geschlecht;
            schüler.Beruf = beruf;
            schüler.Verkuerzt = verkürzt;
            schüler.Bild = schülerImg;

            ÜSchülerDtGrd.ItemsSource = null;
            ÜSchülerDtGrd.ItemsSource = AusgewählteKlasse.SchuelerListe;
            ÜSchülerBildImg.Source = null;
            schülerImg = null;

            AusgewählteKlasse.SchülerAktuallisieren(schüler);

            ÜbersichtMode = EÜbersichtMode.Leer;
        }
        #endregion

        #region ÜSchülerÜbersichtErstellenBtn
        private void ÜSchülerBearbeitenErstellenBtn_Click(object sender, RoutedEventArgs e)
        {
            string vorname = ÜSchülerVornameTxbx.Text.Trim();
            string nachname = ÜSchülerNachnameTxbx.Text.Trim();
            string betrieb = ÜSchülerBetriebTxbx.Text.Trim();

            if (string.IsNullOrWhiteSpace(vorname) || string.IsNullOrWhiteSpace(nachname) || string.IsNullOrWhiteSpace(betrieb))
            {
                ErrorHandler.ZeigeFehler(ErrorHandler.ERR_SH_PflichtfelderNichtAusgefüllt);
                return;
            }

            Person.EGeschlecht geschlecht;
            Person.EBeruf beruf;
            try
            {
                geschlecht = (Person.EGeschlecht)Enum.Parse(typeof(Person.EGeschlecht), ÜSchülerGeschlechtCb.Text, true);
                beruf = (Person.EBeruf)Enum.Parse(typeof(Person.EBeruf), ÜSchülerBerufCb.Text.Replace(" ", ""), true);
            }
            catch (ArgumentException)
            {
                ErrorHandler.ZeigeFehler(ErrorHandler.ERR_SH_PflichtfelderNichtAusgefüllt);
                return;
            }

            bool verkürzt = ÜSchülerVerkürztCBx.IsChecked.Value;

            Schüler neuerSchüler = new Schüler(new Person(vorname, nachname, geschlecht, beruf), betrieb, verkürzt);
            neuerSchüler.Bild = schülerImg;
            schülerImg = null;
            ÜSchülerBildImg.Source = null;

            ÜSchülerDtGrd.ItemsSource = null;
            AusgewählteKlasse.SchülerHinzufügen(neuerSchüler);
            ÜSchülerDtGrd.ItemsSource = AusgewählteKlasse.SchuelerListe;

            //DataHandler.SpeicherSchulKlasse(AusgewählteKlasse);

            ÜKlasseAnzahlSchülerLbl.Content = AusgewählteKlasse.AnzahlSchüler;

            ÜKeineSchülerVorhandenLbl.Visibility = Visibility.Hidden;
            ÜbersichtMode = EÜbersichtMode.Leer;
        }
        #endregion

        #region ÜSitzplanVersteckenBtn
        private void ÜSitzplanVersteckenBtn_Click(object sender, RoutedEventArgs e)
        {
            VersteckeSitzplanAnimation(new TimeSpan(0, 0, 0, 0, 500));
        }

        private void VersteckeSitzplanAnimation(TimeSpan duotation)
        {
            Storyboard storyboard = new Storyboard();
            DoubleAnimation animation = new DoubleAnimation(0D, KlasseÜbersichtGrd.ActualWidth, new Duration(duotation), FillBehavior.Stop);
            Storyboard.SetTargetProperty(animation, new PropertyPath("Width"));
            animation.AccelerationRatio = 0.2D;
            animation.DecelerationRatio = 0.8D;
            animation.Completed += Animation_Completed;
            storyboard.Children.Add(animation);

            foreach (Border tisch in Tische)
            {
                tisch.Visibility = Visibility.Hidden;
            }

            storyboard.Begin(KlasseÜbersichtContentGrd);

            AusgewählteKlasse.SpeichernAsync();
        }

        private void Animation_Completed(object sender, EventArgs e)
        {
            KlasseÜbersichtSitzplanGrd.Visibility = Visibility.Hidden;
            ZeigtSitzplanAn = false;
        }
        #endregion

        #region SExportBtn
        private void SExportBtn_Click(object sender, RoutedEventArgs e)
        {
            ÜAusgewählterSitzplan.AlsHTMLExportieren();
        }
        #endregion

        #region SNeuGenerierenBtn
        private void SNeuGenerierenBtn_Click(object sender, RoutedEventArgs e)
        {
            PsMessageBox msg = new PsMessageBox("Achtung", "Soll der Sitzplan wirklich neu generiert werden?\nDer alte Sitzplan wird überschrieben.", PsMessageBox.EPsMessageBoxButtons.YesNo);
            msg.OnPsMessageBoxButtonPressed += Msg_OnPsMessageBoxButtonPressed;
            msg.ShowDialog();
        }

        private void Msg_OnPsMessageBoxButtonPressed(object source, PsMessagBoxEventArgs e)
        {
            switch (e.PsMessageBoxButtonResult)
            {
                case PsMessageBox.EPsMessageBoxResult.Yes:
                    {
                        ÜAusgewählterSitzplan.NeuGenerieren(AusgewählteKlasse);

                        SetzeSchüler(SBerufCBx.IsChecked.Value, SBetriebCBx.IsChecked.Value, SGeschlechtCBx.IsChecked.Value);
                        break;
                    }
            }
        }
        #endregion
        #endregion
    }
}
