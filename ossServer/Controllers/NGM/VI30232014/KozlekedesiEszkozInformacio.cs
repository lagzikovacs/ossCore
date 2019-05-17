using System.Xml;

namespace NGM.VI30232014
{
    /// <summary>
    /// Közlekedési eszköz információk: Az új közlekedési eszköz számlatétel információinak csoportja.
    /// </summary>
    public class KozlekedesiEszkozInformacio
    {
        /// <summary>
        /// Szárazföldi közlekedési eszköz adatai
        /// </summary>
        public SzarazfoldiKozlekedesiEszkoz SzarazfoldiKozlekedesiEszkozAdatai { get; } = new SzarazfoldiKozlekedesiEszkoz();

        /// <summary>
        /// Légi közlekedési eszköz adatai
        /// </summary>
        public LegiKozlekedesiEszkoz LegiKozlekedesiEszkozAdatai { get; } = new LegiKozlekedesiEszkoz();

        /// <summary>
        /// Vízi közlekedési eszköz adatai
        /// </summary>
        public ViziKozlekedesiEszkoz ViziKozlekedesiEszkozAdatai { get; } = new ViziKozlekedesiEszkoz();

        public bool IsEmpty => SzarazfoldiKozlekedesiEszkozAdatai.IsEmpty && LegiKozlekedesiEszkozAdatai.IsEmpty && ViziKozlekedesiEszkozAdatai.IsEmpty;

        public void ToXml(XmlWriter xml)
        {
            if (IsEmpty)
                return;

            xml.WriteStartElement("kozl_eszk_inf");
            {
                SzarazfoldiKozlekedesiEszkozAdatai.ToXml(xml);
                LegiKozlekedesiEszkozAdatai.ToXml(xml);
                ViziKozlekedesiEszkozAdatai.ToXml(xml);
            }
            xml.WriteEndElement();
        }
    }
}
