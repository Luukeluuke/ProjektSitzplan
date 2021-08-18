using Newtonsoft.Json;
using ProjektSitzplan.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProjektSitzplan.Structures
{
    class Sitzplan
    {
        public int TischAnzahl { get; private set; }
        public List<TischBlock> Tische { get; private set; }
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
        public Sitzplan(int tischAnzahl, SchulKlasse klasse, List<TischBlock> tische, int seed)
        {
            TischAnzahl = tischAnzahl;
            Klasse = klasse;
            Seed = seed;
            Tische = tische;
        }

        public List<Schüler> Mischen(List<Schüler> originalListe)
        {
            List<Schüler> liste = originalListe.OrderBy(schüler => schüler.Nachname).ToList();
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

            Tische = new List<TischBlock>();
            for (int i = 0; i < TischAnzahl; i++)
            {
                Tische.Add(new TischBlock());
            }

            /* Beispiel
                6 tische mit 6 plätzen
                30 Schüler
                berechne anzahl pro tisch und rest
                prüfe ob bereits vorhanden im sleben ausbildungsbetrieb/geschlecht/beruf
             */

            for (int tischCount = 0; GemischteSchülerListe.Count > 0; tischCount++)
            {
                if (tischCount > Tische.Count - 1)
                {
                    tischCount = 0;
                }

                TischBlock tisch = Tische[tischCount];

                Schüler schüler = WähleGeeignetenSchüler(tisch, GemischteSchülerListe);

                tisch.SchülerHinzufügen(schüler);
                GemischteSchülerListe.Remove(schüler);
            }


            // todo test if this realy works...

            /*
            Möglichst unterschiedliche Verteilung in den maximal 6 Blöcken 
            Maximale Trennung von Azubis aus demselben Betrieb 
            Anzahl der Tische und Plätze pro Tisch einstellbar

            Maximale Trennung von Berufen (optional) 
            Berücksichtigung des Geschlechts (optional) 
            Verteilparameter einstellbar (optional)
            */
        }

        private Schüler WähleGeeignetenSchüler(TischBlock tisch, List<Schüler> gemischteSchüler)
        {
            Random rand = new Random(Seed);
            Schüler höchsterSchüler = gemischteSchüler[0];
            int höchstePunkte = BerechneSchülerpunkte(tisch, höchsterSchüler);
            for (int i = 1; i < gemischteSchüler.Count; i++)
            {
                Schüler aktuellerSchüler = gemischteSchüler[i];
                int schülerpunkte = BerechneSchülerpunkte(tisch, aktuellerSchüler);

                if (schülerpunkte > höchstePunkte)
                {
                    höchstePunkte = schülerpunkte;
                    höchsterSchüler = aktuellerSchüler;
                }
                // Zusätzlicher Zufallsfaktor
                else if (schülerpunkte == höchstePunkte && rand.Next(0, 2) == 1)
                {
                    höchsterSchüler = aktuellerSchüler;
                }
            }

            return höchsterSchüler;
        }

        private int BerechneSchülerpunkte(TischBlock tisch, Schüler schüler)
        {
            int punkte = 0;

            punkte += tisch.Sitzplätze.Any(sitzplatz => sitzplatz.AusbildungsBetrieb.Name.ToLower().Equals(schüler.AusbildungsBetrieb.Name.ToLower())) ? -1 : 1;
            punkte += tisch.Sitzplätze.Any(sitzplatz => sitzplatz.Beruf == schüler.Beruf) ? -1 : 1;
            punkte += tisch.Sitzplätze.Any(sitzplatz => sitzplatz.Geschlecht == schüler.Geschlecht) ? -1 : 1;

            return punkte;
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
            throw new PfadNichtGefundenException(path, "Beim laden des Sitzplans ist ein Fehler aufgetreten!");
        }
    }
}