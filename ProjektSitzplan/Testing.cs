using ProjektSitzplan.Structures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ProjektSitzplan
{
    static class Testing
    {
        private static Random random = new Random();
        private static string testKlassenName = "test-class";

        public static void Test()
        {
            TestKlasseGenerieren();
        }


        private static void TestKlasseGenerieren()
        {
            if (DataHandler.ExistiertKlasseBereits(testKlassenName)) DataHandler.LöscheSchulKlasse(testKlassenName);

            TestFolderDialog();

            List<Schüler> schüler = new List<Schüler>();
            schüler.Add(new Schüler(new Person("Ember", "Salmon", Person.EGeschlecht.Weiblich, RandomBeruf()), "Chantelle's IT Force", RandomVerkürzt(), RandomImage()));
            schüler.Add(new Schüler(new Person("Freddy", "Curd", Person.EGeschlecht.Männlich, RandomBeruf()), "Ender Tech", RandomVerkürzt(), RandomImage()));
            schüler.Add(new Schüler(new Person("Danyel", "Tang", Person.EGeschlecht.Männlich, RandomBeruf()), "Network24 GmbH", RandomVerkürzt(), RandomImage()));
            schüler.Add(new Schüler(new Person("Jerimy", "Duvall", Person.EGeschlecht.Männlich, RandomBeruf()), "Crytonix Software", RandomVerkürzt(), RandomImage()));
            schüler.Add(new Schüler(new Person("Martin", "Blanks", Person.EGeschlecht.Männlich, RandomBeruf()), "BuildieTech", RandomVerkürzt(), RandomImage()));
            schüler.Add(new Schüler(new Person("Light", "Yagami", Person.EGeschlecht.Männlich, RandomBeruf()), "Crytonix Software", RandomVerkürzt(), RandomImage()));
            schüler.Add(new Schüler(new Person("Eren", "Jäger", Person.EGeschlecht.Männlich, RandomBeruf()), "Aurora Apps", RandomVerkürzt(), RandomImage()));
            schüler.Add(new Schüler(new Person("Levi", "Ackerman", Person.EGeschlecht.Männlich, RandomBeruf()), "Chantelle's IT Force", RandomVerkürzt(), RandomImage()));
            schüler.Add(new Schüler(new Person("Erwin", "Smith", Person.EGeschlecht.Männlich, RandomBeruf()), "Aurora Apps", RandomVerkürzt(), RandomImage()));
            schüler.Add(new Schüler(new Person("Minato", "Namikaze", Person.EGeschlecht.Männlich, RandomBeruf()), "IT Tiara", RandomVerkürzt(), RandomImage()));
            schüler.Add(new Schüler(new Person("Logan", "Buckland", Person.EGeschlecht.Männlich, RandomBeruf()), "Sim Tech", RandomVerkürzt(), RandomImage()));
            schüler.Add(new Schüler(new Person("Danny", "Winkelman", Person.EGeschlecht.Männlich, RandomBeruf()), "Network24 GmbH", RandomVerkürzt(), RandomImage()));
            schüler.Add(new Schüler(new Person("Denis", "Khang", Person.EGeschlecht.Männlich, RandomBeruf()), "Fusion Tech", RandomVerkürzt(), RandomImage()));
            schüler.Add(new Schüler(new Person("Staci", "Mote", Person.EGeschlecht.Weiblich, RandomBeruf()), "BuildieTech", RandomVerkürzt(), RandomImage()));
            schüler.Add(new Schüler(new Person("Burk", "Balcon", Person.EGeschlecht.Männlich, RandomBeruf()), "Alpha Codehouse 31", RandomVerkürzt(), RandomImage()));
            schüler.Add(new Schüler(new Person("Alex", "Noland", Person.EGeschlecht.Männlich, RandomBeruf()), "Ender Tech", RandomVerkürzt(), RandomImage()));
            schüler.Add(new Schüler(new Person("Junior", "McDole", Person.EGeschlecht.Männlich, RandomBeruf()), "Sub Zero Apps", RandomVerkürzt(), RandomImage()));
            schüler.Add(new Schüler(new Person("Benjamin", "Vega", Person.EGeschlecht.Männlich, RandomBeruf()), "IT Tiara", RandomVerkürzt(), RandomImage()));
            schüler.Add(new Schüler(new Person("Philippe", "Sabol", Person.EGeschlecht.Männlich, RandomBeruf()), "Ender Tech", RandomVerkürzt(), RandomImage()));
            schüler.Add(new Schüler(new Person("Jeremy", "Hosley", Person.EGeschlecht.Männlich, RandomBeruf()), "IT Tiara", RandomVerkürzt(), RandomImage()));
            schüler.Add(new Schüler(new Person("Carl", "Römer", Person.EGeschlecht.Männlich, RandomBeruf()), "Crytonix Software", RandomVerkürzt(), RandomImage()));
            schüler.Add(new Schüler(new Person("Garrick", "Clary", Person.EGeschlecht.Männlich, RandomBeruf()), "Network24 GmbH", RandomVerkürzt(), RandomImage()));
            schüler.Add(new Schüler(new Person("Kipp", "True", Person.EGeschlecht.Männlich, RandomBeruf()), "TopHat Software", RandomVerkürzt(), RandomImage()));
            schüler.Add(new Schüler(new Person("Julisa", "Well", Person.EGeschlecht.Weiblich, RandomBeruf()), "Metric Systems Corporation", RandomVerkürzt(), RandomImage()));
            schüler.Add(new Schüler(new Person("Jackson", "Hartzog", Person.EGeschlecht.Männlich, RandomBeruf()), "Alpha Codehouse 31", RandomVerkürzt(), RandomImage()));
            schüler.Add(new Schüler(new Person("Rick", "Creed", Person.EGeschlecht.Männlich, RandomBeruf()), "Alpha Codehouse 31", RandomVerkürzt(), RandomImage()));
            schüler.Add(new Schüler(new Person("Nico", "Westerman", Person.EGeschlecht.Männlich, RandomBeruf()), "Metric Systems Corporation", RandomVerkürzt(), RandomImage()));
            schüler.Add(new Schüler(new Person("Kevin", "Langenfeld", Person.EGeschlecht.Männlich, RandomBeruf()), "Crytonix Software", RandomVerkürzt(), RandomImage()));
            schüler.Add(new Schüler(new Person("Matt", "Lanz", Person.EGeschlecht.Männlich, RandomBeruf()), "Fusion Tech", RandomVerkürzt(), RandomImage()));
            schüler.Add(new Schüler(new Person("Igor", "Solar", Person.EGeschlecht.Männlich, RandomBeruf()), "Chantelle's IT Force", RandomVerkürzt(), RandomImage()));
            schüler.Add(new Schüler(new Person("Edward", "Hysell", Person.EGeschlecht.Männlich, RandomBeruf()), "Aurora Apps", RandomVerkürzt(), RandomImage()));
            schüler.Add(new Schüler(new Person("Eden", "Douthitt", Person.EGeschlecht.Männlich, RandomBeruf()), "Blue Nebula", RandomVerkürzt(), RandomImage()));
            schüler.Add(new Schüler(new Person("Nat", "Defrancisco", Person.EGeschlecht.Männlich, RandomBeruf()), "Sub Zero Apps", RandomVerkürzt(), RandomImage()));
            schüler.Add(new Schüler(new Person("Norris", "Vargas", Person.EGeschlecht.Männlich, RandomBeruf()), "Chantelle's IT Force", RandomVerkürzt(), RandomImage()));
            schüler.Add(new Schüler(new Person("Jonas", "Chacko", Person.EGeschlecht.Männlich, RandomBeruf()), "Ender Tech", RandomVerkürzt(), RandomImage()));
            schüler.Add(new Schüler(new Person("Benny", "Warnock", Person.EGeschlecht.Männlich, RandomBeruf()), "Genius Tech", RandomVerkürzt(), RandomImage()));
            schüler.Add(new Schüler(new Person("Patrick", "Demaria", Person.EGeschlecht.Männlich, RandomBeruf()), "IT Tiara", RandomVerkürzt(), RandomImage()));
            schüler.Add(new Schüler(new Person("Hans", "Cowley", Person.EGeschlecht.Männlich, RandomBeruf()), "Alpha Codehouse 31", RandomVerkürzt(), RandomImage()));
            schüler.Add(new Schüler(new Person("Gerry", "Herzog", Person.EGeschlecht.Männlich, RandomBeruf()), "Aurora Apps", RandomVerkürzt(), RandomImage()));
            schüler.Add(new Schüler(new Person("Oscar", "Wahl", Person.EGeschlecht.Männlich, RandomBeruf()), "Crytonix Software", RandomVerkürzt(), RandomImage()));
            schüler.Add(new Schüler(new Person("Simon", "Shatley", Person.EGeschlecht.Männlich, RandomBeruf()), "Network24 GmbH", RandomVerkürzt(), RandomImage()));
            schüler.Add(new Schüler(new Person("Scott", "Orlandi", Person.EGeschlecht.Männlich, RandomBeruf()), "BuildieTech", RandomVerkürzt(), RandomImage()));
            schüler.Add(new Schüler(new Person("Christopher", "Palumbo", Person.EGeschlecht.Männlich, RandomBeruf()), "Network24 GmbH", RandomVerkürzt(), RandomImage()));
            schüler.Add(new Schüler(new Person("Erick", "Kühl", Person.EGeschlecht.Männlich, RandomBeruf()), "Chantelle's IT Force", RandomVerkürzt(), RandomImage()));

            SchulKlasse klasse = new SchulKlasse(testKlassenName, schüler);

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
            return (Person.EBeruf)random.Next(7);
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


