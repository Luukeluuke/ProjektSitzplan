using Newtonsoft.Json;

namespace ProjektSitzplan.Structures
{
    class Schüler : Person
    {
        public Betrieb AusbildungsBetrieb { get; private set; }
        // todo bild?

        public Schüler(string vorname, string nachname, EGeschlecht geschlecht, EBeruf beruf, Betrieb ausbildungsBetrieb) : base(vorname, nachname, geschlecht, beruf, null)
        {
            AusbildungsBetrieb = ausbildungsBetrieb;
        }

        [JsonConstructor]
        public Schüler(string vorname, string nachname, EGeschlecht geschlecht, EBeruf beruf, string eMail, Betrieb ausbildungsBetrieb) : base(vorname, nachname, geschlecht, beruf, eMail)
        {
            AusbildungsBetrieb = ausbildungsBetrieb;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, Betrieb: {AusbildungsBetrieb.Name}";
        }
    }
}