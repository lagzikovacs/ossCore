using System.Xml;
using ossServer.Controllers.NGM;

namespace NGM.VI30232014
{
    /// <summary>
    /// „a termékdíj kötelezettség a Ktdt. 14. § (5) bekezdés ...) pontja ...) alpontja alapján a vevőt terheli” szövegű záradékot kell feltüntetni a termékdíj-kötelezettség szerződés alapján történő átvállalása esetén a számlán.
    /// </summary>
    public class TermekdijSzerzodesesAtvallalas
    {
        /// <summary>
        /// Termékdíj szerződéses átvállalás bekezdés pontja: A temékdíj-kötelezettség szerződéses átvállalásának törvényben meghatározott pontja.
        /// </summary>
        public string BekezdesPontja { get; set; }

        /// <summary>
        /// Termékdíj szerződéses átvállalás bekezdés alpontja: A termékdíj-kötelezettség szerződéses átvállalásának törvényben meghatározott alpontja.
        /// </summary>
        public string BekezdesAlpontja { get; set; }

        public bool IsEmpty => string.IsNullOrEmpty(BekezdesPontja) && string.IsNullOrEmpty(BekezdesAlpontja);

        public void ToXml(XmlWriter xml)
        {
            if (IsEmpty)
                return;

            xml.WriteStartElement("termekdij_szerzes_atvallalas");
            {
                XmlTools.WriteValueElement(xml, "bekezdes_pontja", BekezdesPontja, 100, true);
                XmlTools.WriteValueElement(xml, "bekezdes_alpontja", BekezdesAlpontja, 100, true);
            }
            xml.WriteEndElement();
        }
    }
}
