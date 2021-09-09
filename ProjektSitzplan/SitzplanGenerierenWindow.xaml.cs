using ProjektSitzplan.Design;
using ProjektSitzplan.Structures;
using System;
using System.Collections.Generic;
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

            for (int i = 0; i < maxTische; i++)
            {
                tischPlatzVerteilung[i] = 8;
            }
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
            ContentLabels = new Label[] { SGAbbrechenLbl, SGErstellenLbl, null, null, null, null, null, null, null, null, null, null, null, null, null, null };
            ContentPackIconsSets = new PackIconSet[] {
                new PackIconSet(SGAbbrechenPckIco, PackIconSet.EIconType.Content, PSColors.IconHoverRed, PSColors.IconPreviewRed),
                new PackIconSet(SGErstellenPckIco, PackIconSet.EIconType.Content, PSColors.IconHoverGreen, PSColors.IconPreviewGreen),
                
                
                new PackIconSet(SGTischPlusPckIco, PackIconSet.EIconType.Content, PSColors.ContentButtonHoverForeground, PSColors.ContentButtonPreviewForeground),
                new PackIconSet(SGTischMinusPckIco, PackIconSet.EIconType.Content, PSColors.ContentButtonHoverForeground, PSColors.ContentButtonPreviewForeground),



                new PackIconSet(SGTisch1PlusPckIco, PackIconSet.EIconType.Content, PSColors.ContentButtonHoverForeground, PSColors.ContentButtonPreviewForeground),
                new PackIconSet(SGTisch1MinusPckIco, PackIconSet.EIconType.Content, PSColors.ContentButtonHoverForeground, PSColors.ContentButtonPreviewForeground),

                new PackIconSet(SGTisch2PlusPckIco, PackIconSet.EIconType.Content, PSColors.ContentButtonHoverForeground, PSColors.ContentButtonPreviewForeground),
                new PackIconSet(SGTisch2MinusPckIco, PackIconSet.EIconType.Content, PSColors.ContentButtonHoverForeground, PSColors.ContentButtonPreviewForeground),

                new PackIconSet(SGTisch3PlusPckIco, PackIconSet.EIconType.Content, PSColors.ContentButtonHoverForeground, PSColors.ContentButtonPreviewForeground),
                new PackIconSet(SGTisch3MinusPckIco, PackIconSet.EIconType.Content, PSColors.ContentButtonHoverForeground, PSColors.ContentButtonPreviewForeground),

                new PackIconSet(SGTisch4PlusPckIco, PackIconSet.EIconType.Content, PSColors.ContentButtonHoverForeground, PSColors.ContentButtonPreviewForeground),
                new PackIconSet(SGTisch4MinusPckIco, PackIconSet.EIconType.Content, PSColors.ContentButtonHoverForeground, PSColors.ContentButtonPreviewForeground),

                new PackIconSet(SGTisch5PlusPckIco, PackIconSet.EIconType.Content, PSColors.ContentButtonHoverForeground, PSColors.ContentButtonPreviewForeground),
                new PackIconSet(SGTisch5MinusPckIco, PackIconSet.EIconType.Content, PSColors.ContentButtonHoverForeground, PSColors.ContentButtonPreviewForeground),

                new PackIconSet(SGTisch6PlusPckIco, PackIconSet.EIconType.Content, PSColors.ContentButtonHoverForeground, PSColors.ContentButtonPreviewForeground),
                new PackIconSet(SGTisch6MinusPckIco, PackIconSet.EIconType.Content, PSColors.ContentButtonHoverForeground, PSColors.ContentButtonPreviewForeground)
            };

            UpdateGenWindow();

            SGTisch1Grd.Visibility = Visibility.Visible;
        }
        #endregion

        #region ContentButtons
        private void ContentButton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Button sBtn = Utility.GetButton(sender);

            sBtn.Background = PSColors.ContentButtonHoverBackground;
            ContentLabels[Utility.GetUid(sBtn)]?.SetForeground(PSColors.ContentButtonHoverForeground);
            ContentPackIconsSets[Utility.GetUid(sBtn)]?.HandleColor(PackIconSet.EEventType.Enter);
        }

        private void ContentButton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Button sBtn = Utility.GetButton(sender);

            sBtn.Background = PSColors.ContentButtonBackground;
            ContentLabels[Utility.GetUid(sBtn)]?.SetForeground(PSColors.ContentButtonForeground);
            ContentPackIconsSets[Utility.GetUid(sBtn)]?.HandleColor(PackIconSet.EEventType.Leave);
        }

        private void ContentButton_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Button sBtn = Utility.GetButton(sender);

            sBtn.Background = PSColors.ContentButtonPreviewBackground;
            ContentLabels[Utility.GetUid(sBtn)]?.SetForeground(PSColors.ContentButtonPreviewForeground);
            ContentPackIconsSets[Utility.GetUid(sBtn)]?.HandleColor(PackIconSet.EEventType.PreviewDown);
        }

        private void ContentButton_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Button sBtn = Utility.GetButton(sender);

            sBtn.Background = PSColors.ContentButtonHoverBackground;
            ContentLabels[Utility.GetUid(sBtn)]?.SetForeground(PSColors.ContentButtonHoverForeground);
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
            SGTischEinstellungGrd.Visibility = IsImStandardModus() ? Visibility.Hidden : Visibility.Visible;

            SGTischAnzahlLbl.Content = tischAnzahl;

            SGPlätzeTisch1Lbl.Content = tischPlatzVerteilung[0];
            SGPlätzeTisch2Lbl.Content = tischPlatzVerteilung[1];
            SGPlätzeTisch3Lbl.Content = tischPlatzVerteilung[2];
            SGPlätzeTisch4Lbl.Content = tischPlatzVerteilung[3];
            SGPlätzeTisch5Lbl.Content = tischPlatzVerteilung[4];
            SGPlätzeTisch6Lbl.Content = tischPlatzVerteilung[5];

            SGTisch2Grd.Visibility = (tischAnzahl > 1) ? Visibility.Visible : Visibility.Hidden;
            SGTisch3Grd.Visibility = (tischAnzahl > 2) ? Visibility.Visible : Visibility.Hidden;
            SGTisch4Grd.Visibility = (tischAnzahl > 3) ? Visibility.Visible : Visibility.Hidden;
            SGTisch5Grd.Visibility = (tischAnzahl > 4) ? Visibility.Visible : Visibility.Hidden;
            SGTisch6Grd.Visibility = (tischAnzahl == 6) ? Visibility.Visible : Visibility.Hidden;

            SGTischMinusBtn.IsEnabled = tischAnzahl > 1;

            SGTisch1MinusBtn.IsEnabled = tischPlatzVerteilung[0] > 1;
            SGTisch2MinusBtn.IsEnabled = tischPlatzVerteilung[1] > 1;
            SGTisch3MinusBtn.IsEnabled = tischPlatzVerteilung[2] > 1;
            SGTisch4MinusBtn.IsEnabled = tischPlatzVerteilung[3] > 1;
            SGTisch5MinusBtn.IsEnabled = tischPlatzVerteilung[4] > 1;
            SGTisch6MinusBtn.IsEnabled = tischPlatzVerteilung[5] > 1;

            SGTischPlusBtn.IsEnabled = tischAnzahl < maxTische;

            SGTisch1PlusBtn.IsEnabled = tischPlatzVerteilung[0] < maxPlätze;
            SGTisch2PlusBtn.IsEnabled = tischPlatzVerteilung[1] < maxPlätze;
            SGTisch3PlusBtn.IsEnabled = tischPlatzVerteilung[2] < maxPlätze;
            SGTisch4PlusBtn.IsEnabled = tischPlatzVerteilung[3] < maxPlätze;
            SGTisch5PlusBtn.IsEnabled = tischPlatzVerteilung[4] < maxPlätze;
            SGTisch6PlusBtn.IsEnabled = tischPlatzVerteilung[5] < maxPlätze;

            Height = IsImStandardModus() ? 240 : 240 + ((tischAnzahl-1) * 30);
        }

        public bool IsImStandardModus()
        {
            return SGDefaultSettingsCBx.IsChecked.Value;
        }

        public Dictionary<int, int> tischPlatzVerteilung = new Dictionary<int, int>();

        #region Buttons
        private void SGErstellenBtn_Click(object sender, RoutedEventArgs e)
        {
            Generator = new SitzplanGenerator(Klasse, null, tischAnzahl, null, SGBerufCBx.IsChecked.Value, SGBetriebCBx.IsChecked.Value, SGGeschlechtCBx.IsChecked.Value, SchulBlock.Current, tischPlatzVerteilung);

            Erfolgreich = true;

            Close();
        }

        private void SGAbbrechenBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion

        private void SGSettingsCBx_Changed(object sender, RoutedEventArgs e)
        {
            if (IsLoaded)
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

        private void SGTischPlätzeTxbx_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            string text = tb.Text + e.Text;
            bool hand = !(int.TryParse(text, out int i) && i <= 8 && i != 0);
            e.Handled = hand;
        }


        #region Plus Minus Buttons
        private static readonly int maxTische = 6;

        private int tischAnzahl = 6;
        private void SGTischPlusBtn_Click(object sender, RoutedEventArgs e)
        {
            if (tischAnzahl < maxTische)
                tischAnzahl++;
            UpdateGenWindow();
        }

        private void SGTischMinusBtn_Click(object sender, RoutedEventArgs e)
        {
            if (tischAnzahl > 1)
                tischAnzahl--;
            UpdateGenWindow();
        }

        private static readonly int maxPlätze = 8;

        private void SGTisch1PlusBtn_Click(object sender, RoutedEventArgs e)
        {
            if (tischPlatzVerteilung[0] < maxPlätze)
                tischPlatzVerteilung[0]++;
            UpdateGenWindow();
        }

        private void SGTisch1MinusBtn_Click(object sender, RoutedEventArgs e)
        {
            if (tischPlatzVerteilung[0] > 1)
                tischPlatzVerteilung[0]--;
            UpdateGenWindow();
        }


        private void SGTisch2PlusBtn_Click(object sender, RoutedEventArgs e)
        {
            if (tischPlatzVerteilung[1] < maxPlätze)
                tischPlatzVerteilung[1]++;
            UpdateGenWindow();
        }

        private void SGTisch2MinusBtn_Click(object sender, RoutedEventArgs e)
        {
            if (tischPlatzVerteilung[1] > 1)
                tischPlatzVerteilung[1]--;
            UpdateGenWindow();
        }


        private void SGTisch3PlusBtn_Click(object sender, RoutedEventArgs e)
        {
            if (tischPlatzVerteilung[2] < maxPlätze)
                tischPlatzVerteilung[2]++;
            UpdateGenWindow();
        }

        private void SGTisch3MinusBtn_Click(object sender, RoutedEventArgs e)
        {
            if (tischPlatzVerteilung[2] > 1)
                tischPlatzVerteilung[2]--;
            UpdateGenWindow();
        }


        private void SGTisch4PlusBtn_Click(object sender, RoutedEventArgs e)
        {
            if (tischPlatzVerteilung[3] < maxPlätze)
                tischPlatzVerteilung[3]++;
            UpdateGenWindow();
        }

        private void SGTisch4MinusBtn_Click(object sender, RoutedEventArgs e)
        {
            if (tischPlatzVerteilung[3] > 1)
                tischPlatzVerteilung[3]--;
            UpdateGenWindow();
        }


        private void SGTisch5PlusBtn_Click(object sender, RoutedEventArgs e)
        {
            if (tischPlatzVerteilung[4] < maxPlätze)
                tischPlatzVerteilung[4]++;
            UpdateGenWindow();
        }

        private void SGTisch5MinusBtn_Click(object sender, RoutedEventArgs e)
        {
            if (tischPlatzVerteilung[4] > 1)
                tischPlatzVerteilung[4]--;
            UpdateGenWindow();
        }


        private void SGTisch6PlusBtn_Click(object sender, RoutedEventArgs e)
        {
            if (tischPlatzVerteilung[5] < maxPlätze)
                tischPlatzVerteilung[5]++;
            UpdateGenWindow();
        }

        private void SGTisch6MinusBtn_Click(object sender, RoutedEventArgs e)
        {
            if (tischPlatzVerteilung[5] > 1)
                tischPlatzVerteilung[5]--;
            UpdateGenWindow();
        }
        #endregion
    }
}
