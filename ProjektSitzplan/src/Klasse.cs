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

        private static string errorEntfernen = "Schüler konnte nicht aus der Klasse entfernt werden.";
        private static string errorHinzufügen = "Schüler konnte der Klasse nicht hinzugefügt werden.";

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
                throw new SchülerNullException(errorHinzufügen);
            }

            if (SchülerListe.Contains(schüler))
            {
                throw new SchülerInListeException(schüler, errorHinzufügen);
            }

            SchülerListe.Add(schüler);
        }

        public void SchülerEntfernen(Schüler schüler)
        {
            if (schüler == null)
            {
                throw new SchülerNullException(errorEntfernen);
            }

            if (!SchülerListe.Contains(schüler))
            {
                throw new SchülerNichtInListeException(schüler, errorEntfernen);
            }

            SchülerListe.Remove(schüler);
        }
    }
}
