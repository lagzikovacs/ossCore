using System.Xml;
using ossServer.Controllers.NGM;

namespace NGM.VI30232014
{
    /// <summary>
    /// Jövedéki engedélyes kereskedő jövedéki termék értékesítése: A jövedéki engedélyes kereskedő jövedéki termék értékesítésekor a számlán fel kell tüntetnie<br/>
    /// · a jövedéki termék vámtarifaszámát<br/>
    /// · a jövedéki engedélye számát<br/>
    /// · a vevő adóigazgatási azonosító számát<br/>
    /// · adott esetben a vevő őstermelői igazolvány számát.<br/>
    /// </summary>
    public class JovedekiEngedelyesKereskedoAdat
    {
        /// <summary>
        /// Jövedéki engedély száma
        /// </summary>
        public string JovedekiEngedelySzama { get; set; }

        /// <summary>
        /// Vevő adóigazgatási azonosító száma, vagy őstermelői igazolvány száma
        /// </summary>
        public string VevoSzama { get; set; }

        public bool IsEmpty => string.IsNullOrEmpty(JovedekiEngedelySzama) && string.IsNullOrEmpty(VevoSzama);

        public void ToXml(XmlWriter xml)
        {
            if (IsEmpty)
                return;

            Utils.ExceptionIfEmpty(nameof(JovedekiEngedelySzama), JovedekiEngedelySzama);
            Utils.ExceptionIfEmpty(nameof(VevoSzama), VevoSzama);

            xml.WriteStartElement("jov_eng_keresk_jov_ert");
            {
                XmlTools.WriteValueElement(xml, "eng_szam", JovedekiEngedelySzama, 100, true);
                XmlTools.WriteValueElement(xml, "vevo_szam", VevoSzama, 100, true);
            }
            xml.WriteEndElement();
        }
    }
}
