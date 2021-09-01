using ProjektSitzplan.Structures;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

            SpeicherSchulKlassen();
        }

        public static void SpeicherSchulKlassen()
        {
            foreach (SchulKlasse sKlasse in SchulKlassen)
            {
                sKlasse.AlsDateiSpeichern($"SchulKlassen\\{sKlasse.Name}.json");
            }
        }

        public static void LadeSchulKlassen()
        {
            SchulKlassen.Clear();
            foreach (string sKlasseFile in Directory.GetFiles("SchulKlassen"))
            {
                if (sKlasseFile.EndsWith(".json"))
                {
                    SchulKlassen.Add(SchulKlasse.AusDateiLaden(sKlasseFile));
                }
            }
        }
        #endregion
    }
}
