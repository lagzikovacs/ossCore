using System.Xml;
using ossServer.Controllers.NGM;

namespace NGM.VI30232014
{
    public class AfaRovat
    {
        /// <summary>
        /// Adóalap
        /// </summary>
        public decimal NettoAr { get; set; }

        /// <summary>
        /// Adókulcs
        /// </summary>
        public decimal Adokulcs { get; set; }

        /// <summary>
        /// Adó
        /// </summary>
        public decimal AdoErtek { get; set; }

        /// <summary>
        /// Ellenérték
        /// </summary>
        public decimal BruttoAr { get; set; }

        public void ToXml(XmlWriter xml)
        {
            xml.WriteStartElement("afarovat");
            {
                XmlTools.WriteValueElement(xml, "nettoar", NettoAr, true);
                XmlTools.WriteValueElement(xml, "adokulcs", Adokulcs, true);
                XmlTools.WriteValueElement(xml, "adoertek", AdoErtek, true);
                XmlTools.WriteValueElement(xml, "bruttoar", BruttoAr, true);
            }
            xml.WriteEndElement();
        }
    }
}
