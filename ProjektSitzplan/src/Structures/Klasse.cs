using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ProjektSitzplan.PsMessageBox;

namespace ProjektSitzplan.Structures
{
    public class SchulKlasse
    {
        public static readonly int MaxSchüler = 48;

        public string Name { get; private set; }
        public readonly List<Schüler> SchuelerListe;
        public readonly List<Sitzplan> Sitzplaene = new List<Sitzplan>();

        [JsonIgnore]
        public int AnzahlSchüler { get => SchuelerListe.Count; }

        [JsonIgnore]
        public string ToolTipÜbersicht { get => ToToolTipString(); }

        #region Constructors
        public SchulKlasse(string name, List<Schüler> schuelerListe = null)
        {
            Name = name;
            SchuelerListe = (schuelerListe == null) ? new List<Schüler>() : schuelerListe;
        }

        [JsonConstructor]
        public SchulKlasse(string name, List<Schüler> schuelerListe, List<Sitzplan> sitzplaene) : this(name)
        {
            SchuelerListe = schuelerListe;
            Sitzplaene = sitzplaene;

            foreach (Sitzplan sitzplan in Sitzplaene)
            {
                sitzplan.LadeSchülerListeAusIdListe(this);
            }
        }

        public SchulKlasse(SchulKlasse klasse) : this(klasse.Name, new List<Schüler>(klasse.SchuelerListe), new List<Sitzplan>(klasse.Sitzplaene)) { }
        #endregion

        #region Public Methods

        #region Sitzpläne
        public void SitzplanHinzufügen(Sitzplan sitzplan)
        {
            if (sitzplan == null || Sitzplaene.Any(s => s.Name.Equals(sitzplan.Name)))
            {
                new PsMessageBox("Achtung", $"Der Sitzplan \"{sitzplan.Name}\" konnte nicht hinzugefügt werden.\nEntweder ist er Fehlerhaft, oder es exisitert bereits ein Sitzplan mit diesem Namen.", EPsMessageBoxButtons.OK).ShowDialog();
            }

            Sitzplaene.Add(sitzplan);
        }

        public void SitzplanEntfernen(Sitzplan sitzplan)
        {
            if (sitzplan == null || !Sitzplaene.Contains(sitzplan))
            {
                new PsMessageBox("Achtung", $"Der Sitzplan \"{sitzplan.Name}\" konnte nicht entfernt werden.\nEr hat uns bereits verlassen (Existiert schon nicht mehr).", EPsMessageBoxButtons.OK).ShowDialog();
            }


            Sitzplaene.Remove(sitzplan);
        }

        public Sitzplan ErstelleSitzplanDialog()
        {
            SitzplanGenerierenWindow erstellDialog = new SitzplanGenerierenWindow(this);

            erstellDialog.ShowDialog();

            if (!erstellDialog.Erfolgreich)
            {
                return null;
            }

            return ErstelleSitzplan(erstellDialog.Generator);
        }


        public Sitzplan ErstelleSitzplan()
        {
            return ErstelleSitzplan(new SitzplanGenerator(this));
        }

        public Sitzplan ErstelleSitzplan(SitzplanGenerator sitzplanGenerator)
        {
            Sitzplan sitzplan = new Sitzplan(sitzplanGenerator);

            if (!sitzplan.ErfolgreichGeneriert)
            {
                new PsMessageBox("Sitzplan", "Der Sitzplan konnte nicht generiert werden.", PsMessageBox.EPsMessageBoxButtons.OK).ShowDialog();

                return null;
            }

            new PsMessageBox("Sitzplan", $"Der Sitzplan \"{sitzplan.Name}\" wurde erfolgreich für \"{Name}\" generiert.", PsMessageBox.EPsMessageBoxButtons.OK).ShowDialog();

            SitzplanHinzufügen(sitzplan);

            Speichern();
            return sitzplan;
        }

