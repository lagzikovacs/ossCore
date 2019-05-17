using System.Xml;
using ossServer.Controllers.NGM;

namespace NGM.VI30232014
{
    /// <summary>
    /// Záradékok adatai
    /// </summary>
    public class Zaradekok
    {
        /// <summary>
        /// Pénzforgalmi elszámolás: A „pénzforgalmi elszámolás” kifejezés feltüntetése a számlán, különös szabályok szerinti adózás alkalmazása esetében.
        /// </summary>
        public bool? PenzforgalmiElszamolas { get; set; }

        /// <summary>
        /// Önszámlázás: Az „önszámlázás” kifejezés feltüntetése a számlán, ha a számlát a terméket beszerző vagy a szolgáltatást igénybevevő állítja ki.
        /// </summary>
        public bool? Onszamlazas { get; set; }

        /// <summary>
        /// Fordított adózás: A „fordított adózás” kifejezés feltüntetése a számlán, ha adófizetésre a termék beszerzője, vagy a szolgáltatás igénybevevője kötelezett.
        /// </summary>
        public bool? ForditottAdozas { get; set; }

        /// <summary>
        /// Adómentességi hivatkozás: Adómentesség esetében jogszabályi vagy a Héa-irányelv vonatkozó rendelkezéseire történő hivatkozás, vagy bármely más, de egyértelmű utalás arra, hogy a termék értékesítése, szolgáltatás nyújtása mentes az adó alól.
        /// </summary>
        public bool? AdomentessegiHivatkozas { get; set; }

        /// <summary>
        /// Különbözet szerinti szabályozás:<br/>
        /// · „különbözet szerinti szabályozás - utazási irodák”<br/>
        /// · „különbözet szerinti szabályozás - használt cikkek”<br/>
        /// · „különbözet szerinti szabályozás - műalkotások”<br/>
        /// · „különbözet szerinti szabályozás gyűjteménydarabok és régiségek”<br/>
        /// közül a megfelelő kifejezés feltüntetése a számlán.
        /// </summary>
        public KulonbozetSzerintiSzabalyozasTipusok? KulonbozetSzerintiSzabalyozas { get; set; }

        /// <summary>
        /// Termékdíj-feltüntetési kötelezettség számlán történő átvállalás esetén: „az egyéb kőolajtermék vevője nem termékdíj-kötelezett, a bruttó árból … Ft termékdíj átvállalásra került” szövegű záradékot kell feltüntetni az egyéb kőolajtermék termékdíj-kötelezettség számla alapján való átvállalása esetén a számlán.
        /// </summary>
        public decimal? TermekdijSzamlasAtvallalas { get; set; }

        /// <summary>
        /// Termékdíj-feltüntetési kötelezettség szerződéssel történő átvállalás esetén: „a termékdíj kötelezettség a Ktdt. 14. § (5) bekezdés ...) pontja ...) alpontja alapján a vevőt terheli” szövegű záradékot kell feltüntetni a termékdíj-kötelezettség szerződés alapján történő átvállalása esetén a számlán.
        /// </summary>
        public TermekdijSzerzodesesAtvallalas TermekdijSzerzodesesAtvallalas { get; } = new TermekdijSzerzodesesAtvallalas();

        /// <summary>
        /// Termékdíj visszaigénylés datai
        /// </summary>
        public TermekdijVisszaigenyles TermekdijVisszaigenyles { get; } = new TermekdijVisszaigenyles();

        /// <summary>
        /// Termékdíj-feltüntetési kötelezettség csomagolószer első belföldi forgalomba hozatalakor
        /// </summary>
        public CsomagoloszerElsoForgalombaHozatal CsomagoloszerElsoForgalombaHozatal { get; } = new CsomagoloszerElsoForgalombaHozatal();

        /// <summary>
        /// Termékdíj-feltüntetési kötelezettség reklámhordozó papír első belföldi forgalomba hozatalakor: A reklámhordozó papír első belföldi forgalomba hozójának a következő záradékot kell feltüntetni a számlán: „a reklámhordozó papír termékdíj összege a bruttó árból … Ft”.
        /// </summary>
        public decimal? ReklamhordozoPapir { get; set; }

        /// <summary>
        /// Energia értékesítése esetén: Az energia értékesítést végző adóalany köteles a számlán elkülönítve feltüntetni az adó összegét.
        /// </summary>
        public decimal? EnergiaAdo { get; set; }

