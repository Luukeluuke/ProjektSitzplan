using System.Windows;
using System.Windows.Media;

namespace ProjektSitzplan
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static SolidColorBrush DarkBlue = GetColor("212A49");
        private static SolidColorBrush Blue = GetColor("223055");
        private static SolidColorBrush Gray = GetColor("3E4866");
        private static SolidColorBrush LightGray = GetColor("4E5A80");
        private static SolidColorBrush White = GetColor("FBFAFF");
        private static SolidColorBrush Turqouise = GetColor("20E9B5");

        private static SolidColorBrush LightBlue = GetColor("253154");
        private static SolidColorBrush VeryLightBlue = GetColor("28355C");

        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            Test.TestFunction ();
        }

        #region Window
        private void Window_Activated(object sender, System.EventArgs e)
        {
            WindowBorder.BorderBrush = Gray;
        }

        private void Window_Deactivated(object sender, System.EventArgs e)
        {
            WindowBorder.BorderBrush = LightGray;
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
