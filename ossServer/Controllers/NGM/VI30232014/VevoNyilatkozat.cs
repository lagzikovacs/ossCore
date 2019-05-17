using System;
using System.Xml;
using ossServer.Controllers.NGM;

namespace NGM.VI30232014
{
    /// <summary>
    /// Vevő nyilatkozat adatai
    /// </summary>
    public class VevoNyilatkozat
    {
        /// <summary>
        /// Vevő nem fizet: Záradék szövegének feltüntetése (igen/nem).
        /// </summary>
        public bool? VevoNemFizet { get; set; }

        /// <summary>
        /// Iktatott időszak: A záradék szövegében szereplő iktatószám feltüntetése: „a csomagolószer termékdíja a vevő … számon iktatott időszakra vonatkozó nyilatkozata alapján nem kerül megfizetésre”.
        /// </summary>
        public string IktatottIdoszak { get; set; }

        public bool IsEmpty => VevoNemFizet == null && string.IsNullOrEmpty(IktatottIdoszak);

        public void ToXml(XmlWriter xml)
        {
            if (IsEmpty)
                return;

            if (VevoNemFizet == null)
                throw new ArgumentNullException(nameof(VevoNemFizet));

            xml.WriteStartElement("vevo_nyilatkozat");
            {
                XmlTools.WriteValueElement(xml, "vevo_nem_fizet", VevoNemFizet, true);
                XmlTools.WriteValueElement(xml, "iktatott_idoszak", IktatottIdoszak, 100, true);
            }
            xml.WriteEndElement();
        }
    }
}
