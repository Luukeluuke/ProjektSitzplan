using ProjektSitzplan.Structures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ProjektSitzplan
{
    static class DataHandler
    {
        public static List<SchulKlasse> SchulKlassen { get; private set; } = new List<SchulKlasse>();

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
                //return SchulKlassen.Where(k => k.Name.Equals(klassenName)).Count() > 0;
                return SchulKlassen.Any(k => k.Name.Equals(klassenName));
            }
            return false;
        }

        public static void FügeSchulKlasseHinzu(SchulKlasse schulKlasse)
        {
            //if (!SchulKlassen.Contains(schulKlasse))
            //TODO Macht das so nicht mehr Sinn? Außerdem sollte man da ne fehlermeldung zeigen oder so? vor allem beim import :D
            if (!ExistiertKlasseBereits(schulKlasse))
            {
                SchulKlassen.Add(schulKlasse);
            }

            SpeicherSchulKlasse(schulKlasse);
        }

        public static void EntferneSchulKlasse(string name) { EntferneSchulKlasse(HohleSchulKlasse(name)); }
        public static void EntferneSchulKlasse(SchulKlasse schulKlasse)
        {
            if (ExistiertKlasseBereits(schulKlasse))
            {
                SchulKlassen.Remove(schulKlasse);
            }

            SpeicherSchulKlassen();
        }

        /// <param name="name"></param>
        /// <returns>Gibt erste gefundene klasse mit dem entsprechenden namen zurück oder "null" wenn keine gefunden wurde</returns>
        public static SchulKlasse HohleSchulKlasse(string name)
        {
            return SchulKlassen.FirstOrDefault(klasse => klasse.Name.Equals(name));
        }

        public static void SpeicherSchulKlassen()
        {
            SchulKlassen.ForEach(SpeicherSchulKlasse);
        }

        public static void SpeicherSchulKlasse(SchulKlasse klasse)
        {
            klasse.AlsDateiSpeichern($"SchulKlassen\\{klasse.Name}.json");
        }

        public static void LadeSchulKlassen()
        {
            SchulKlassen.Clear();

            Parallel.ForEach(Directory.GetFiles("SchulKlassen"), LadeSchulKlasse);
        }

        public static void LadeSchulKlasse(string pfad)
        {
            if (pfad.EndsWith(".json"))
            {
                SchulKlasse klasse = SchulKlasse.AusDateiLaden(pfad);
                if (klasse != null)
                {
                    if (ExistiertKlasseBereits(klasse))
                    {
                        ErrorHandler.ZeigeFehler(ErrorHandler.ERR_IM_KlasseExistiertBereits, klasse.Name, "");

                        return;
                    }
                    SchulKlassen.Add(klasse);
                }
            }
        }
        #endregion
    }
}
