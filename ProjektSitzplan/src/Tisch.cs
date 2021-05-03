using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektSitzplan
{
    class Tisch
    {
        public int SitzplatzAnzahl { get; private set; }
        public List<Schüler> Sitzplätze { get; private set; } = new List<Schüler>();

        public Tisch(int sitzplatzAnzahl)
        {
            SitzplatzAnzahl = sitzplatzAnzahl;
        }

        [JsonConstructor]
        public Tisch(int sitzplatzAnzahl, List<Schüler> schülerListe)
        {
            SitzplatzAnzahl = sitzplatzAnzahl;
            Sitzplätze = schülerListe;
        }

        public void SchülerHinzufügen(Schüler schüler)
        {
            if (schüler == null)
            {
                // todo throw schüler null exception
                return;
            }

            if (Sitzplätze.Count >= SitzplatzAnzahl)
            {
                // todo throw tisch voll exception
                return;
            }

            if (Sitzplätze.Contains(schüler))
            {
                // todo throw schüler bereits in tisch exception
                return;
            }

            Sitzplätze.Add(schüler);
        }

        public void SchülerEntfernen(Schüler schüler)
        {
            if (schüler == null)
            {
                // todo throw schüler null exception
                return;
            }

            if (!Sitzplätze.Contains(schüler))
            {
                // todo schüler nicht in list exception
                return;
            }

            Sitzplätze.Remove(schüler);
        }
    }
}
