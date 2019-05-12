using GemBox.Document;
using ossServer.Controllers.Csoport;
using ossServer.Controllers.Irat;
using ossServer.Controllers.Particio;
using ossServer.Controllers.Projekt;
using ossServer.Controllers.Session;
using ossServer.Controllers.Ugyfel;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Iratminta
{
    public class IratmintaBll
    {
        private static readonly string serialKey = "D873-P6FI-T8I0-4WLW";

        public static byte[] Elegedettseg(ossContext context, string sid, int projektKod)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PROJEKT);

            var entityProjekt = ProjektDal.Get(context, projektKod);
            var entityParticio = ParticioDal.Get(context);
            var iratKod = entityParticio.ProjektElegedettsegifelmeresIratkod != null ?
              (int)entityParticio.ProjektElegedettsegifelmeresIratkod : throw new Exception(string.Format(Messages.ParticioHiba, "ProjektElegedettsegifelmeresIratkod"));

            var original = IratBll.Letoltes(context, sid, iratKod);

            ComponentInfo.SetLicense(serialKey);

            DocumentModel document;
            using (var msDocx = new MemoryStream())
            {
                msDocx.Write(original.b, 0, original.b.Count());
                document = DocumentModel.Load(msDocx, GemBox.Document.LoadOptions.DocxDefault);
            }

            var mezoertekek = new
            {
                PROJEKTKOD = entityProjekt.Projektkod.ToString(),
                UGYFELNEV = entityProjekt.UgyfelkodNavigation.Nev,
                TELEPITESICIM = entityProjekt.Telepitesicim
            };

            document.MailMerge.Execute(mezoertekek);

            byte[] result;

            using (var msDocx = new MemoryStream())
            {
                document.Save(msDocx, GemBox.Document.SaveOptions.DocxDefault);
                result = msDocx.ToArray();
            }

            return result;
        }

        public static byte[] KeszrejelentesDemasz(ossContext context, string sid, int projektKod)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PROJEKT);

            var entityProjekt = ProjektDal.Get(context, projektKod);
            var entityParticio = ParticioDal.Get(context);
            var iratKod = entityParticio.ProjektKeszrejelentesDemaszIratkod != null ?
              (int)entityParticio.ProjektKeszrejelentesDemaszIratkod : throw new Exception(string.Format(Messages.ParticioHiba, "ProjektKeszrejelentesDemaszIratkod"));

            var original = IratBll.Letoltes(context, sid, iratKod);

            ComponentInfo.SetLicense(serialKey);

            DocumentModel document;
            using (var msDocx = new MemoryStream())
            {
                msDocx.Write(original.b, 0, original.b.Count());
                document = DocumentModel.Load(msDocx, GemBox.Document.LoadOptions.DocxDefault);
            }

            var mezoertekek = new
            {
                UGYFELNEV = entityProjekt.UgyfelkodNavigation.Nev,
                TELEPITESICIM = entityProjekt.Telepitesicim,
                INVERTER = entityProjekt.Inverter,
                TELEFONSZAM = entityProjekt.UgyfelkodNavigation.Telefon,
                EMAIL = entityProjekt.UgyfelkodNavigation.Email,
                AC = entityProjekt.Ackva.ToString(CultureInfo.CurrentCulture),
                DATUM = DateTime.Now.Date.ToLongDateString()
            };

            document.MailMerge.Execute(mezoertekek);

            byte[] result;

            using (var msDocx = new MemoryStream())
            {
                document.Save(msDocx, GemBox.Document.SaveOptions.DocxDefault);
                result = msDocx.ToArray();
            }

            return result;
        }
        public static byte[] KeszrejelentesElmuEmasz(ossContext context, string sid, int projektKod)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PROJEKT);

            var entityProjekt = ProjektDal.Get(context, projektKod);
            var entityParticio = ParticioDal.Get(context);
            var iratKod = entityParticio.ProjektKeszrejelentesElmuemaszIratkod != null ?
              (int)entityParticio.ProjektKeszrejelentesElmuemaszIratkod : throw new Exception(string.Format(Messages.ParticioHiba, "ProjektKeszrejelentesElmuemaszIratkod"));

            var original = IratBll.Letoltes(context, sid, iratKod);

            ComponentInfo.SetLicense(serialKey);

            DocumentModel document;
            using (var msDocx = new MemoryStream())
            {
                msDocx.Write(original.b, 0, original.b.Count());
                document = DocumentModel.Load(msDocx, GemBox.Document.LoadOptions.DocxDefault);
            }

            var mezoertekek = new
            {
                UGYFELNEV = entityProjekt.UgyfelkodNavigation.Nev,
                TELEPITESICIM = entityProjekt.Telepitesicim,
                INVERTER = entityProjekt.Inverter,
                TELEFONSZAM = entityProjekt.UgyfelkodNavigation.Telefon,
                EMAIL = entityProjekt.UgyfelkodNavigation.Email,
                AC = entityProjekt.Ackva.ToString(CultureInfo.CurrentCulture),
                DATUM = DateTime.Now.Date.ToLongDateString()
            };

            document.MailMerge.Execute(mezoertekek);

            byte[] result;

            using (var msDocx = new MemoryStream())
            {
                document.Save(msDocx, GemBox.Document.SaveOptions.DocxDefault);
                result = msDocx.ToArray();
            }

            return result;
        }
        public static byte[] KeszrejelentesEon(ossContext context, string sid, int projektKod)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PROJEKT);

            var entityProjekt = ProjektDal.Get(context, projektKod);
            var entityParticio = ParticioDal.Get(context);
            var iratKod = entityParticio.ProjektKeszrejelentesEonIratkod != null ?
              (int)entityParticio.ProjektKeszrejelentesEonIratkod : throw new Exception(string.Format(Messages.ParticioHiba, "ProjektKeszrejelentesEonIratkod"));

            var original = IratBll.Letoltes(context, sid, iratKod);

            ComponentInfo.SetLicense(serialKey);

            DocumentModel document;
            using (var msDocx = new MemoryStream())
            {
                msDocx.Write(original.b, 0, original.b.Count());
                document = DocumentModel.Load(msDocx, GemBox.Document.LoadOptions.DocxDefault);
            }

            var mezoertekek = new
            {
                UGYFELNEV = entityProjekt.UgyfelkodNavigation.Nev,
                TELEPITESICIM = entityProjekt.Telepitesicim,
                INVERTER = entityProjekt.Inverter,
                TELEFONSZAM = entityProjekt.UgyfelkodNavigation.Telefon,
                EMAIL = entityProjekt.UgyfelkodNavigation.Email,
                AC = entityProjekt.Ackva.ToString(CultureInfo.CurrentCulture),
                DATUM = DateTime.Now.Date.ToLongDateString()
            };

            document.MailMerge.Execute(mezoertekek);

            byte[] result;

            using (var msDocx = new MemoryStream())
            {
                document.Save(msDocx, GemBox.Document.SaveOptions.DocxDefault);
                result = msDocx.ToArray();
            }

            return result;
        }
        public static byte[] Munkalap(ossContext context, string sid, int projektKod)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PROJEKT);

            var entityProjekt = ProjektDal.Get(context, projektKod);
            var entityParticio = ParticioDal.Get(context);
            var iratKod = entityParticio.ProjektMunkalapIratkod != null ?
              (int)entityParticio.ProjektMunkalapIratkod : throw new Exception(string.Format(Messages.ParticioHiba, "ProjektMunkalapIratkod"));

            if (entityProjekt.Munkalapszam == null)
            {
                entityProjekt.Munkalapszam = context.KodGen(KodNev.Munkalapszam) + "/" + DateTime.Now.Year;
                ProjektDal.Update(context, entityProjekt);
                ProjektDal.Get(context, projektKod);
            }
            var original = IratBll.Letoltes(context, sid, iratKod);

            ComponentInfo.SetLicense(serialKey);

            DocumentModel document;
            using (var msDocx = new MemoryStream())
            {
                msDocx.Write(original.b, 0, original.b.Count());
                document = DocumentModel.Load(msDocx, GemBox.Document.LoadOptions.DocxDefault);
            }

            var mezoertekek = new
            {
                PROJEKTKOD = entityProjekt.Projektkod.ToString(),
                UGYFELNEV = entityProjekt.UgyfelkodNavigation.Nev,
                TELEPITESICIM = entityProjekt.Telepitesicim,
                PROJEKTJELLEGE = entityProjekt.Projektjellege,
                MUNKALAPSZAM = entityProjekt.Munkalapszam,
                DC = entityProjekt.Dckw.ToString(CultureInfo.CurrentCulture),
                AC = entityProjekt.Ackva.ToString(CultureInfo.CurrentCulture),
                TELEFONSZAM = entityProjekt.UgyfelkodNavigation.Telefon,
                NAPELEM = entityProjekt.Napelem,
                INVERTER = entityProjekt.Inverter
            };

            document.MailMerge.Execute(mezoertekek);

            byte[] result;

            using (var msDocx = new MemoryStream())
            {
                document.Save(msDocx, GemBox.Document.SaveOptions.DocxDefault);
                result = msDocx.ToArray();
            }

            return result;
        }

        public static byte[] SzallitasiSzerzodes(ossContext context, string sid, int projektKod)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PROJEKT);

            var entityProjekt = ProjektDal.Get(context, projektKod);
            var entityParticio = ParticioDal.Get(context);
            var iratKod = entityParticio.ProjektSzallitasiszerzodesIratkod != null ?
              (int)entityParticio.ProjektSzallitasiszerzodesIratkod : throw new Exception(string.Format(Messages.ParticioHiba, "ProjektSzallitasiszerzodesIratkod"));
            var original = IratBll.Letoltes(context, sid, iratKod);

            NumberFormatInfo nfi = new CultureInfo("hu-HU", false).NumberFormat;
            nfi.NumberGroupSeparator = ".";

            var arNetto = entityProjekt.Vallalasiarnetto;
            var elolegNetto = (int)(arNetto * (decimal)0.7) / 1000 * 1000;

            ComponentInfo.SetLicense(serialKey);

            DocumentModel document;
            using (var msDocx = new MemoryStream())
            {
                msDocx.Write(original.b, 0, original.b.Count());
                document = DocumentModel.Load(msDocx, GemBox.Document.LoadOptions.DocxDefault);
            }

            var mezoertekek = new
            {
                UGYFELNEV = entityProjekt.UgyfelkodNavigation.Nev,
                UGYFELCIM = UgyfelBll.Cim(entityProjekt.UgyfelkodNavigation),
                TELEFONSZAM = entityProjekt.UgyfelkodNavigation.Telefon,
                DC = entityProjekt.Dckw.ToString(CultureInfo.CurrentCulture),
                NAPELEM = entityProjekt.Napelem,
                INVERTER = entityProjekt.Inverter,
                TELEPITESICIM = entityProjekt.Telepitesicim,
                KIVITELEZESIHATARIDO = entityProjekt.Kivitelezesihatarido.Value.ToLongDateString(),
                MUNKATERULETATADASA = DateTime.Now.Date.AddDays(1).ToLongDateString(),
                ARNETTO = arNetto.ToString("#,#", nfi),
                ARBRUTTO = Calc.RealRound(arNetto * (decimal)1.27, 1m).ToString("#,#", nfi),
                ELOLEGNETTO = elolegNetto.ToString("#,#", nfi),
                ELOLEGBRUTTO = Calc.RealRound(elolegNetto * (decimal)1.27, 1m).ToString("#,#", nfi),
                DATUM = DateTime.Now.Date.ToLongDateString()
            };

            document.MailMerge.Execute(mezoertekek);

            byte[] result;

            using (var msDocx = new MemoryStream())
            {
                document.Save(msDocx, GemBox.Document.SaveOptions.DocxDefault);
                result = msDocx.ToArray();
            }

            return result;
        }

        public static byte[] Szerzodes(ossContext context, string sid, int projektKod)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PROJEKT);

            var entityProjekt = ProjektDal.Get(context, projektKod);
            var entityParticio = ParticioDal.Get(context);
            var iratKod = entityParticio.ProjektSzerzodesIratkod != null ?
              (int)entityParticio.ProjektSzerzodesIratkod : throw new Exception(string.Format(Messages.ParticioHiba, "ProjektSzerzodesIratkod"));
            var original = IratBll.Letoltes(context, sid, iratKod);

            NumberFormatInfo nfi = new CultureInfo("hu-HU", false).NumberFormat;
            nfi.NumberGroupSeparator = ".";

            var arNetto = entityProjekt.Vallalasiarnetto;
            var elolegNetto = (int)(arNetto * (decimal)0.7) / 1000 * 1000;

            ComponentInfo.SetLicense(serialKey);

            DocumentModel document;
            using (var msDocx = new MemoryStream())
            {
                msDocx.Write(original.b, 0, original.b.Count());
                document = DocumentModel.Load(msDocx, GemBox.Document.LoadOptions.DocxDefault);
            }

            var mezoertekek = new
            {
                UGYFELNEV = entityProjekt.UgyfelkodNavigation.Nev,
                UGYFELCIM = UgyfelBll.Cim(entityProjekt.UgyfelkodNavigation),
                TELEFONSZAM = entityProjekt.UgyfelkodNavigation.Telefon,
                DC = entityProjekt.Dckw.ToString(CultureInfo.CurrentCulture),
                NAPELEM = entityProjekt.Napelem,
                INVERTER = entityProjekt.Inverter,
                TELEPITESICIM = entityProjekt.Telepitesicim,
                KIVITELEZESIHATARIDO = entityProjekt.Kivitelezesihatarido.Value.ToLongDateString(),
                MUNKATERULETATADASA = DateTime.Now.Date.AddDays(1).ToLongDateString(),
                ARNETTO = arNetto.ToString("#,#", nfi),
                ARBRUTTO = Calc.RealRound(arNetto * (decimal)1.27, 1m).ToString("#,#", nfi),
                ELOLEGNETTO = elolegNetto.ToString("#,#", nfi),
                ELOLEGBRUTTO = Calc.RealRound(elolegNetto * (decimal)1.27, 1m).ToString("#,#", nfi),
                DATUM = DateTime.Now.Date.ToLongDateString()
            };

            document.MailMerge.Execute(mezoertekek);

            byte[] result;

            using (var msDocx = new MemoryStream())
            {
                document.Save(msDocx, GemBox.Document.SaveOptions.DocxDefault);
                result = msDocx.ToArray();
            }

            return result;
        }
    }
}
