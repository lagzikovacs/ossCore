using System;
using System.Xml;
using ossServer.Controllers.NGM;

namespace NGM.VI30232014
{
    /// <summary>
    /// Szárazföldi közlekedési eszköz adatai
    /// </summary>
    public class SzarazfoldiKozlekedesiEszkoz
    {
        /// <summary>
        /// Henger űrtartalom
        /// </summary>
        public decimal? HengerUrtartalom { get; set; }

        /// <summary>
        /// Teljesítmény
        /// </summary>
        public decimal? Teljesitmeny { get; set; }

        /// <summary>
        /// Első forgalomba helyezés időpontja
        /// </summary>
        public DateTime? ElsoForgalombaHelyezesIdopontja { get; set; }

        /// <summary>
        /// Megtett távolság
        /// </summary>
        public decimal? MegtettTavolsag { get; set; }

        public bool IsEmpty => HengerUrtartalom == null && Teljesitmeny == null && ElsoForgalombaHelyezesIdopontja == null && MegtettTavolsag == null;

        public void ToXml(XmlWriter xml)
        {
            if (IsEmpty)
                return;

            xml.WriteStartElement("foldikozl");
            {
                XmlTools.WriteValueElement(xml, "hengerur", HengerUrtartalom, false);
                XmlTools.WriteValueElement(xml, "teljesitmeny", Teljesitmeny, false);
                XmlTools.WriteValueElement(xml, "forgba_datum", ElsoForgalombaHelyezesIdopontja, false);
                XmlTools.WriteValueElement(xml, "futottkm", MegtettTavolsag, false);
            }
            xml.WriteEndElement();
        }
    }
}
