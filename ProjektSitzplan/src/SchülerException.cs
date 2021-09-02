using ProjektSitzplan.Structures;
using System;

namespace ProjektSitzplan.Exceptions
{
    class CustomException : Exception
    {
        public CustomException(string fehlerMeldung) : base(fehlerMeldung)
        {
            //MessageBox.Show(errorMessage);
        }
    }

    #region Sitzplan Exceptions

    class SitzplanNullException : SchülerException
    {
        public SitzplanNullException(string fehlerMeldung) : base($"{fehlerMeldung}\nDer Sitzplan den du verwenden wolltest ist nicht vorhanden :/") { }
        public SitzplanNullException() : base($"Der Sitzplan den du verwenden wolltest ist nicht vorhanden :/") { }
    }


    class SitzplanInListeException : SchülerException
    {
        public SitzplanInListeException(Sitzplan sitzplan, string fehlerMeldung) : base($"{fehlerMeldung}\nDer Schüler \"{sitzplan}\" ist bereits in der Liste.") { }
        public SitzplanInListeException(Sitzplan sitzplan) : base($"Der Schüler \"{sitzplan}\" ist bereits in der Liste.") { }
    }

    class SitzplanNichtInListeException : SchülerException
    {
        public SitzplanNichtInListeException(Sitzplan sitzplan, string fehlerMeldung) : base($"{fehlerMeldung}\nDer Schüler \"{sitzplan}\" ist nicht in der Liste.") { }
        public SitzplanNichtInListeException(Sitzplan sitzplan) : base($"Der Schüler \"{sitzplan}\" ist nicht in der Liste.") { }
    }

    #endregion


    #region OS/File Exceptions
    class FileException : CustomException
    {
        public FileException(string fehlerMeldung) : base(fehlerMeldung) { }
    }

    class PfadNichtGefundenException : FileException
    {
        public PfadNichtGefundenException(string pfad, string fehlerMeldung) : base($"{fehlerMeldung}\nDer angegebenen Dateipfad {pfad} konnte nicht gefunden werden.") { }
        public PfadNichtGefundenException(string pfad) : base($"Der angegebenen Dateipfad {pfad} konnte nicht gefunden werden.") { }
    }
    #endregion


    #region Schüler Exceptions
    class SchülerException : CustomException
    {
        public SchülerException(string fehlerMeldung) : base(fehlerMeldung) { }
    }


    class SchülerNullException : SchülerException
    {
        public SchülerNullException(string fehlerMeldung) : base($"{fehlerMeldung}\nDer Schüler den du verwenden wolltest ist nicht vorhanden :/") { }
        public SchülerNullException() : base($"Der Schüler den du verwenden wolltest ist nicht vorhanden :/") { }
    }


    class SchülerInListeException : SchülerException
    {
        public SchülerInListeException(Schüler schüler, string fehlerMeldung) : base($"{fehlerMeldung}\nDer Schüler \"{schüler}\" ist bereits in der Liste.") { }
        public SchülerInListeException(Schüler schüler) : base($"Der Schüler \"{schüler}\" ist bereits in der Liste.") { }
    }

    class SchülerNichtInListeException : SchülerException
    {
        public SchülerNichtInListeException(Schüler schüler, string fehlerMeldung) : base($"{fehlerMeldung}\nDer Schüler \"{schüler}\" ist nicht in der Liste.") { }
        public SchülerNichtInListeException(Schüler schüler) : base($"Der Schüler \"{schüler}\" ist nicht in der Liste.") { }
    }
    #endregion
}
