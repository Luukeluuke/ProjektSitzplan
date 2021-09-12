using ProjektSitzplan.Design;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for CSVImportErrDialog.xaml
    /// </summary>
    public partial class CSVImportErrDialog : Window
    {
        private Label[] ContentLabels;

        public CSVImportErrDialog(string text)
        {
            InitializeComponent();

            string[] lines = text.Split('\n');

            Height = 40 + (30 * lines.Length);

            MessageTxbk.Text = string.Join("\n", text);
        }


        #region General Events
        #region TopBarButtons
        private void TopBarButton_Click(object sender, RoutedEventArgs e)
        {
            Button sBtn = Utility.GetButton(sender);

            if (sBtn.Uid == "2")
            {
                Eingabe = DialogEingabe.Abbrechen;
                Close();

                return;
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

        #region ContentButtons
        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Button sBtn = Utility.GetButton(sender);

            sBtn.Background = PSColors.ContentButtonHoverBackground;
            ContentLabels[Utility.GetUid(sBtn)].Foreground = PSColors.ContentButtonHoverForeground;
        }

        private void Button_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Button sBtn = Utility.GetButton(sender);

            sBtn.Background = PSColors.ContentButtonBackground;
            ContentLabels[Utility.GetUid(sBtn)].Foreground = PSColors.ContentButtonForeground;
        }

        private void Button_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Button sBtn = Utility.GetButton(sender);

            sBtn.Background = PSColors.ContentButtonPreviewBackground;
            ContentLabels[Utility.GetUid(sBtn)].Foreground = PSColors.ContentButtonPreviewForeground;
        }

        private void Button_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Button sBtn = Utility.GetButton(sender);

            sBtn.Background = PSColors.ContentButtonHoverBackground;
            ContentLabels[Utility.GetUid(sBtn)].Foreground = PSColors.ContentButtonHoverForeground;
        }
        #endregion
        #endregion

        #region Window
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ContentLabels = new Label[] { FortfahrenLbl, KorrigierenLbl, AbbrechenLbl };
        }
        #endregion

        #region TitleBarGrid
        private void TitleBarGrid_MouseDown(object sender, MouseButtonEventArgs e)
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
        #endregion



        public enum DialogEingabe
        {
            Abbrechen,
            Fortfahren,
            Korrigieren
        }

        public DialogEingabe Eingabe { get; private set; } = DialogEingabe.Abbrechen;

        private void AbbrechenBtn_Click(object sender, RoutedEventArgs e)
        {
            Eingabe = DialogEingabe.Abbrechen;
            Close();
        }

        private void FortfahrenBtn_Click(object sender, RoutedEventArgs e)
        {
            Eingabe = DialogEingabe.Fortfahren;
            Close();
        }

        private void KorrigierenBtn_Click(object sender, RoutedEventArgs e)
        {
            Eingabe = DialogEingabe.Korrigieren;
            Close();
        }
    }
}