        public SchulBlock FreierBlock()
        {
            SchulBlock schulBlock = SchulBlock.Current;
            foreach (SchulBlock block in Enum.GetValues(typeof(SchulBlock)))
            {
                schulBlock = block;
                if (!Sitzplaene.Any(sitzplan => sitzplan.BlockSitzplan.Equals(block))) break;
            }

            return schulBlock.Equals(SchulBlock.Current) ? SchulBlock.Custom : schulBlock;
        }
        #endregion

        #region Schüler
        public void SchülerHinzufügen(Schüler schüler)
        {
            if (SchuelerListe.Count >= MaxSchüler)
            {
                ErrorHandler.ZeigeFehler(ErrorHandler.ERR_MaxSchüler);
            }

            if (schüler == null || SchuelerListe.Any(s => s.UniqueId.Equals(schüler.UniqueId)))
            {
                new PsMessageBox("Achtung", $"Der Schüler \"{schüler.Vorname} {schüler.Nachname}\" konnte nicht geladen werden.\nEr ist bereits in der Klasse, oder ist Fehlerhaft.", EPsMessageBoxButtons.OK).ShowDialog();
            }

            SchuelerListe.Add(schüler);

            foreach (Sitzplan sitzplan in Sitzplaene)
            {
                string text = $"{schüler.Nachname}, {schüler.Vorname} wurde hinzugefügt, er ist nicht in Sitzplan-{sitzplan.Name} enthalten. Soll dieser neu generiert werden?";

                PsMessageBox dialog = new PsMessageBox("Achtung", text, EPsMessageBoxButtons.YesNo);
                dialog.ShowDialog();

                if (dialog.Result.Equals(EPsMessageBoxResult.Yes))
                {
                    sitzplan.NeuGenerieren(this);
                }
            }

            SpeichernAsync();
        }

        public void SchülerEntfernen(Schüler schüler)
        {
            if (schüler == null || !SchuelerListe.Contains(schüler))
            {
                new PsMessageBox("Achtung", $"Der Schüler \"{schüler.Vorname} {schüler.Nachname}\" konnte nicht entfernt werden.\nEr ist nicht in der Klasse, oder er ist Fehlerhaft.", EPsMessageBoxButtons.OK).ShowDialog();
            }

            SchuelerListe.Remove(schüler);

            foreach (Sitzplan sitzplan in Sitzplaene)
            {
                string text = $"{schüler.Nachname}, {schüler.Vorname} wurde entfernt, er ist in Sitzplan-{sitzplan.Name} enthalten. Soll dieser neu generiert werden?\nAnsonsten ist er futsch.";

                PsMessageBox dialog = new PsMessageBox("Achtung", text, EPsMessageBoxButtons.YesNo);
                dialog.ShowDialog();

                if (dialog.Result.Equals(EPsMessageBoxResult.Yes))
                {
                    sitzplan.NeuGenerieren(this);
                }
                else
                {
                    sitzplan.SchülerEntfernen(schüler);
                }
            }

            SpeichernAsync();
        }

        public void SchülerAktuallisieren(Schüler schüler)
        {
            if (schüler == null)
            {
                return;
            }

            Schüler original = SchuelerListe.FirstOrDefault(s => s.UniqueId.Equals(schüler.UniqueId));

            if (original == null)
            {
                // Der schüler ist nicht in der klasse
                return;
            }

            Sitzplan sitzplan = Sitzplaene.FirstOrDefault(s => s.BlockSitzplan.Equals(SchulBlock.Block6));

            if (sitzplan != null && original.Verkuerzt != schüler.Verkuerzt)
            {
                PsMessageBox mbox = new PsMessageBox("Achtung", "Der Sitzplan für Block 6 wurde bereits generiert.\nBei einem Schüler hat sich der Verkürzungszustand geändert,\nsoll der Sitzplan neu generiert werden?", EPsMessageBoxButtons.YesNo);
                mbox.ShowDialog();

                if (mbox.Result.Equals(EPsMessageBoxResult.Yes))
                {
                    SitzplanEntfernen(sitzplan);
                    Sitzplan neuerSitzplan = ErstelleSitzplanDialog();
                    if (neuerSitzplan == null)
                    {
                        SitzplanHinzufügen(sitzplan);
                    }
                }
            }

            SchuelerListe[SchuelerListe.IndexOf(original)] = schüler;
            SpeichernAsync();
        }
        #endregion

