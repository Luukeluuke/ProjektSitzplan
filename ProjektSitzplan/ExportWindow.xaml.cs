using Microsoft.Win32;
using ProjektSitzplan.Design;
using ProjektSitzplan.Structures;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ProjektSitzplan
{
    /// <summary>
    /// Interaktionslogik für ExportWindow.xaml
    /// </summary>
    public partial class ExportWindow : Window, INotifyPropertyChanged
    {
        private Label[] ContentLabels;
        private PackIconSet[] ContentPackIconsSets;

        public List<SchulKlasse> GefundeneKlassen { get; } = DataHandler.SchulKlassen;
        public SchulKlasse ausgewählteKlasse = null;
        public SchulKlasse AusgewählteKlasse
        {
            get
            {
                return ausgewählteKlasse;
            }
            set
            {
                Set(ref ausgewählteKlasse, value);
            }
        }

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

        #region Radio Button
        public enum EXType
        {
            JSON,
            PDF,

            None
        }

        public EXType SelectedOption
        {
            get
            {
                if (EXPDFRad.IsChecked.Value)
                    return EXType.PDF;

                if (EXJSONRad.IsChecked.Value)
                    return EXType.JSON;

                return EXType.None;
            }
        }
        #endregion

        #region Constructors
        public ExportWindow()
        {
            InitializeComponent();

            EXGefundenKlassenDtGrd.ItemsSource = GefundeneKlassen;
        }

        //TODO: constructor mit schulklasse schon drin?
        #endregion

        #region General Events
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


        #region Window
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ContentLabels = new Label[] { EXAbbrechenLbl, EXExportLbl };
            ContentPackIconsSets = new PackIconSet[] {
                new PackIconSet(EXExitPckIco, PackIconSet.EIconType.Content, PSColors.IconHoverRed, PSColors.IconPreviewRed),
                new PackIconSet(EXExportPckIco, PackIconSet.EIconType.Content, PSColors.IconHoverGreen, PSColors.IconPreviewGreen)};
        }
        #endregion


        #region ContentButtons
        private void ContentButton_MouseEnter(object sender, MouseEventArgs e)
        {
            Button sBtn = Utility.GetButton(sender);

            sBtn.Background = PSColors.ContentButtonHoverBackground;
            ContentLabels[Utility.GetUid(sBtn)].Foreground = PSColors.ContentButtonHoverForeground;

            ContentPackIconsSets[Utility.GetUid(sBtn)]?.HandleColor(PackIconSet.EEventType.Enter);
        }

        private void ContentButton_MouseLeave(object sender, MouseEventArgs e)
        {
            Button sBtn = Utility.GetButton(sender);

            sBtn.Background = PSColors.ContentButtonBackground;
            ContentLabels[Utility.GetUid(sBtn)].Foreground = PSColors.ContentButtonForeground;
            ContentPackIconsSets[Utility.GetUid(sBtn)]?.HandleColor(PackIconSet.EEventType.Leave);
        }

        private void ContentButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Button sBtn = Utility.GetButton(sender);

            sBtn.Background = PSColors.ContentButtonPreviewBackground;
            ContentLabels[Utility.GetUid(sBtn)].Foreground = PSColors.ContentButtonPreviewForeground;
            ContentPackIconsSets[Utility.GetUid(sBtn)]?.HandleColor(PackIconSet.EEventType.PreviewDown);
        }

        private void ContentButton_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Button sBtn = Utility.GetButton(sender);

            sBtn.Background = PSColors.ContentButtonHoverBackground;
            ContentLabels[Utility.GetUid(sBtn)].Foreground = PSColors.ContentButtonHoverForeground;
            ContentPackIconsSets[Utility.GetUid(sBtn)]?.HandleColor(PackIconSet.EEventType.PreviewUp);
        }
        #endregion

        public bool KannExportieren()
        {
            return (EXGefundenKlassenDtGrd.SelectedIndex > -1) && SelectedOption != EXType.None;
        }

        private void UpdateExportBtn()
        {
            EXExportBtn.IsEnabled = KannExportieren();
        }

        private void EXGefundenKlassenDtGrd_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            UpdateExportBtn();
        }

        private void EXAbbrechenBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void EXExportBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!KannExportieren())
                return;

            switch (SelectedOption)
            {
                case EXType.JSON:
                    {
                        SchulKlasse klasse = (SchulKlasse)EXGefundenKlassenDtGrd.SelectedItem;

                        if (klasse == null)
                        {
                            ErrorHandler.ZeigeFehler(ErrorHandler.ERR_EX_KeineKlasseAusgewählt);
                            return;
                        }

                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.Filter = "Json files (*.json)|*.json";
                        saveFileDialog.FileName = $"{klasse.Name}.json";
                        saveFileDialog.DefaultExt = ".json";
                        saveFileDialog.InitialDirectory = $@"{Environment.CurrentDirectory}\SchulKlassen";


                        if (!saveFileDialog.ShowDialog().Value)
                            return;

                        klasse.AlsDateiSpeichern(saveFileDialog.FileName);

                        EXErfolgreich($"Die Klasse \"{klasse.Name}\" wurde unter\n{saveFileDialog.FileName}\nabgelegt!");
                        return;
                    }
                case EXType.PDF:
                    {
                        //TODO: Implement PDF export


                        SchulKlasse klasse = (SchulKlasse)EXGefundenKlassenDtGrd.SelectedItem;

                        if (klasse == null)
                        {
                            ErrorHandler.ZeigeFehler(ErrorHandler.ERR_EX_KeineKlasseAusgewählt);
                            return;
                        }

                        if (klasse.Sitzpläne.Count == 0)
                        {
                            ErrorHandler.ZeigeFehler(ErrorHandler.ERR_EX_KeinSitzplanInKlasse, klasse.Name, "");
                            return;
                        }

                        ExportWindowPDF pdfExportWindow = new ExportWindowPDF(klasse);
                        if (pdfExportWindow.ShowDialog().Value)
                        {
                            EXErfolgreich($"Der Sitzplan \"{pdfExportWindow.exportPath}\" aus der Klasse\n{klasse.Name}\nWurde als \"{pdfExportWindow.exportSitzplan}\" exportiert!");
                        }
                        return;
                    }
                case EXType.None:
                    {
                        //TODO: IDK another this should never happen thing :D
                        return;
                    }
            }
        }

        private void EXErfolgreich(string text)
        {
            new PsMessageBox("Erfolgreich exportiert", text, PsMessageBox.EPsMessageBoxButtons.OK).ShowDialog();
            Close();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            UpdateExportBtn();
        }
    }
}