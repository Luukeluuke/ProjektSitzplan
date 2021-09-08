using ProjektSitzplan.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace ProjektSitzplan
{
    static class DataHandler
    {
        public static readonly List<SchulKlasse> SchulKlassen = new List<SchulKlasse>();

        #region Public Methods
        public static bool HatKlassen()
        {
            return SchulKlassen.Count > 0;
        }

        public static bool ExistiertKlasseBereits(SchulKlasse schulKlasse)
        {
            if (schulKlasse != null)
            {
                return ExistiertKlasseBereits(schulKlasse.Name);
            }
            return false;
        }

        public static bool ExistiertKlasseBereits(string klassenName)
        {
            if (klassenName != null)
            {
                lock (SchulKlassen)
                {
                    return SchulKlassen.Any(k => k.Name.Equals(klassenName));
                }
            }
            return false;
        }

        public static void FügeSchulKlasseHinzu(SchulKlasse schulKlasse)
        {
            if (!ExistiertKlasseBereits(schulKlasse))
            {
                SchulKlassen.Add(schulKlasse);
            }

            SpeicherSchulKlasse(schulKlasse);
        }

        public static void LöscheSchulKlasse(string name) { LöscheSchulKlasse(HohleSchulKlasse(name)); }
        public static void LöscheSchulKlasse(SchulKlasse schulKlasse)
        {
            if (ExistiertKlasseBereits(schulKlasse))
            {
                //TODO: Delete json file....
                SchulKlassen.Remove(schulKlasse);
            }
        }

        /// <param name="name"></param>
        /// <returns>Gibt erste gefundene klasse mit dem entsprechenden namen zurück oder "null" wenn keine gefunden wurde</returns>
        public static SchulKlasse HohleSchulKlasse(string name)
        {
            return SchulKlassen.FirstOrDefault(klasse => klasse.Name.Equals(name));
        }

        public static void SpeicherSchulKlassen()
        {
            Array.ForEach(Directory.GetFiles("SchulKlassen"), File.Delete);

            SchulKlassen.ForEach(SpeicherSchulKlasse);
        }

        public static void SpeicherSchulKlasse(SchulKlasse klasse)
        {
            klasse.AlsDateiSpeichern($"SchulKlassen\\{klasse.Name}.json");
        }

        public static void LadeSchulKlassen()
        {
            SchulKlassen.Clear();

            Array.ForEach(Directory.GetFiles("SchulKlassen"), LadeSchulKlasse);
        }

        public static void LadeSchulKlasse(string pfad) { LadeSchulKlasse(new FileInfo(pfad)); }
        public static void LadeSchulKlasse(string pfad, bool importieren) { LadeSchulKlasse(new FileInfo(pfad), importieren); }
        public static void LadeSchulKlasse(FileInfo datei) { LadeSchulKlasse(datei, false); }
        public static void LadeSchulKlasse(FileInfo datei, bool importieren)
        {
            if (!datei.Extension.Equals(".json"))
            {
                return;
            }

            SchulKlasse klasse = SchulKlasse.AusDateiLaden(datei);
            if (klasse == null)
            {
                return;
            }

            if (ExistiertKlasseBereits(klasse))
            {
                ErrorHandler.ZeigeFehler(ErrorHandler.ERR_IM_KlasseExistiertBereits, klasse.Name, "");

                return;
            }

            if (importieren) klasse.SpeichernAsync();
            SchulKlassen.Add(klasse);
        }

        
        #endregion
    }
}
