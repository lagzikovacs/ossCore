using System;
using System.Xml;
using ossServer.Controllers.NGM;

namespace NGM.VI30232014
{
    /// <summary>
    /// A csomagolószer kötelezettje a számlán köteles feltüntetni a számla tételeire hivatkozva:<br/>
    /// · amennyiben a csomagolószer termékdíj-kötelezettség az eladót terheli:<br/>
    /// „a csomagolószer termékdíj összege bruttó árból...Ft”<br/>
    /// · amennyiben a csomagolás részeként forgalomba hozott csomagolószer termékdíj-kötelezettség az eladót terheli:<br/>
    /// „a csomagolás termékdíj-kötelezettség az eladót terheli”, vagy<br/>
    /// · amennyiben az eladót a vevő nyilatkozata alapján termékdíj megfizetése nem terheli:<br/>
    /// „a csomagolószer termékdíja a vevő eseti nyilatkozata alapján nem kerül megfizetésre”, vagy<br/>
    /// „a csomagolószer termékdíja a vevő...számon iktatott időszakra vonatkozó nyilatkozata alapján nem kerül megfizetésre”.
    /// </summary>
    public class CsomagoloszerElsoForgalombaHozatal
    {
        /// <summary>
        /// Termékdíj: Záradék szövegében szereplő összeg feltüntetése: „a csomagolószer termékdíj összege bruttó árból … Ft”.
        /// </summary>
        public decimal? Termekdij { get; set; }

        /// <summary>
        /// Csomagolószer részeként forgalomba hozott: Záradék szövegének feltüntetése (igen/nem).
        /// </summary>
        public bool? ReszenkentForgalombaHozott { get; set; }

        /// <summary>
        /// Vevő nyilatkozat
        /// </summary>
        public VevoNyilatkozat VevoNyilatkozat { get; } = new VevoNyilatkozat();

        public bool IsEmpty => Termekdij == null
                               && ReszenkentForgalombaHozott == null
                               && VevoNyilatkozat.IsEmpty;

        public void ToXml(XmlWriter xml)
        {
            if (IsEmpty)
                return;

            if (Termekdij == null)
                throw new ArgumentNullException(nameof(Termekdij));
            if (ReszenkentForgalombaHozott == null)
                throw new ArgumentNullException(nameof(ReszenkentForgalombaHozott));

            xml.WriteStartElement("csomszer_forg_hoz");
            {
                XmlTools.WriteValueElement(xml, "termek_dij", Termekdij, true);
                XmlTools.WriteValueElement(xml, "reszenkent_forg_hoz", ReszenkentForgalombaHozott, true);
                VevoNyilatkozat.ToXml(xml);
            }
            xml.WriteEndElement();
        }
    }
}
