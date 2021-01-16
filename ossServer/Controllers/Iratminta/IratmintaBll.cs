using GemBox.Document;
using Newtonsoft.Json;
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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Iratminta
{
    public class IratmintaBll
    {
        private static readonly string serialKey = "D873-P6FI-T8I0-4WLW";

        public static async Task<byte[]> ElegedettsegAsync(ossContext context, string sid, int projektKod)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PROJEKT);

            var entityProjekt = await ProjektDal.GetAsync(context, projektKod);

            var entityParticio = await ParticioDal.GetAsync(context);
            var pc = JsonConvert.DeserializeObject<ProjektConf>(entityParticio.Projekt);
            var iratKod = pc.ElegedettsegiFelmeresIratkod != null ? (int)pc.ElegedettsegiFelmeresIratkod : 
              throw new Exception(string.Format(Messages.ParticioHiba, "ElegedettsegiFelmeresIratkod"));

            var original = await IratBll.LetoltesAsync(context, sid, iratKod);

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

        public static async Task<byte[]> KeszrejelentesNkmAsync(ossContext context, string sid, int projektKod)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PROJEKT);

            var entityProjekt = await ProjektDal.GetAsync(context, projektKod);

            var entityParticio = await ParticioDal.GetAsync(context);
            var pc = JsonConvert.DeserializeObject<ProjektConf>(entityParticio.Projekt);
            var iratKod = pc.KeszrejelentesNkmIratkod != null ? (int)pc.KeszrejelentesNkmIratkod : 
              throw new Exception(string.Format(Messages.ParticioHiba, "KeszrejelentesNkmIratkod"));

            var original = await IratBll.LetoltesAsync(context, sid, iratKod);

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
                DATUM = DateTime.Now.Date.ToShortDateString()
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
        public static async Task<byte[]> KeszrejelentesElmuEmaszAsync(ossContext context, string sid, int projektKod)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PROJEKT);

            var entityProjekt = await ProjektDal.GetAsync(context, projektKod);

            var entityParticio = await ParticioDal.GetAsync(context);
            var pc = JsonConvert.DeserializeObject<ProjektConf>(entityParticio.Projekt);
            var iratKod = pc.KeszrejelentesElmuemaszIratkod != null ? (int)pc.KeszrejelentesElmuemaszIratkod : 
              throw new Exception(string.Format(Messages.ParticioHiba, "KeszrejelentesElmuemaszIratkod"));

            var original = await IratBll.LetoltesAsync(context, sid, iratKod);

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
                DATUM = DateTime.Now.Date.ToShortDateString()
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
        public static async Task<byte[]> KeszrejelentesEonAsync(ossContext context, string sid, int projektKod)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PROJEKT);

            var entityProjekt = await ProjektDal.GetAsync(context, projektKod);

            var entityParticio = await ParticioDal.GetAsync(context);
            var pc = JsonConvert.DeserializeObject<ProjektConf>(entityParticio.Projekt);
            var iratKod = pc.KeszrejelentesEonIratkod != null ? (int)pc.KeszrejelentesEonIratkod : 
                throw new Exception(string.Format(Messages.ParticioHiba, "KeszrejelentesEonIratkod"));

            var original = await IratBll.LetoltesAsync(context, sid, iratKod);

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
                DATUM = DateTime.Now.Date.ToShortDateString()
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
        public static async Task<byte[]> MunkalapAsync(ossContext context, string sid, int projektKod)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PROJEKT);

            var entityProjekt = await ProjektDal.GetAsync(context, projektKod);

            var entityParticio = await ParticioDal.GetAsync(context);
            var pc = JsonConvert.DeserializeObject<ProjektConf>(entityParticio.Projekt);
            var iratKod = pc.MunkalapIratkod != null ? (int)pc.MunkalapIratkod : 
                throw new Exception(string.Format(Messages.ParticioHiba, "MunkalapIratkod"));

            if (entityProjekt.Munkalapszam == null)
            {
                entityProjekt.Munkalapszam = context.KodGen(KodNev.Munkalapszam) + "/" + DateTime.Now.Year;
                await ProjektDal.UpdateAsync(context, entityProjekt);
                await ProjektDal.GetAsync(context, projektKod);
            }
            var original = await IratBll.LetoltesAsync(context, sid, iratKod);

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

        public static async Task<byte[]> SzerzodesAsync(ossContext context, string sid, int projektKod)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PROJEKT);

            var entityProjekt = await ProjektDal.GetAsync(context, projektKod);

            var entityParticio = await ParticioDal.GetAsync(context);
            var pc = JsonConvert.DeserializeObject<ProjektConf>(entityParticio.Projekt);
            var iratKod = pc.SzerzodesIratkod != null ? (int)pc.SzerzodesIratkod :
                throw new Exception(string.Format(Messages.ParticioHiba, "ProjektSzerzodesIratkod"));

            var original = await IratBll.LetoltesAsync(context, sid, iratKod);

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
                KIVITELEZESIHATARIDO = entityProjekt.Kivitelezesihatarido.Value.ToShortDateString(),
                MUNKATERULETATADASA = DateTime.Now.Date.AddDays(1).ToShortDateString(),
                ARNETTO = arNetto.ToString("#,#", nfi),
                ARBRUTTO = Calc.RealRound(arNetto * (decimal)1.27, 1m).ToString("#,#", nfi),
                ELOLEGNETTO = elolegNetto.ToString("#,#", nfi),
                ELOLEGBRUTTO = Calc.RealRound(elolegNetto * (decimal)1.27, 1m).ToString("#,#", nfi),
                DATUM = DateTime.Now.Date.ToShortDateString()
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
        public static async Task<byte[]> SzallitasiSzerzodesAsync(ossContext context, string sid, int projektKod)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PROJEKT);

            var entityProjekt = await ProjektDal.GetAsync(context, projektKod);

            var entityParticio = await ParticioDal.GetAsync(context);
            var pc = JsonConvert.DeserializeObject<ProjektConf>(entityParticio.Projekt);
            var iratKod = pc.SzallitasiSzerzodesIratkod != null ?
              (int)pc.SzallitasiSzerzodesIratkod : throw new Exception(string.Format(Messages.ParticioHiba, "SzallitasiSzerzodesIratkod"));

            var original = await IratBll.LetoltesAsync(context, sid, iratKod);

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
                KIVITELEZESIHATARIDO = entityProjekt.Kivitelezesihatarido.Value.ToShortDateString(),
                MUNKATERULETATADASA = DateTime.Now.Date.AddDays(1).ToShortDateString(),
                ARNETTO = arNetto.ToString("#,#", nfi),
                ARBRUTTO = Calc.RealRound(arNetto * (decimal)1.27, 1m).ToString("#,#", nfi),
                ELOLEGNETTO = elolegNetto.ToString("#,#", nfi),
                ELOLEGBRUTTO = Calc.RealRound(elolegNetto * (decimal)1.27, 1m).ToString("#,#", nfi),
                DATUM = DateTime.Now.Date.ToShortDateString()
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
        public static async Task<byte[]> FeltetelesSzerzodesAsync(ossContext context, string sid, int projektKod)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PROJEKT);

            var entityProjekt = await ProjektDal.GetAsync(context, projektKod);

            var entityParticio = await ParticioDal.GetAsync(context);
            var pc = JsonConvert.DeserializeObject<ProjektConf>(entityParticio.Projekt);
            var iratKod = pc.FeltetelesSzerzodesIratkod != null ? (int)pc.FeltetelesSzerzodesIratkod : 
                throw new Exception(string.Format(Messages.ParticioHiba, "FeltetelesSzerzodesIratkod"));

            var original = await IratBll.LetoltesAsync(context, sid, iratKod);

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
                KIVITELEZESIHATARIDO = entityProjekt.Kivitelezesihatarido.Value.ToShortDateString(),
                MUNKATERULETATADASA = DateTime.Now.Date.AddDays(1).ToShortDateString(),
                ARNETTO = (arNetto - 75000).ToString("#,#", nfi),
                ARBRUTTO = Calc.RealRound((arNetto - 75000) * (decimal)1.27, 1m).ToString("#,#", nfi),
                DATUM = DateTime.Now.Date.ToShortDateString()
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
        public static async Task<byte[]> OFTSzerzodesAsync(ossContext context, string sid, int projektKod)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PROJEKT);

            var entityProjekt = await ProjektDal.GetAsync(context, projektKod);

            var entityParticio = await ParticioDal.GetAsync(context);
            var pc = JsonConvert.DeserializeObject<ProjektConf>(entityParticio.Projekt);
            var iratKod = pc.OFTSzerzodesIratkod != null ? (int)pc.OFTSzerzodesIratkod :
                throw new Exception(string.Format(Messages.ParticioHiba, "OFTSzerzodesIratkod"));

            var original = await IratBll.LetoltesAsync(context, sid, iratKod);

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
                KIVITELEZESIHATARIDO = entityProjekt.Kivitelezesihatarido.Value.ToShortDateString(),
                MUNKATERULETATADASA = DateTime.Now.Date.AddDays(1).ToShortDateString(),
                ARNETTO = arNetto.ToString("#,#", nfi),
                ARBRUTTO = Calc.RealRound(arNetto * (decimal)1.27, 1m).ToString("#,#", nfi),
                ELOLEGNETTO = elolegNetto.ToString("#,#", nfi),
                ELOLEGBRUTTO = Calc.RealRound(elolegNetto * (decimal)1.27, 1m).ToString("#,#", nfi),
                DATUM = DateTime.Now.Date.ToShortDateString()
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

        public static async Task<byte[]> HMKEtulajdonoshozzajarulas(ossContext context, string sid, int projektKod)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PROJEKT);

            var entityProjekt = await ProjektDal.GetAsync(context, projektKod);

            var entityParticio = await ParticioDal.GetAsync(context);
            var pc = JsonConvert.DeserializeObject<ProjektConf>(entityParticio.Projekt);
            var iratKod = pc.HMKEtulajdonoshozzajarulasIratkod != null ? (int)pc.HMKEtulajdonoshozzajarulasIratkod :
                throw new Exception(string.Format(Messages.ParticioHiba, "HMKEtulajdonoshozzajarulasIratkod"));
            
            var original = await IratBll.LetoltesAsync(context, sid, iratKod);
            
            NumberFormatInfo nfi = new CultureInfo("hu-HU", false).NumberFormat;
            nfi.NumberGroupSeparator = ".";

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
                DATUM = DateTime.Now.Date.ToShortDateString()
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
