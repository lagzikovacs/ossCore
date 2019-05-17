using System.Xml;
using ossServer.Controllers.NGM;

namespace NGM.VI30232014
{
    /// <summary>
    /// Módosító számla adatai
    /// </summary>
    public class ModositoSzamla
    {
        /// <summary>
        /// Eredeti számla sorszáma: Hivatkozás arra a számlára, amelynek adattartalmát módosítja.
        /// </summary>
        public string EredetiSzamlaSorszama { get; set; }

        public void ToXml(XmlWriter xml)
        {
            xml.WriteStartElement("modosito_szla");
            {
                XmlTools.WriteValueElement(xml, "eredeti_sorszam", EredetiSzamlaSorszama, 100, true);
            }
            xml.WriteEndElement();
        }
    }
}
