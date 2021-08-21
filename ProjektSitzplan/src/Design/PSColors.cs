using System.Windows.Media;

namespace ProjektSitzplan.Design
{
    static class PSColors
    {
        public static readonly SolidColorBrush TopBarBackground = GetColor("202225");
        public static readonly SolidColorBrush TopBarForeground = GetColor("878B91");
        public static readonly SolidColorBrush TopBarHoverBackground = GetColor("282B2E");
        public static readonly SolidColorBrush TopBarHoverForeground = GetColor("D1DDDE");
        public static readonly SolidColorBrush TopBarPreviewBackground = GetColor("282E32");
        public static readonly SolidColorBrush TopBarPreviewForeground = GetColor("FFFFFF");

        public static SolidColorBrush GetColor(string hex)
        {
            return (SolidColorBrush)new BrushConverter().ConvertFrom($"#{hex}");
        }
    }
}