        /// <summary>
        /// Népegészségügyi termékadó kötelezettség: Az adó alanya az adóköteles termék értékesítéséről kiállított számlán köteles feltüntetni, hogy a népegészségügyi termékadó kötelezettség őt terheli.
        /// </summary>
        public bool? NepegeszsegugyiTermekadoKotelezettseg { get; set; }

        /// <summary>
        /// AHK-szám feltüntetése: A jövedéki termék közösségi adófelfüggesztési eljárásban adóraktárból történő kitároláshoz kapcsolódik az AHK-számot tartalmazó számla.
        /// </summary>
        public string AhkSzam { get; set; }

        /// <summary>
        /// Csomagküldő kereskedőtől beszerzett jövedéki termék eredetének igazolása: A jövedéki terméket belföldön terhelő adót külön fel kell tüntetni a számlán.
        /// </summary>
        public decimal? CsomagkuldoJovedekiAdo { get; set; }

        /// <summary>
        /// Adóraktár engedélyesének jövedéki termék értékesítése: Az adóraktár engedélyese a jövedéki termék értékesítéséről kiállított számlán köteles a jövedéki termék vámtarifaszámát feltüntetni.
        /// </summary>
        public bool? JovedekiVamtarifa { get; set; }

        /// <summary>
        /// Adóraktár engedélyese/importáló/bejegyzett kereskedő/jövedéki engedélyes kereskedő által megfizetett adó feltüntetése: Adóraktár engedélyese/importáló/bejegyzett kereskedő/jövedéki engedélyes kereskedő az általa megfizetett, vételárban felszámított adót a vevő kérésére köteles a számlán elkülönítve feltüntetni.
        /// </summary>
        public decimal? EngedelyesImportaloAdo { get; set; }

        /// <summary>
        /// Gázolaj beszerzéséről szóló számla adatai
        /// </summary>
        public GazolajBeszerzesSzamla GazolajBeszerzesSzamla { get; } = new GazolajBeszerzesSzamla();

        /// <summary>
        /// Jövedéki engedélyes kereskedő jövedéki termék értékesítése: A jövedéki engedélyes kereskedő jövedéki termék értékesítésekor a számlán fel kell tüntetnie<br/>
        /// · a jövedéki termék vámtarifaszámát<br/>
        /// · a jövedéki engedélye számát<br/>
        /// · a vevő adóigazgatási azonosító számát<br/>
        /// · adott esetben a vevő őstermelői igazolvány számát.<br/>
        /// </summary>
        public bool? JovedekiEngedelyesKereskedo { get; set; }

        /// <summary>
        /// Jövedéki engedélyes kereskedő adatai
        /// </summary>
        public JovedekiEngedelyesKereskedoAdat JovedekiEngedelyesKereskedoAdat { get; } = new JovedekiEngedelyesKereskedoAdat();

        /// <summary>
        /// Jövedéki engedélyes kereskedő a raktárhelyisége, illetve tárolótartálya telephelyének nem jövedéki engedélyes kereskedelmi elárusítóhelyként történő használatakor: A jövedéki engedélyes kereskedő – a vevő kérésére – olyan számlát bocsát ki, amely tartalmazza a következő záradékot: „Továbbértékesítés esetén a jövedéki termék származásának igazolására nem alkalmas”.
        /// </summary>
        public bool? JovedekiEngedelyesKereskedoSzamla { get; set; }

        /// <summary>
        /// Nem jövedéki engedélyes kereskedő meghatározott kereskedelmi mennyiséget elérő mennyiségű jövedéki termék értékesítésekor: A nem jövedéki engedélyes kereskedő – a vevő kérésére – olyan számlát bocsát ki, amely tartalmazza a következő záradékot: „Továbbértékesítés esetén a jövedéki termék származásának igazolására nem alkalmas”.
        /// </summary>
        public bool? NemJovedekiEngedelyesKereskedoSzamla { get; set; }

        /// <summary>
        /// Importáló a jövedéki termék értékesítésekor: Az importáló a számlán köteles feltüntetni a jövedéki termék vámtarifaszámát.
        /// </summary>
        public bool? ImportaloVamtarifa { get; set; }

        /// <summary>
        /// Betétdíj alkalmazása esetén: A betétdíj összegét a számlán a betétdíjas termék árától elkülönítve kell feltüntetni.
        /// </summary>
        public decimal? BetetiDij { get; set; }

        /// <summary>
        /// Rezsicsökkentés számlázása: A rezsicsökkentés eredményeként jelentkező megtakarítás összegét jól láthatóan, színes mezőben kiemelve kell feltüntetni a számlán.
        /// </summary>
        public decimal? Rezsicsokkentes { get; set; }

