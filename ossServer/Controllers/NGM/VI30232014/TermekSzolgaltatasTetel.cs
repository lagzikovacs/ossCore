using System.Xml;
using ossServer.Controllers.NGM;

namespace NGM.VI30232014
{
    /// <summary>
    /// Termék/szolgáltatás tételek adatai
    /// </summary>
    public class TermekSzolgaltatasTetel
    {
        /// <summary>
        /// Megnevezés: Az értékesített termék/szolgáltatás megnevezése.
        /// </summary>
        public string Megnevezes { get; set; }

        /// <summary>
        /// Gyűjtőszámla csoport: A gyűjtőszámlába foglalás csoportosító eleme, általában dátum, vagy annak valamely része.
        /// </summary>
        public string GyujtoszamlaCsoport { get; set; }

        /// <summary>
        /// Előleg: Amennyiben a termékre/szolgáltatásra adott előleg szerepel a számlán, annak jelölése.
        /// </summary>
        public ElolegszamlaTipusok? Eloleg { get; set; } = ElolegszamlaTipusok.Nincs;

        /// <summary>
        /// Besorolási szám: A termék/szolgáltatás jelölésére alkalmazott vámtarifaszám/SZJ szám.
        /// </summary>
        public string BesorolasiSzam { get; set; }

        /// <summary>
        /// Termék/szolgáltatás mennyisége: Az értékesített termék/nyújtott szolgáltatás mennyisége, feltéve, hogy az természetes mértékegységben kifejezhető.
        /// </summary>
        public decimal Mennyiseg { get; set; }

        /// <summary>
        /// Termék/szolgáltatás mennyiségi mértékegysége: A termék/szolgáltatás – feltéve, hogy az természetes mértékegységben kifejezhető – mennyiségi mértékegysége.
        /// </summary>
        public string Mertekegyseg { get; set; }

        /// <summary>
        /// Közvetített szolgáltatás: Közvetített szolgáltatás esetén a számlából a közvetítés tényének ki kell derülnie.
        /// </summary>
        public bool? KozvetitettSzolgaltatas { get; set; }

        /// <summary>
        /// Termék/szolgáltatás nettó ára: Az adó alapja.
        /// </summary>
        public decimal? NettoAr { get; set; }

        /// <summary>
        /// Termék/szolgáltatás nettó egységára: Értékesített termék adó nélküli egységára, vagy a nyújtott szolgáltatás adó nélküli egységára, ha az természetes mértékegységben kifejezhető.
        /// </summary>
        public decimal? NettoEgysegar { get; set; }

        /// <summary>
        /// Az értékesített dohánygyártmány adójegyén szereplő kiskereskedelmi eladási ár: Általánosforgalmiadó-alanynak az adójeggyel ellátott dohánygyártmányok értékesítése tekintetében, ezen termékértékesítésről kibocsátott számlában – a nem jövedéki engedélyes kereskedelmi tevékenység keretében végzett termékértékesítés kivételével – a termék megnevezése mellett tájékoztató adatként fel kell tüntetni az értékesített dohánygyártmány adójegyén szereplő kiskereskedelmi eladási árat is.
        /// </summary>
        public decimal? DohanyAr { get; set; }

        /// <summary>
        /// Adó kulcs: Az alkalmazott adó mértéke.
        /// </summary>
        public decimal? AdoKulcs { get; set; }

        /// <summary>
        /// Adó értéke: Az áthárított adó, kivéve, ha annak feltüntetését a törvény kizárja (a számlán az áthárított adót forintban kifejezve abban az esetben is fel kell tüntetni, ha az egyéb adatok külföldi pénznemben kifejezettek).
        /// </summary>
        public decimal? AdoErtek { get; set; }

        /// <summary>
        /// Százalékérték: Az egyszerűsített adattartalmú számla esetén az alkalmazott adó mértékének megfelelő százalékértékek (21,26%, 4,76%, 15,25%).
        /// </summary>
        public decimal? Szazalekertek { get; set; }

        /// <summary>
        /// Termék/szolgáltatás bruttó ára: Az ellenérték adót is tartalmazó összege.
        /// </summary>
        public decimal? BruttoAr { get; set; }

