using Newtonsoft.Json;

namespace ProjektSitzplan.Structures
{
    class Betrieb
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
    }
}
