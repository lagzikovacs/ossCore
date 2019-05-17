using System;
using System.Xml;
using ossServer.Controllers.NGM;

namespace NGM.VI30232014
{
    /// <summary>
    /// Képviselő adatai
    /// </summary>
    public class Kepviselo
    {
        /// <summary>
        /// Képviselő adószáma: Pénzügyi képviselő alkalmazása esetében a pénzügyi képviselő adószáma.
        /// </summary>
        public string Adoszam { get; set; }

        /// <summary>
        /// Képviselő neve: Pénzügyi képviselő alkalmazása esetében a pénzügyi képviselő neve (a számla kiállítója).
        /// </summary>
        public string Nev { get; set; }

        /// <summary>
        /// Képviselő címe: Pénzügyi képviselő alkalmazása esetében a pénzügyi képviselő címe.
        /// </summary>
        public Cim Cim { get; } = new Cim();

        public bool IsEmpty => string.IsNullOrEmpty(Adoszam) && string.IsNullOrEmpty(Nev) && Cim.IsEmpty;

        public void ToXml(XmlWriter xml)
        {
            if (IsEmpty)
                return;

            Utils.ExceptionIfEmpty(nameof(Nev), Nev);
            if (Cim.IsEmpty)
                throw new Exception("A képviselő címét kötelező kitölteni.");

            xml.WriteStartElement("kepviselo");
            {
                XmlTools.WriteValueElement(xml, "adoszam", Adoszam, 20, false); // 8-20 karakter
                XmlTools.WriteValueElement(xml, "nev", Nev, 100, true);
                Cim.ToXml(xml, "cim");
            }
            xml.WriteEndElement();
        }
    }
}