        #region Import / Export
        public static List<Schüler> SchülerImportCSV()
        {
            List<Schüler> schüler = new List<Schüler>();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files (*.csv)|*.csv";
            openFileDialog.InitialDirectory = $@"{Environment.CurrentDirectory}\SchulKlassen";
            if (!openFileDialog.ShowDialog().Equals(DialogResult.OK))
            {
                return schüler;
            }

            string[] zeilen = File.ReadAllLines(openFileDialog.FileName);

            if (zeilen.Length <= 1)
            {
                ErrorHandler.ZeigeFehler(ErrorHandler.ERR_CSV_Leer);
                return schüler;
            }

            //Format Vorname,Nachname,Beruf,Betrieb,Geschlecht
            Dictionary<string, int> anordnung = new Dictionary<string, int>();

            string[] spalten = zeilen[0].Split(';').Select(s => s.Trim().ToLowerInvariant()).ToArray();
            if (spalten.Length == 0)
            {
                spalten = zeilen[0].Split(',').Select(s => s.Trim().ToLowerInvariant()).ToArray();
            }

            List<string> benötigteSpalten = new List<string>{ "vorname", "nachname", "beruf", "betrieb", "geschlecht"};

            for (int i = 0; i < spalten.Length; i++ )
            {
                string spaltenName = spalten[i].Trim().ToLowerInvariant();
                if (benötigteSpalten.Contains(spaltenName))
                {
                    anordnung[spaltenName] = i;
                    benötigteSpalten.Remove(spaltenName);
                }
            }

            if (benötigteSpalten.Count > 0)
            {
                ErrorHandler.ZeigeFehler(ErrorHandler.ERR_CSV_NichtAlleSpalten, string.Join(", ", benötigteSpalten), "");
                return schüler;
            }

            Dictionary<int, string> importFehler = new Dictionary<int, string>();
            for (int i = 1; i < zeilen.Length; i++)
            {
                string zeilenFehler;

                Schüler neuerSchüeler = SchülerCSVParser.SchülerAusCSVString(zeilen[i], anordnung, out zeilenFehler);

                if (neuerSchüeler == null)
                {
                    importFehler[i+1] = zeilenFehler;
                }
                else
                {
                    schüler.Add(neuerSchüeler);
                }
            }

            if (importFehler.Count == 0)
            {
                new PsMessageBox("Import Erfolgreich", "Der CSV Import wahr erfolgreich.", EPsMessageBoxButtons.OK).ShowDialog();
                return schüler;
            }

            StringBuilder builder = new StringBuilder();

            builder.Append($"Es wurden {importFehler.Count} Fehler in der CSV Datei gefunden.");

            foreach (string fehler in importFehler.Values.Distinct())
            {
                builder.Append($"\n{string.Format(fehler, importFehler.Values.Count(f => f.Equals(fehler)))}");
            }

            builder.Append($"\n\nIn Zeile {string.Join(", ", importFehler.Keys)} wurde ein Fehler gefunden.\n");

            builder.Append("\nFortfahren: Import fortsetzen und fehlerhafte Zeilen ignorieren.");
            builder.Append("\nKorriegieren: Alle fehlerhaften Zeilen manuell korrigieren.");
            builder.Append("\nAbbrechen: CSV Import abbrechen.");

            CSVImportErrDialog dialog = new CSVImportErrDialog(builder.ToString());

            dialog.ShowDialog();

            switch (dialog.Eingabe)
            {
                case CSVImportErrDialog.DialogEingabe.Abbrechen:
                    {
                        return new List<Schüler>();
                    }
                case CSVImportErrDialog.DialogEingabe.Fortfahren:
                    {
                        return schüler;
                    }
                case CSVImportErrDialog.DialogEingabe.Korrigieren:
                    {
                        break;
                    }
            }

            foreach(KeyValuePair<int, string> fehler in importFehler)
            {
                string zeile = zeilen[fehler.Key - 1];

                CSVKorrekturDialog korrekturDialog = new CSVKorrekturDialog(zeile, anordnung);
                korrekturDialog.ShowDialog();

                if (korrekturDialog.Eingabe.Equals(CSVKorrekturDialog.DialogEingabe.Erfolgreich))
                {
                    schüler.Add(korrekturDialog.Schüler);
                }

            }

            return schüler;
        }

