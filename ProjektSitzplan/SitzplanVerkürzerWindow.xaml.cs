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
    /// Interaktionslogik für SitzplanVerkürzerWindow.xaml
    /// </summary>
    public partial class SitzplanVerkürzerWindow : Window
    {
        public Label[] ContentLabels { get; private set; }
        internal PackIconSet[] ContentPackIconsSets { get; private set; }

        public bool Canceled = true;

        public List<Schüler> Verkürzer;
        public List<Schüler> NichtVerkürzer;


        #region Constructor
        public SitzplanVerkürzerWindow(List<Schüler> schüler)
        {
            InitializeComponent();

            Verkürzer = schüler;
            NichtVerkürzer = new List<Schüler>();

            SVVerkürzerDtGrd.ItemsSource = Verkürzer;
            SVNichtVerkürzerDtGrd.ItemsSource = NichtVerkürzer;

            new PsMessageBox("Achtung", $"Für den letzten Block wurden {Verkürzer.Count} Schüler gefunden die verkürzen.\nBitte überprüfen Korrektheit überprüfen.\nBeim Generieren des Sitzplans werden nicht berücksichtigt.\nLinks sind die Verkürzer, rechts die Nicht Verkürzer.", PsMessageBox.EPsMessageBoxButtons.OK).ShowDialog();
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
            ContentLabels = new Label[] { SVAbbrechenLbl, SVErstellenLbl };
            ContentPackIconsSets = new PackIconSet[] {
                new PackIconSet(SVAbbrechenPckIco, PackIconSet.EIconType.Content, PSColors.IconHoverRed, PSColors.IconPreviewRed),
                new PackIconSet(SVErstellenPckIco, PackIconSet.EIconType.Content, PSColors.IconHoverGreen, PSColors.IconPreviewGreen)};
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

        private void SVVerkürzerDtGrd_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            Schüler selected = (Schüler)((DataGrid)sender).SelectedItem;
            if (selected != null)
            {
                Verkürzer.Remove(selected);
                NichtVerkürzer.Add(selected);

                AuswahlZurücksetzten();
                return;
            }
        }

        private void SVNichtVerkürzerDtGrd_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            Schüler selected = (Schüler)((DataGrid)sender).SelectedItem;
            if (selected != null)
            {
                NichtVerkürzer.Remove(selected);
                Verkürzer.Add(selected);

                AuswahlZurücksetzten();
                return;
            }
        }

        private void AuswahlZurücksetzten()
        {
            SVVerkürzerDtGrd.SelectedIndex = -1;
            SVNichtVerkürzerDtGrd.SelectedIndex = -1;

            SVVerkürzerDtGrd.ItemsSource = null;
            SVVerkürzerDtGrd.ItemsSource = Verkürzer;

            SVNichtVerkürzerDtGrd.ItemsSource = null;
            SVNichtVerkürzerDtGrd.ItemsSource = NichtVerkürzer;
        }

        private void SVAbbrechenBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SVErstellenBtn_Click(object sender, RoutedEventArgs e)
        {
            Canceled = false;
            Close();
        }

    }
}
