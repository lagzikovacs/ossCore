using System.Collections.Generic;
using System.Xml;

namespace NGM.VI30232014
{
    /// <summary>
    /// Egy számla lehetséges adatai
    /// </summary>
    public class Szamla
    {
        /// <summary>
        /// Fejléc
        /// </summary>
        public Fejlec Fejlec { get; } = new Fejlec();

        /// <summary>
        /// Számlakibocsátó adatai
        /// </summary>
        public SzamlaKibocsato SzamlaKibocsato { get; } = new SzamlaKibocsato();

        /// <summary>
        /// Számlabefogadó adatai
        /// </summary>
        public SzamlaBefogado SzamlaBefogado { get; } = new SzamlaBefogado();

        /// <summary>
        /// Képviselő adatai
        /// </summary>
        public Kepviselo Kepviselo { get; } = new Kepviselo();

        /// <summary>
        /// Termék/szolgáltatás tételek adatai
        /// </summary>
        public List<TermekSzolgaltatasTetel> TermekSzolgaltatasTetelek { get; } = new List<TermekSzolgaltatasTetel>();

        /// <summary>
        /// Módosító számlák adatai
        /// </summary>
        public List<ModositoSzamla> ModositoSzamlakAdatai { get; } = new List<ModositoSzamla>();

        /// <summary>
        /// Gyűjtőszámlák datai
        /// </summary>
        public List<Gyujtoszamla> GyujtoszamlakAdatai { get; } = new List<Gyujtoszamla>();

        /// <summary>
        /// Záradékok
        /// </summary>
        public Zaradekok Zaradekok { get; } = new Zaradekok();

        /// <summary>
        /// Nem kötelező elemek
        /// </summary>
        public NemKotelezo NemKotelezo { get; } = new NemKotelezo();

        /// <summary>
        /// Összesítés
        /// </summary>
        public Osszesites Osszesites { get; } = new Osszesites();

        public void ToXml(XmlWriter xml)
        {
            xml.WriteStartElement("szamla");
            {
                Fejlec.ToXml(xml);
                SzamlaKibocsato.ToXml(xml);
                SzamlaBefogado.ToXml(xml);
                Kepviselo.ToXml(xml);
                foreach (var termekSzolgaltatasTetel in TermekSzolgaltatasTetelek)
                    termekSzolgaltatasTetel.ToXml(xml);
                foreach (var modositoSzamlaAdat in ModositoSzamlakAdatai)
                    modositoSzamlaAdat.ToXml(xml);
                foreach (var gyujtoSzamlaAdat in GyujtoszamlakAdatai)
                    gyujtoSzamlaAdat.ToXml(xml);
                Zaradekok.ToXml(xml);
                NemKotelezo.ToXml(xml);
                Osszesites.ToXml(xml);
            }
            xml.WriteEndElement();
        }
    }
}
