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

        public int Seed { get; private set; }

        public Sitzplan(int tischAnzahl, SchulKlasse klasse, int seed)
        {
            TischAnzahl = tischAnzahl;
            Klasse = klasse;
            Seed = seed;

            GeneriereSitzplan();
        }

        public Sitzplan(int tischAnzahl, SchulKlasse klasse) : this(tischAnzahl, klasse, Environment.TickCount) { }

        [JsonConstructor]
        public Sitzplan(int tischAnzahl, SchulKlasse klasse, List<Tisch> tische, int seed)
        {
            TischAnzahl = tischAnzahl;
            Klasse = klasse;
            Seed = seed;
            Tische = tische;
        }

        public List<Schüler> Mischen(List<Schüler> liste)
        {
            //Fisher-Yates shuffle https://en.wikipedia.org/wiki/Fisher–Yates_shuffle
            Random rand = new Random(Seed);
            int n = liste.Count;
            while (n > 1)
            {
                n--;
                int randomInt = rand.Next(n + 1);
                Schüler randomSchüler = liste[randomInt];
                liste[randomInt] = liste[n];
                liste[n] = randomSchüler;
            }
            return liste;
        }

        private void GeneriereSitzplan()
        {
            List<Schüler> GemischteSchülerListe = Mischen(Klasse.SchülerListe);

            /* Beispiel
                6 tische mit 6 plätzen
                30 Schüler
                berechne anzahl pro tisch und rest
                prüfe ob bereits vorhanden im sleben ausbildungsbetrieb/geschlecht/beruf
             */

            /*while(GemischteSchülerListe.Count > 0)
            {
                
            }*/

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