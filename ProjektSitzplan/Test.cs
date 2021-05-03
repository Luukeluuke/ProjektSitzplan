using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjektSitzplan
{
    static class Test
    {
        private static Random random;

        public static void TestFunction()
        {
            random = new Random(123);
            Sitzplan sitzplan = GeneriereTestSitzplan(6, 40, 123);

            string path = @"test\abc\123\sitzplan1.json";

            sitzplan.AlsDateiSpeichern(path);
            sitzplan = Sitzplan.AusDateiLaden(path);

            Print(sitzplan.ToString());
        }

        private static Sitzplan GeneriereTestSitzplan(int Tische, int SchülerAnzahl, int seed)
        {
            return new Sitzplan(Tische, GeneriereTestKlasse(SchülerAnzahl), seed);
        }

        private static SchulKlasse GeneriereTestKlasse(int SchülerAnzahl)
        {
            Lehrer lehrer = new Lehrer("KlassenLehrer", "KlassenLehrer", RandomGeschlecht(), Person.EBeruf.Lehrer);
            SchulKlasse klasse = new SchulKlasse(lehrer);
            AddTestData(klasse, SchülerAnzahl);
            return klasse;
        }


        private static void AddTestData(SchulKlasse klasse, int Anzahl)
        {
            for (int i = 0; i < Anzahl; i++)
            {
                klasse.SchülerHinzufügen(new Schüler($"Test {i}v", $"Text {i}n", RandomGeschlecht(), RandomBeruf(), new Betrieb($"Text {i}b")));
            }
        }

        private static Person.EGeschlecht RandomGeschlecht()
        {
            return (Person.EGeschlecht)random.Next(2);
        }

        private static Person.EBeruf RandomBeruf()
        {
            return (Person.EBeruf)random.Next(1, 7);
        }

        public static void Print(string s)
        {
            MessageBox.Show(s);
        }
    }
}