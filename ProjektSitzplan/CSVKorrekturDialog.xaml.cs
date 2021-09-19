using ProjektSitzplan.Design;
using ProjektSitzplan.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ProjektSitzplan
{
    /// <summary>
    /// Interaction logic for CSVKorrekturDialog.xaml
    /// </summary>
    public partial class CSVKorrekturDialog : Window
    {
        private Label[] ContentLabels;
        public enum DialogEingabe
        {
            Erfolgreich,
            Abbrechen
        }

        public Schüler Schüler { get; private set; } = null;

        public string Vorname { get; private set; } = "";
        public string Nachname { get; private set; } = "";
        public string Betrieb { get; private set; } = "";

        public Person.EBeruf Beruf { get; private set; }
        public Person.EGeschlecht Geschlecht { get; private set; }

        public DialogEingabe Eingabe = DialogEingabe.Abbrechen;

        public CSVKorrekturDialog(string csvSchüler, Dictionary<string, int> anordnung)
        {
            InitializeComponent();

            //Format Vorname,Nachname,Beruf,Betrieb,Geschlecht

            string[] split = csvSchüler.Split(',').Select(s => s.Trim()).ToArray();

            GeschlechtCb.SelectedIndex = -1;
            BerufCb.SelectedIndex = -1;

            if (anordnung["geschlecht"] < split.Length)
            {
                Person.EGeschlecht geschlecht;
                if (Enum.TryParse(split[anordnung["geschlecht"]].Replace(" ", ""), true, out geschlecht))
                {
                    Geschlecht = geschlecht;
                    GeschlechtCb.SelectedIndex = (int)Geschlecht;
                }
            }

            if (anordnung["beruf"] < split.Length)
            {
                Person.EBeruf beruf;
                if (Enum.TryParse(split[anordnung["beruf"]].Replace(" ", ""), true, out beruf))
                {
                    Beruf = beruf;
                    BerufCb.SelectedIndex = (int)Beruf;
                }

            }

            if (anordnung["vorname"] < split.Length)
                Vorname = split[anordnung["vorname"]];

            if (anordnung["nachname"] < split.Length)
                Nachname = split[anordnung["nachname"]];

            if (anordnung["betrieb"] < split.Length)
                Betrieb = split[anordnung["betrieb"]];

            VornameTxbx.Text = Vorname;
            NachnameTxbx.Text = Nachname;
            BetriebTxbx.Text = Betrieb;

        }

        #region Window
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ContentLabels = new Label[] { AbbrechenLbl, ÜbernehmenLbl };
        }
        #endregion

        #region General Events
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

        #region ContentComboBox
        private void ContentComboBox_GotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            (sender as ComboBox).Foreground = PSColors.ContentTextBoxSelectedForeground;
        }

        private void ContentComboBox_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            (sender as ComboBox).Foreground = PSColors.ContentForeground;
        }
        #endregion
        #endregion

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

        public bool KannFortfahren()
        {
            Vorname = VornameTxbx.Text;
            Nachname = NachnameTxbx.Text;
            Betrieb = BetriebTxbx.Text;

            if (string.IsNullOrWhiteSpace(Vorname) || string.IsNullOrWhiteSpace(Nachname) || string.IsNullOrWhiteSpace(Betrieb))
            {
                ErrorHandler.ZeigeFehler(ErrorHandler.ERR_SH_PflichtfelderNichtAusgefüllt);
                return false;
            }

            try
            {
                Geschlecht = (Person.EGeschlecht)Enum.Parse(typeof(Person.EGeschlecht), GeschlechtCb.Text, true);
                Beruf = (Person.EBeruf)Enum.Parse(typeof(Person.EBeruf), BerufCb.Text.Replace(" ", ""), true);
            }
            catch (ArgumentException)
            {
                ErrorHandler.ZeigeFehler(ErrorHandler.ERR_SH_PflichtfelderNichtAusgefüllt);
                return false;
            }

            bool verkürzt = SchülerVerkürztCBx.IsChecked.Value;

            Schüler = new Schüler(new Person(Vorname, Nachname, Geschlecht, Beruf), Betrieb, verkürzt);
            return true;
        }

        private void AbbrechenBtn_Click(object sender, RoutedEventArgs e)
        {
            Eingabe = DialogEingabe.Abbrechen;
            Close();
        }

        private void ÜbernehmenBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!KannFortfahren())
            {
                return;
            }

            Eingabe = DialogEingabe.Erfolgreich;
            Close();
        }

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
