using Newtonsoft.Json;
using ProjektSitzplan.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace ProjektSitzplan.Structures
{
    public class TischBlock
    {
        public static readonly int MaxSchüler = 8;

        [JsonIgnore]
        public Dictionary<int, Schüler> Sitzplätze { get; private set; } = new Dictionary<int, Schüler>();

        [JsonIgnore]
        Dictionary<int, string> SchülerIds = null;

        public Dictionary<int, string> ShortSchueler => Sitzplätze.ToDictionary(k=>k.Key, k => (k.Value != null) ? k.Value.UniqueId : null);

        private static string errorEntfernen = "Schüler konnte nicht von dem TischBlock entfernt werden.";
        private static string errorHinzufügen = "Schüler konnte dem TischBlock nicht hinzugefügt werden.";


        [JsonConstructor]
        public TischBlock(Dictionary<int, string> shortSchueler)
        {
            SchülerIds = shortSchueler;
        }

        public TischBlock() 
        {
            for (int i = 0; i < MaxSchüler; i++)
            {
                Sitzplätze[i] = null;
            }
        }


        public void HohleSchülerPerId(Sitzplan sitzplan)
        {
            if (SchülerIds == null || SchülerIds.Count == 0)
            {
                return;
            }

            Sitzplätze = new Dictionary<int, Schüler>();

            foreach (KeyValuePair<int, string> eintrag in SchülerIds)
            {
                string id = eintrag.Value;

                Schüler schüler = SchülerHelfer.SchülerViaId(sitzplan.Schüler, id);
                if (schüler != null)
                {
                    Sitzplätze[eintrag.Key] = schüler;
                }
            }

            SchülerIds.Clear();
        }

        public Schüler HohleSchülerVonIndex(int index)
        {
            Sitzplätze.TryGetValue(index, out Schüler rückgabe);
            return rückgabe;
        }
        public int HohleIndexVonSchüler(Schüler schüler)
        {
            if (Sitzplätze.ContainsValue(schüler))
            {
                return Sitzplätze.First(map => map.Value.Equals(schüler)).Key;
            } return -1;
        }

        public void SchülerHinzufügen(Schüler schüler, int index)
        {
            if (schüler == null)
            {
                throw new SchülerNullException(errorHinzufügen);
            }

            if (Sitzplätze.ContainsValue(schüler))
            {
                throw new SchülerInListeException(schüler, errorHinzufügen);
            }
            Sitzplätze[index] = schüler;
        }
        public void SchülerHinzufügen(Schüler schüler)
        {
            if (schüler == null)
            {
                throw new SchülerNullException(errorHinzufügen);
            }

            if (Sitzplätze.ContainsValue(schüler))
            {
                throw new SchülerInListeException(schüler, errorHinzufügen);
            }

            int i = 0;
            while(Sitzplätze[i] != null)
            {
                i++;
            }
            Sitzplätze[i] = schüler;
        }

        public void SchülerEntfernen(int index)
        {
            Sitzplätze[index] = null;
        }
        public void SchülerEntfernen(Schüler schüler)
        {
            if (schüler == null)
            {
                throw new SchülerNullException(errorEntfernen);
            }

            if (!Sitzplätze.ContainsValue(schüler))
            {
                throw new SchülerNichtInListeException(schüler, errorEntfernen);
            }

            int key = HohleIndexVonSchüler(schüler);
            Sitzplätze[key] = null;
        }
    }
}
