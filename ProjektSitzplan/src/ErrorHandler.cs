namespace ProjektSitzplan
{
    static class ErrorHandler
    {

        #region Klasse
        // eigentlich ein should never heppen ding aber naja egal :D
        public static readonly string ERR_EX_KeineKlasseAusgewählt = "Keine Klasse zum exportieren ausgewählt.";

        public static readonly string ERR_IM_KlasseExistiertBereits = "Import der Klasse \"{0}\" ist fehlgeschlagen.\nEs existiert bereits eine Klasse mit diesem Namen.";

        public static readonly string ERR_MaxSchüler = "Diese Klasse hat bereits das Limit von 50 Schülern erreicht.";

        #region Klasse Erstellen
        // der fehler tritt auf wenn eine klasse erstellt wird aber Uri.IsWellFormedUriString() der name nicht passt
        public static readonly string ERR_KE_UriUngültig = "Der Name der Klasse enthält ungültige Symbole.\nWie zum Beispiel: (\\ / : * ? \" < > |).\nBitte überprüfen.";
        public static readonly string ERR_KE_KeinName = "Bitte tragen Sie einen Namen für die Klasse ein.";

        public static readonly string ERR_KE_KlasserExistiertBereits = "Es existiert bereits eine Klasse mit diesem Namen.\nBitte geben Sie einen anderen Namen ein.";
        #endregion
        #endregion

        #region Sitzplan
        public static readonly string ERR_EX_KeinSitzplanInKlasse = "Der PDF Export benötigt Sitzpläne. \"{0}\" hat noch keine Sitzpläne, bitte erstellen Sie zuerst welche.";

        // eigentlich ein should never heppen ding aber naja egal :D
        public static readonly string ERR_EX_KeinSitzplanAusgewählt = "Kein Sitzplan zum exportieren ausgewählt.";

        public static readonly string ERR_GEN_ZuWenigFreiePlätze = "Es gab nicht genügend freie Sitzplätze für alle Schüler. Die überigen müssen leider stehen.";
        #endregion


        #region Schüler Hinzufügen
        public static readonly string ERR_SH_PflichtfelderNichtAusgefüllt = "Der Schüler konnte nicht erstellt werden.\nEs sind nicht alle Pflichtfelder ausgefüllt.";
        #endregion

        #region JSON
        public static readonly string ERR_JSON_KlasseLaden = "Beim laden der Klasse {0} ist ein Fehler aufgetreten. Die Datei is möglicherweise fehlerhaft.";
        #endregion

        public static void ZeigeFehler(string fehler, string titel)
        {
            new PsMessageBox(titel, fehler, PsMessageBox.EPsMessageBoxButtons.OK).ShowDialog();
        }

        public static void ZeigeFehler(string fehler) { ZeigeFehler(fehler, "Fehler"); }

        public static void ZeigeFehler(string fehler, string arg1, string arg2) { ZeigeFehler(string.Format(fehler, arg1, arg2)); }
        public static void ZeigeFehler(string fehler, string titel, string arg1, string arg2) { ZeigeFehler(string.Format(fehler, arg1, arg2), titel); }
    }
}
