using ProjektSitzplan.Design;
using ProjektSitzplan.Structures;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ProjektSitzplan
{
    /// <summary>
    /// Interaktionslogik für SitzplanGenerierenWindow.xaml
    /// </summary>
    public partial class SitzplanGenerierenWindow : Window
    {
        public Label[] ContentLabels { get; private set; }
        internal PackIconSet[] ContentPackIconsSets { get; private set; }

        public bool Erfolgreich { get; private set; } = false;
        public SitzplanGenerator Generator { get; private set; }

        public SchulKlasse Klasse { get; private set; }

        //TODO: Beim aktivieren der erweiterten einstellungen sollte eine meldung ausgegeben werden was das genau heißt


        #region Constructor
        public SitzplanGenerierenWindow(SchulKlasse klasse)
        {
            InitializeComponent();

            Klasse = klasse;

            UpdateGenWindow();
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
            ContentLabels = new Label[] { SGAbbrechenLbl, SGErstellenLbl };
            ContentPackIconsSets = new PackIconSet[] {
                new PackIconSet(SGAbbrechenPckIco, PackIconSet.EIconType.Content, PSColors.IconHoverRed, PSColors.IconPreviewRed),
                new PackIconSet(SGErstellenPckIco, PackIconSet.EIconType.Content, PSColors.IconHoverGreen, PSColors.IconPreviewGreen)};

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

        private void UpdateGenWindow()
        {
            SGAdvancedGrd.Visibility = IsImErweitertenModus() ? Visibility.Visible : Visibility.Hidden;
            SGNameTxbx.Text = "";
            SGTischZahlTxbx.Text = "";
            SGSeedTxbx.Text = "";
        }

        public bool IsImErweitertenModus()
        {
            return SGAdvancedSettingsCBx.IsChecked.Value;
        }

        #region Buttons
        private void SGErstellenBtn_Click(object sender, RoutedEventArgs e)
        {
            string name = SGNameTxbx.Text.Trim();

            int tischAnzahl = int.TryParse(SGTischZahlTxbx.Text, out int tryTischAnzahl) ? tryTischAnzahl : 6;

            int? seed = null;
            if (int.TryParse(SGSeedTxbx.Text, out int trySeed)) seed = trySeed;

            SchulBlock blockType = IsImErweitertenModus() ? SchulBlock.Custom : SchulBlock.Current;

            Generator = new SitzplanGenerator(Klasse, name, tischAnzahl, seed, SGBerufCBx.IsChecked.Value, SGBetriebCBx.IsChecked.Value, SGGeschlechtCBx.IsChecked.Value, blockType);

            Erfolgreich = true;
            Close();
        }

        private void SGAbbrechenBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion

        private void SGAdvancedSettingsCBx_Changed(object sender, RoutedEventArgs e)
        {
            UpdateGenWindow();
        }

        private void SGTischZahlTxbx_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            string text = tb.Text + e.Text;
            bool hand = !(int.TryParse(text, out int i) && i <= 6 && i != 0);
            e.Handled = hand;
        }

        private void SGSeedTxbx_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            string text = tb.Text + e.Text;
            bool hand = !int.TryParse(text, out _);
            e.Handled = hand;
        }
    }
}
