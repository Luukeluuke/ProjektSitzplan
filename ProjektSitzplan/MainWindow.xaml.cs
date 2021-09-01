﻿using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using ProjektSitzplan.Design;
using ProjektSitzplan.Structures;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
                            //TODO: Make this with a schnieke method
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

        #region Commands
        public static RoutedCommand CommandCreate = new RoutedCommand();
        public static RoutedCommand CommandImport = new RoutedCommand();
        public static RoutedCommand CommandExport = new RoutedCommand();
        public static RoutedCommand CommandSave = new RoutedCommand();
        public static RoutedCommand CommandRefresh = new RoutedCommand();
        public static RoutedCommand CommandUndo = new RoutedCommand();
        public static RoutedCommand CommandRedo = new RoutedCommand();

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
            
            CommandBindings.Add(new CommandBinding(CommandCreate, MenuKlasseErstellenBtn_Click));
            CommandBindings.Add(new CommandBinding(CommandImport, MenuKlasseImportierenBtn_Click));
            CommandBindings.Add(new CommandBinding(CommandExport, MenuKlasseExportierenBtn_Click));
            CommandBindings.Add(new CommandBinding(CommandRefresh, KlassenAktualisieren));
        }
        #endregion

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
            //TODO: Wenn datein importiert die wo die klasse den selben namen hat wie eine bereits vorhandene klasse?
            
            
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
            DataHandler.LadeSchulKlassen();
            MenuKlassenDtGrd.ItemsSource = null;
            MenuKlassenDtGrd.ItemsSource = DataHandler.SchulKlassen;

            if (DataHandler.SchulKlassen.Count > 0)
            {
                LKeineKlasseAusgewähltStkPnl.Visibility = Visibility.Visible;
                KeineKlassenGefundenStkPnl.Visibility = Visibility.Hidden;
            }
            else
            {
                LKeineKlasseAusgewähltStkPnl.Visibility = Visibility.Hidden;
                KeineKlassenGefundenStkPnl.Visibility = Visibility.Visible;
            }
        }

        //TODO: Diese MEthode in den AusgewähltenSchülerEntfernenbtn einbauen. AM ende einfach yeyeyeokyeofsj
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
            ÜKlasseNameLbl.Content = AusgewählteKlasse.Name;
            ÜKlasseAnzahlSchülerLbl.Content = AusgewählteKlasse.AnzahlSchüler;


        }
        #endregion


        #region Window
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataHandler.KlassenVorhanden())
            {
                LKeineKlasseAusgewähltStkPnl.Visibility = Visibility.Visible;
            }
            else
            {
                KeineKlassenGefundenStkPnl.Visibility = Visibility.Visible;
            }


            MenuKlassenDtGrd.ItemsSource = DataHandler.SchulKlassen;
            //KESchülerDtGrd.ItemsSource = KESchülerListe;

            ContentLabels = new Label[]
            {
                KEFelderLeerenLbl,
                KEKSchülerHinzufügenLbl,
                KESchülerEntfernenLbl,
                KEAbbrechenLbl,
                KEKlasseErstellenLbl,
                null
            };
            ContentPackIconsSets = new PackIconSet[]
            {
                null, //Kein Icon vorhanden dann null, aber die Uids sollten halt trotzdem ausgefüllt werden
                new PackIconSet(KESchülerHinzufügenPkIco, PackIconSet.EIconType.Content, PSColors.IconHoverGreen, PSColors.IconPreviewGreen),
                new PackIconSet(KESchülerEntfernenPkIco, PackIconSet.EIconType.Content, PSColors.IconHoverRed, PSColors.IconPreviewRed),
                new PackIconSet(KEAbbrechenPkIco, PackIconSet.EIconType.Content, PSColors.IconHoverRed, PSColors.IconPreviewRed),
                new PackIconSet(KEKlasseErstellenPkIco, PackIconSet.EIconType.Content, PSColors.IconHoverGreen, PSColors.IconPreviewGreen),
                new PackIconSet(ÜSitzplanAnzeigenPkIco, PackIconSet.EIconType.Content, PSColors.ContentButtonHoverForeground, PSColors.ContentButtonPreviewForeground)
            };
        }
        #endregion

        #region Menu
        #region SitzplanLbl
        private void SitzplanLbl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Process.Start(Environment.CurrentDirectory);
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
                SchulKlasse klasse = SchulKlasse.AusDateiLaden(openFileDialog.FileName);
                DataHandler.FügeSchulKlasseHinzu(klasse);
            }

            KlassenAktualisieren();
        }
        #endregion

        #region MenuKlasseExportierenBtn
        private void MenuKlasseExportierenBtn_Click(object sender, RoutedEventArgs e)
        {
            //TODO:
        }
        #endregion

        #region MenuKlasseSpeichernBtn
        private void MenuKlasseSpeichernBtn_Click(object sender, RoutedEventArgs e)
        {
            //TODO:
        }
        #endregion


        #region MenuHilfeBtn
        private void MenuHilfeBtn_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Hinterher die Anleitungs Pdf einfach aufrufen oder so.
        }
        #endregion

        #region MenuAktualisierenBtn
        private void MenuAktualisierenBtn_Click(object sender, RoutedEventArgs e)
        {
            KlassenAktualisieren();
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
            if(!MenuKlassenDtGrd.SelectedIndex.Equals(-1))
            {
                //TODO: Wenn eine Klassen entfernt wird muss auch Klassen aktualisieren gemacht werden
                AusgewählteKlasse = DataHandler.SchulKlassen[MenuKlassenDtGrd.SelectedIndex];

                WindowContent = EWindowContent.KlasseÜbersicht;
            }
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

        #region KeineKlassenVorhandenKlasseErstellen
        private void MenuKlasseErstellenBtn_Click(object sender, RoutedEventArgs e)
        {
            WindowContent = EWindowContent.KlasseErstellen;
            KeineKlassenGefundenStkPnl.Visibility = Visibility.Hidden;
            MenuKlassenDtGrd.SelectedIndex = -1;

            KESchülerListe.Clear();
        }
        #endregion
        #endregion

        #region Content - Klasse erstellen
        #region KEFelderLeerenBtn
        private void KEFelderLeerenBtn_Click(object sender, RoutedEventArgs e)
        {
            KESchülerFelderLeeren();
        }

        private void KESchülerFelderLeeren()
        {
            KESchülerNachnameTxbx.Text = "";
            KESchülerVornameTxbx.Text = "";
            KESchülerBetriebTxbx.Text = "";
            KESchülerGeschlechtCb.SelectedIndex = -1;
            KESchülerBerufCb.SelectedIndex = -1;
        }
        #endregion

        #region KESchülerHinzufügenBtn
        private void KESchülerHinzufügenBtn_Click(object sender, RoutedEventArgs e)
        {
			string vorname = KESchülerVornameTxbx.Text.Trim();
            string nachname = KESchülerNachnameTxbx.Text.Trim();
            string betrieb = KESchülerBetriebTxbx.Text.Trim();

            if (vorname == "" || nachname == "" || betrieb == "")
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

            Schüler neuerSchüler = new Schüler(vorname, nachname, geschlecht, beruf, new Betrieb(betrieb));

            KESchülerListe.Add(neuerSchüler);
            KESchülerDtGrd.ItemsSource = null;
            KESchülerDtGrd.ItemsSource = KESchülerListe;
            
            KEAnzahlSchülerTxbk.Text = KESchülerListe.Count.ToString();
            AktualisiereKESchülerDtGrd();

            KESchülerFelderLeeren();
            // TODO: schüler werden nicht richtig angezeigt!!
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
            WindowContent = EWindowContent.Leer;
        }
        #endregion

        #region KEKlasseErstellenBtn
        private List<Schüler> KESchülerListe = new List<Schüler>();
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

            SchulKlasse neueKlasse = new SchulKlasse(klassenName, KESchülerListe);
            DataHandler.FügeSchulKlasseHinzu(neueKlasse);

            AusgewählteKlasse = neueKlasse;
            KESchülerListe.Clear();

            KESchülerFelderLeeren();
            KEKlassenNameTxbx.Text = "";


            WindowContent = EWindowContent.Leer;

            KlassenAktualisieren();
        }
        #endregion

        #endregion

        private void ÜSitzplanAnzeigenBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
