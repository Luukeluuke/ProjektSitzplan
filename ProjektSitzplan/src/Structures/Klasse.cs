using Newtonsoft.Json;
using ProjektSitzplan.Exceptions;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProjektSitzplan.Structures
{
    class SchulKlasse
    {
        public string Name { get; private set; }
        public List<Schüler> SchülerListe { get; private set; } = new List<Schüler>();
        public List<Sitzplan> Sitzpläne { get; private set; } = new List<Sitzplan>();
        public int AnzahlSchüler { get => SchülerListe.Count; }
        public string ToolTipÜbersicht { get => ToToolTipString(); }

        private static string errorEntfernen = "Schüler konnte nicht aus der Klasse entfernt werden.";
        private static string errorHinzufügen = "Schüler konnte der Klasse nicht hinzugefügt werden.";

        #region Constructors
        public SchulKlasse(string name)
        {
            Name = name;
        }

        [JsonConstructor]
        public SchulKlasse(string name, List<Schüler> schülerListe) : this(name)
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

        #region Private Methods
        private string ToToolTipString()
        {
            //TODO: Sweer do your thing. Die wo 0 ist entfernen
            //wo 0

            return $"Schüler: {AnzahlSchüler}\n\n" +
                $"Systemintegration: {SchülerListe.Where(s => s.Beruf.Equals(Person.EBeruf.Systemintegration)).Count()}\n" +
                $"Anwendungsentwicklung: {SchülerListe.Where(s => s.Beruf.Equals(Person.EBeruf.Anwendungsentwicklung)).Count()}\n" +
                $"System Elektroniker: {SchülerListe.Where(s => s.Beruf.Equals(Person.EBeruf.SystemElektroniker)).Count()}\n" +
                $"Kaufman IT-Systemmanagement: {SchülerListe.Where(s => s.Beruf.Equals(Person.EBeruf.KaufmanFürITSystemManagement)).Count()}\n" +
                $"Kaufman Digitalisierungsmanagement: {SchülerListe.Where(s => s.Beruf.Equals(Person.EBeruf.KaufmanFürDigitalisierungsManagement)).Count()}\n" +
                $"Daten und Prozessanalyse: {SchülerListe.Where(s => s.Beruf.Equals(Person.EBeruf.DatenUndProzessanalyse)).Count()}\n" +
                $"Digitale Vernetzung: {SchülerListe.Where(s => s.Beruf.Equals(Person.EBeruf.DigitaleVernetzung)).Count()}";
        }
        #endregion
    }
}
