﻿using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;

namespace ProjektSitzplan.Structures
{
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
        public Betrieb AusbildungsBetrieb;

        [JsonIgnore]
        public string Betrieb => AusbildungsBetrieb.Name;


        public bool Verkürzt;

        public byte[] bildBytes => BildZuBytes(Bild);

        [JsonIgnore]
        public Image Bild;

        public Schüler(Person person, Betrieb ausbildungsBetrieb, bool verkürzt = false, Image bild = null) : base(person)
        {
            AusbildungsBetrieb = ausbildungsBetrieb;
            Verkürzt = verkürzt;
            Bild = bild;
        }

        public Schüler(Schüler schüler) : base(schüler)
        {
            AusbildungsBetrieb = schüler.AusbildungsBetrieb;
            Verkürzt = schüler.Verkürzt;
            Bild = schüler.Bild;
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

            Image bild2 = new Bitmap(bild);

            try
            {
                bytes = (byte[])converter.ConvertTo(bild2, typeof(byte[]));
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

        [JsonConstructor]
        public Schüler(string vorname, string nachname, EGeschlecht geschlecht, EBeruf beruf, Betrieb ausbildungsBetrieb, bool verkürzt, byte[] bildBytes) : base(vorname, nachname, geschlecht, beruf)
        {
            AusbildungsBetrieb = ausbildungsBetrieb;
            Verkürzt = verkürzt;

            Bild = (bildBytes != null && bildBytes.Length > 0) ? BytesZuBild(bildBytes) : null;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, Betrieb: {AusbildungsBetrieb.Name}, Verkürzt: {Verkürzt}";
        }
    }
}