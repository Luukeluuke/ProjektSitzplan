namespace ProjektSitzplan
{
    static class ErrorHandler
    {
        #region Klasse Erstellen
        // der fehler tritt auf wenn eine klasse erstellt wird aber Uri.IsWellFormedUriString() der name nicht passt
        public static string ERR_KE_UriUngültig = "Der Name der Klasse enthält ungültige Symbole.\nWie zum Beispiel: (\\ / : * ? \" < > |).\nBitte überprüfen.";
        public static string ERR_KE_KeinName = "Bitte tragen Sie einen Namen für die Klasse ein.";

        public static string ERR_KE_KlasserExistiertBereits = "Es existiert bereits eine Klasse mit diesem Namen.\nBitte geben Sie einen anderen Namen ein.";
        #endregion

        #region Schüler Hinzufügen
        public static string ERR_SH_PflichtfelderNichtAusgefüllt = "Der Schüler konnte nicht erstellt werden.\nEs sind nicht alle Pflichtfelder ausgefüllt.";
        #endregion

        public static void ZeigeFehler(string fehler, string titel)
        {
            new PsMessageBox(titel, fehler, PsMessageBox.EPsMessageBoxButtons.OK).ShowDialog();
        }
        public static void ZeigeFehler(string fehler)
        {
            ZeigeFehler(fehler, "Fehler");
        }
    }
}
