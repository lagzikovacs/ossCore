using System.Xml;
using ossServer.Controllers.NGM;

namespace NGM.VI30232014
{
    /// <summary>
    /// Visszaigénylés tétel adatai
    /// </summary>
    public class VisszaigenylesTetel
    {
        /// <summary>
        /// CsK-kód
        /// </summary>
        public string CskKod { get; set; }

        /// <summary>
        /// KT-kód
        /// </summary>
        public string KtKod { get; set; }

        /// <summary>
        /// Termékdíj összeg
        /// </summary>
        public decimal TermekdijOsszeg { get; set; }

        public void ToXml(XmlWriter xml)
        {
            xml.WriteStartElement("vissz_igenytetel");
            {
                XmlTools.WriteValueElement(xml, "", CskKod, 100, false);
                XmlTools.WriteValueElement(xml, "", KtKod, 100, false);
                XmlTools.WriteValueElement(xml, "", TermekdijOsszeg, true);
            }
            xml.WriteEndElement();
        }
    }
}
