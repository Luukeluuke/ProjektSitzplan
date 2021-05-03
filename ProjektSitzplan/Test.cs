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
        public static void TestFunction()
        {
            Tisch tisch1 = new Tisch(6);
            Tisch tisch2 = new Tisch(6);
            Tisch tisch3 = new Tisch(6);
            Tisch tisch4 = new Tisch(6);

            Betrieb idemia = new Betrieb("Idemia");
            Betrieb ENKreis = new Betrieb("Ennepe-Ruhr-Kreis");

            Schüler sweer = new Schüler("Sweer", "Sülberg", Person.EGeschlecht.Männlich, Person.EBeruf.Anwendungsentwicklung, idemia);
            Schüler luca = new Schüler("Luca", "Berger", Person.EGeschlecht.Männlich, Person.EBeruf.Anwendungsentwicklung, ENKreis);
            Schüler björn = new Schüler("Björn", "Nölle", Person.EGeschlecht.Männlich, Person.EBeruf.SystemIntegration, ENKreis);

            Lehrer lehrer = new Lehrer("Sebastian", "Wieschollek", Person.EGeschlecht.Männlich, Person.EBeruf.Lehrer);

            SchulKlasse klasse = new SchulKlasse(lehrer);

            klasse.SchülerHinzufügen(sweer);
            klasse.SchülerHinzufügen(luca);
            klasse.SchülerHinzufügen(björn);

            Sitzplan sitzplan = new Sitzplan(6, klasse);

            string path = @"test\abc\123\sitzplan1.json";

            sitzplan.AlsDateiSpeichern(path);
            sitzplan = Sitzplan.AusDateiLaden(path);

            Print(sitzplan.ToString());
        }

        public static void Print(string s)
        {
            MessageBox.Show(s);
        }
    }
}