        /// <summary>
        /// Tranzitadóraktár-engedélyes a tranzitadóraktárból történő értékesítés esetén.
        /// </summary>
        public Beszallokartya Beszallokartya { get; } = new Beszallokartya();

        private bool IsEmpty()
        {
            return PenzforgalmiElszamolas == null &&
                   Onszamlazas == null &&
                   ForditottAdozas == null &&
                   AdomentessegiHivatkozas == null &&
                   KulonbozetSzerintiSzabalyozas == null &&
                   TermekdijSzamlasAtvallalas == null &&
                   TermekdijSzerzodesesAtvallalas.IsEmpty &&
                   TermekdijVisszaigenyles.IsEmpty &&
                   CsomagoloszerElsoForgalombaHozatal.IsEmpty &&
                   ReklamhordozoPapir == null &&
                   EnergiaAdo == null &&
                   NepegeszsegugyiTermekadoKotelezettseg == null &&
                   string.IsNullOrEmpty(AhkSzam) &&
                   CsomagkuldoJovedekiAdo == null &&
                   JovedekiVamtarifa == null &&
                   EngedelyesImportaloAdo == null &&
                   GazolajBeszerzesSzamla.IsEmpty &&
                   JovedekiEngedelyesKereskedo == null &&
                   JovedekiEngedelyesKereskedoAdat.IsEmpty &&
                   JovedekiEngedelyesKereskedoSzamla == null &&
                   NemJovedekiEngedelyesKereskedoSzamla == null &&
                   ImportaloVamtarifa == null &&
                   BetetiDij == null &&
                   Rezsicsokkentes == null &&
                   Beszallokartya.IsEmpty;
        }

        public void ToXml(XmlWriter xml)
        {
            if (IsEmpty())
                return;

            xml.WriteStartElement("zaradekok");
            {
                XmlTools.WriteValueElement(xml, "penzforgelsz", PenzforgalmiElszamolas, false);
                XmlTools.WriteValueElement(xml, "onszamla", Onszamlazas, false);
                XmlTools.WriteValueElement(xml, "ford_ado", ForditottAdozas, false);
                XmlTools.WriteValueElement(xml, "adoment_hiv", AdomentessegiHivatkozas, false);
                XmlTools.WriteValueElement(xml, "kulonb_szer_szab", (int?)KulonbozetSzerintiSzabalyozas, false);
                XmlTools.WriteValueElement(xml, "termekdij_szlas_atvallalas", TermekdijSzamlasAtvallalas, false);
                TermekdijSzerzodesesAtvallalas.ToXml(xml);
                TermekdijVisszaigenyles.ToXml(xml);
                CsomagoloszerElsoForgalombaHozatal.ToXml(xml);
                XmlTools.WriteValueElement(xml, "reklam_papir", ReklamhordozoPapir, false);
                XmlTools.WriteValueElement(xml, "energia_ado", EnergiaAdo, false);
                XmlTools.WriteValueElement(xml, "neta", NepegeszsegugyiTermekadoKotelezettseg, false);
                XmlTools.WriteValueElement(xml, "ahk", AhkSzam, 100, false);
                XmlTools.WriteValueElement(xml, "csomagk_jovedeki_ado", CsomagkuldoJovedekiAdo, false);
                XmlTools.WriteValueElement(xml, "jovedeki_vamtarifa", JovedekiVamtarifa, false);
                XmlTools.WriteValueElement(xml, "eng_import_ado", EngedelyesImportaloAdo, false);
                GazolajBeszerzesSzamla.ToXml(xml);
                XmlTools.WriteValueElement(xml, "jov_eng_ker", JovedekiEngedelyesKereskedo, false);
                JovedekiEngedelyesKereskedoAdat.ToXml(xml);
                XmlTools.WriteValueElement(xml, "jov_eng_keresk_szla", JovedekiEngedelyesKereskedoSzamla, false);
                XmlTools.WriteValueElement(xml, "nem_jov_eng_keresk_szla", NemJovedekiEngedelyesKereskedoSzamla, false);
                XmlTools.WriteValueElement(xml, "import_vamtarifa", ImportaloVamtarifa, false);
                XmlTools.WriteValueElement(xml, "betet_dij", BetetiDij, false);
                XmlTools.WriteValueElement(xml, "rezsicsokkentes", Rezsicsokkentes, false);
                Beszallokartya.ToXml(xml);
            }
            xml.WriteEndElement();
        }
    }
}
