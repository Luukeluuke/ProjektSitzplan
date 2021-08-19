using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace ProjektSitzplan
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static SolidColorBrush c1 = GetColor("232834");
        private static SolidColorBrush c2 = GetColor("32394B");
        private static SolidColorBrush c2_1 = GetColor("282E3C");
        private static SolidColorBrush c3 = GetColor("FBD03C");
        private static SolidColorBrush c3_1 = GetColor("FCDA61");
        private static SolidColorBrush c4 = GetColor("F1F1F1");

        public MainWindow()
        {
            //TODO:In Menu typische symbole einbauen

            InitializeComponent();

            SourceInitialized += (s, e) =>
            {
                IntPtr handle = (new WindowInteropHelper(this)).Handle;
                HwndSource.FromHwnd(handle).AddHook(new HwndSourceHook(WindowProc));
            };
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            Test.TestFunction();
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

        #region WindowMinimizeButton
        private void WindowMinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void WindowMinimizeButton_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void WindowMinimizeButton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }

        private void WindowMinimizeButton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }
        #endregion

        #region WindowRestoreButton
        private void WindowRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Equals(WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
        }

        private void WindowRestoreButton_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void WindowRestoreButton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }

        private void WindowRestoreButton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }
        #endregion

        #region WindowCloseButton
        private void WindowCloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void WindowCloseButton_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void WindowCloseButton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }

        private void WindowCloseButton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Converts a hex string into a color
        /// </summary>
        /// <param name="hex">without #</param>
        /// <returns>Color object</returns>
        private static SolidColorBrush GetColor(string hex)
        {
            hex = hex.TrimStart('#');
            return (SolidColorBrush)new BrushConverter().ConvertFrom($"#{hex}");
        }
        #endregion
    }
}
