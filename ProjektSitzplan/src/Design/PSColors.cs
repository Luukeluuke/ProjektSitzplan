using System.Windows.Media;

namespace ProjektSitzplan.Design
{
    static class PSColors
    {
        public static readonly SolidColorBrush WindowBorderBrush = GetColor("202225");

        public static readonly SolidColorBrush TopBarBackground = GetColor("202225");
        public static readonly SolidColorBrush TopBarForeground = GetColor("878B91");
        public static readonly SolidColorBrush TopBarHoverBackground = GetColor("282B2E");
        public static readonly SolidColorBrush TopBarHoverForeground = GetColor("D1DDDE");
        public static readonly SolidColorBrush TopBarPreviewBackground = GetColor("282E32");
        public static readonly SolidColorBrush TopBarPreviewForeground = GetColor("FFFFFF");

        public static readonly SolidColorBrush TransitionStart = GetColor("2B2E33");
        public static readonly SolidColorBrush TransitionEnd = GetColor("32353B");

        public static readonly SolidColorBrush ContentBackground = GetColor("36393F");
        public static readonly SolidColorBrush ContentForeground = GetColor("878B91");
        public static readonly SolidColorBrush ContentHoverForeground = GetColor("D1DDDE");
        public static readonly SolidColorBrush ContentDivider = GetColor("42454A");
        public static readonly SolidColorBrush ContentControlBorder = GetColor("32353B");
        public static readonly SolidColorBrush ContentDataGridRowHover = GetColor("32353B");
        public static readonly SolidColorBrush ContentDataGridRowBorderHover = GetColor("32353B");
        public static readonly SolidColorBrush ContentDataGridRowBorderSelectedBackground = GetColor("2B2E33");
        public static readonly SolidColorBrush ContentDataGridRowBorderSelectedBorder = GetColor("2B2E33");
        public static readonly SolidColorBrush ContentDataGridRowBorderSelectedForeground = GetColor("FFFFFF");
        public static readonly SolidColorBrush ContentDataGridSelectedCellForeground = GetColor("FFFFFF");
        public static readonly SolidColorBrush ContentButtonBackground = GetColor("32353B");
        public static readonly SolidColorBrush ContentButtonForeground = GetColor("72767D");
        public static readonly SolidColorBrush ContentButtonHoverBackground = GetColor("3B3E45");
        public static readonly SolidColorBrush ContentButtonHoverForeground = GetColor("D1DDDE");
        public static readonly SolidColorBrush ContentButtonPreviewBackground = GetColor("43474F");
        public static readonly SolidColorBrush ContentButtonPreviewForeground = GetColor("FFFFFF");
        public static readonly SolidColorBrush ContentTextBoxSelectedForeground = GetColor("FFFFFF");
        public static readonly SolidColorBrush ContentTextBoxSelectedBorder = GetColor("D1DDDE");
        public static readonly SolidColorBrush ContentTextBoxSelectedCaret = GetColor("D1DDDE");

        public static readonly SolidColorBrush IconHoverRed = GetColor("C80004");
        public static readonly SolidColorBrush IconPreviewRed = GetColor("DC0005");
        public static readonly SolidColorBrush IconHoverGreen = GetColor("007300");
        public static readonly SolidColorBrush IconPreviewGreen = GetColor("008000");

        public static readonly SolidColorBrush MenuBackground = GetColor("2F3136");
        public static readonly SolidColorBrush MenuForeground = GetColor("D7D8D9");
        public static readonly SolidColorBrush MenuShortcut = GetColor("878B91");
        public static readonly SolidColorBrush MenuComment = GetColor("878B91");
        public static readonly SolidColorBrush MenuFreeButtonBackground = GetColor("36393F");
        public static readonly SolidColorBrush MenuFreeButtonForeground = GetColor("D7D8D9");
        public static readonly SolidColorBrush SubMenuButtonHover = GetColor("292B2F");

        public static readonly SolidColorBrush ToolTipBackground = GetColor("18191C");
        public static readonly SolidColorBrush ToolTipForeground = GetColor("D7D8D9");

        public static SolidColorBrush GetColor(string hex)
        {
            return (SolidColorBrush)new BrushConverter().ConvertFrom($"#{hex}");
        }
    }
}
