using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektSitzplan
{
    class Schüler : Person
    {
        public Betrieb AusbildungsBetrieb { get; private set; }

        [JsonConstructor]
        public Schüler(string vorname, string nachname, EGeschlecht geschlecht, EBeruf beruf, Betrieb betrieb) : base(vorname, nachname, geschlecht, beruf)
        {
            AusbildungsBetrieb = betrieb;
        }
    }
}