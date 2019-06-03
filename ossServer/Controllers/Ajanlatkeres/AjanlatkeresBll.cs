using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace ossServer.Controllers.Ajanlatkeres
{
    public class AjanlatkeresBll
    {
        public static void WebesAjanlatkeres(ossContext context, WebesAjanlatkeresParam par)
        {
            var dto = new AjanlatkeresDto
            {
                Particiokod = par.Particiokod,
                Ugynoknev = par.Ugynoknev,
                Nev = par.Nev,
                Cim = par.Cim,
                Email = par.Email,
                Telefonszam = par.Telefon,
                Havifogyasztaskwh = par.Havifogyasztaskwh,
                Haviszamlaft = par.Haviszamlaft,
                Napelemekteljesitmenyekw = par.Napelemekteljesitmenyekw,
                Megjegyzes = par.Megjegyzes,
                Letrehozta = par.Ugynoknev,
                Letrehozva = DateTime.Now,
                Modositotta = par.Ugynoknev,
                Modositva = DateTime.Now,
            };

            var entity = ObjectUtils.Convert<AjanlatkeresDto, Models.Ajanlatkeres>(dto);
            var id = AjanlatkeresDal.AddWeb(context, entity);

            //ügyfél
            var uzenet = $"Tisztelt {par.Nev}!<br><br>A következő adatokkal kért tőlünk ajánlatot: <br><br>Cím: {par.Cim}<br>Email: {par.Email}<br>Telefonszám: {par.Telefon}<br><br>Hamarosan keresni fogjuk a részletek egyeztetése céljából!<br><br>www.gridsolar.hu";
            EmailKuldes(par.Email, "Re: ajánlatkérés", uzenet);
            //sales
            uzenet = $"Hello Timi,<br><br>webes ajánlatkérés érkezett, Id: {id}.<br><br>OSS";
            EmailKuldes("sales@gridsolar.hu", "Webes ajánlatkérés", uzenet);
        }

        // TODO konfigból?
        private static void EmailKuldes(string cimzett, string tema, string uzenet)
        {
            using (var smtpClient = SmtpClientFactory.GetClient(SmtpClientFactory.ClientType.Gmail,
              new NetworkCredential("gridsolarsales", "$tornado1"), true, "", 0))
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("gridsolarsales@gmail.com", "GridSolar Group Kft."),
                    IsBodyHtml = true,
                    Body = uzenet,
                    Subject = tema
                };
                mailMessage.To.Add(cimzett);
                smtpClient.Send(mailMessage);
            }
        }

        public static int Add(ossContext context, string sid, AjanlatkeresDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.AJANLATKERESMOD);

            var entity = ObjectUtils.Convert<AjanlatkeresDto, Models.Ajanlatkeres>(dto);
            return AjanlatkeresDal.Add(context, entity);
        }

        public static AjanlatkeresDto CreateNew(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.AJANLATKERESMOD);

            return new AjanlatkeresDto { Ugynoknev = context.CurrentSession.Felhasznalo };
        }

        public static void Delete(ossContext context, string sid, AjanlatkeresDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.AJANLATKERESMOD);

            AjanlatkeresDal.Lock(context, dto.Ajanlatkereskod, dto.Modositva);
            var entity = AjanlatkeresDal.Get(context, dto.Ajanlatkereskod);
            AjanlatkeresDal.Delete(context, entity);
        }

        public static AjanlatkeresDto Get(ossContext context, string sid, int key)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.AJANLATKERES);

            var entity = AjanlatkeresDal.Get(context, key);
            return ObjectUtils.Convert<Models.Ajanlatkeres, AjanlatkeresDto>(entity);
        }

        public static List<AjanlatkeresDto> Select(ossContext context, string sid, int rekordTol, 
            int lapMeret, List<SzMT> szmt, out int osszesRekord)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.AJANLATKERES);

            var qry = AjanlatkeresDal.GetQuery(context, szmt);
            osszesRekord = qry.Count();
            var entities = qry.Skip(rekordTol).Take(lapMeret).ToList();
            return ObjectUtils.Convert<Models.Ajanlatkeres, AjanlatkeresDto>(entities);
        }

        public static int Update(ossContext context, string sid, AjanlatkeresDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.AJANLATKERESMOD);

            AjanlatkeresDal.Lock(context, dto.Ajanlatkereskod, dto.Modositva);
            var entity = AjanlatkeresDal.Get(context, dto.Ajanlatkereskod);
            ObjectUtils.Update(dto, entity);
            return AjanlatkeresDal.Update(context, entity);
        }

        private static List<ColumnSettings> BaseColumns()
        {
            return new List<ColumnSettings>
            {
                new ColumnSettings {Name="Ajanlatkereskod", Title = "Id", Type = ColumnType.INT },
                new ColumnSettings {Name="Ugynoknev", Title = "Ügynök", Type = ColumnType.STRING },
                new ColumnSettings {Name="Nev", Title = "Név", Type = ColumnType.STRING },
                new ColumnSettings {Name="Cim", Title = "Cím", Type = ColumnType.STRING },
                new ColumnSettings {Name="Email", Title = "E-mail", Type = ColumnType.STRING },
                new ColumnSettings {Name="Telefonszam", Title = "Telefon", Type = ColumnType.STRING },
                new ColumnSettings {Name="Havifogyasztaskwh", Title = "Havi fogyasztás, kWh", Type = ColumnType.NUMBER },
                new ColumnSettings {Name="Haviszamlaft", Title = "Havi számla, Ft", Type = ColumnType.NUMBER },
                new ColumnSettings {Name="Napelemekteljesitmenyekw", Title = "Napelemek teljesítménye, kW", Type = ColumnType.NUMBER },
                new ColumnSettings {Name="Megjegyzes", Title = "Megjegyzés", Type = ColumnType.STRING },
            };
        }

        public static List<ColumnSettings> GridSettings(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            return BaseColumns();
        }

        public static List<ColumnSettings> ReszletekSettings(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            return ColumnSettingsUtil.AddIdobelyeg(BaseColumns());
        }
    }
}