        /// <summary>
        /// Alkalmazott árengedmény: Alkalmazott árengedmény, feltéve, hogy azt az egységár nem tartalmazza.
        /// </summary>
        public decimal? Arengedmeny { get; set; }

        /// <summary>
        /// Az árverési vételárul szolgáló ellenérték: Az árverési vételár, mint ellenérték.
        /// </summary>
        public decimal? ArveresiVetelarEllenertek { get; set; }

        /// <summary>
        /// Az adók, vámok, illetékek, járulékok, hozzájárulások, lefölözések és más, kötelező jellegű befizetések: Az adók, vámok, illetékek, járulékok, hozzájárulások, lefölözések és más, kötelező jellegű befizetések értéke.
        /// </summary>
        public decimal? Kozteher { get; set; }

        /// <summary>
        /// A felmerült járulékos költségek, amelyeket a nyilvános árverés szervezője hárít át az árverési vevőnek: Ilyennek minősülnek különösen a bizománnyal, egyéb közvetítéssel, csomagolással, fuvarozással és biztosítással összefüggő díjak és költségek.
        /// </summary>
        public decimal? ArveresJarulekosKoltseg { get; set; }

        /// <summary>
        /// Kisebbítendő tag: Az 'árverési vételárul szolgáló ellenérték', 'közteher' és 'árverés járulékos költség' tagok együttes összege.
        /// </summary>
        public decimal? KisebbitendoTag => ArveresiVetelarEllenertek + Kozteher + ArveresJarulekosKoltseg;

        /// <summary>
        /// Közlekedési eszköz információk: Az új közlekedési eszköz számlatétel információinak csoportja.
        /// </summary>
        public KozlekedesiEszkozInformacio KozlekedesiEszkozInformaciok { get; } = new KozlekedesiEszkozInformacio();

        /// <summary>
        /// Az ásványolaj adóraktárból történő kitárolása esetén.
        /// </summary>
        public Asvanyolaj Asvanyolaj { get; } = new Asvanyolaj();

        public void ToXml(XmlWriter xml)
        {
            xml.WriteStartElement("termek_szolgaltatas_tetelek");
            {
                XmlTools.WriteValueElement(xml, "termeknev", Megnevezes, 100, true);
                XmlTools.WriteValueElement(xml, "gyujto_szla_csoport", GyujtoszamlaCsoport, 100, false);
                if (Eloleg != ElolegszamlaTipusok.Nincs)
                    XmlTools.WriteValueElement(xml, "eloleg", (int?)Eloleg, false);
                XmlTools.WriteValueElement(xml, "besorszam", BesorolasiSzam, 100, false);
                XmlTools.WriteValueElement(xml, "menny", Mennyiseg, true);
                XmlTools.WriteValueElement(xml, "mertekegys", Mertekegyseg, 100, true);
                XmlTools.WriteValueElement(xml, "kozv_szolgaltatas", KozvetitettSzolgaltatas, false);
                XmlTools.WriteValueElement(xml, "nettoar", NettoAr, false);
                XmlTools.WriteValueElement(xml, "nettoegysar", NettoEgysegar, false);
                XmlTools.WriteValueElement(xml, "dohany_ar", DohanyAr, false);
                XmlTools.WriteValueElement(xml, "adokulcs", AdoKulcs, false);
                XmlTools.WriteValueElement(xml, "adoertek", AdoErtek, false);
                XmlTools.WriteValueElement(xml, "szazalekertek", Szazalekertek, false);
                XmlTools.WriteValueElement(xml, "bruttoar", BruttoAr, false);
                XmlTools.WriteValueElement(xml, "arengedm", Arengedmeny, false);
                XmlTools.WriteValueElement(xml, "vellenertek", ArveresiVetelarEllenertek, false);
                XmlTools.WriteValueElement(xml, "vkozteher", Kozteher, false);
                XmlTools.WriteValueElement(xml, "vktg", ArveresJarulekosKoltseg, false);
                XmlTools.WriteValueElement(xml, "vkistag", KisebbitendoTag, false);
                KozlekedesiEszkozInformaciok.ToXml(xml);
                Asvanyolaj.ToXml(xml);
            }
            xml.WriteEndElement();
        }
    }
}
