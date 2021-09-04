using ProjektSitzplan.Structures;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

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
            TestKlasseGenerieren();
        }


        private static void TestKlasseGenerieren()
        {
            if (DataHandler.ExistiertKlasseBereits(testKlassenName)) DataHandler.EntferneSchulKlasse(testKlassenName);

            SchulKlasse klasse = new SchulKlasse(testKlassenName);
            TestFolderDialog();
            

            // AWE
            klasse.SchülerHinzufügen(new Schüler(new Person("Ember", "Salmon", Person.EGeschlecht.Weiblich, Person.EBeruf.Anwendungsentwicklung), new Betrieb("Chantelle's IT Force"), RandomVerkürzt(), RandomImage()));
            klasse.SchülerHinzufügen(new Schüler(new Person("Freddy", "Curd", Person.EGeschlecht.Männlich, Person.EBeruf.Anwendungsentwicklung), new Betrieb("Ender Tech"), RandomVerkürzt(), RandomImage()));
            klasse.SchülerHinzufügen(new Schüler(new Person("Danyel", "Tang", Person.EGeschlecht.Männlich, Person.EBeruf.Anwendungsentwicklung), new Betrieb("Network24 GmbH"), RandomVerkürzt(), RandomImage()));
            klasse.SchülerHinzufügen(new Schüler(new Person("Jerimy", "Duvall", Person.EGeschlecht.Männlich, Person.EBeruf.Anwendungsentwicklung), new Betrieb("Crytonix Software"), RandomVerkürzt(), RandomImage()));
            klasse.SchülerHinzufügen(new Schüler(new Person("Martin", "Blanks", Person.EGeschlecht.Männlich, Person.EBeruf.Anwendungsentwicklung), new Betrieb("BuildieTech"), RandomVerkürzt(), RandomImage()));
            klasse.SchülerHinzufügen(new Schüler(new Person("Light", "Yagami", Person.EGeschlecht.Männlich, Person.EBeruf.Anwendungsentwicklung), new Betrieb("Crytonix Software"), RandomVerkürzt(), RandomImage()));
            klasse.SchülerHinzufügen(new Schüler(new Person("Eren", "Jäger", Person.EGeschlecht.Männlich, Person.EBeruf.Anwendungsentwicklung), new Betrieb("Aurora Apps"), RandomVerkürzt(), RandomImage()));
            klasse.SchülerHinzufügen(new Schüler(new Person("Levi", "Ackerman", Person.EGeschlecht.Männlich, Person.EBeruf.Anwendungsentwicklung), new Betrieb("Chantelle's IT Force"), RandomVerkürzt(), RandomImage()));
            klasse.SchülerHinzufügen(new Schüler(new Person("Erwin", "Smith", Person.EGeschlecht.Männlich, Person.EBeruf.Anwendungsentwicklung), new Betrieb("Aurora Apps"), RandomVerkürzt(), RandomImage()));
            klasse.SchülerHinzufügen(new Schüler(new Person("Minato", "Namikaze", Person.EGeschlecht.Männlich, Person.EBeruf.Anwendungsentwicklung), new Betrieb("IT Tiara"), RandomVerkürzt(), RandomImage()));

            // SEL
            klasse.SchülerHinzufügen(new Schüler(new Person("Logan", "Buckland", Person.EGeschlecht.Männlich, Person.EBeruf.SystemElektroniker), new Betrieb("Sim Tech"), RandomVerkürzt(), RandomImage()));
            klasse.SchülerHinzufügen(new Schüler(new Person("Danny", "Winkelman", Person.EGeschlecht.Männlich, Person.EBeruf.SystemElektroniker), new Betrieb("Network24 GmbH"), RandomVerkürzt(), RandomImage()));
            klasse.SchülerHinzufügen(new Schüler(new Person("Denis", "Khang", Person.EGeschlecht.Männlich, Person.EBeruf.SystemElektroniker), new Betrieb("Fusion Tech"), RandomVerkürzt(), RandomImage()));

            // SIN
            klasse.SchülerHinzufügen(new Schüler(new Person("Staci", "Mote", Person.EGeschlecht.Weiblich, Person.EBeruf.Systemintegration), new Betrieb("BuildieTech"), RandomVerkürzt(), RandomImage()));
            klasse.SchülerHinzufügen(new Schüler(new Person("Burk", "Balcon", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration), new Betrieb("Alpha Codehouse 31"), RandomVerkürzt(), RandomImage()));
            klasse.SchülerHinzufügen(new Schüler(new Person("Alex", "Noland", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration), new Betrieb("Ender Tech"), RandomVerkürzt(), RandomImage()));
            klasse.SchülerHinzufügen(new Schüler(new Person("Junior", "McDole", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration), new Betrieb("Sub Zero Apps"), RandomVerkürzt(), RandomImage()));
            klasse.SchülerHinzufügen(new Schüler(new Person("Benjamin", "Vega", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration), new Betrieb("IT Tiara"), RandomVerkürzt(), RandomImage()));
            klasse.SchülerHinzufügen(new Schüler(new Person("Philippe", "Sabol", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration), new Betrieb("Ender Tech"), RandomVerkürzt(), RandomImage()));
            klasse.SchülerHinzufügen(new Schüler(new Person("Jeremy", "Hosley", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration), new Betrieb("IT Tiara"), RandomVerkürzt(), RandomImage()));
            klasse.SchülerHinzufügen(new Schüler(new Person("Carl", "Römer", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration), new Betrieb("Crytonix Software"), RandomVerkürzt(), RandomImage()));
            klasse.SchülerHinzufügen(new Schüler(new Person("Garrick", "Clary", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration), new Betrieb("Network24 GmbH"), RandomVerkürzt(), RandomImage()));
            klasse.SchülerHinzufügen(new Schüler(new Person("Kipp", "True", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration), new Betrieb("TopHat Software"), RandomVerkürzt(), RandomImage()));
            klasse.SchülerHinzufügen(new Schüler(new Person("Julisa", "Well", Person.EGeschlecht.Weiblich, Person.EBeruf.Systemintegration), new Betrieb("Metric Systems Corporation"), RandomVerkürzt(), RandomImage()));
            klasse.SchülerHinzufügen(new Schüler(new Person("Jackson", "Hartzog", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration), new Betrieb("Alpha Codehouse 31"), RandomVerkürzt(), RandomImage()));
            klasse.SchülerHinzufügen(new Schüler(new Person("Rick", "Creed", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration), new Betrieb("Alpha Codehouse 31"), RandomVerkürzt(), RandomImage()));
            klasse.SchülerHinzufügen(new Schüler(new Person("Nico", "Westerman", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration), new Betrieb("Metric Systems Corporation"), RandomVerkürzt(), RandomImage()));
            klasse.SchülerHinzufügen(new Schüler(new Person("Kevin", "Langenfeld", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration), new Betrieb("Crytonix Software"), RandomVerkürzt(), RandomImage()));
            klasse.SchülerHinzufügen(new Schüler(new Person("Matt", "Lanz", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration), new Betrieb("Fusion Tech"), RandomVerkürzt(), RandomImage()));
            klasse.SchülerHinzufügen(new Schüler(new Person("Igor", "Solar", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration), new Betrieb("Chantelle's IT Force"), RandomVerkürzt(), RandomImage()));
            klasse.SchülerHinzufügen(new Schüler(new Person("Edward", "Hysell", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration), new Betrieb("Aurora Apps"), RandomVerkürzt(), RandomImage()));
            klasse.SchülerHinzufügen(new Schüler(new Person("Eden", "Douthitt", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration), new Betrieb("Blue Nebula"), RandomVerkürzt(), RandomImage()));
            klasse.SchülerHinzufügen(new Schüler(new Person("Nat", "Defrancisco", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration), new Betrieb("Sub Zero Apps"), RandomVerkürzt(), RandomImage()));
            klasse.SchülerHinzufügen(new Schüler(new Person("Norris", "Vargas", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration), new Betrieb("Chantelle's IT Force"), RandomVerkürzt(), RandomImage()));
            klasse.SchülerHinzufügen(new Schüler(new Person("Jonas", "Chacko", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration), new Betrieb("Ender Tech"), RandomVerkürzt(), RandomImage()));
            klasse.SchülerHinzufügen(new Schüler(new Person("Benny", "Warnock", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration), new Betrieb("Genius Tech"), RandomVerkürzt(), RandomImage()));
            klasse.SchülerHinzufügen(new Schüler(new Person("Patrick", "Demaria", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration), new Betrieb("IT Tiara"), RandomVerkürzt(), RandomImage()));
            klasse.SchülerHinzufügen(new Schüler(new Person("Hans", "Cowley", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration), new Betrieb("Alpha Codehouse 31"), RandomVerkürzt(), RandomImage()));
            klasse.SchülerHinzufügen(new Schüler(new Person("Gerry", "Herzog", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration), new Betrieb("Aurora Apps"), RandomVerkürzt(), RandomImage()));
            klasse.SchülerHinzufügen(new Schüler(new Person("Oscar", "Wahl", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration), new Betrieb("Crytonix Software"), RandomVerkürzt(), RandomImage()));
            klasse.SchülerHinzufügen(new Schüler(new Person("Simon", "Shatley", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration), new Betrieb("Network24 GmbH"), RandomVerkürzt(), RandomImage()));
            klasse.SchülerHinzufügen(new Schüler(new Person("Scott", "Orlandi", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration), new Betrieb("BuildieTech"), RandomVerkürzt(), RandomImage()));
            klasse.SchülerHinzufügen(new Schüler(new Person("Christopher", "Palumbo", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration), new Betrieb("Network24 GmbH"), RandomVerkürzt(), RandomImage()));
            klasse.SchülerHinzufügen(new Schüler(new Person("Erick", "Kühl", Person.EGeschlecht.Männlich, Person.EBeruf.Systemintegration), new Betrieb("Chantelle's IT Force"), RandomVerkürzt(), RandomImage()));

            klasse.ErstelleSitzplan();
            klasse.ErstelleSitzplan();

            DataHandler.FügeSchulKlasseHinzu(klasse);
            DataHandler.SpeicherSchulKlasse(klasse);
        }

        private static string imageFolder = null;

        private static void TestFolderDialog()
        {
            if (imageFolder != null) { return; }
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();

            if (folderDialog.ShowDialog().Equals(DialogResult.OK))
            {
                imageFolder = folderDialog.SelectedPath;
            }
        }

        private static Image RandomImage()
        {
            if (imageFolder == null)
            {
                return null;
            }

            string[] files = Directory.GetFiles(imageFolder);

            string file = files[random.Next(files.Length)];

            return Schüler.LadeBildAusPfad(file);
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


