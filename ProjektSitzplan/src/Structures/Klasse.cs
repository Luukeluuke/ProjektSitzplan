﻿using Newtonsoft.Json;
using ProjektSitzplan.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjektSitzplan.Structures
{
    public class SchulKlasse
    {
        public string Name { get; private set; }
        public readonly List<Schüler> SchülerListe;
        public readonly List<Sitzplan> Sitzpläne = new List<Sitzplan>();

        [JsonIgnore]
        public int AnzahlSchüler { get => SchülerListe.Count; }

        [JsonIgnore]
        public string ToolTipÜbersicht { get => ToToolTipString(); }


        #region error nachrichten
        private static string errorSchülerEntfernen = "Schüler konnte nicht aus der Klasse entfernt werden.";
        private static string errorSchülerHinzufügen = "Schüler konnte der Klasse nicht hinzugefügt werden.";

        private static string errorSitzplanEntfernen = "Sitzplan konnte nicht aus der Klasse entfernt werden.";
        private static string errorSitzplanHinzufügen = "Sitzplan konnte der Klasse nicht hinzugefügt werden.";
        #endregion

        #region Constructors
        public SchulKlasse(string name, List<Schüler> schülerListe = null)
        {
            Name = name;
            SchülerListe = (schülerListe == null) ? new List<Schüler>() : schülerListe;
        }

        [JsonConstructor]
        public SchulKlasse(string name, List<Schüler> schülerListe, List<Sitzplan> sitzpläne) : this(name)
        {
            SchülerListe = schülerListe;
            Sitzpläne = sitzpläne;
            
            foreach(Sitzplan sitzplan in Sitzpläne)
            {
                sitzplan.ConvertShüler(this);
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

            if (Sitzpläne.Contains(sitzplan))
            {
                throw new SitzplanInListeException(sitzplan, errorSitzplanHinzufügen);
            }

            Sitzpläne.Add(sitzplan);
        }

        public void SitzplanEntfernen(Sitzplan sitzplan)
        {
            if (sitzplan == null)
            {
                throw new SitzplanNullException(errorSchülerEntfernen);
            }

            if (!Sitzpläne.Contains(sitzplan))
            {
                throw new SitzplanNichtInListeException(sitzplan, errorSitzplanEntfernen);
            }

            Sitzpläne.Remove(sitzplan);
        }

        public Sitzplan ErstelleSitzplanDialog()
        {
            SitzplanGenerierenWindow erstellDialog = new SitzplanGenerierenWindow(this);

            erstellDialog.ShowDialog();

            if (!erstellDialog.Erfolgreich)
            {
                return null;
            }

            Sitzplan neuerSitzplan = ErstelleSitzplan(erstellDialog.Generator);
            return neuerSitzplan;
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

            new PsMessageBox("Sitzplan", $"Der Sitzplan \"{sitzplan.Name}\" wurde erfolgreich generiert.", PsMessageBox.EPsMessageBoxButtons.OK).ShowDialog();

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
                if (!Sitzpläne.Any(sitzplan => sitzplan.BlockSitzplan.Equals(block))) break;
            }

            return schulBlock.Equals(SchulBlock.Current) ? SchulBlock.Custom : schulBlock;
        }
        #endregion

        #region Schüler
        public void SchülerHinzufügen(Schüler schüler)
        {
            if (SchülerListe.Count >= 50)
            {
                ErrorHandler.ZeigeFehler(ErrorHandler.ERR_MaxSchüler);
            }

            if (schüler == null)
            {
                throw new SchülerNullException(errorSchülerHinzufügen);
            }

            if (SchülerListe.Contains(schüler))
            {
                throw new SchülerInListeException(schüler, errorSchülerHinzufügen);
            }

            SchülerListe.Add(schüler);
        }

        public void SchülerEntfernen(Schüler schüler)
        {
            if (schüler == null)
            {
                throw new SchülerNullException(errorSchülerEntfernen);
            }

            if (!SchülerListe.Contains(schüler))
            {
                throw new SchülerNichtInListeException(schüler, errorSchülerEntfernen);
            }

            SchülerListe.Remove(schüler);
        }
        #endregion

        #region Import / Export
        public void AlsDateiSpeichern(string path)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllText(path, JsonConvert.SerializeObject(this));
        }

        public static SchulKlasse AusDateiLaden(string path)
        {
            if (File.Exists(path))
            {
                SchulKlasse klasse = AusJsonStringLaden(File.ReadAllText(path));

                if (klasse == null)
                {
                    ErrorHandler.ZeigeFehler(ErrorHandler.ERR_JSON_KlasseLaden, Path.GetFullPath(path), "");
                }

                return klasse;
            }
            throw new PfadNichtGefundenException(path, "Beim laden der Klasse ist ein Fehler aufgetreten!");
        }

        public static SchulKlasse AusJsonStringLaden(string json)
        {
            Exception ex;
            try
            {
                return JsonConvert.DeserializeObject<SchulKlasse>(json);
            }
            catch (JsonReaderException exc) { ex = exc; }
            catch (JsonSerializationException exc) { ex = exc; }
            
            return null;
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
                int cnt = SchülerListe.Count(s => s.Beruf.Equals(beruf));
                if (cnt > 0)
                {
                    builder.Append($"\n{Person.BerufStrings[(int)beruf]}: {cnt}");
                }
            }

            return builder.ToString();
        }
        #endregion
    }
}
