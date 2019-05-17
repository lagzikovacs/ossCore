using System;
using System.Xml;
using ossServer.Controllers.NGM;

namespace NGM.VI30232014
{
    /// <summary>
    /// Vízi közlekedési eszköz adatai
    /// </summary>
    public class ViziKozlekedesiEszkoz
    {
        /// <summary>
        /// Hosszúság
        /// </summary>
        public decimal? Hosszusag { get; set; }

        /// <summary>
        /// Tevékenység
        /// </summary>
        public string Tevekenyseg { get; set; }

        /// <summary>
        /// Első forgalomba helyezés időpontja
        /// </summary>
        public DateTime? ElsoForgalombaHelyezesIdopontja { get; set; }

        /// <summary>
        /// Hajózott órák száma
        /// </summary>
        public decimal? HajozottOrakSzama { get; set; }

        public bool IsEmpty => Hosszusag == null && string.IsNullOrEmpty(Tevekenyseg) && ElsoForgalombaHelyezesIdopontja == null && HajozottOrakSzama == null;

        public void ToXml(XmlWriter xml)
        {
            if (IsEmpty)
                return;

            xml.WriteStartElement("vizikozl");
            {
                XmlTools.WriteValueElement(xml, "hajo_hossz", Hosszusag, false);
                XmlTools.WriteValueElement(xml, "vizitev", Tevekenyseg, 100, false);
                XmlTools.WriteValueElement(xml, "forgba_datum", ElsoForgalombaHelyezesIdopontja, false);
                XmlTools.WriteValueElement(xml, "hajozott_ora", HajozottOrakSzama, false);
            }
            xml.WriteEndElement();
        }
    }
}
