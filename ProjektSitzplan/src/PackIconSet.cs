using MaterialDesignThemes.Wpf;
using ProjektSitzplan.Design;
using System;
using System.Windows.Media;

namespace ProjektSitzplan
{
    class PackIconSet
    {
        public PackIcon PackIcon { get; private set; }
        public EIconType IconType { get; private set; }
        public SolidColorBrush DefaultColor { get; private set; }
        public SolidColorBrush HoverColor { get; private set; }
        public SolidColorBrush PreviewColor { get; private set; }

        public enum EIconType
        {
            ContentButton,
            Content,
            MenuButton,
            Menu
        }

        public enum EEventType
        {
            Enter,
            Leave,
            PreviewDown,
            PreviewUp
        }

        public PackIconSet(PackIcon packIcon, EIconType iconType, SolidColorBrush hoverColor, SolidColorBrush previewColor)
        {
            PackIcon = packIcon;
            IconType = iconType;
            HoverColor = hoverColor;
            PreviewColor = previewColor;

            switch (iconType)
            {
                case EIconType.Content:
                case EIconType.ContentButton:
                    {
                        DefaultColor = PSColors.ContentForeground;
                        break;
                    }
                case EIconType.MenuButton:
                    {
                        throw new NotImplementedException();
                    }
                case EIconType.Menu:
                    {
                        throw new NotImplementedException();
                    }
            }
        }

        #region Public Methods
        public void HandleColor(EEventType eventType)
        {
            switch (eventType)
            {
                case EEventType.PreviewUp:
                case EEventType.Enter:
                    {
                        PackIcon.Foreground = HoverColor;
                        break;
                    }
                case EEventType.Leave:
                    {
                        PackIcon.Foreground = DefaultColor;
                        break;
                    }
                case EEventType.PreviewDown:
                    {
                        PackIcon.Foreground = PreviewColor;
                        break;
                    }
            }
        }
        #endregion
    }
}
