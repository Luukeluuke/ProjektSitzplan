using Newtonsoft.Json;

namespace ProjektSitzplan.Structures
{
    public class Person
    {
        public string Vorname { get; private set; }
        public string Nachname { get; private set; }
        //public int Alter { get; private set; }
        public EGeschlecht Geschlecht { get; private set; }
        public EBeruf Beruf { get; private set; }
        public string EMail { get; private set; }

        // todo bild?

        public enum EGeschlecht : ushort
        {
            Männlich = 0,
            Weiblich = 1
        }

        public enum EBeruf : ushort
        {
            Lehrer = 0,
            Anwendungsentwicklung = 1,
            Systemintegration = 2,
            DatenUndProzessanalyse = 3,
            DigitaleVernetzung = 4,
            KaufmanFürITSystemManagement = 5,
            KaufmanFürDigitalisierungsManagement = 6,
            SystemElektroniker = 7
        }

        [JsonConstructor]
        public Person(string vorname, string nachname, EGeschlecht geschlecht, EBeruf beruf, string eMail)
        {
            Vorname = vorname;
            Nachname = nachname;
            Geschlecht = geschlecht;
            Beruf = beruf;
            EMail = eMail;
        }

        public override string ToString()
        {
            return $"({Vorname}, {Nachname}, {Geschlecht}, {Beruf})";
        }
    }
}
