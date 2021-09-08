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
    /// Interaction logic for SchülerAuswahlDialog.xaml
    /// </summary>
    public partial class SchülerAuswahlDialog : Window
    {
        public Label[] ContentLabels { get; private set; }
        internal PackIconSet[] ContentPackIconsSets { get; private set; }

        public bool Canceled = true;



        public List<Schüler> Ausgewählt = new List<Schüler>();
        public List<Schüler> NichtAusgewählt = new List<Schüler>();

        #region Constructor
        public SchülerAuswahlDialog(string titel, List<Schüler> schüler, bool istAusgewählt) : this(titel, schüler, istAusgewählt, "Ausgewählt", "Nicht Ausgewählt"){ }
        public SchülerAuswahlDialog(string titel, List<Schüler> schüler, bool istAusgewählt, string ausgewähltTitel, string nichtAusgewähltTitel)
        {
            InitializeComponent();

            SDTitelLbl.Content = titel;
            SDAusgewähltLbl.Content = ausgewähltTitel;
            SDNichtAusgewähltLbl.Content = nichtAusgewähltTitel;

            if (istAusgewählt)
            {
                Ausgewählt = schüler;
            } else
            {
                NichtAusgewählt = schüler;
            }

            SDAusgewähltDtGrd.ItemsSource = Ausgewählt;
            SDNichtAusgewähltDtGrd.ItemsSource = NichtAusgewählt;
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
            ContentLabels = new Label[] { SDAbbrechenLbl, SDBestätigenLbl };
            ContentPackIconsSets = new PackIconSet[] {
                new PackIconSet(SDAbbrechenPckIco, PackIconSet.EIconType.Content, PSColors.IconHoverRed, PSColors.IconPreviewRed),
                new PackIconSet(SDBestätigenPckIco, PackIconSet.EIconType.Content, PSColors.IconHoverGreen, PSColors.IconPreviewGreen)};
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

        private void Aktuallisieren()
        {
            SDAusgewähltDtGrd.SelectedIndex = -1;
            SDNichtAusgewähltDtGrd.SelectedIndex = -1;

            SDAusgewähltDtGrd.ItemsSource = null;
            SDAusgewähltDtGrd.ItemsSource = Ausgewählt;

            SDNichtAusgewähltDtGrd.ItemsSource = null;
            SDNichtAusgewähltDtGrd.ItemsSource = NichtAusgewählt;
        }



        private void SDAusgewähltDtGrd_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            Schüler selected = (Schüler)((DataGrid)sender).SelectedItem;
            if (selected != null)
            {
                Ausgewählt.Remove(selected);
                NichtAusgewählt.Add(selected);

                Aktuallisieren();
            }
        }

        private void SDNichtAusgewähltDtGrd_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            Schüler selected = (Schüler)((DataGrid)sender).SelectedItem;
            if (selected != null)
            {
                NichtAusgewählt.Remove(selected);
                Ausgewählt.Add(selected);

                Aktuallisieren();
            }
        }



        private void SDAbbrechenBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SDBestätigenBtn_Click(object sender, RoutedEventArgs e)
        {
            Canceled = false;
            Close();
        }
    }
}
