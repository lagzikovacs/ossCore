using System;
using System.Xml;
using ossServer.Controllers.NGM;

namespace NGM.VI30232014
{
    /// <summary>
    /// Gázolaj beszerzéséről szóló számla adatai. Gázolaj beszerzéséről szóló számlán a kereskedelmi jármű forgalmi rendszámát és a kilométeróra állását is fel kell tüntetni.
    /// </summary>
    public class GazolajBeszerzesSzamla
    {
        /// <summary>
        /// Jármű forgalmi rendszáma
        /// </summary>
        public string Rendszam { get; set; }

        /// <summary>
        /// Kilométeróra állása
        /// </summary>
        public decimal? KilometeroraAllas { get; set; }

        public bool IsEmpty => string.IsNullOrEmpty(Rendszam) && KilometeroraAllas == null;

        public void ToXml(XmlWriter xml)
        {
            if (IsEmpty)
                return;

            Utils.ExceptionIfEmpty(nameof(Rendszam), Rendszam);
            if (KilometeroraAllas == null)
                throw new ArgumentNullException(nameof(KilometeroraAllas));

            xml.WriteStartElement("gazolaj_beszerz");
            {
                XmlTools.WriteValueElement(xml, "rendszam", Rendszam, 20, true);
                XmlTools.WriteValueElement(xml, "km_ora_allas", KilometeroraAllas, true);
            }
            xml.WriteEndElement();
        }
    }
}
