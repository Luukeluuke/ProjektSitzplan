using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektSitzplan
{
    class Betrieb
    {
        public string Name { get; private set; }
        public string EMail { get; private set; } = null;

        public Betrieb (string name)
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
