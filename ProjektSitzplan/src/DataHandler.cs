﻿using ProjektSitzplan.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
                return SchulKlassen.Any(k => k.Name.Equals(klassenName));
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

            Parallel.ForEach(Directory.GetFiles("SchulKlassen"), LadeSchulKlasse);
        }



        public static void LadeSchulKlasse(string pfad) { LadeSchulKlasse(pfad, false); }
        public static void LadeSchulKlasse(string pfad, bool speichern)
        {
            if (!pfad.EndsWith(".json"))
            {
                return;
            }

            SchulKlasse klasse = SchulKlasse.AusDateiLaden(pfad);
            if (klasse == null)
            {
                return;
            }

            if (ExistiertKlasseBereits(klasse))
            {
                ErrorHandler.ZeigeFehler(ErrorHandler.ERR_IM_KlasseExistiertBereits, klasse.Name, "");

                return;
            }

            if (speichern) SpeicherSchulKlasse(klasse);
            SchulKlassen.Add(klasse);
        }
        #endregion
    }
}
