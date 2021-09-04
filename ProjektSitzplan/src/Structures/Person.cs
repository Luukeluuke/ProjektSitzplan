using Newtonsoft.Json;
using System;

namespace ProjektSitzplan.Structures
{
    public class Person
    {
        public string UniqueId { get; private set; }

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

        public Person(string vorname, string nachname, EGeschlecht geschlecht, EBeruf beruf) : this(vorname, nachname, geschlecht, beruf, Guid.NewGuid().ToString()) { }

        public Person(Person person) : this(person.Vorname, person.Nachname, person.Geschlecht, person.Beruf, person.UniqueId) { }

        [JsonConstructor]
        public Person(string vorname, string nachname, EGeschlecht geschlecht, EBeruf beruf, string uniqueId)
        {
            Vorname = vorname;
            Nachname = nachname;
            Geschlecht = geschlecht;
            Beruf = beruf;
            UniqueId = uniqueId;
        }

        public override string ToString()
        {
            return $"({Vorname}, {Nachname}, {Geschlecht}, {Beruf})";
        }
    }
}
