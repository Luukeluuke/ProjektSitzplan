namespace ProjektSitzplan
{
    static class ErrorHandler
    {
        #region Klasse Erstellen
        // der fehler tritt auf wenn eine klasse erstellt wird aber Uri.IsWellFormedUriString() der name nicht passt
        public static readonly string ERR_KE_UriUngültig = "Der Name der Klasse enthält ungültige Symbole.\nWie zum Beispiel: (\\ / : * ? \" < > |).\nBitte überprüfen.";
        public static readonly string ERR_KE_KeinName = "Bitte tragen Sie einen Namen für die Klasse ein.";

        public static readonly string ERR_KE_KlasserExistiertBereits = "Es existiert bereits eine Klasse mit diesem Namen.\nBitte geben Sie einen anderen Namen ein.";
        #endregion

        #region Klasse Export
        // eigentlich ein should never heppen ding aber naja egal :D
        public static readonly string ERR_EX_KeineKlasseAusgewählt = "Keine Klasse zum exportieren ausgewählt.";
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
