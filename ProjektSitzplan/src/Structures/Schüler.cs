using Newtonsoft.Json;

namespace ProjektSitzplan.Structures
{
    public class Schüler : Person
    {
        public Betrieb AusbildungsBetrieb { get; private set; }
        // todo bild
        // todo verkürzt

        [JsonConstructor]
        public Schüler(string vorname, string nachname, EGeschlecht geschlecht, EBeruf beruf, Betrieb ausbildungsBetrieb) : base(vorname, nachname, geschlecht, beruf)
        {
            AusbildungsBetrieb = ausbildungsBetrieb;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, Betrieb: {AusbildungsBetrieb.Name}";
        }
    }
}