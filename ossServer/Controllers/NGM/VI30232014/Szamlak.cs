using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using ossServer.Controllers.NGM;

namespace NGM.VI30232014
{
    /// <summary>
    /// A számlákról készült export állomány adatai
    /// </summary>
    public class Szamlak
    {
        private const string TargetNamespace = "http://schemas.nav.gov.hu/2013/szamla";

        /// <summary>
        /// Előállítás dátuma: Az export fájl előállításának dátuma.
        /// </summary>
        public DateTime EloallitasDatuma { get; set; }

        /// <summary>
        /// Számlák darabszáma: Az adatszolgáltatásba bekerült számlák darabszáma.
        /// </summary>
        public int SzamlakDarabszama => SzamlakAdatai.Count;

        /// <summary>
        /// Kezdő dátum: Az adatszolgáltatás intervallumának kezdő dátuma.
        /// </summary>
        public DateTime? KezdoDatum { get; set; }

        /// <summary>
        /// Záró dátum: Az adatszolgáltatás intervallumának záró dátuma.
        /// </summary>
        public DateTime? ZaroDatum { get; set; }

        /// <summary>
        /// Kezdő számla száma: Az adatszolgáltatásban szereplő számla intervallum első számlájának sorszáma.
        /// </summary>
        public string KezdoSzamlaSzama { get; set; }

        /// <summary>
        /// Záró számla száma: Az adatszolgáltatásban szereplő számla intervallum utolsó számlájának sorszáma.
        /// </summary>
        public string ZaroSzamlaSzama { get; set; }

        public List<Szamla> SzamlakAdatai { get; } = new List<Szamla>();

        public void ToXml(XmlWriter xml)
        {
            xml.WriteStartElement("szamlak", TargetNamespace);
            {
                XmlTools.WriteValueElement(xml, "export_datuma", EloallitasDatuma, true);
                XmlTools.WriteValueElement(xml, "export_szla_db", SzamlakDarabszama, true);
                XmlTools.WriteValueElement(xml, "kezdo_ido", KezdoDatum, true);
                XmlTools.WriteValueElement(xml, "zaro_ido", ZaroDatum, true);
                XmlTools.WriteValueElement(xml, "kezdo_szla_szam", KezdoSzamlaSzama, 100, true);
                XmlTools.WriteValueElement(xml, "zaro_szla_szam", ZaroSzamlaSzama, 100, true);

                foreach (var szamla in SzamlakAdatai)
                    szamla.ToXml(xml);
            }
            xml.WriteEndElement();
        }

        public void SaveToFile(string path)
        {
            var settings = new XmlWriterSettings
            {
                CheckCharacters = true,
                CloseOutput = true, // Emiatt nem kell a filestream-et kézzel bezárni.
                Encoding = Encoding.UTF8,
                Indent = true,
                IndentChars = " ",
                NewLineChars = Environment.NewLine,
                NewLineOnAttributes = false,
                NewLineHandling = NewLineHandling.Entitize
            };

            var file = File.CreateText(path);
            try
            {
                using (var xml = XmlWriter.Create(file, settings))
                    ToXml(xml);
            }
            catch
            {
                if (File.Exists(path))
                    File.Delete(path);
                throw;
            }
        }

        private string GetXsd()
        {
            //var names = assembly.GetManifestResourceNames();
            var resourceName = $"{nameof(NGM)}.{nameof(VI30232014)}.ElektronikusSzamlakAdatai.xsd";
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
                throw new Exception($"Nem találom a(z) '{resourceName}' resource-t.");
            using (StreamReader reader = new StreamReader(stream))
                return reader.ReadToEnd();
        }

        public List<string> Validate(Stream input)
        {
            var schemas = new XmlSchemaSet();
            schemas.Add(TargetNamespace, XmlReader.Create(new StringReader(GetXsd())));
            var xdoc = XDocument.Load(input);

            var errors = new List<string>();
            xdoc.Validate(schemas, (sedner, validationEventArgs) =>
            {
                errors.Add($"{validationEventArgs.Exception.LineNumber}, {validationEventArgs.Exception.LinePosition}, {validationEventArgs.Severity}: {validationEventArgs.Message}");
            });
            return errors;
        }

        public List<string> Validate(string path)
        {
            using (var stream = File.OpenRead(path))
                return Validate(stream);
        }
    }
}
