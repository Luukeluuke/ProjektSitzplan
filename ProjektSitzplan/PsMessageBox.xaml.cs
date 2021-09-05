using ProjektSitzplan.Design;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ProjektSitzplan
{
    public delegate void PsMessageBoxButtonPressedEventHandler(object source, PsMessagBoxEventArgs e);
    public class PsMessagBoxEventArgs : EventArgs
    {
        public PsMessagBoxEventArgs(PsMessageBox.EPsMessageBoxResult psMessageBoxResult)
        {
            PsMessageBoxButtonResult = psMessageBoxResult;
        }

        public PsMessageBox.EPsMessageBoxResult PsMessageBoxButtonResult { get; private set; }
    }

    /// <summary>
    /// Interaktionslogik für PsMessageBox.xaml
    /// </summary>
    public partial class PsMessageBox : Window
    {
        public event PsMessageBoxButtonPressedEventHandler OnPsMessageBoxButtonPressed;

        public EPsMessageBoxResult Result { get; private set; }

        public enum EPsMessageBoxResult
        {
            Yes,
            No,
            OK,
            WindowClosed
        }

        public enum EPsMessageBoxButtons
        {
            YesNo,
            OK
        }

        private Label[] ContentLabels;

        #region Constructors
        public PsMessageBox(EPsMessageBoxButtons psMessageBoxButtons)
        {
            InitializeComponent();

            switch (psMessageBoxButtons)
            {
                case EPsMessageBoxButtons.YesNo:
                    {
                        YesNoGrd.Visibility = Visibility.Visible;
                        break;
                    }
                case EPsMessageBoxButtons.OK:
                    {
                        OKGrd.Visibility = Visibility.Visible;
                        break;
                    }
            }

        }

        public PsMessageBox(string text, EPsMessageBoxButtons psMessageBoxButtons) : this(psMessageBoxButtons)
        {
            MessageTxbk.Text = text;
        }

        public PsMessageBox(string title, string text, EPsMessageBoxButtons psMessageBoxButtons) : this(text, psMessageBoxButtons)
        {
            SitzplanLbl.Content = $"{title}";
        }
        #endregion

        #region General Events
        #region TopBarButtons
        private void TopBarButton_Click(object sender, RoutedEventArgs e)
        {
            Button sBtn = Utility.GetButton(sender);

            switch (sBtn.Uid)
            {
                case "0": WindowState = WindowState.Minimized; return;
                case "2":
                    {
                        if (!(OnPsMessageBoxButtonPressed is null))
                        {
                            OnPsMessageBoxButtonPressed(sender, new PsMessagBoxEventArgs(EPsMessageBoxResult.WindowClosed));
                            Result = EPsMessageBoxResult.WindowClosed;
                        }
                        Close();

                        return;
                    }
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

        #region YesBtn
        private void YesBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!(OnPsMessageBoxButtonPressed is null))
            {
                OnPsMessageBoxButtonPressed(sender, new PsMessagBoxEventArgs(EPsMessageBoxResult.Yes));
            }
            Result = EPsMessageBoxResult.Yes;
            Close();
        }
        #endregion

        #region NoBtn
        private void NoBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!(OnPsMessageBoxButtonPressed is null))
            {
                OnPsMessageBoxButtonPressed(sender, new PsMessagBoxEventArgs(EPsMessageBoxResult.No));
            }
            Result = EPsMessageBoxResult.No;
            Close();
        }
        #endregion

        #region OKBtn
        private void OKBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!(OnPsMessageBoxButtonPressed is null))
            {
                OnPsMessageBoxButtonPressed(sender, new PsMessagBoxEventArgs(EPsMessageBoxResult.OK));
            }
            Result = EPsMessageBoxResult.OK;
            Close();
        }
        #endregion

        #region Window
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ContentLabels = new Label[] { YesLbl, NoLbl, OKLbl };
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
    }
}
