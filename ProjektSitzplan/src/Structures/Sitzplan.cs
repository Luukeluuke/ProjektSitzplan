using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProjektSitzplan.Structures
{
    public class SitzplanGenerator
    {
        public bool BerücksichtigeBeruf;
        public bool BerücksichtigeBetrieb;
        public bool BerücksichtigeGeschlecht;

        public string Name;
        public int TischAnzahl;
        public int Seed;
        public List<Schüler> Schüler;

        public bool LetzterBlock;

        public SitzplanGenerator(List<Schüler> schüler, string name = "", int tischAnzahl = 6, int? seed = null, bool berücksichtigeBeruf = true, bool berücksichtigeBetrieb = true, bool berücksichtigeGeschlecht = true, bool letzterBlock = false)
        {
            if (seed == null)
                Seed = Environment.TickCount;
            else
                Seed = seed.Value;

            Name = name;
            Schüler = schüler;
            TischAnzahl = tischAnzahl;
            BerücksichtigeBeruf = berücksichtigeBeruf;
            BerücksichtigeBetrieb = berücksichtigeBetrieb;
            BerücksichtigeGeschlecht = berücksichtigeGeschlecht;
            LetzterBlock = letzterBlock;
        }

        public SitzplanGenerator(SchulKlasse klasse, string name, int tischAnzahl, int? seed = null, bool berücksichtigeBeruf = true, bool berücksichtigeBetrieb = true, bool berücksichtigeGeschlecht = true, bool letzterBlock = false) : this(klasse.SchülerListe, name, tischAnzahl, seed, berücksichtigeBeruf, berücksichtigeBetrieb, berücksichtigeGeschlecht, letzterBlock) { }
    }


    public class Sitzplan
    {
        public string Name { get; private set; }
        public int TischAnzahl { get; private set; }
        public List<Schüler> Schüler { get; private set; }
        public List<TischBlock> Tische { get; private set; }

        public bool BerücksichtigeBeruf = true;
        public bool BerücksichtigeBetrieb = true;
        public bool BerücksichtigeGeschlecht = true;

        public bool LetzterBlock = false;

        public int Seed { get; private set; }

        private Sitzplan(string name, int tischAnzahl, List<Schüler> schüler, bool letzterBlock, bool berücksichtigeBeruf, bool berücksichtigeBetrieb, bool berücksichtigeGeschlecht, int seed)
        {
            Name = name;
            TischAnzahl = tischAnzahl;
            Schüler = schüler;

            BerücksichtigeBeruf = berücksichtigeBeruf;
            BerücksichtigeBetrieb = berücksichtigeBetrieb;
            BerücksichtigeGeschlecht = berücksichtigeGeschlecht;

            LetzterBlock = letzterBlock;
            Seed = seed;
        }

        public Sitzplan(SitzplanGenerator generator) : this(generator.Name, generator.TischAnzahl, generator.Schüler, generator.LetzterBlock, generator.BerücksichtigeBeruf, generator.BerücksichtigeBetrieb, generator.BerücksichtigeGeschlecht, generator.Seed)
        {
            Generieren();
        }

        /// <summary>
        /// JSON Constructor | Dieser constructor sollte nur für das laden von json objekten genutzt werden!
        /// </summary>
        [JsonConstructor]
        public Sitzplan(string name, int tischAnzahl, List<Schüler> schüler, List<TischBlock> tische, bool letzterBlock, bool berücksichtigeBeruf, bool berücksichtigeBetrieb, bool berücksichtigeGeschlecht, int seed) : this(name, tischAnzahl,schüler,letzterBlock,berücksichtigeBeruf,berücksichtigeBetrieb, berücksichtigeGeschlecht, seed)
        {
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

        private void Generieren()
        {
            if (LetzterBlock && Schüler.Any(s => s.Verkürzt))
            {
                //TODO: Dialog öffnen um schkürzte schüler zu handhaben :D
            }

            List<Schüler> GemischteSchülerListe = Mischen(Schüler);

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

            if (BerücksichtigeBetrieb)
                punkte += tisch.Sitzplätze.Any(sitzplatz => sitzplatz.AusbildungsBetrieb.Name.Equals(schüler.AusbildungsBetrieb.Name, StringComparison.OrdinalIgnoreCase)) ? -1 : 1;
            if (BerücksichtigeBeruf)
                punkte += tisch.Sitzplätze.Any(sitzplatz => sitzplatz.Beruf == schüler.Beruf) ? -1 : 1;
            if (BerücksichtigeGeschlecht)
                punkte += tisch.Sitzplätze.Any(sitzplatz => sitzplatz.Geschlecht == schüler.Geschlecht) ? -1 : 1;

            return punkte;
        }


        public void AlsPDFExportieren(string path)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            
            // TODO: @Marco Export logic here or be called from here :D
        }
    }
}