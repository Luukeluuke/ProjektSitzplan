﻿using ProjektSitzplan.Structures;
using System;
using System.Windows;

namespace ProjektSitzplan
{
    static class Testing
    {
        /*
         this is a temporary class remove it later!
         TODO: REMOVE THIS CLASS + ALL REFERENCE WHEN PROJECT IS DONE :D
         Search for @TESTCLASS to find the references
         */

        private static Random random = new Random();
        private static string testKlassenName = "test-class";

        public static void Test()
        {
            TestklasseGenerierenWennNichtVorhanden();
            TestSitzpläneGenerierenWennNichtVorhanden();
        }


        private static void TestklasseGenerierenWennNichtVorhanden()
        {
            SchulKlasse klasse = DataHandler.HohleSchulKlasse(testKlassenName);
            if (klasse != null)
                return;

            klasse = new SchulKlasse(testKlassenName);

            // AWE
            klasse.SchülerHinzufügen(new Schüler("Ember", "Salmon", Person.EGeschlecht.Weiblich, Person.EBeruf.Anwendungsentwicklung, new Betrieb("Chantelle's IT Force"), RandomVerkürzt()));
            klasse.SchülerHinzufügen(new Schüler("Freddy", "Curd", Person.EGeschlecht.Männlich, Person.EBeruf.Anwendungsentwicklung, new Betrieb("Ender Tech"), RandomVerkürzt()));
            klasse.SchülerHinzufügen(new Schüler("Danyel", "Tang", Person.EGeschlecht.Männlich, Person.EBeruf.Anwendungsentwicklung, new Betrieb("Network24 GmbH"), RandomVerkürzt()));
            klasse.SchülerHinzufügen(new Schüler("Jerimy", "Duvall", Person.EGeschlecht.Männlich, Person.EBeruf.Anwendungsentwicklung, new Betrieb("Crytonix Software"), RandomVerkürzt()));
            klasse.SchülerHinzufügen(new Schüler("Martin", "Blanks", Person.EGeschlecht.Männlich, Person.EBeruf.Anwendungsentwicklung, new Betrieb("BuildieTech"), RandomVerkürzt()));
            klasse.SchülerHinzufügen(new Schüler("Light", "Yagami", Person.EGeschlecht.Männlich, Person.EBeruf.Anwendungsentwicklung, new Betrieb("Crytonix Software"), RandomVerkürzt()));
            klasse.SchülerHinzufügen(new Schüler("Eren", "Jäger", Person.EGeschlecht.Männlich, Person.EBeruf.Anwendungsentwicklung, new Betrieb("Aurora Apps"), RandomVerkürzt()));
            klasse.SchülerHinzufügen(new Schüler("Levi", "Ackerman", Person.EGeschlecht.Männlich, Person.EBeruf.Anwendungsentwicklung, new Betrieb("Chantelle's IT Force"), RandomVerkürzt()));
            klasse.SchülerHinzufügen(new Schüler("Erwin", "Smith", Person.EGeschlecht.Männlich, Person.EBeruf.Anwendungsentwicklung, new Betrieb("Aurora Apps"), RandomVerkürzt()));
            klasse.SchülerHinzufügen(new Schüler("Minato", "Namikaze", Person.EGeschlecht.Männlich, Person.EBeruf.Anwendungsentwicklung, new Betrieb("IT Tiara"), RandomVerkürzt()));

            // SEL
            klasse.SchülerHinzufügen(new Schüler("Logan", "Buckland", Person.EGeschlecht.Männlich, Person.EBeruf.SystemElektroniker, new Betrieb("Sim Tech"), RandomVerkürzt()));
            klasse.SchülerHinzufügen(new Schüler("Danny", "Winkelman", Person.EGeschlecht.Männlich, Person.EBeruf.SystemElektroniker, new Betrieb("Network24 GmbH"), RandomVerkürzt()));
            klasse.SchülerHinzufügen(new Schüler("Denis", "Khang", Person.EGeschlecht.Männlich, Person.EBeruf.SystemElektroniker, new Betrieb("Fusion Tech"), RandomVerkürzt()));

            // SIN
            klasse.SchülerHinzufügen(new Schüler("Staci", "Mote", Person.EGeschlecht.Weiblich, Person.EBeruf.Systemintegration, new Betrieb("BuildieTech"), RandomVerkürzt()));
            klasse.SchülerHinzufügen(new Schüler("Burk", "Balcon", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration, new Betrieb("Alpha Codehouse 31"), RandomVerkürzt()));
            klasse.SchülerHinzufügen(new Schüler("Alex", "Noland", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration, new Betrieb("Ender Tech"), RandomVerkürzt()));
            klasse.SchülerHinzufügen(new Schüler("Junior", "McDole", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration, new Betrieb("Sub Zero Apps"), RandomVerkürzt()));
            klasse.SchülerHinzufügen(new Schüler("Benjamin", "Vega", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration, new Betrieb("IT Tiara"), RandomVerkürzt()));
            klasse.SchülerHinzufügen(new Schüler("Philippe", "Sabol", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration, new Betrieb("Ender Tech"), RandomVerkürzt()));
            klasse.SchülerHinzufügen(new Schüler("Jeremy", "Hosley", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration, new Betrieb("IT Tiara"), RandomVerkürzt()));
            klasse.SchülerHinzufügen(new Schüler("Carl", "Römer", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration, new Betrieb("Crytonix Software"), RandomVerkürzt()));
            klasse.SchülerHinzufügen(new Schüler("Garrick", "Clary", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration, new Betrieb("Network24 GmbH"), RandomVerkürzt()));
            klasse.SchülerHinzufügen(new Schüler("Kipp", "True", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration, new Betrieb("TopHat Software"), RandomVerkürzt()));
            klasse.SchülerHinzufügen(new Schüler("Julisa", "Well", Person.EGeschlecht.Weiblich, Person.EBeruf.Systemintegration, new Betrieb("Metric Systems Corporation"), RandomVerkürzt()));
            klasse.SchülerHinzufügen(new Schüler("Jackson", "Hartzog", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration, new Betrieb("Alpha Codehouse 31"), RandomVerkürzt()));
            klasse.SchülerHinzufügen(new Schüler("Rick", "Creed", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration, new Betrieb("Alpha Codehouse 31"), RandomVerkürzt()));
            klasse.SchülerHinzufügen(new Schüler("Nico", "Westerman", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration, new Betrieb("Metric Systems Corporation"), RandomVerkürzt()));
            klasse.SchülerHinzufügen(new Schüler("Kevin", "Langenfeld", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration, new Betrieb("Crytonix Software"), RandomVerkürzt()));
            klasse.SchülerHinzufügen(new Schüler("Matt", "Lanz", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration, new Betrieb("Fusion Tech"), RandomVerkürzt()));
            klasse.SchülerHinzufügen(new Schüler("Igor", "Solar", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration, new Betrieb("Chantelle's IT Force"), RandomVerkürzt()));
            klasse.SchülerHinzufügen(new Schüler("Edward", "Hysell", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration, new Betrieb("Aurora Apps"), RandomVerkürzt()));
            klasse.SchülerHinzufügen(new Schüler("Eden", "Douthitt", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration, new Betrieb("Blue Nebula"), RandomVerkürzt()));
            klasse.SchülerHinzufügen(new Schüler("Nat", "Defrancisco", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration, new Betrieb("Sub Zero Apps"), RandomVerkürzt()));
            klasse.SchülerHinzufügen(new Schüler("Norris", "Vargas", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration, new Betrieb("Chantelle's IT Force"), RandomVerkürzt()));
            klasse.SchülerHinzufügen(new Schüler("Jonas", "Chacko", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration, new Betrieb("Ender Tech"), RandomVerkürzt()));
            klasse.SchülerHinzufügen(new Schüler("Benny", "Warnock", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration, new Betrieb("Genius Tech"), RandomVerkürzt()));
            klasse.SchülerHinzufügen(new Schüler("Patrick", "Demaria", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration, new Betrieb("IT Tiara"), RandomVerkürzt()));
            klasse.SchülerHinzufügen(new Schüler("Hans", "Cowley", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration, new Betrieb("Alpha Codehouse 31"), RandomVerkürzt()));
            klasse.SchülerHinzufügen(new Schüler("Gerry", "Herzog", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration, new Betrieb("Aurora Apps"), RandomVerkürzt()));
            klasse.SchülerHinzufügen(new Schüler("Oscar", "Wahl", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration, new Betrieb("Crytonix Software"), RandomVerkürzt()));
            klasse.SchülerHinzufügen(new Schüler("Simon", "Shatley", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration, new Betrieb("Network24 GmbH"), RandomVerkürzt()));
            klasse.SchülerHinzufügen(new Schüler("Scott", "Orlandi", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration, new Betrieb("BuildieTech"), RandomVerkürzt()));
            klasse.SchülerHinzufügen(new Schüler("Christopher", "Palumbo", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration, new Betrieb("Network24 GmbH"), RandomVerkürzt()));
            klasse.SchülerHinzufügen(new Schüler("Erick", "Kühl", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration, new Betrieb("Chantelle's IT Force"), RandomVerkürzt()));

            DataHandler.FügeSchulKlasseHinzu(klasse);
        }

        private static void TestSitzpläneGenerierenWennNichtVorhanden()
        {
            TestklasseGenerierenWennNichtVorhanden();

            SchulKlasse klasse = DataHandler.HohleSchulKlasse(testKlassenName);

            while (klasse.Sitzpläne.Count < 6)
            {
                klasse.ErstelleSitzplan();
            }

            DataHandler.SpeicherSchulKlasse(klasse);
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

        public static bool RandomVerkürzt()
        {
            return random.Next(1, 100) < 10; // should be around 10%???
        }
    }
}