        //public string CSVString => $"{Vorname},{Nachname},{Beruf},{Betrieb},{Geschlecht}";
        private static readonly string CSVHeader = "Vorname, Nachname, Beruf, Betrieb, Geschlecht";

        public void SchülerExportCSV(string pfad)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(pfad));

            string csvResult = $"{CSVHeader}\n{string.Join("\n", SchuelerListe.Select(schüler => schüler.CSVString).ToList())}";
            File.WriteAllText(pfad, csvResult);
        }

        public void AlsDateiSpeichern(string pfad)
        {
            SchulKlasse copy = new SchulKlasse(this);

            Directory.CreateDirectory(Path.GetDirectoryName(pfad));
            WarteBisDateiFreiIst(pfad);
            File.WriteAllText(pfad, JsonConvert.SerializeObject(copy));
        }

        public static SchulKlasse AusDateiLaden(string pfad)
        {
            return AusDateiLaden(new FileInfo(pfad));
        }
        public static SchulKlasse AusDateiLaden(FileInfo pfad)
        {
            if (!pfad.Exists)
            {
                new PsMessageBox("Achtung", $"Die Klasse unter folgendem Pfad:\n\"{pfad.FullName}\"\nkonnte nicht geladen werden. Der Pfad existiert nicht.", EPsMessageBoxButtons.OK).ShowDialog();
                return null;
            }
            WarteBisDateiFreiIst(pfad);

            SchulKlasse klasse = AusJsonStringLaden(File.ReadAllText(pfad.FullName));

            if (klasse == null)
            {
                ErrorHandler.ZeigeFehler(ErrorHandler.ERR_JSON_KlasseLaden, pfad.FullName, "");
            }

            return klasse;
        }

        public static SchulKlasse AusJsonStringLaden(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<SchulKlasse>(json);
            }
            catch (Exception) { }

            return null;
        }

        public async Task SpeichernAsync()
        {
            await Task.Run(() => DataHandler.SpeicherSchulKlasse(this));
        }

        public void Speichern()
        {
            DataHandler.SpeicherSchulKlasse(this);
        }
        #endregion

        #endregion

        #region Private Methods
        private string ToToolTipString()
        {
            if (AnzahlSchüler == 0)
            {
                return "Diese Klasse hat noch keine Schüler";
            }

            StringBuilder builder = new StringBuilder();
            builder.Append($"Schüler: {AnzahlSchüler}\n");

            foreach (Person.EBeruf beruf in Enum.GetValues(typeof(Person.EBeruf)))
            {
                int cnt = SchuelerListe.Count(s => s.Beruf.Equals(beruf));
                if (cnt > 0)
                {
                    builder.Append($"\n{Person.BerufStrings[(int)beruf]}: {cnt}");
                }
            }

            return builder.ToString();
        }
        #endregion

        #region private static
        public static void WarteBisDateiFreiIst(string pfad)
        {
            WarteBisDateiFreiIst(new FileInfo(pfad));
        }
        private static void WarteBisDateiFreiIst(FileInfo datei)
        {

            while (IstDateiBlockiert(datei))
            {
                Thread.Sleep(100);
            }
        }

        private static bool IstDateiBlockiert(FileInfo datei)
        {
            if (!datei.Exists)
            {
                return false;
            }
            try
            {
                //using (datei.Open(FileMode.Open, FileAccess.Read)) { }
                using (FileStream stream = datei.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                //Datei ist grade blockiert (wird von andem prozess benutzt oder nicht vorhanden)
                return true;
            }

            //Datei kann genutzt werden
            return false;
        }
        #endregion
    }
}
