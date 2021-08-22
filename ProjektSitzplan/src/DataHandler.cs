using ProjektSitzplan.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektSitzplan
{
    static class DataHandler
    {
        public static List<SchulKlasse> SchulKlassen { get; private set; } = new List<SchulKlasse>();

        #region Public Methods
        public static bool ExistiertKlasseBereits(string klassenName)
        {
            return SchulKlassen.Where(k => k.Name.Equals(klassenName)).Count() > 0;
        }

        public static void FügeSchulKlasseHinzu(SchulKlasse schulKlasse)
        {
            if (!SchulKlassen.Contains(schulKlasse))
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
            foreach (string sKlasseFile in Directory.GetFiles("SchulKlassen"))
            {
                SchulKlassen.Add(SchulKlasse.AusDateiLaden(sKlasseFile));
            }
        }
        #endregion
    }
}
