using System;
using System.Xml;
using ossServer.Controllers.NGM;

namespace NGM.VI30232014
{
    /// <summary>
    /// Nem kötelező elemek adatai
    /// </summary>
    public class NemKotelezo
    {
        /// <summary>
        /// Fizetési határidő: A számla teljesítésének határideje.
        /// </summary>
        public DateTime? FizetesiHatarido { get; set; }

        /// <summary>
        /// Fizetés módja: A számla fizetési teljesítésének módja.
        /// </summary>
        public string FizetesModja { get; set; }

        /// <summary>
        /// Számla pénzneme: A számlán szereplő összegek pénzneme.
        /// </summary>
        public string SzamlaPenzneme { get; set; }

        /// <summary>
        /// Számla megjelenési formája: Számla megjelenési formája: e-számla / EDI számla / papír alapon továbbított számla / papír alapon előállított, de elektronikus formában továbbított számla.
        /// </summary>
        public string SzamlaMegjelenesiFormaja { get; set; }

        /// <summary>
        /// Számlakibocsátó bankszámlaszáma: A termék értékesítőjének/szolgáltatás nyújtójának bankszámlaszáma.
        /// </summary>
        public string SzamlakibocsatoBankszamlaszama { get; set; }

        /// <summary>
        /// Számlabefogadó bankszámlaszáma: A termék beszerzőjének/szolgáltatás igénybevevőjének bankszámlaszáma.
        /// </summary>
        public string SzamlabefogadoBankszamlaszama { get; set; }

        public bool IsEmpty => FizetesiHatarido == null
                               && string.IsNullOrEmpty(FizetesModja)
                               && string.IsNullOrEmpty(SzamlaPenzneme)
                               && string.IsNullOrEmpty(SzamlaMegjelenesiFormaja)
                               && string.IsNullOrEmpty(SzamlakibocsatoBankszamlaszama)
                               && string.IsNullOrEmpty(SzamlabefogadoBankszamlaszama);

        public void ToXml(XmlWriter xml)
        {
            if (IsEmpty)
                return;

            xml.WriteStartElement("nem_kotelezo");
            {
                    XmlTools.WriteValueElement(xml, "fiz_hatarido", FizetesiHatarido, false);
                    XmlTools.WriteValueElement(xml, "fiz_mod", FizetesModja, 100, false);
                    XmlTools.WriteValueElement(xml, "penznem", SzamlaPenzneme, 100, false);
                    XmlTools.WriteValueElement(xml, "szla_forma", SzamlaMegjelenesiFormaja, 100, false);
                    XmlTools.WriteValueElement(xml, "kibocsato_bankszla", SzamlakibocsatoBankszamlaszama, 100, false);
                    XmlTools.WriteValueElement(xml, "befogado_bankszla", SzamlabefogadoBankszamlaszama, 100, false);
            }
            xml.WriteEndElement();
        }
    }
}
