using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace ProjektSitzplan.Structures
{
    public class TischBlock
    {
        [JsonIgnore]
        public Dictionary<int, Schüler> Sitzplätze { get; private set; } = new Dictionary<int, Schüler>();

        [JsonIgnore]
        Dictionary<int, string> SchülerIds = null;

        public Dictionary<int, string> ShortSchueler => Sitzplätze.ToDictionary(k=>k.Key, k => (k.Value != null) ? k.Value.UniqueId : null);
        
        public int MaxSchueler { get; private set; } = 8;

        [JsonConstructor]
        public TischBlock(Dictionary<int, string> shortSchueler, int maxSchueler)
        {
            SchülerIds = shortSchueler;
            MaxSchueler = maxSchueler;
        }

        public TischBlock(int maxSchüler = 8) 
        {
            MaxSchueler = maxSchüler;

            for (int i = 0; i < MaxSchueler; i++)
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

        public bool IstVoll()
        {
            return !Sitzplätze.Any(platz => platz.Value == null);
        }

        public bool SchülerHinzufügen(Schüler schüler, int index)
        {
            if (schüler == null || index > MaxSchueler - 1 || Sitzplätze.ContainsValue(schüler))
            {
                return false;
            }
            Sitzplätze[index] = schüler;
            return true;
        }
        public bool SchülerHinzufügen(Schüler schüler)
        {
            if (schüler == null || IstVoll() || Sitzplätze.ContainsValue(schüler))
            {
                return false;
            }

            int i = 0;
            while(Sitzplätze[i] != null)
            {
                i++;
            }
            Sitzplätze[i] = schüler;
            return true;
        }

        public void SchülerEntfernen(int index)
        {
            Sitzplätze[index] = null;
        }
        public bool SchülerEntfernen(Schüler schüler)
        {
            if (schüler == null || !Sitzplätze.ContainsValue(schüler))
            {
                return false;
            }

            int key = HohleIndexVonSchüler(schüler);
            Sitzplätze[key] = null;
            return true;
        }
    }
}
