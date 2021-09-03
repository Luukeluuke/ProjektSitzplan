using Newtonsoft.Json;

namespace ProjektSitzplan.Structures
{
    public class Schüler : Person
    {
        public Betrieb AusbildungsBetrieb;
        public bool Verkürzt;
        // todo bild


        public Schüler(Person person, Betrieb ausbildungsBetrieb, bool verkürzt) : base(person)
        {
            AusbildungsBetrieb = ausbildungsBetrieb;
            Verkürzt = verkürzt;
        }

        [JsonConstructor]
        public Schüler(string vorname, string nachname, EGeschlecht geschlecht, EBeruf beruf, Betrieb ausbildungsBetrieb, bool verkürzt) : base(vorname, nachname, geschlecht, beruf)
        {
            AusbildungsBetrieb = ausbildungsBetrieb;
            Verkürzt = verkürzt;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, Betrieb: {AusbildungsBetrieb.Name}, Verkürzt: {Verkürzt}";
        }
    }
}