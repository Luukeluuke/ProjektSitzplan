using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektSitzplan
{
    class Lehrer : Person
    {
        public Lehrer(string vorname, string nachname, EGeschlecht geschlecht, EBeruf beruf) : base(vorname, nachname, geschlecht, beruf) { }
    }
}
