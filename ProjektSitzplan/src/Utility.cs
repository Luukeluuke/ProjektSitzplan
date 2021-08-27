using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ProjektSitzplan
{
    public static class Utility
    {
        public enum EImageType
        {
            png,
            jpg
        }

        public static Button GetButton(object sender)
        {
            return sender as Button;
        }

        public static short GetUid(Button sBtn)
        {
            return short.Parse(sBtn.Uid);
        }

        public static Image GetImage(string imageFilename, EImageType imageType = EImageType.png)
        {
            return new Image { Source = new BitmapImage(new Uri($"pack://application:,,,/ProjektSitzplan;component/src/Design/Images/{imageFilename}.{imageType.ToString()}", UriKind.Absolute)), IsHitTestVisible = false };
        }

        public static string GetTopBarImagePrefix(Button sBtn, Window thisWindow)
        {
            switch (sBtn.Uid)
            {
                case "0":
                    {
                        return "Minimize";
                    }
                case "1":
                    {
                        return thisWindow.WindowState.Equals(WindowState.Normal) || thisWindow.WindowState.Equals(WindowState.Minimized) ? "Restore1" : "Restore2";
                    }
                case "2":
                    {
                        return "Close";
                    }
            }

            throw new NotImplementedException();
        }
    }
}
