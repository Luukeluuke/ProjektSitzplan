using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektSitzplan
{
    static class ErrorHandler
    {
        #region Klasse Erstellen
        // der fehler tritt auf wenn eine klasse erstellt wird aber Uri.IsWellFormedUriString() der name nicht passt
        public static string ERR_KE_UriUngültig = "Der Name der Klasse enthält ungültige Symbole, wie zum Beispiel: (\\ / : * ? \" < > |)";
        public static string ERR_KE_LeererName = "Bitte tragen sie ein namen für die Klasse ein)";


        public static string ERR_KE_KlasserExistiertBereits = "Diese Klasse oder eine Klasse mit dem selben namen existiert bereits!";
        #endregion

        #region Schüler Hinzufügen
        public static string ERR_SH_PflichtfelderNichtAusgefüllt = "Der Schüler konnte nicht erstellt werden da nicht alle Pflichtfelder ausgefüllt wurden!";
        #endregion

        public static void ZeigeFehler(string fehler, string titel)
        {
            new PsMessageBox(titel, fehler, PsMessageBox.EPsMessageBoxButtons.OK).Show();
        }
        public static void ZeigeFehler(string fehler)
        {
            ZeigeFehler(fehler, "Fehler");
        }
    }
}
