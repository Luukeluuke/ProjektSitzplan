using Newtonsoft.Json;

namespace ProjektSitzplan.Structures
{
    public class Betrieb
    {
        public string Name { get; private set; }
        public string EMail { get; private set; } = null;

        public Betrieb(string name)
        {
            Name = name;
        }

        [JsonConstructor]
        public Betrieb(string name, string eMail) : this(name)
        {
            EMail = eMail;
        }

        #region Public Methods
        public override string ToString()
        {
            return Name;
        }
        #endregion
    }
}
