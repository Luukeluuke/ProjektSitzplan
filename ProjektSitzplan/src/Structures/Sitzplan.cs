using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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

        public Dictionary<int, int> TischPlatzVerteilung;

        public SitzplanGenerator(SchulKlasse klasse, string name = "", int tischAnzahl = 6, int? seed = null, bool berücksichtigeBeruf = true, bool berücksichtigeBetrieb = true, bool berücksichtigeGeschlecht = true, SchulBlock blockSitzplan = SchulBlock.Current, Dictionary<int, int> tischPlatzVerteilung = null)
        {
            Klasse = klasse;
            Schüler = klasse.SchuelerListe;
            TischAnzahl = tischAnzahl;
            BerücksichtigeBeruf = berücksichtigeBeruf;
            BerücksichtigeBetrieb = berücksichtigeBetrieb;
            BerücksichtigeGeschlecht = berücksichtigeGeschlecht;

            BlockType = blockSitzplan.Equals(SchulBlock.Current) ? klasse.FreierBlock() : blockSitzplan;

            #region Tischplatz
            if (tischPlatzVerteilung == null)
            {
                tischPlatzVerteilung = new Dictionary<int, int>();
                for (int i = 0; i < tischAnzahl; i++)
                {
                    tischPlatzVerteilung[i] = 8;
                }
            }
            else
            {
                for (int i = 0; i < 6; i++)
                {
                    if (i < tischAnzahl)
                    {
                        if (!tischPlatzVerteilung.ContainsKey(i))
                        {
                            tischPlatzVerteilung[i] = 8;
                        }
                    }
                    else
                    {
                        tischPlatzVerteilung.Remove(i);
                    }
                }
            }
            #endregion

            TischPlatzVerteilung = tischPlatzVerteilung;

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
            switch (BlockType)
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
            return Klasse.Sitzplaene.Any(sitzplan => sitzplan.Name.Equals(name));
        }

        #endregion


    }


    public class Sitzplan
    {
        [JsonIgnore]
        public List<Schüler> Schüler { get; private set; } = new List<Schüler>();

        public List<string> ShortSchueler => Schüler.Select(person => person.UniqueId).ToList();
        [JsonIgnore]
        private List<string> SchülerIds = null;

        public Dictionary<int, int> TischPlatzVerteilung;

        public string Name { get; private set; }
        public int TischAnzahl { get; private set; }

        public TischBlock[] Tische { get; private set; }

        public bool BeruecksichtigeBeruf { get; private set; }
        public bool BeruecksichtigeBetrieb { get; private set; }
        public bool BeruecksichtigeGeschlecht { get; private set; }

        public SchulBlock BlockSitzplan;

        public int Seed { get; private set; }

        [JsonIgnore]
        public bool ErfolgreichGeneriert { get; private set; }



        #region Constructoren
        public Sitzplan(SitzplanGenerator generator)
        {
            Name = generator.Name;
            TischAnzahl = generator.TischAnzahl;
            Schüler = generator.Schüler;

            BeruecksichtigeBeruf = generator.BerücksichtigeBeruf;
            BeruecksichtigeBetrieb = generator.BerücksichtigeBetrieb;
            BeruecksichtigeGeschlecht = generator.BerücksichtigeGeschlecht;

            TischPlatzVerteilung = generator.TischPlatzVerteilung;

            BlockSitzplan = generator.BlockType;
            Seed = generator.Seed;

            ErfolgreichGeneriert = Generieren();
        }

        /// <summary>
        /// JSON Constructor | Dieser constructor sollte nur für das laden von json objekten genutzt werden!
        /// </summary>
        [JsonConstructor]
        public Sitzplan(string name, int tischAnzahl, List<string> shortSchueler, TischBlock[] tische, SchulBlock blockSitzplan, bool beruecksichtigeBeruf, bool beruecksichtigeBetrieb, bool beruecksichtigeGeschlecht, int seed, Dictionary<int,int> tischPlatzVerteilung)
        {
            Name = name;
            TischAnzahl = tischAnzahl;
            SchülerIds = shortSchueler;
            Tische = tische;
            BlockSitzplan = blockSitzplan;
            BeruecksichtigeBeruf = beruecksichtigeBeruf;
            BeruecksichtigeBetrieb = beruecksichtigeBetrieb;
            BeruecksichtigeGeschlecht = beruecksichtigeGeschlecht;
            Seed = seed;

            TischPlatzVerteilung = tischPlatzVerteilung;

            ErfolgreichGeneriert = true;
        }
        #endregion



        public void LadeSchülerListeAusIdListe(SchulKlasse klasse)
        {
            if (SchülerIds == null || SchülerIds.Count == 0)
            {
                return;
            }

            Schüler = new List<Schüler>();

            foreach (string id in SchülerIds)
            {
                Schüler schüler = SchülerHelfer.SchülerViaId(klasse.SchuelerListe, id);
                if (schüler != null)
                {
                    Schüler.Add(schüler);
                }
            }

            SchülerIds.Clear();

            foreach (TischBlock tisch in Tische)
            {
                tisch.HohleSchülerPerId(this);
            }
        }



        #region Generieren
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

        public bool NeuGenerieren(List<Schüler> schüler)
        {
            List<Schüler> alteListe = new List<Schüler>(Schüler);
            Schüler = schüler;

            bool erfolg = Generieren();

            if (!erfolg)
            {
                Schüler = new List<Schüler>(alteListe);
            }

            return erfolg;
        }

        private bool VerkürzerEntfernen()
        {
            if (BlockSitzplan.Equals(SchulBlock.Block6) && Schüler.Any(s => s.Verkuerzt))
            {
                List<Schüler> verkürzer = Schüler.FindAll(s => s.Verkuerzt);

                new PsMessageBox("Achtung", $"Für den letzten Block wurden {verkürzer.Count} Schüler gefunden die verkürzen.\nBitte überprüfen Korrektheit überprüfen.\nBeim Generieren des Sitzplans werden nicht berücksichtigt.\nLinks sind die Verkürzer, rechts die Nicht Verkürzer.", PsMessageBox.EPsMessageBoxButtons.OK).ShowDialog();
                SchülerAuswahlDialog auswahlDialog = new SchülerAuswahlDialog("Verkürzer", verkürzer, true, "Verkürzer", "Nicht Verkürzer");

                auswahlDialog.ShowDialog();

                if (auswahlDialog.Canceled)
                {
                    return false;
                }

                Schüler = Schüler.Except(auswahlDialog.Ausgewählt).ToList();
            }
            return true;
        }

        private bool Generieren()
        {
            if (!VerkürzerEntfernen())
            {
                return false;
            }

            List<Schüler> GemischteSchülerListe = Mischen(Schüler);

            Tische = new TischBlock[TischAnzahl];
            foreach (KeyValuePair<int, int> tischPlatz in TischPlatzVerteilung.OrderBy(k => k.Key))
            {
                Tische[tischPlatz.Key] = new TischBlock(tischPlatz.Value);
            }

            for (int tischCount = 0; GemischteSchülerListe.Count > 0; tischCount++)
            {
                if (tischCount > Tische.Length - 1)
                {
                    tischCount = 0;
                }

                TischBlock tisch = NächsterFreierTisch(tischCount);
                if (tisch == null)
                {
                    ErrorHandler.ZeigeFehler(ErrorHandler.ERR_GEN_ZuWenigFreiePlätze);
                    break;
                }

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

        private TischBlock NächsterFreierTisch(int startIndex)
        {
            TischBlock tisch = null;
            for (int i = 0; i < TischAnzahl; i++, startIndex++)
            {
                if (startIndex >= TischAnzahl) 
                    startIndex = 0;

                tisch = Tische[startIndex];
                if (!tisch.IstVoll())
                    return tisch;
            }
            return tisch;
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

            if (BeruecksichtigeBetrieb)
                punkte += tisch.Sitzplätze.Any(sitzplatz => sitzplatz.Value != null && sitzplatz.Value.AusbildungsBetrieb.Name.Equals(schüler.AusbildungsBetrieb.Name, StringComparison.OrdinalIgnoreCase)) ? -1 : 1;
            if (BeruecksichtigeBeruf)
                punkte += tisch.Sitzplätze.Any(sitzplatz => sitzplatz.Value != null && sitzplatz.Value.Beruf == schüler.Beruf) ? -1 : 1;
            if (BeruecksichtigeGeschlecht)
                punkte += tisch.Sitzplätze.Any(sitzplatz => sitzplatz.Value != null && sitzplatz.Value.Geschlecht == schüler.Geschlecht) ? -1 : 1;

            return punkte;
        }
        #endregion




        public bool HatSchüler(Schüler schüler)
        {
            return Schüler.Any(s => s.UniqueId.Equals(schüler.UniqueId));
        }

        public bool SchülerTauschen(int tischIndex1, int sitzplatzIndex1, int tischIndex2, int sitzplatzIndex2)
        {
            if (tischIndex1 >= TischAnzahl || tischIndex2 >= TischAnzahl)
            {
                return false;
            }

            TischBlock tisch1 = Tische[tischIndex1];
            TischBlock tisch2 = Tische[tischIndex2];
            if (tisch1 == null || tisch2 == null)
            {
                return false;
            }

            Schüler schüler1 = tisch1.HohleSchülerVonIndex(sitzplatzIndex1);
            Schüler schüler2 = tisch2.HohleSchülerVonIndex(sitzplatzIndex2);
            
            if (schüler1 == schüler2)
            {
                return true;
            }

            tisch1.SchülerEntfernen(sitzplatzIndex1);
            tisch2.SchülerEntfernen(sitzplatzIndex2);

            tisch1.SchülerHinzufügen(schüler2, sitzplatzIndex1);
            tisch2.SchülerHinzufügen(schüler1, sitzplatzIndex2);

            return true;
        }


        public string AlsPDFExportieren()
        {
            //Speicher dialog für pfad
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
            saveFileDialog.FileName = $"{Name}.pdf";
            saveFileDialog.DefaultExt = ".pdf";
            saveFileDialog.InitialDirectory = $@"{Environment.CurrentDirectory}\SchulKlassen";

            if (!saveFileDialog.ShowDialog().Equals(DialogResult.OK))
                return null;

            string pfad = saveFileDialog.FileName;
            Directory.CreateDirectory(Path.GetDirectoryName(pfad));


            // erstellen der html
            StringBuilder builder = new StringBuilder();

            #region style/css
            //string style2 = "<link rel='stylesheet' href='styles.css'>";

            string style = "  <style type='text/css'>\n" +

                "  * {\n" +
                "    margin: 0px;\n" +
                "    padding: 0px;\n" +
                "    font-family: 'Segoe UI'\n" +
                "  }\n" +

                "  .mainWrapper\n" +
                "  {\n" +
                "    display: grid;\n" +
                /* Anzahl der Zeilen */
                "    grid-template-columns: repeat(3, 1fr);\n" +
                "    grid-template-rows: auto;\n" +
                "    margin: 0 auto;\n" +
                "    width: fit-content;\n" +
                "    position: absolute;\n" +
                "    left: 50%;\n" +
                "    top: 50%;\n" +
                "    transform: translate(-50%, -50%);\n" +
                "  }\n" +

                "  .table \n" +
                "  {\n" +
                "    height: 350px;\n" +
                "    width: 350px;\n" +
                "    display: grid;\n" +
                "    grid-gap: 0.5rem;\n" +
                "    grid-template-columns: repeat(2, 1fr);\n" +
                "    background-color: rgba(225, 225, 225, 0.52);\n" +
                "    margin: 1rem;\n" +
                "    padding: 1rem;\n" +
                "    border-radius: 1rem;\n" +
                "    border: 1px solid rgba(0, 0, 0, 0.25);\n" +
                "    box-shadow: 0px 0.0675rem 0.25rem 0.125rem rgba(0, 0, 0, 0.2);\n" +
                "  }\n" +

                "  .table .table_headline\n" +
                "  {\n" +
                "    font-size: 20px;\n" +
                "    font-weight: bold;\n" +
                "    grid-column: 1 / 3;\n" +
                "    align-self: center;\n" +
                "    justify-self: center;\n" +
                "    width: -webkit-fill-available;\n" +
                "    text-align: center;\n" +
                "  }\n" +

                "  .table .place\n" +
                "  {\n" +
                "    text-align: center; \n" +
                "    background-color: rgba(225, 225, 225, 0.92);\n" +
                "    border-radius: 1rem;\n" +
                "    border: 1px solid rgba(0, 0, 0, 0.125);\n" +
                "    box-shadow: 0px 0.0675rem 0.125rem 0.0125rem rgba(0, 0, 0, 0.2);\n" +
                "  }\n" +

                "  .table .place .student_name\n" +
                "  {\n" +
                "    font-size: 16px;\n" +
                "    display: block;\n" +
                "    position: relative;\n" +
                "    justify-self: center;\n" +
                "    align-self: center;\n" +
                "    left: 50%;\n" +
                "    top: 50%;\n" +
                "    transform: translate(-50%, -50%);\n" +
                "  }\n" +
                "  </style>\n";
            #endregion

            builder.Append("<!DOCTYPE html>\n");
            builder.Append("<head>\n");

            builder.Append(style);

            builder.Append("  <title>Sitzplan</title>\n");
            builder.Append("  <html lang = 'de'>\n");
            builder.Append("  <meta charset = 'UTF-8'>\n");
            builder.Append("</head>\n<body>\n");
            builder.Append("  <div class='mainWrapper'>\n");

            for (int i = 0; i < Tische.Length; i++)
            {
                TischBlock tisch = Tische[i];
                builder.Append($"    <div class='table'>\n");
                builder.Append($"      <p class='table_headline'> Tisch-{i + 1} </p>\n");
                foreach (KeyValuePair<int, Schüler> sitzPlatz in tisch.Sitzplätze)
                {
                    string name = "";
                    if (sitzPlatz.Value != null)
                    {
                        name = $"{ sitzPlatz.Value.Nachname }, { sitzPlatz.Value.Vorname}";
                    }

                    builder.Append("      <div class='place'>\n");
                    builder.Append($"        <span class='student_name'>{name}</span>\n");
                    builder.Append("      </div>\n");
                }
                builder.Append("    </div>\n");
            }

            builder.Append("  </div>\n");
            builder.Append("</body>\n");

            string html = builder.ToString();

            //erstellen der pdf

            //TODO: put this as PDF...
            File.WriteAllText(pfad.Replace(".pdf", ".html"), html);

            // TODO: @Marco convert html to pdf...
            /*
            HtmlToPdfConverter converter = new HtmlToPdfConverter(HtmlRenderingEngine.WebKit);

            WebKitConverterSettings webKitSettings = new WebKitConverterSettings();
            webKitSettings.WebKitPath = @"D:\Projects\github\ProjektSitzplan\packages\Syncfusion.HtmlToPdfConverter.QtWebKit.Wpf.19.2.0.55\lib\QtBinaries";
            webKitSettings.EnableForm = true;
            converter.ConverterSettings = webKitSettings;

            PdfDocument doc = converter.Convert(html, @"D:\Projects\github\ProjektSitzplan\ProjektSitzplan\bin\Debug\SchulKlassen");
            doc.Save(pfad);
            doc.Close(true);
            */


            return pfad;

        }
    }
}