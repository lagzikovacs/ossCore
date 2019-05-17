using System.Xml;
using ossServer.Controllers.NGM;

namespace NGM.VI30232014
{
    /// <summary>
    /// Számla végösszegének adatai
    /// </summary>
    public class Vegosszeg
    {
        /// <summary>
        /// Nettó ár összesen. Csak "számla" típusú számla esetében kell kitölteni.
        /// </summary>
        public decimal? NettoArOsszesen { get; set; }

        /// <summary>
        /// Áfa érték összesen. Csak "számla" típusú számla esetében kell kitölteni.
        /// </summary>
        public decimal? AfaOsszesen { get; set; }

        /// <summary>
        /// Bruttó ár összesen. Csak "számla" típusú számla esetében kell kitölteni.
        /// </summary>
        public decimal? BruttoOsszesen { get; set; }

        /// <summary>
        /// Csak egyszerűsített adattartalmú számla esetén kell kitölteni. Amennyiben nem számlatételenként tünteti fel a százalékértéket, akkor itt kell megadni(max. 2 tizedesjegy).
        /// </summary>
        public decimal? AfaTartalom { get; set; }

        public void ToXml(XmlWriter xml)
        {
            xml.WriteStartElement("vegosszeg");
            {
                XmlTools.WriteValueElement(xml, "nettoarossz", NettoArOsszesen, false);
                XmlTools.WriteValueElement(xml, "afaertekossz", AfaOsszesen, false);
                XmlTools.WriteValueElement(xml, "bruttoarossz", BruttoOsszesen, false);
                XmlTools.WriteValueElement(xml, "afa_tartalom", AfaTartalom, false);
            }
            xml.WriteEndElement();
        }
    }
}
