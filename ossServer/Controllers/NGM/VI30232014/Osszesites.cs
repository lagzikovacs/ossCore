using System.Collections.Generic;
using System.Xml;

namespace NGM.VI30232014
{
    /// <summary>
    /// Összesítés adatai
    /// </summary>
    public class Osszesites
    {
        /// <summary>
        /// A számlában szereplő tételek adóalapjainak összege (mindösszesen, végösszeg).
        /// </summary>
        public Vegosszeg Vegosszeg { get; } = new Vegosszeg();

        /// <summary>
        /// Áfarovat, adókulcsok szerinti összegzés. Csak "számla" típusú számla esetében kell kitölteni.        
        /// </summary>
        public List<AfaRovat> AfaRovatok { get; } = new List<AfaRovat>();

        public void ToXml(XmlWriter xml)
        {
            xml.WriteStartElement("osszesites");
            {
                foreach (var afarovat in AfaRovatok)
                    afarovat.ToXml(xml);
                Vegosszeg.ToXml(xml);
            }
            xml.WriteEndElement();
        }
    }
}
