using Newtonsoft.Json;
using System;

namespace ProjektSitzplan.Structures
{
    public class Person
    {
        public string Vorname { get; set; }
        public string Nachname { get; set; }
        public EGeschlecht Geschlecht { get; set; }
        public EBeruf Beruf { get; set; }

        public string UniqueId { get; set; }

        public enum EGeschlecht : ushort
        {
            Männlich = 0,
            Weiblich = 1
        }

        public enum EBeruf : ushort
        {
            Anwendungsentwicklung = 0,
            Systemintegration = 1,
            DatenundProzessanalyse = 2,
            DigitaleVernetzung = 3,
            ITSystemmanagement = 4,
            DigitalisierungsManagement = 5,
            SystemElektroniker = 6
        }

        public Person(string vorname, string nachname, EGeschlecht geschlecht, EBeruf beruf) : this(vorname, nachname, geschlecht, beruf, Guid.NewGuid().ToString()) { }

        public Person(Person person) : this(person.Vorname, person.Nachname, person.Geschlecht, person.Beruf, person.UniqueId) { }
        
        public static string[] BerufStrings = 
        { 
            "",
            "Anwendungsentwicklung", 
            "Systemintegration",
            "Daten und Prozessanalyse",
            "Digitale Vernetzung", 
            "IT-Systemmanagement", 
            "Digitalisierungsmanagement", 
            "System Elektroniker" 
        };

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
