﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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

            Sitzplan sitzplan2 = LadeTestSitzplan();
            Print(sitzplan2.ToString());

            /*
            Sitzplan sitzplan = GeneriereTestSitzplan(6, 40, 123);

            string path = @"test\abc\123\sitzplan1.json";

            sitzplan.AlsDateiSpeichern(path);
            sitzplan = Sitzplan.AusDateiLaden(path);
            Print(sitzplan.ToString());
            //*/


        }

        private static Sitzplan GeneriereTestSitzplan(int Tische, int SchülerAnzahl, int seed)
        {
            return new Sitzplan(Tische, GeneriereTestKlasse(SchülerAnzahl), seed);
        }

        private static Sitzplan LadeTestSitzplan()
        {
            string path = @"testing\TestKlasse.json";
            try
            {
                return Sitzplan.AusDateiLaden(path);
            }
            catch (FileNotFoundException) { }

            Lehrer lehrer = new Lehrer("Hanz", "Eisel", Person.EGeschlecht.Männlich);

            SchulKlasse klasse = new SchulKlasse(lehrer);

            // AWE
            klasse.SchülerHinzufügen(new Schüler("Ember", "Salmon", Person.EGeschlecht.Weiblich, Person.EBeruf.Anwendungsentwicklung, new Betrieb("")));
            klasse.SchülerHinzufügen(new Schüler("Freddy", "Curd", Person.EGeschlecht.Männlich, Person.EBeruf.Anwendungsentwicklung, new Betrieb("")));
            klasse.SchülerHinzufügen(new Schüler("Danyel", "Tang", Person.EGeschlecht.Männlich, Person.EBeruf.Anwendungsentwicklung, new Betrieb("")));
            klasse.SchülerHinzufügen(new Schüler("Jerimy", "Duvall", Person.EGeschlecht.Männlich, Person.EBeruf.Anwendungsentwicklung, new Betrieb("")));
            klasse.SchülerHinzufügen(new Schüler("Martin", "Blanks", Person.EGeschlecht.Männlich, Person.EBeruf.Anwendungsentwicklung, new Betrieb("")));
            klasse.SchülerHinzufügen(new Schüler("Light", "Yagami", Person.EGeschlecht.Männlich, Person.EBeruf.Anwendungsentwicklung, new Betrieb("")));
            klasse.SchülerHinzufügen(new Schüler("Eren", "Jäger", Person.EGeschlecht.Männlich, Person.EBeruf.Anwendungsentwicklung, new Betrieb("")));
            klasse.SchülerHinzufügen(new Schüler("Levi", "Ackerman", Person.EGeschlecht.Männlich, Person.EBeruf.Anwendungsentwicklung, new Betrieb("")));
            klasse.SchülerHinzufügen(new Schüler("Erwin", "Smith", Person.EGeschlecht.Männlich, Person.EBeruf.Anwendungsentwicklung, new Betrieb("")));
            klasse.SchülerHinzufügen(new Schüler("Minato", "Namikaze", Person.EGeschlecht.Männlich, Person.EBeruf.Anwendungsentwicklung, new Betrieb("")));

            // SEL
            klasse.SchülerHinzufügen(new Schüler("Logan", "Buckland", Person.EGeschlecht.Männlich, Person.EBeruf.SystemElektroniker, new Betrieb("")));
            klasse.SchülerHinzufügen(new Schüler("Danny", "Winkelman", Person.EGeschlecht.Männlich, Person.EBeruf.SystemElektroniker, new Betrieb("")));
            klasse.SchülerHinzufügen(new Schüler("Denis", "Khang", Person.EGeschlecht.Männlich, Person.EBeruf.SystemElektroniker, new Betrieb("")));
            
            // SIN
            klasse.SchülerHinzufügen(new Schüler("Staci", "Mote", Person.EGeschlecht.Weiblich, Person.EBeruf.SystemIntegration, new Betrieb("")));
            klasse.SchülerHinzufügen(new Schüler("Burk", "Balcon", Person.EGeschlecht.Männlich, Person.EBeruf.SystemIntegration, new Betrieb("")));
            klasse.SchülerHinzufügen(new Schüler("Alex", "Noland", Person.EGeschlecht.Männlich, Person.EBeruf.SystemIntegration, new Betrieb("")));
            klasse.SchülerHinzufügen(new Schüler("Junior", "McDole", Person.EGeschlecht.Männlich, Person.EBeruf.SystemIntegration, new Betrieb("")));
            klasse.SchülerHinzufügen(new Schüler("Benjamin", "Vega", Person.EGeschlecht.Männlich, Person.EBeruf.SystemIntegration, new Betrieb("")));
            klasse.SchülerHinzufügen(new Schüler("Philippe", "Sabol", Person.EGeschlecht.Männlich, Person.EBeruf.SystemIntegration, new Betrieb("")));
            klasse.SchülerHinzufügen(new Schüler("Jeremy", "Hosley", Person.EGeschlecht.Männlich, Person.EBeruf.SystemIntegration, new Betrieb("")));
            klasse.SchülerHinzufügen(new Schüler("Carl", "Römer", Person.EGeschlecht.Männlich, Person.EBeruf.SystemIntegration, new Betrieb("")));
            klasse.SchülerHinzufügen(new Schüler("Garrick", "Clary", Person.EGeschlecht.Männlich, Person.EBeruf.SystemIntegration, new Betrieb("")));
            klasse.SchülerHinzufügen(new Schüler("Kipp", "True", Person.EGeschlecht.Männlich, Person.EBeruf.SystemIntegration, new Betrieb("")));
            klasse.SchülerHinzufügen(new Schüler("Julisa", "Well", Person.EGeschlecht.Weiblich, Person.EBeruf.SystemIntegration, new Betrieb("")));
            klasse.SchülerHinzufügen(new Schüler("Jackson", "Hartzog", Person.EGeschlecht.Männlich, Person.EBeruf.SystemIntegration, new Betrieb("")));
            klasse.SchülerHinzufügen(new Schüler("Rick", "Creed", Person.EGeschlecht.Männlich, Person.EBeruf.SystemIntegration, new Betrieb("")));
            klasse.SchülerHinzufügen(new Schüler("Nico", "Westerman", Person.EGeschlecht.Männlich, Person.EBeruf.SystemIntegration, new Betrieb("")));
            klasse.SchülerHinzufügen(new Schüler("Kevin", "Langenfeld", Person.EGeschlecht.Männlich, Person.EBeruf.SystemIntegration, new Betrieb("")));
            klasse.SchülerHinzufügen(new Schüler("Matt", "Lanz", Person.EGeschlecht.Männlich, Person.EBeruf.SystemIntegration, new Betrieb("")));
            klasse.SchülerHinzufügen(new Schüler("Igor", "Solar", Person.EGeschlecht.Männlich, Person.EBeruf.SystemIntegration, new Betrieb("")));
            klasse.SchülerHinzufügen(new Schüler("Edward", "Hysell", Person.EGeschlecht.Männlich, Person.EBeruf.SystemIntegration, new Betrieb("")));
            klasse.SchülerHinzufügen(new Schüler("Eden", "Douthitt", Person.EGeschlecht.Männlich, Person.EBeruf.SystemIntegration, new Betrieb("")));
            klasse.SchülerHinzufügen(new Schüler("Nat", "Defrancisco", Person.EGeschlecht.Männlich, Person.EBeruf.SystemIntegration, new Betrieb("")));
            klasse.SchülerHinzufügen(new Schüler("Norris", "Vargas", Person.EGeschlecht.Männlich, Person.EBeruf.SystemIntegration, new Betrieb("")));
            klasse.SchülerHinzufügen(new Schüler("Jonas", "Chacko", Person.EGeschlecht.Männlich, Person.EBeruf.SystemIntegration, new Betrieb("")));
            klasse.SchülerHinzufügen(new Schüler("Benny", "Warnock", Person.EGeschlecht.Männlich, Person.EBeruf.SystemIntegration, new Betrieb("")));
            klasse.SchülerHinzufügen(new Schüler("Patrick", "Demaria", Person.EGeschlecht.Männlich, Person.EBeruf.SystemIntegration, new Betrieb("")));
            klasse.SchülerHinzufügen(new Schüler("Hans", "Cowley", Person.EGeschlecht.Männlich, Person.EBeruf.SystemIntegration, new Betrieb("")));
            klasse.SchülerHinzufügen(new Schüler("Gerry", "Herzog", Person.EGeschlecht.Männlich, Person.EBeruf.SystemIntegration, new Betrieb("")));
            klasse.SchülerHinzufügen(new Schüler("Oscar", "Wahl", Person.EGeschlecht.Männlich, Person.EBeruf.SystemIntegration, new Betrieb("")));
            klasse.SchülerHinzufügen(new Schüler("Simon", "Shatley", Person.EGeschlecht.Männlich, Person.EBeruf.SystemIntegration, new Betrieb("")));
            klasse.SchülerHinzufügen(new Schüler("Scott", "Orlandi", Person.EGeschlecht.Männlich, Person.EBeruf.SystemIntegration, new Betrieb("")));
            klasse.SchülerHinzufügen(new Schüler("Christopher", "Palumbo", Person.EGeschlecht.Männlich, Person.EBeruf.SystemIntegration, new Betrieb("")));
            klasse.SchülerHinzufügen(new Schüler("Erick", "Kühl", Person.EGeschlecht.Männlich, Person.EBeruf.SystemIntegration, new Betrieb("")));

            Sitzplan sitzplan = new Sitzplan(6, klasse);
            sitzplan.AlsDateiSpeichern(path);

            return sitzplan;
        }

        private static SchulKlasse GeneriereTestKlasse(int SchülerAnzahl)
        {
            Lehrer lehrer = new Lehrer("KlassenLehrer", "KlassenLehrer", RandomGeschlecht());
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