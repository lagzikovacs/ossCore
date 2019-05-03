using ossServer.Controllers.Csoport;
using ossServer.Controllers.Primitiv.Felhasznalo;
using ossServer.Controllers.Session;
using ossServer.Models;
using ossServer.Utils;
using System.Collections.Generic;

namespace ossServer.Controllers.Logon
{
    public class LogonBll
    {
        public static string Bejelentkezes(ossContext context, string azonosito, string jelszo,
          string ip, string winHost, string winUser)
        {
            var felhasznalo = FelhasznaloDal.Get(context, azonosito, jelszo);
            var sid = SessionBll.CreateNew(context, ip, winHost, winUser,
              felhasznalo.Felhasznalokod, felhasznalo.Nev, azonosito, felhasznalo.Logonlog);

            //if (context.CurrentSession.Logol)
            //{
            //    EsemenynaploBll.Bejegyzes(context, EsemenynaploBejegyzesek.Bejelentkezes);
            //    OssHub.Uzenet(context.Session.FELHASZNALO, EsemenynaploBejegyzesek.Bejelentkezes);
            //}

            return sid;
        }

        public static List<CsoportDto> Szerepkorok(ossContext context, string sid)
        {
            SessionBll.Check(context, sid, false);

            var entities = CsoportDal.GetSzerepkorok(context);
            var result = new List<CsoportDto>();
            foreach (var entity in entities)
            {
                var dto = ObjectUtils.Convert<Models.Csoport, CsoportDto>(entity);
                dto.Particiomegnevezes = entity.ParticiokodNavigation.Megnevezes;

                result.Add(dto);
            }

            //if (context.CurrentSession.Logol)
            //    EsemenynaploBll.Bejegyzes(context, EsemenynaploBejegyzesek.LehetsegesSzerepkorok);

            return result;
        }

        public static void SzerepkorValasztas(ossContext context, string sid, int particioKod, int csoportKod)
        {
            SessionBll.Check(context, sid, false);
            SessionBll.UpdateRole(context, sid, particioKod, csoportKod);

            //if (context.CurrentSession.Logol)
            //    EsemenynaploBll.Bejegyzes(model, EsemenynaploBejegyzesek.SzerepkorValasztas);
        }

        public static void Kijelentkezes(ossContext context, string sid)
        {
            SessionBll.Check(context, sid, false);
            SessionBll.Delete(context, sid);

            if (context.CurrentSession.Logol)
            {
                //EsemenynaploBll.Bejegyzes(model, EsemenynaploBejegyzesek.Kijelentkezes);
                //OssHub.Uzenet(model.Session.FELHASZNALO, EsemenynaploBejegyzesek.Kijelentkezes);
            }
        }
    }
}
