using Newtonsoft.Json;
using ProjektSitzplan.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace ProjektSitzplan.Structures
{
    public class TischBlock
    {
        [JsonIgnore]
        public List<Schüler> Sitzplätze { get; private set; } = new List<Schüler>();

        [JsonIgnore]
        private List<string> SchülerIds = null;

        public List<string> ShortSchüler => Sitzplätze.Select(person => person.UniqueId).ToList();

        private static string errorEntfernen = "Schüler konnte nicht von dem TischBlock entfernt werden.";
        private static string errorHinzufügen = "Schüler konnte dem TischBlock nicht hinzugefügt werden.";


        [JsonConstructor]
        public TischBlock(List<string> shortSchüler)
        {
            SchülerIds = shortSchüler;
        }

        public TischBlock() { }
        public TischBlock(List<Schüler> sitzplätze)
        {
            Sitzplätze = sitzplätze;
        }


        public void ConvertShüler(Sitzplan sitzplan)
        {
            if (SchülerIds == null || SchülerIds.Count == 0)
            {
                return;
            }

            Sitzplätze = new List<Schüler>();

            while (SchülerIds.Count > 0)
            {
                string id = SchülerIds[0];

                Schüler schüler = SchülerHelfer.SchülerViaId(sitzplan.Schüler, id);
                if (schüler != null)
                {
                    Sitzplätze.Add(schüler);
                }
                SchülerIds.RemoveAt(0);
            }
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
