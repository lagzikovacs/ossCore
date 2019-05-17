using System.Xml;
using ossServer.Controllers.NGM;

namespace NGM.VI30232014
{
    /// <summary>
    /// Gyűjtőszámla adatai
    /// </summary>
    public class Gyujtoszamla
    {
        /// <summary>
        /// Gyűjtőszámla csoport jelölője: A gyűjtőszámlába foglalás csoportosító eleme, általában dátum, vagy annak valamely része.
        /// </summary>
        public string Jelolo { get; set; }

        /// <summary>
        /// Gyűjtő nettó összesen: Gyűjtőcsoportba tartozó számlatételek nettó összege.
        /// </summary>
        public decimal NettoOsszesen { get; set; }

        /// <summary>
        /// Gyűjtő bruttó összesen: Gyűjtőcsoportba tartozó számlatételek bruttó összege.
        /// </summary>
        public decimal BruttoOsszesen { get; set; }

        public void ToXml(XmlWriter xml)
        {
            Utils.ExceptionIfEmpty(nameof(Jelolo), Jelolo);

            xml.WriteStartElement("gyujto_szla");
            {
                XmlTools.WriteValueElement(xml, "gyujtocsopo_ossz", Jelolo, 100, true);
                XmlTools.WriteValueElement(xml, "gyujtocsopo_nossz", NettoOsszesen, true);
                XmlTools.WriteValueElement(xml, "gyujtocsopo_bossz", BruttoOsszesen, true);
            }
            xml.WriteEndElement();
        }
    }
}
