using Newtonsoft.Json;

namespace ProjektSitzplan.Structures
{
    public class Betrieb
    {
        public string Name { get; set; }

        [JsonConstructor]
        public Betrieb(string name)
        {
            Name = name;
        }

        #region Public Methods
        public override string ToString()
        {
            return Name;
        }
        #endregion
    }
}
