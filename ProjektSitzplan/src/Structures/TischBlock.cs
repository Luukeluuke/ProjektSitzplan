using Newtonsoft.Json;
using ProjektSitzplan.Exceptions;
using System.Collections.Generic;

namespace ProjektSitzplan.Structures
{
    public class TischBlock
    {
        public List<Schüler> Sitzplätze { get; private set; } = new List<Schüler>();

        private static string errorEntfernen = "Schüler konnte nicht von dem TischBlock entfernt werden.";
        private static string errorHinzufügen = "Schüler konnte dem TischBlock nicht hinzugefügt werden.";

        public TischBlock() { }

        [JsonConstructor]
        public TischBlock(List<Schüler> sitzplätze)
        {
            Sitzplätze = sitzplätze;
        }

        public void SchülerHinzufügen(Schüler schüler)
        {
            if (schüler == null)
            {
                throw new SchülerNullException(errorHinzufügen);
            }

            if (Sitzplätze.Contains(schüler))
            {
                throw new SchülerInListeException(schüler, errorHinzufügen);
            }

            Sitzplätze.Add(schüler);
        }

        public void SchülerEntfernen(Schüler schüler)
        {
            if (schüler == null)
            {
                throw new SchülerNullException(errorEntfernen);
            }

            if (!Sitzplätze.Contains(schüler))
            {
                throw new SchülerNichtInListeException(schüler, errorEntfernen);
            }

            Sitzplätze.Remove(schüler);
        }
    }
}
