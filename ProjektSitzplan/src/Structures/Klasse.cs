using Newtonsoft.Json;
using ProjektSitzplan.Exceptions;
using System.Collections.Generic;
using System.IO;

namespace ProjektSitzplan.Structures
{
    class SchulKlasse
    {
        public string Name { get; private set; }
        public List<Schüler> SchülerListe { get; private set; } = new List<Schüler>();
        public Lehrer KlassenLehrer { get; private set; }
        public List<Sitzplan> Sitzpläne { get; private set; } = new List<Sitzplan>();
        public int AnzahlSchüler { get => SchülerListe.Count; }

        private static string errorEntfernen = "Schüler konnte nicht aus der Klasse entfernt werden.";
        private static string errorHinzufügen = "Schüler konnte der Klasse nicht hinzugefügt werden.";

        #region Constructors
        public SchulKlasse(string name, Lehrer lehrer)
        {
            Name = name;
            KlassenLehrer = lehrer;
        }

        [JsonConstructor]
        public SchulKlasse(string name, Lehrer klassenLehrer, List<Schüler> schülerListe) : this(name, klassenLehrer)
        {
            SchülerListe = schülerListe;
        }
        #endregion

        #region Public Methods
        public void SitzplanHinzufügen(Sitzplan sitzplan)
        {
            Sitzpläne.Add(sitzplan);
        }

        public void SchülerHinzufügen(Schüler schüler)
        {
            if (schüler == null)
            {
                throw new SchülerNullException(errorHinzufügen);
            }

            if (SchülerListe.Contains(schüler))
            {
                throw new SchülerInListeException(schüler, errorHinzufügen);
            }

            SchülerListe.Add(schüler);
        }

        public void SchülerEntfernen(Schüler schüler)
        {
            if (schüler == null)
            {
                throw new SchülerNullException(errorEntfernen);
            }

            if (!SchülerListe.Contains(schüler))
            {
                throw new SchülerNichtInListeException(schüler, errorEntfernen);
            }

            SchülerListe.Remove(schüler);
        }

        public void AlsDateiSpeichern(string path)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllText(path, JsonConvert.SerializeObject(this));
        }

        public static SchulKlasse AusDateiLaden(string path)
        {
            if (File.Exists(path))
            {
                return JsonConvert.DeserializeObject<SchulKlasse>(File.ReadAllText(path));
            }
            throw new PfadNichtGefundenException(path, "Beim laden der Klasse ist ein Fehler aufgetreten!");
        }

        public Sitzplan ErstelleSitzplan(int tischAnzahl)
        {
            Sitzplan sitzplan = new Sitzplan(tischAnzahl, SchülerListe);
            Sitzpläne.Add(sitzplan);

            return sitzplan;
        }
        #endregion
    }
}
