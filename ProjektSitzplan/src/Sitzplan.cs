using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektSitzplan
{
    class Sitzplan
    {
        public int TischAnzahl { get; private set; }
        public List<Tisch> Tische { get; private set; } = new List<Tisch>();
        public SchulKlasse Klasse { get; private set; }

        public Sitzplan(int tischAnzahl, SchulKlasse klasse)
        {
            TischAnzahl = tischAnzahl;
            Klasse = klasse;
        }

        [JsonConstructor]
        public Sitzplan(int tischAnzahl, SchulKlasse klasse, List<Tisch> tische) : this(tischAnzahl, klasse)
        {
            Tische = tische;
        }

        private void GeneriereSitzplan(Random rand)
        {
            // todo



            /*
            Möglichst unterschiedliche Verteilung in den maximal 6 Blöcken 
            Maximale Trennung von Azubis aus demselben Betrieb 
            Anzahl der Tische und Plätze pro Tisch einstellbar

            Maximale Trennung von Berufen (optional) 
            Berücksichtigung des Geschlechts (optional) 
            Verteilparameter einstellbar (optional)
            */
        }

        public void GeneriereSitzplan(int seed)
        {
            GeneriereSitzplan(new Random(seed));
        }

        public void GeneriereSitzplan()
        {
            GeneriereSitzplan(new Random());
        }

        public void AlsDateiSpeichern(string path)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllText(path, JsonConvert.SerializeObject(this));
        }

        public static Sitzplan AusDateiLaden(string path)
        {
            if (File.Exists(path))
            {
                return JsonConvert.DeserializeObject<Sitzplan>(File.ReadAllText(path));
            }
            // todo exception handling? custom exception
            throw new FileNotFoundException();
        }
    }
}