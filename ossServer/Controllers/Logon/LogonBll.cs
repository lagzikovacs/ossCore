using Microsoft.AspNetCore.SignalR;
using ossServer.Controllers.Csoport;
using ossServer.Controllers.Esemenynaplo;
using ossServer.Controllers.Primitiv.Felhasznalo;
using ossServer.Controllers.Session;
using ossServer.Hubs;
using ossServer.Models;
using ossServer.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ossServer.Controllers.Logon
{
    public class LogonBll
    {
        public static async Task<string> BejelentkezesAsync(ossContext context, IHubContext<OssHub> hubcontext, 
            string azonosito, string jelszo, string ip, string winHost, string winUser)
        {
            var felhasznalo = await FelhasznaloDal.GetAsync(context, azonosito, jelszo);
            var sid = SessionBll.CreateNew(context, ip, winHost, winUser,
              felhasznalo.Felhasznalokod, felhasznalo.Nev, azonosito, felhasznalo.Logonlog);

            if (context.CurrentSession.Logol)
            {
                EsemenynaploBll.Bejegyzes(context, EsemenynaploBejegyzesek.Bejelentkezes);
                HubUtils.Uzenet(hubcontext, context.CurrentSession.Felhasznalo, EsemenynaploBejegyzesek.Bejelentkezes);
            }

            return sid;
        }

        public static async Task<List<CsoportDto>> SzerepkorokAsync(ossContext context, string sid)
        {
            SessionBll.Check(context, sid, false);

            var entities = await CsoportDal.GetSzerepkorokAsync(context);
            var result = new List<CsoportDto>();
            foreach (var entity in entities)
            {
                var dto = ObjectUtils.Convert<Models.Csoport, CsoportDto>(entity);
                dto.Particiomegnevezes = entity.ParticiokodNavigation.Megnevezes;

                result.Add(dto);
            }

            return result;
        }

        public static async Task SzerepkorValasztasAsync(ossContext context, string sid, int particioKod, int csoportKod)
        {
            SessionBll.Check(context, sid, false);
            await SessionBll.UpdateRoleAsync(context, sid, particioKod, csoportKod);

            if (context.CurrentSession.Logol)
                EsemenynaploBll.Bejegyzes(context, EsemenynaploBejegyzesek.SzerepkorValasztas);
        }

        public static void Kijelentkezes(ossContext context, string sid)
        {
            SessionBll.Check(context, sid, false);
            SessionBll.Delete(context, sid);

            if (context.CurrentSession.Logol)
            {
                EsemenynaploBll.Bejegyzes(context, EsemenynaploBejegyzesek.Kijelentkezes);
                //OssHub.Uzenet(model.Session.FELHASZNALO, EsemenynaploBejegyzesek.Kijelentkezes);
            }
        }
    }
}
