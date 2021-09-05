using ProjektSitzplan.Design;
using ProjektSitzplan.Structures;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProjektSitzplan
{
    /// <summary>
    /// Interaktionslogik für SitzplanVerkürzerWindow.xaml
    /// </summary>
    public partial class SitzplanVerkürzerWindow : Window
    {
        public Label[] ContentLabels { get; private set; }
        internal PackIconSet[] ContentPackIconsSets { get; private set; }

        private List<Schüler> Verkürzen;

        public List<Schüler> NichtVerkürzen;


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


        #region Constructor
        public SitzplanVerkürzerWindow(List<Schüler> schüler)
        {
            InitializeComponent();

            Verkürzen = schüler;
        }
        #endregion

        #region TitleBarGrid
        private void TitleBarGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            {
                try
                {
                    DragMove();
                }
                catch (InvalidOperationException)
                {
                    //Passiert wenn man die rechte Maustaste drückt
                }
            }

        }
        #endregion

        #region TopBarButtons
        private void TopBarButton_Click(object sender, RoutedEventArgs e)
        {
            Button sBtn = Utility.GetButton(sender);

            switch (sBtn.Uid)
            {
                case "0": WindowState = WindowState.Minimized; return;
                case "2": Close(); return;
            }
        }

        private void TopBarButton_MouseEnter(object sender, MouseEventArgs e)
        {
            Button sBtn = Utility.GetButton(sender);

            sBtn.Background = PSColors.TopBarHoverBackground;
            sBtn.Content = Utility.GetImage($"{Utility.GetTopBarImagePrefix(sBtn, this)}DCDDDE");
        }

        private void TopBarButton_MouseLeave(object sender, MouseEventArgs e)
        {
            Button sBtn = Utility.GetButton(sender);

            sBtn.Background = Brushes.Transparent;
            sBtn.Content = Utility.GetImage($"{Utility.GetTopBarImagePrefix(sBtn, this)}B9BBBE");
        }

        private void TopBarButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Button sBtn = Utility.GetButton(sender);

            sBtn.Background = PSColors.TopBarPreviewBackground;
            sBtn.Content = Utility.GetImage($"{Utility.GetTopBarImagePrefix(sBtn, this)}FFFFFF");
        }

        private void TopBarButton_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Button sBtn = Utility.GetButton(sender);

            sBtn.Background = PSColors.TopBarHoverBackground;
            sBtn.Content = Utility.GetImage($"{Utility.GetTopBarImagePrefix(sBtn, this)}DCDDDE");
        }
        #endregion

        #region Window
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            /*
            ContentLabels = new Label[] { SGAbbrechenLbl, SGErstellenLbl };
            ContentPackIconsSets = new PackIconSet[] {
                new PackIconSet(SGAbbrechenPckIco, PackIconSet.EIconType.Content, PSColors.IconHoverRed, PSColors.IconPreviewRed),
                new PackIconSet(SGErstellenPckIco, PackIconSet.EIconType.Content, PSColors.IconHoverGreen, PSColors.IconPreviewGreen)};
            */
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
    }
}
