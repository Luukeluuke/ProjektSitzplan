using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektSitzplan
{
    class SchulKlasse
    {
        public List<Schüler> SchülerListe { get; private set; } = new List<Schüler>();
        public Lehrer KlassenLehrer { get; private set; }

        public SchulKlasse(Lehrer lehrer)
        {
            KlassenLehrer = lehrer;
        }

        [JsonConstructor]
        public SchulKlasse(Lehrer klassenLehrer, List<Schüler> schülerListe) : this(klassenLehrer)
        {
            SchülerListe = schülerListe;
        }

        public void SchülerHinzufügen(Schüler schüler)
        {
            if (schüler == null)
            {
                // todo throw schüler null exception
                return;
            }

            if (SchülerListe.Contains(schüler))
            {
                // todo throw schüler bereits in tisch exception
                return;
            }

            SchülerListe.Add(schüler);
        }

        public void SchülerEntfernen(Schüler schüler)
        {
            if (schüler == null)
            {
                // todo throw schüler null exception
                return;
            }

            if (!SchülerListe.Contains(schüler))
            {
                // todo schüler nicht in list exception
                return;
            }

            SchülerListe.Remove(schüler);
        }
    }
}
