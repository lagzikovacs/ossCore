using System;
using System.Xml;
using ossServer.Controllers.NGM;

namespace NGM.VI30232014
{
    /// <summary>
    /// Az ásványolaj adóraktárból történő kitárolása esetén meghatározott esetben fel kell tüntetni a számlán a következő adatokat:<br/>
    /// · a termék tényleges hőmérséklete;<br/>
    /// · kitárolt mennyisége környezeti hőmérsékleten mért tényleges térfogatban, illetve vasúti, közúti és uszályszállítás esetén mért vagy számított tömege;<br/>
    /// · 15 Celsius-fok hőmérséklethez tartozó sűrűsége;<br/>
    /// · 15 Celsius-fok hőmérséklethez tartozó térfogata;<br/>
    /// · a termék minősége, az érvényes MSZ-re, műszaki leírásra vagy szerződésben rögzített specifikációra való hivatkozással;<br/>
    /// · az üzemanyag és a tüzelőolaj külön jogszabályban előírt minőségi követelménynek való megfelelésének tanúsítása.
    /// </summary>
    public class Asvanyolaj
    {
        /// <summary>
        /// A termék tényleges hőmérséklete Celsius fokban.
        /// </summary>
        public decimal? Homerseklet { get; set; }

        /// <summary>
        /// Kitárolt mennyisége környezeti hőmérsékleten mért tényleges térfogatban, illetve vasúti, közúti és uszályszállítás esetén mért vagy számított tömege. Ha megad tömeget, akkor a mértékegységet is meg kell adni.
        /// </summary>
        public decimal? Tomeg { get; set; }

        /// <summary>
        /// Tömeg mértékegysége.
        /// </summary>
        public string TomegMertekegysege { get; set; }

        /// <summary>
        /// 15 Celsius-fok hőmérséklethez tartozó sűrűsége.
        /// </summary>
        public decimal? Suruseg { get; set; }

        /// <summary>
        /// Sűrűség mértékegysége. Ha megad sűrűséget, akkor a mértékegységet is meg kell adni.
        /// </summary>
        public string SurusegMertekegysege { get; set; }

        /// <summary>
        /// 15 Celsius-fok hőmérséklethez tartozó térfogata.
        /// </summary>
        public decimal? Terfogat { get; set; }

        /// <summary>
        /// Térfogat mértékegysége. Ha megad térfogatot, akkor a mértékegységet is meg kell adni.
        /// </summary>
        public string TerfogatMertekegysege { get; set; }

        /// <summary>
        /// A termék minősége, az érvényes MSZ-re, műszaki leírásra vagy szerződésben rögzített specifikációra való hivatkozással.
        /// </summary>
        public string Minoseg { get; set; }

        /// <summary>
        /// Az üzemanyag és a tüzelőolaj külön jogszabályban előírt minőségi követelménynek való megfelelésének tanúsítása.
        /// </summary>
        public string Tanusitas { get; set; }

        private bool IsEmpty => Homerseklet == null
                                && Tomeg == null
                                && string.IsNullOrEmpty(TomegMertekegysege)
                                && Suruseg == null
                                && string.IsNullOrEmpty(SurusegMertekegysege)
                                && Terfogat == null
                                && string.IsNullOrEmpty(TerfogatMertekegysege)
                                && string.IsNullOrEmpty(Minoseg)
                                && string.IsNullOrEmpty(Tanusitas);

        public void ToXml(XmlWriter xml)
        {
            if (IsEmpty)
                return;

            if (Tomeg != null && string.IsNullOrEmpty(TomegMertekegysege))
                throw new Exception("Ha megad tömeget, akkor a mértékegységet is meg kell adni.");

            if (Suruseg != null && string.IsNullOrEmpty(SurusegMertekegysege))
                throw new Exception("Ha megad sűrűséget, akkor a mértékegységet is meg kell adni.");

            if (Terfogat != null && string.IsNullOrEmpty(TerfogatMertekegysege))
                throw new Exception("Ha megad térfogatot, akkor a mértékegységet is meg kell adni.");

            xml.WriteStartElement("asvanyolaj");
            {
                XmlTools.WriteValueElement(xml, "homerseklet", Homerseklet, false);
                XmlTools.WriteValueElement(xml, "tomeg", Tomeg, false);
                XmlTools.WriteValueElement(xml, "tomeg_mert", TomegMertekegysege, 100, false);
                XmlTools.WriteValueElement(xml, "suruseg", Suruseg, false);
                XmlTools.WriteValueElement(xml, "suruseg_mert", SurusegMertekegysege, 100, false);
                XmlTools.WriteValueElement(xml, "terfogat", Terfogat, false);
                XmlTools.WriteValueElement(xml, "terfogat_mert", TerfogatMertekegysege, 100, false);
                XmlTools.WriteValueElement(xml, "minoseg", Minoseg, 100, false);
                XmlTools.WriteValueElement(xml, "tanusitas", Tanusitas, 100, false);
            }
            xml.WriteEndElement();
        }
    }
}
