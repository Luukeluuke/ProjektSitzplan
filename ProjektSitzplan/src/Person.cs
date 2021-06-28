using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektSitzplan
{
    class Person
    {
        public string Vorname { get; private set; }
        public string Nachname { get; private set; }
        //public int Alter { get; private set; }
        public EGeschlecht Geschlecht { get; private set; }
        public EBeruf Beruf { get; private set; }
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
            SystemIntegration = 2,
            DatenUndProzessanalyse = 3,
            DigitaleVernetzung = 4,
            KaufmanFürITSystemManagement = 5,
            KaufmanFürDigitalisierungsManagement = 6,
            SystemElektroniker = 7
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
