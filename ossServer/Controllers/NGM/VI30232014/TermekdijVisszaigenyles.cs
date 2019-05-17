using System;
using System.Collections.Generic;
using System.Xml;
using ossServer.Controllers.NGM;

namespace NGM.VI30232014
{
    /// <summary>
    /// Termékdíj visszaigénylés adatai:<br/>
    /// Termékdíj-feltüntetési kötelezettség a kötelezett és vevőinek visszaigénylésre jogosult vevő partnerei által igényelt esetben:<br/>
    /// A visszaigénylésre jogosult vevő igénye alapján a kötelezettnek, illetve a kötelezett vevőinek a számlán záradékot kell feltüntetni, amely tartalmazza:<br/>
    /// · a termékdíjköteles termék CsK, KT kódját<br/>
    /// · a termékdíj mértékét és összegét<br/>
    /// · a termékdíj megfizetését(bevallását) igazoló dokumentumok adatai közül legalább<br/>
    ///   - a kötelezett által kibocsátott számla számát<br/>
    ///   - a kötelezett által kibocsátott számla keltét<br/>
    ///   - a kötelezett nevét<br/>
    ///   - a kötelezett címét<br/>
    ///   - a kötelezett adószámát.
    /// </summary>
    public class TermekdijVisszaigenyles
    {
        /// <summary>
        /// Visszaigénylés tételei
        /// </summary>
        public List<VisszaigenylesTetel> VisszaigenylesTetelei { get; } = new List<VisszaigenylesTetel>();

        /// <summary>
        /// Kötelezett számla száma
        /// </summary>
        public string KotelezettSzamlaSzama { get; set; }

        /// <summary>
        /// Kötelezett számla kelte
        /// </summary>
        public DateTime? KotelezettSzamlaKelte { get; set; }

        /// <summary>
        /// Kötelezett neve
        /// </summary>
        public string KotelezettNeve { get; set; }

        /// <summary>
        /// Kötelezett címe
        /// </summary>
        public Cim KotelezettCime { get; } = new Cim();

        /// <summary>
        /// Kötelezett adószáma
        /// </summary>
        public string KotelezettAdoszama { get; set; }

        public bool IsEmpty => VisszaigenylesTetelei.Count == 0 &&
                               string.IsNullOrEmpty(KotelezettSzamlaSzama) &&
                               KotelezettSzamlaKelte == null &&
                               string.IsNullOrEmpty(KotelezettNeve) &&
                               KotelezettCime.IsEmpty &&
                               string.IsNullOrEmpty(KotelezettAdoszama);


        public void ToXml(XmlWriter xml)
        {
            if (IsEmpty)
                return;

            if (VisszaigenylesTetelei.Count == 0)
                throw new Exception($"A {nameof(VisszaigenylesTetelei)} mezőt kötelező feltölteni.");
            Utils.ExceptionIfEmpty(nameof(KotelezettSzamlaSzama), KotelezettSzamlaSzama);
            Utils.ExceptionIfEmpty(nameof(KotelezettSzamlaKelte), KotelezettSzamlaKelte);
            Utils.ExceptionIfEmpty(nameof(KotelezettNeve), KotelezettNeve);
            if (KotelezettCime.IsEmpty)
                throw new Exception("A kötelezett címét kötelező megadni.");
            Utils.ExceptionIfEmpty(nameof(KotelezettAdoszama), KotelezettAdoszama);

            xml.WriteStartElement("vissz_igeny");
            {
                foreach (var visszaigenylesTetel in VisszaigenylesTetelei)
                    visszaigenylesTetel.ToXml(xml);
                XmlTools.WriteValueElement(xml, "szla", KotelezettSzamlaSzama, 100, true);
                XmlTools.WriteValueElement(xml, "szla_kelte", KotelezettSzamlaKelte, true);
                XmlTools.WriteValueElement(xml, "kotelezett_neve", KotelezettNeve, 100, true);
                KotelezettCime.ToXml(xml, "kotelezett_cime");
                XmlTools.WriteValueElement(xml, "kotelezett_adoszama", KotelezettAdoszama, 20, true); // 8-20
            }
            xml.WriteEndElement();
        }
    }
}
