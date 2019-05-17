using System.Xml;
using ossServer.Controllers.NGM;

namespace NGM.VI30232014
{
    /// <summary>
    /// Cím adatai
    /// </summary>
    public class Cim
    {
        /// <summary>
        /// Irányítószám
        /// </summary>
        public string Iranyitoszam { get; set; }

        /// <summary>
        /// Település
        /// </summary>
        public string Telepules { get; set; }

        /// <summary>
        /// Kerület
        /// </summary>
        public string Kerulet { get; set; }

        /// <summary>
        /// Közterület neve
        /// </summary>
        public string KozteruletNeve { get; set; }

        /// <summary>
        /// Közterület jellege
        /// </summary>
        public string KozteruletJellege { get; set; }

        /// <summary>
        /// Házszám
        /// </summary>
        public string Hazszam { get; set; }

        /// <summary>
        /// Épület
        /// </summary>
        public string Epulet { get; set; }

        /// <summary>
        /// Lépcsőház
        /// </summary>
        public string Lepcsohaz { get; set; }

        /// <summary>
        /// Szint
        /// </summary>
        public string Szint { get; set; }

        /// <summary>
        /// Ajtó
        /// </summary>
        public string Ajto { get; set; }

        public bool IsEmpty => string.IsNullOrEmpty(Iranyitoszam)
                               && string.IsNullOrEmpty(Telepules)
                               && string.IsNullOrEmpty(Kerulet)
                               && string.IsNullOrEmpty(KozteruletNeve)
                               && string.IsNullOrEmpty(KozteruletJellege)
                               && string.IsNullOrEmpty(Hazszam)
                               && string.IsNullOrEmpty(Epulet)
                               && string.IsNullOrEmpty(Lepcsohaz)
                               && string.IsNullOrEmpty(Szint)
                               && string.IsNullOrEmpty(Ajto);

        public void ToXml(XmlWriter xml, string elementName)
        {
            Utils.ExceptionIfEmpty(nameof(Iranyitoszam), Iranyitoszam);
            Utils.ExceptionIfEmpty(nameof(Telepules), Iranyitoszam);
            Utils.ExceptionIfEmpty(nameof(KozteruletNeve), Iranyitoszam);

            xml.WriteStartElement(elementName);
            {
                XmlTools.WriteValueElement(xml, "iranyitoszam", Iranyitoszam, 10, true); // Vizsgálni kellene azt is hogy min. 4 karakter...
                XmlTools.WriteValueElement(xml, "telepules", Telepules, 100, true);
                XmlTools.WriteValueElement(xml, "kerulet", Kerulet, 100, false);
                XmlTools.WriteValueElement(xml, "kozterulet_neve", KozteruletNeve, 100, true);
                XmlTools.WriteValueElement(xml, "kozterulet_jellege", KozteruletJellege, 100, false);
                XmlTools.WriteValueElement(xml, "hazszam", Hazszam, 100, false);
                XmlTools.WriteValueElement(xml, "epulet", Epulet, 100, false);
                XmlTools.WriteValueElement(xml, "lepcsohaz", Lepcsohaz, 100, false);
                XmlTools.WriteValueElement(xml, "szint", Szint, 100, false);
                XmlTools.WriteValueElement(xml, "ajto", Ajto, 100, false);
            }
            xml.WriteEndElement();
        }
    }
}
