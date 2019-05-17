using System;
using System.Xml;
using ossServer.Controllers.NGM;

namespace NGM.VI30232014
{
    /// <summary>
    /// Számlabefogadó adatai
    /// </summary>
    public class SzamlaBefogado
    {
        /// <summary>
        /// Számlabefogadó adószáma: A termék beszerzőjének, szolgáltatás igénybevevőjének adószáma.
        /// </summary>
        public string Adoszam { get; set; }

        /// <summary>
        /// Közösségi adószám: A termék beszerzőjének, szolgáltatás igénybevevőjének közösségi adószáma.
        /// </summary>
        public string KozossegiAdoszam { get; set; }

        /// <summary>
        /// Számlabefogadó neve: A termék beszerzőjének, szolgáltatás igénybevevőjének neve.
        /// </summary>
        public string Nev { get; set; }

        /// <summary>
        /// Számlabefogadó címe: A termék beszerzőjének, szolgáltatás igénybevevőjének címe.
        /// </summary>
        public Cim Cim { get; } = new Cim();

        public void ToXml(XmlWriter xml)
        {
            Utils.ExceptionIfEmpty(nameof(Nev), Nev);
            if (Cim.IsEmpty)
                throw new Exception("A számla befogadó címét kötelező kitölteni.");

            xml.WriteStartElement("vevo");
            {
                XmlTools.WriteValueElement(xml, "adoszam", Adoszam, 20, false); // 8-20 karakter
                XmlTools.WriteValueElement(xml, "kozadoszam", KozossegiAdoszam, 20, false); // 8-20 karakter
                XmlTools.WriteValueElement(xml, "nev", Nev, 100, true);
                Cim.ToXml(xml, "cim");
            }
            xml.WriteEndElement();
        }
    }
}
