using System;
using System.Xml;
using ossServer.Controllers.NGM;

namespace NGM.VI30232014
{
    /// <summary>
    /// Fejléc adai
    /// </summary>
    public class Fejlec
    {
        /// <summary>
        /// Számla sorszáma: A számla sorszáma, amely a számlát kétséget kizáróan azonosítja.
        /// </summary>
        public string SzamlaSorszama { get; set; }

        /// <summary>
        /// Számla típusa: Számla lehetséges típusai:<br />
        /// · számla<br />
        /// · egyszerűsített adattartalmú számla<br />
        /// · módosító számla<br />
        /// · érvénytelenítő számla<br />
        /// · gyűjtőszámla<br />
        /// · számlával egy tekintet alá eső okirat.
        /// </summary>
        public SzamlaTipusok SzamlaTipusa { get; set; }

        /// <summary>
        /// Számla kelte: A számla kibocsátásának kelte.
        /// </summary>
        public DateTime SzamlaKelte { get; set; }

        /// <summary>
        /// Teljesítés dátuma:<br />
        /// · a teljesítés időpontja, vagy<br />
        /// · előleg fizetése esetében a fizetendő adó megállapítás időpontja, ha az eltér a számla kibocsátásának keltétől.
        /// </summary>
        public DateTime TeljesitesDatuma { get; set; }

        public void ToXml(XmlWriter xml)
        {
            Utils.ExceptionIfEmpty(nameof(SzamlaSorszama), SzamlaSorszama);

            xml.WriteStartElement("fejlec");
            {
                XmlTools.WriteValueElement(xml, "szlasorszam", SzamlaSorszama, 100, true);
                XmlTools.WriteValueElement(xml, "szlatipus", (int)SzamlaTipusa, true);
                XmlTools.WriteValueElement(xml, "szladatum", SzamlaKelte, true);
                XmlTools.WriteValueElement(xml, "teljdatum", TeljesitesDatuma, true);
            }
            xml.WriteEndElement();
        }
    }
}
