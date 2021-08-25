﻿using MaterialDesignThemes.Wpf;
using ProjektSitzplan.Design;
using ProjektSitzplan.Structures;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;

namespace ProjektSitzplan
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
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

                            break;
                        }
                    case EWindowContent.KlasseErstellen:
                        {
                            KlasseÜbersichtGrd.Visibility = Visibility.Hidden;

                            KlasseHinzufügenGrd.Visibility = Visibility.Visible;

                            break;
                        }
                    case EWindowContent.KlasseÜbersicht:
                        {
                            KlasseHinzufügenGrd.Visibility = Visibility.Hidden;

                            KlasseÜbersichtGrd.Visibility = Visibility.Visible;

                            break;
                        }
                }

                windowContent = value;
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

                switch (WindowContent)
                {
                    case EWindowContent.KlasseErstellen:
                        {
                            KESchülerDtGrd.ItemsSource = AusgewählteKlasse.SchülerListe;
                            KEAnzahlSchülerTxbk.Text = AusgewählteKlasse.AnzahlSchüler.ToString();

                            break;
                        }
                }
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
            //TODO: Weitere hier hinzufügen
        }

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
            //TODO: In Menu typische symbole einbauen // Import export save help
            //TODO: DIe Klasse Exportieren Funktion im Datei Menü Disablen wenn keine ausgewählt ist. Und die speichern funktion auch
            //TODO: Beim erstellen der klasse darauf achten das der klassenname keine Path.GetInvalidCHars beinhaltet
            //TODO: WindowCloseButton backgrouznd sollte rot werden
            
            InitializeComponent();

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
                IntPtr handle = (new WindowInteropHelper(this)).Handle;
                HwndSource.FromHwnd(handle).AddHook(new HwndSourceHook(WindowProc));
            };

            Directory.CreateDirectory("SchulKlassen");
            DataHandler.LadeSchulKlassen();
        }

        #region WindowProc Handling
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
        #region TopBarButtons
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
                        WindowState = WindowState.Equals(WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
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

        #region ContentButtons
        private void ContentButton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Button sBtn = Utility.GetButton(sender);

            sBtn.Background = PSColors.ContentButtonHoverBackground;
            ContentLabels[Utility.GetUid(sBtn)].Foreground = PSColors.ContentButtonHoverForeground;

            ContentPackIconsSets[Utility.GetUid(sBtn)]?.HandleColor(PackIconSet.EEventType.Enter);
        }

        private void ContentButton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Button sBtn = Utility.GetButton(sender);

            sBtn.Background = PSColors.ContentButtonBackground;
            ContentLabels[Utility.GetUid(sBtn)].Foreground = PSColors.ContentButtonForeground;
            ContentPackIconsSets[Utility.GetUid(sBtn)]?.HandleColor(PackIconSet.EEventType.Leave);
        }

        private void ContentButton_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Button sBtn = Utility.GetButton(sender);

            sBtn.Background = PSColors.ContentButtonPreviewBackground;
            ContentLabels[Utility.GetUid(sBtn)].Foreground = PSColors.ContentButtonPreviewForeground;
            ContentPackIconsSets[Utility.GetUid(sBtn)]?.HandleColor(PackIconSet.EEventType.PreviewDown);
        }

        private void ContentButton_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Button sBtn = Utility.GetButton(sender);

            sBtn.Background = PSColors.ContentButtonHoverBackground;
            ContentLabels[Utility.GetUid(sBtn)].Foreground = PSColors.ContentButtonHoverForeground;
            ContentPackIconsSets[Utility.GetUid(sBtn)]?.HandleColor(PackIconSet.EEventType.PreviewUp);
        }
        #endregion
        #endregion

        #region Private Methods
        //lol wir haben keine
        #endregion


        #region Window
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataHandler.SchulKlassen.Count.Equals(0))
            {
                KeineKlassenGefundenStPnl.Visibility = Visibility.Visible;
            }

            MenuKlassenDtGrd.ItemsSource = DataHandler.SchulKlassen;

            ContentLabels = new Label[]
            {
                KEFelderLeerenLbl,
                KEKSchülerHinzufügenLbl,
                KESchülerEntfernenLbl,
                KEAbbrechenLbl,
                KEKlasseErstellenLbl
            };
            ContentPackIconsSets = new PackIconSet[]
            {
                null, //Kein Icon vorhanden dann null, aber die Uids sollten halt trotzdem ausgefüllt werden
                null,
                new PackIconSet(KESchülerEntfernenPkIco, PackIconSet.EIconType.Content, PSColors.IconHoverRed, PSColors.IconPreviewRed),
                new PackIconSet(KEAbbrechenPkIco, PackIconSet.EIconType.Content, PSColors.IconHoverRed, PSColors.IconPreviewRed),
                new PackIconSet(KEKlasseErstellenPkIco, PackIconSet.EIconType.Content, PSColors.IconHoverGreen, PSColors.IconPreviewGreen)
            };
        }
        #endregion

        #region Menu
        #region Beenden
        private void MenuBeendenBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion

        #region MenuKlassenDtGrd
        private void MenuKlassenDtGrd_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            WindowContent = EWindowContent.KlasseÜbersicht;
        }

        private void MenuKlassenDtGrd_Sorting(object sender, DataGridSortingEventArgs e)
        {
            if (e.Column.SortDirection.Equals(ListSortDirection.Ascending))
            {
                MenuKlassenSortingPkIco.Kind = MaterialDesignThemes.Wpf.PackIconKind.SortAlphabeticalAscending;
            }
            else
            {
                MenuKlassenSortingPkIco.Kind = MaterialDesignThemes.Wpf.PackIconKind.SortAlphabeticalDescending;
            }
        }
        #endregion

        #region KeineKlassenVorhandenKlasseErstellen
        private void MenuKeineKlassenVorhandenKlasseErstellenBtn_Click(object sender, RoutedEventArgs e)
        {
            WindowContent = EWindowContent.KlasseErstellen;
            KeineKlassenGefundenStPnl.Visibility = Visibility.Hidden;
        }
        #endregion
        #endregion

        #region Content - Klasse erstellen
        #region KEKlasseNameTxBx
        private void KEKlassenNameTxbx_GotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            KEKlassenNameTxbx.Foreground = PSColors.ContentTextBoxSelectedForeground;
        }

        private void KEKlassenNameTxbx_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            KEKlassenNameTxbx.Foreground = PSColors.ContentForeground;
        }
        #endregion

        #region KEGEschlechtCb
        private void KEGeschlechtCb_GotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {

        }

        private void KEGeschlechtCb_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {

        }
        #endregion

        #region KESchülerDtGrd
        private void KESchülerDtGrd_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

        }
        #endregion

        #region KEKlasseErstellenBtn
        private void KEKlasseErstellenBtn_Click(object sender, RoutedEventArgs e)
        {
            DataHandler.FügeSchulKlasseHinzu(AusgewählteKlasse);
        }
        #endregion
        #endregion
    }
}
