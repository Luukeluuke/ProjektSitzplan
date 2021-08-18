namespace ProjektSitzplan.Structures
{
    class Lehrer : Person
    {
        public Lehrer(string vorname, string nachname, EGeschlecht geschlecht) : base(vorname, nachname, geschlecht, EBeruf.Lehrer, null) { }
    }
}
