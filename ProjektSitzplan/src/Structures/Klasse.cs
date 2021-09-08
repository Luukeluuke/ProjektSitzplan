using Newtonsoft.Json;
using ProjektSitzplan.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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


        #region error nachrichten
        private static string errorSchülerEntfernen = "Schüler konnte nicht aus der Klasse entfernt werden.";
        private static string errorSchülerHinzufügen = "Schüler konnte der Klasse nicht hinzugefügt werden.";

        private static string errorSitzplanEntfernen = "Sitzplan konnte nicht aus der Klasse entfernt werden.";
        private static string errorSitzplanHinzufügen = "Sitzplan konnte der Klasse nicht hinzugefügt werden.";
        #endregion

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
                sitzplan.HohleSchülerPerId(this);
            }
        }
        #endregion

        #region Public Methods

        #region Sitzpläne
        public void SitzplanHinzufügen(Sitzplan sitzplan)
        {
            if (sitzplan == null)
            {
                throw new SitzplanNullException(errorSchülerHinzufügen);
            }

            if (Sitzplaene.Any(s => s.Name.Equals(sitzplan.Name)))
            {
                throw new SitzplanInListeException(sitzplan, errorSitzplanHinzufügen);
            }

            Sitzplaene.Add(sitzplan);
        }

        public void SitzplanEntfernen(Sitzplan sitzplan)
        {
            if (sitzplan == null)
            {
                throw new SitzplanNullException(errorSchülerEntfernen);
            }

            if (!Sitzplaene.Contains(sitzplan))
            {
                throw new SitzplanNichtInListeException(sitzplan, errorSitzplanEntfernen);
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

            if (schüler == null)
            {
                throw new SchülerNullException(errorSchülerHinzufügen);
            }

            if (SchuelerListe.Contains(schüler))
            {
                throw new SchülerInListeException(schüler, errorSchülerHinzufügen);
            }

            SchuelerListe.Add(schüler);

            SpeichernAsync();
        }

        public void SchülerEntfernen(Schüler schüler)
        {
            if (schüler == null)
            {
                throw new SchülerNullException(errorSchülerEntfernen);
            }

            if (!SchuelerListe.Contains(schüler))
            {
                throw new SchülerNichtInListeException(schüler, errorSchülerEntfernen);
            }

            SchuelerListe.Remove(schüler);

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
        public void AlsDateiSpeichern(string pfad)
        {
            WarteBisDateiFreiIst(pfad);
            Directory.CreateDirectory(Path.GetDirectoryName(pfad));
            File.WriteAllText(pfad, JsonConvert.SerializeObject(this));
        }

        public static SchulKlasse AusDateiLaden(string pfad)
        {
            return AusDateiLaden(new FileInfo(pfad));
        }
        public static SchulKlasse AusDateiLaden(FileInfo pfad)
        {
            if (pfad.Exists)
            {
                WarteBisDateiFreiIst(pfad);
                
                SchulKlasse klasse = AusJsonStringLaden(File.ReadAllText(pfad.FullName));

                if (klasse == null)
                {
                    ErrorHandler.ZeigeFehler(ErrorHandler.ERR_JSON_KlasseLaden, pfad.FullName, "");
                }

                return klasse;
            }
            throw new PfadNichtGefundenException(pfad.FullName, "Beim laden der Klasse ist ein Fehler aufgetreten!");
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

        //TODO: Test if this works finde lmao
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
        private static void WarteBisDateiFreiIst(string pfad)
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
                using (datei.Open(FileMode.Open, FileAccess.Read)) { }
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
