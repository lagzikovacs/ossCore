using System.Xml;
using ossServer.Controllers.NGM;

namespace NGM.VI30232014
{
    /// <summary>
    /// Tranzitadóraktár-engedélyes a tranzitadóraktárból történő értékesítés esetén: A beszállókártya számát a tranzitadóraktár-engedélyes az értékesítésről kiállított számlán köteles feltüntetni.<br/>
    /// Beszállókártya szám alatt az alábbi adatok együttes adatát kell érteni:<br/>
    /// - a járatszámot,<br/>
    /// - a kedvezményre jogosult nevét,<br/>
    /// - a beszállókártyán szereplő úti célt.
    /// </summary>
    public class Beszallokartya
    {
        /// <summary>
        /// Járatszám
        /// </summary>
        public string Jaratszam { get; set; }

        /// <summary>
        /// Kedvezményre jogosult neve
        /// </summary>
        public string KedvezmenyezettNeve { get; set; }

        /// <summary>
        /// Beszállókártyán szereplő uticél
        /// </summary>
        public string Uticel { get; set; }

        public bool IsEmpty => string.IsNullOrEmpty(Jaratszam)
                               && string.IsNullOrEmpty(KedvezmenyezettNeve)
                               && string.IsNullOrEmpty(Uticel);

        public void ToXml(XmlWriter xml)
        {
            if (IsEmpty)
                return;

            xml.WriteStartElement("beszallokartya");
            {
                XmlTools.WriteValueElement(xml, "jaratszam", Jaratszam, 100, true);
                XmlTools.WriteValueElement(xml, "kedv_neve", KedvezmenyezettNeve, 100, true);
                XmlTools.WriteValueElement(xml, "uticel", Uticel, 100, true);
            }
            xml.WriteEndElement();
        }
    }
}
