using System;
using System.Xml;
using ossServer.Controllers.NGM;

namespace NGM.VI30232014
{
    /// <summary>
    /// Légi közlekedési eszköz adatai
    /// </summary>
    public class LegiKozlekedesiEszkoz
    {
        /// <summary>
        /// Teljes felszállási tömeg
        /// </summary>
        public decimal? TeljesFelszallasiTomeg { get; set; }

        /// <summary>
        /// Légi kereskedelem
        /// </summary>
        public bool? LegiKereskedelem { get; set; }

        /// <summary>
        /// Első forgalomba helyezés időpontja
        /// </summary>
        public DateTime? ElsoForgalombaHelyezesIdopontja { get; set; }

        /// <summary>
        /// Repült órák száma
        /// </summary>
        public decimal? RepultOrakSzama { get; set; }

        public bool IsEmpty => TeljesFelszallasiTomeg == null && LegiKereskedelem == null && ElsoForgalombaHelyezesIdopontja == null && RepultOrakSzama == null;

        public void ToXml(XmlWriter xml)
        {
            if (IsEmpty)
                return;

            xml.WriteStartElement("legikozl");
            {
                XmlTools.WriteValueElement(xml, "felsztomeg", TeljesFelszallasiTomeg, false);
                XmlTools.WriteValueElement(xml, "legiker", LegiKereskedelem, false);
                XmlTools.WriteValueElement(xml, "forgba_datum", ElsoForgalombaHelyezesIdopontja, false);
                XmlTools.WriteValueElement(xml, "repultora", RepultOrakSzama, false);
            }
            xml.WriteEndElement();
        }
    }
}
