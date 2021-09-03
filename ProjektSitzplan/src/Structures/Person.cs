using Newtonsoft.Json;

namespace ProjektSitzplan.Structures
{
    public class Person
    {
        public string Vorname { get; private set; }
        public string Nachname { get; private set; }
        public EGeschlecht Geschlecht { get; private set; }
        public EBeruf Beruf { get; private set; }

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
            DatenundProzessanalyse = 3,
            DigitaleVernetzung = 4,
            ITSystemmanagement = 5,
            DigitalisierungsManagement = 6,
            SystemElektroniker = 7
        }

        public Person(Person person)
        {
            Vorname = person.Vorname;
            Nachname = person.Nachname;
            Geschlecht = person.Geschlecht;
            Beruf = person.Beruf;
        }

        [JsonConstructor]
        public Person(string vorname, string nachname, EGeschlecht geschlecht, EBeruf beruf)
        {
            Vorname = vorname;
            Nachname = nachname;
            Geschlecht = geschlecht;
            Beruf = beruf;
        }

        public override string ToString()
        {
            return $"({Vorname}, {Nachname}, {Geschlecht}, {Beruf})";
        }
    }
}
