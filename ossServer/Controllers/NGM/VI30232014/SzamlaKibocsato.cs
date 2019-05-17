using System.Xml;
using ossServer.Controllers.NGM;

namespace NGM.VI30232014
{
    /// <summary>
    /// Számlakibocsátó adatai
    /// </summary>
    public class SzamlaKibocsato
    {
        /// <summary>
        /// Adószám: Számlakibocsátó adószáma.
        /// </summary>
        public string Adoszam { get; set; }

        /// <summary>
        /// Közösségi adószám: A számlakibocsátó közösségi adószáma.
        /// </summary>
        public string KozossegiAdoszam { get; set; }

        /// <summary>
        /// Kisadózó: A számlakibocsátó kisadózói státuszának jelölése.
        /// </summary>
        public bool Kisadozo { get; set; }

        /// <summary>
        /// Név: Számlakibocsátó megnevezése.
        /// </summary>
        public string Nev { get; set; }

        /// <summary>
        /// Cím: A számlakibocsátó cím adatainak csoportja.
        /// </summary>
        public Cim Cim { get; } = new Cim();

        /// <summary>
        /// Egyéni vállalkozó megjelölés: Egyéni vállalkozó számlakibocsátó esetén az "egyéni vállalkozó" megjelölést, vagy annak e.v. rövidítését fel kell tüntetni.
        /// </summary>
        public bool EgyeniVallalkozo { get; set; }

        /// <summary>
        /// Egyéni vállalkozó nyilvántartási száma: Egyéni vállalkozó számlakibocsátó esetén az egyéni vállalkozó nyilvántartási számát fel kell tüntetni.
        /// </summary>
        public string EgyeniVallalkozoNyilvantartasiSzama { get; set; }

        /// <summary>
        /// Egyéni vállalkozó neve (aláírása): Egyéni vállalkozó számlakibocsátó esetén az egyéi vállalkozó nevét (aláírását) fel kell tüntetni.
        /// </summary>
        public string EgyeniVallalkozoAlairasa { get; set; }

        public void ToXml(XmlWriter xml)
        {
            Utils.ExceptionIfEmpty(nameof(Adoszam), Adoszam);
            if (Cim.IsEmpty)
                throw new XmlException("A számlakibocsátó címét kötelező kitölteni.");

            xml.WriteStartElement("szamlakibocsato");
            {
                XmlTools.WriteValueElement(xml, "adoszam", Adoszam, 20, true); // 8-20
                XmlTools.WriteValueElement(xml, "kozadoszam", KozossegiAdoszam, 20, false); // 8-20
                XmlTools.WriteValueElement(xml, "kisadozo", Kisadozo, false);
                XmlTools.WriteValueElement(xml, "nev", Nev, 100, true);
                Cim.ToXml(xml, "cim");
                XmlTools.WriteValueElement(xml, "egyeni_vallalkozo", EgyeniVallalkozo, false);
                XmlTools.WriteValueElement(xml, "ev_nyilv_tart_szam", EgyeniVallalkozoNyilvantartasiSzama, 100, false);
                XmlTools.WriteValueElement(xml, "ev_neve", EgyeniVallalkozoAlairasa, 100, false);
            }
            xml.WriteEndElement();
        }
    }
}
