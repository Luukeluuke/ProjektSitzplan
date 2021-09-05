using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProjektSitzplan.Structures
{
    public enum SchulBlock
    {
        Block1,
        Block2,
        Block3,
        Block4,
        Block5,
        Block6,
        Custom,
        Current
    }

    public class SitzplanGenerator
    {
        private readonly string standardName = "Sitzplan-{0}";

        public bool BerücksichtigeBeruf;
        public bool BerücksichtigeBetrieb;
        public bool BerücksichtigeGeschlecht;

        public string Name;
        public int TischAnzahl;
        public int Seed;
        public List<Schüler> Schüler;

        private SchulKlasse Klasse;

        public SchulBlock BlockType;

        public SitzplanGenerator(SchulKlasse klasse, string name = "", int tischAnzahl = 6, int? seed = null, bool berücksichtigeBeruf = true, bool berücksichtigeBetrieb = true, bool berücksichtigeGeschlecht = true, SchulBlock blockSitzplan = SchulBlock.Current)
        {
            Klasse = klasse;
            Schüler = klasse.SchülerListe;
            TischAnzahl = tischAnzahl;
            BerücksichtigeBeruf = berücksichtigeBeruf;
            BerücksichtigeBetrieb = berücksichtigeBetrieb;
            BerücksichtigeGeschlecht = berücksichtigeGeschlecht;

            BlockType = blockSitzplan.Equals(SchulBlock.Current) ? klasse.FreierBlock() : blockSitzplan;

            if (seed == null)
                Seed = Environment.TickCount;
            else
                Seed = seed.Value;

            #region Name
            Name = string.IsNullOrWhiteSpace(name) ? GeneriereNamen() : name;

            if (NameBereitsInliste(Name))
            {
                Name = FindeNächstenNamen(Name);
            }
            #endregion
        }


        #region Klassen Suchen

        private string GeneriereNamen()
        {
            switch(BlockType)
            {
                case SchulBlock.Block1:
                    {
                        return "Block-1";
                    }
                case SchulBlock.Block2:
                    {
                        return "Block-2";
                    }
                case SchulBlock.Block3:
                    {
                        return "Block-3";
                    }
                case SchulBlock.Block4:
                    {
                        return "Block-4";
                    }
                case SchulBlock.Block5:
                    {
                        return "Block-5";
                    }
                case SchulBlock.Block6:
                    {
                        return "Block-6";
                    }
            }


            return FindeNächstenNamen(standardName);
        }

        private string FindeNächstenNamen(string name)
        {
            if (!name.EndsWith("-{0}")) name += "-{0}";

            int i = 0;
            string neuerName;
            do
            {
                i++;
                neuerName = string.Format(name, i);
            }
            while (NameBereitsInliste(neuerName));

            return neuerName;
        }

        private bool NameBereitsInliste(string name)
        {
            return Klasse.Sitzpläne.Any(sitzplan => sitzplan.Name.Equals(name));
        }

        #endregion


    }


    public class Sitzplan
    {
        [JsonIgnore]
        public List<Schüler> Schüler { get; private set; } = new List<Schüler>();

        [JsonIgnore]
        private List<string> SchülerIds = null;
        public List<string> ShortSchüler => Schüler.Select(person => person.UniqueId).ToList();


        public string Name { get; private set; }
        public int TischAnzahl { get; private set; }

        public List<TischBlock> Tische { get; private set; }

        public bool BerücksichtigeBeruf { get; private set; }
        public bool BerücksichtigeBetrieb { get; private set; }
        public bool BerücksichtigeGeschlecht { get; private set; }

        public SchulBlock BlockSitzplan;

        public int Seed { get; private set; }

        [JsonIgnore]
        public bool ErfolgreichGeneriert { get; private set; }

        public Sitzplan(SitzplanGenerator generator)
        {
            Name = generator.Name;
            TischAnzahl = generator.TischAnzahl;
            Schüler = generator.Schüler;

            BerücksichtigeBeruf = generator.BerücksichtigeBeruf;
            BerücksichtigeBetrieb = generator.BerücksichtigeBetrieb;
            BerücksichtigeGeschlecht = generator.BerücksichtigeGeschlecht;

            BlockSitzplan = generator.BlockType;
            Seed = generator.Seed;

            ErfolgreichGeneriert = Generieren();
        }

        /// <summary>
        /// JSON Constructor | Dieser constructor sollte nur für das laden von json objekten genutzt werden!
        /// </summary>
        [JsonConstructor]
        public Sitzplan(string name, int tischAnzahl, List<string> schülerIds, List<TischBlock> tische, SchulBlock blockSitzplan, bool berücksichtigeBeruf, bool berücksichtigeBetrieb, bool berücksichtigeGeschlecht, int seed)
        {
            Name = name;
            TischAnzahl = tischAnzahl;
            SchülerIds = schülerIds;
            Tische = tische;
            BlockSitzplan = blockSitzplan;
            BerücksichtigeBeruf = berücksichtigeBeruf;
            BerücksichtigeBetrieb = berücksichtigeBetrieb;
            BerücksichtigeGeschlecht = berücksichtigeGeschlecht;
            Seed = seed;

            ErfolgreichGeneriert = true;
        }

        public void ConvertShüler(SchulKlasse klasse)
        {
            if (SchülerIds == null || SchülerIds.Count == 0)
            {
                return;
            }

            Schüler = new List<Schüler>();

            while (SchülerIds.Count > 0)
            {
                string id = SchülerIds[0];

                Schüler schüler = SchülerHelfer.SchülerViaId(klasse.SchülerListe, id);
                if (schüler != null)
                {
                    Schüler.Add(schüler);
                }
                SchülerIds.RemoveAt(0);
            }

            foreach(TischBlock tisch in Tische)
            {
                tisch.ConvertShüler(this);
            }
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

        private bool Generieren()
        {
            if (BlockSitzplan.Equals(SchulBlock.Block6) && Schüler.Any(s => s.Verkürzt))
            {
                List<Schüler> verkürzer = Schüler.FindAll(s => s.Verkürzt);

                SitzplanVerkürzerWindow verkürzerWindow = new SitzplanVerkürzerWindow(verkürzer);

                verkürzerWindow.ShowDialog();

                if (verkürzerWindow.Canceled)
                {
                    return false;
                }

                Schüler = Schüler.Except(verkürzerWindow.Verkürzer).ToList();
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

            return true;
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