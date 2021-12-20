using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

namespace ProjektSitzplan.Structures
{
    public static class SchülerCSVParser
    {

        public static string[] TeileZeile(string zeile)
        {
            string[] split = zeile.Split(';').Select(s => s.Trim()).ToArray();

            if (split.Length == 0)
            {
                return zeile.Split(';').Select(s => s.Trim()).ToArray();
            }

            return split;
        }

        public static Schüler SchülerAusCSVString(string csv, Dictionary<string, int> anordnung, out string fehler)
        {
            string[] split = TeileZeile(csv);

            fehler = "Keiner";

            if (split.Length < 5 || split.Any(s => string.IsNullOrWhiteSpace(s)))
            {
                fehler = "Bei {0} Schülern fehlen ein oder mehrere Attribute.";
                return null;
            }

            Person.EGeschlecht geschlecht;
            if (!Enum.TryParse(split[anordnung["geschlecht"]].Replace(" ", ""), true, out geschlecht))
            {
                fehler = "Bei {0} Schülern konnte das Geschlecht nicht ermittelt werden.";
                return null;
            }

            Person.EBeruf beruf;
            if (!Enum.TryParse(split[anordnung["beruf"]].Replace(" ", ""), true, out beruf))
            {
                fehler = "Bei {0} Schülern wahr die Berufsbezeichnung ist inkorrekt.";
                return null;
            }

            return new Schüler(new Person(split[anordnung["vorname"]], split[anordnung["nachname"]], geschlecht, beruf), split[anordnung["betrieb"]]);
        }
    }

    public static class SchülerHelfer
    {
        public static Schüler SchülerViaId(List<Schüler> schülerList, string id)
        {
            return schülerList.FirstOrDefault(schüler => schüler.UniqueId == id);
        }

        public static Image SchülerBildDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            const string bmp = "Bitmap Files (*.bmp)|*.bmp";
            const string tiff = "TIFF Files (*.tif, *.tiff)|*.tif;*.tiff";
            const string jpeg = "JPEG Files (*.jpg, *.jpeg)|*.jpg;*.jpeg";
            const string png = "PNG Files (*.png)|*.png";

            openFileDialog.Filter = $"{jpeg}|{png}|{bmp}|{tiff}";
            openFileDialog.InitialDirectory = $@"{Environment.CurrentDirectory}\SchulKlassen";
            if (openFileDialog.ShowDialog() == true)
            {
                return Schüler.LadeBildAusPfad(openFileDialog.FileName);
            }

            return null;
        }
    }

    public class Schüler : Person
    {
        public string Betrieb { get; set; }

        [JsonIgnore]
        public BitmapImage BildBitmap { get => GetBitmapBild(); }

        public bool Verkuerzt;

        public byte[] bildBytes => BildZuBytes(Bild);

        [JsonIgnore]
        public Image Bild;

        [JsonConstructor]
        public Schüler(string vorname, string nachname, EGeschlecht geschlecht, EBeruf beruf, string betrieb, bool verkürzt, byte[] bildBytes) : base(vorname, nachname, geschlecht, beruf)
        {
            Betrieb = betrieb;
            Verkuerzt = verkürzt;

            Bild = (bildBytes != null && bildBytes.Length > 0) ? BytesZuBild(bildBytes) : null;
        }

        public Schüler(Person person, string betrieb, bool verkuerzt = false, Image bild = null) : base(person)
        {
            Betrieb = betrieb;
            Verkuerzt = verkuerzt;
            Bild = bild;
        }

        public Schüler(Schüler schüler) : base(schüler)
        {
            Betrieb = schüler.Betrieb;
            Verkuerzt = schüler.Verkuerzt;
            Bild = schüler.Bild;
        }

        public BitmapImage GetBitmapBild()
        {
            if (Bild is null) return null;

            using (var memory = new MemoryStream())
            {
                System.Drawing.Image bildKopie = new System.Drawing.Bitmap(Bild);
                bildKopie.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);

                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        public static Image LadeBildAusPfad(string pfad)
        {
            try
            {
                return Image.FromFile(pfad);
            }
            catch (IOException) { }
            catch (SystemException) { }

            return null;
        }

        private static byte[] BildZuBytes(Image bild)
        {
            if (bild == null)
            {
                return null;
            }
            ImageConverter converter = new ImageConverter();

            byte[] bytes = null;

            // Dies kopie wird benötigt, weil das Image objekt "bild" gesperrt ist, um dies also als bytearray zu konvertieren wird temporär eine kopie erstellt

            Image bildKopie;
            lock (bild)
            {
                bildKopie = new Bitmap(bild);
            }


            try
            {
                bytes = (byte[])converter.ConvertTo(bildKopie, typeof(byte[]));
            }
            catch (ArgumentNullException) { }
            catch (NotSupportedException) { }

            return bytes;
        }

        private static Image BytesZuBild(byte[] bytes)
        {
            if (bytes == null || bytes.Length < 2)
            {
                return null;
            }

            using (var ms = new MemoryStream(bytes))
            {
                return Image.FromStream(ms);
            }
        }

        public string CSVString => $"{Vorname},{Nachname},{Beruf},{Betrieb},{Geschlecht}";

        public override string ToString()
        {
            return $"{base.ToString()}, Betrieb: {Betrieb}, Verkürzt: {Verkuerzt}";
        }
    }
}