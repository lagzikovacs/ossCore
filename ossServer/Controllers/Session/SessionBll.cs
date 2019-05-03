using ossServer.Controllers.Csoport;
using ossServer.Controllers.Particio;
using ossServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ossServer.Controllers.Session
{
  public class SessionBll
  {
    private static readonly object LockMe = new object();
    private readonly string _sid;

    public SessionBll(string sid)
    {
      _sid = sid;
    }

    internal static string CreateNew(ossContext context, string ip, string host, string osUser,
      int felhasznaloKod, string felhasznalo, string azonosito, bool logol)
    {
      string result;

      lock (LockMe)
      {
        Purge(context);

        var entity = new Models.Session
        {
          Sessionid = Guid.NewGuid().ToString(),

          Felhasznalokod = felhasznaloKod,
          Felhasznalo = felhasznalo,
          Azonosito = azonosito,
          Logol = logol,

          Ip = ip,
          Host = host,
          Osuser = osUser,

          Ervenyes = DateTime.Now.AddHours(8), //hűha
          Letrehozva = DateTime.Now,
        };

        result = SessionDal.Add(context, entity);
        entity = SessionDal.Get(context, result);

                context.CurrentSession = entity;
            }

      return result;
    }

        public static void UpdateRole(ossContext context, string sid, int particioKod, int csoportKod)
        {
            CsoportDal.CheckSzerepkor(context, particioKod, csoportKod);

            var entityParticio = ParticioDal.Get(context, particioKod);
            var entityCsoport = CsoportDal.Get(context, csoportKod);

            lock (LockMe)
            {
                var entity = SessionDal.Get(context, sid);

                entity.Particiokod = particioKod;
                entity.Particio = entityParticio.Megnevezes;
                entity.Csoportkod = csoportKod;
                entity.Csoport = entityCsoport.Csoport1;

                SessionDal.Update(context, entity);
                entity = SessionDal.Get(context, sid);

                context.CurrentSession = entity;
            }
        }

    public static void Check(ossContext context, string sid, bool roleMustBeChosen = true)
    {
        if (string.IsNullOrEmpty(sid))
            throw new ArgumentNullException(nameof(sid));

        lock (LockMe)
        {
            context.CurrentSession = null;

            var entities = Purge(context).Where(s => s.Sessionid == sid);
            if (entities.Count() == 0)
                throw new Exception("Ismeretlen Sid vagy lejárt munkamenet!");
            var entity = entities.First();

            if (roleMustBeChosen)
                if (entity.Particiokod == null || entity.Csoportkod == null)
                    throw new Exception("Bejelentkezés után kötelező szerepkört választani!");

            // entity.ERVENYES = DateTime.Now.AddHours(1);
            entity.Ervenyes = entity.Ervenyes.AddMinutes(1);

            SessionDal.Update(context, entity);
            entity = SessionDal.Get(context, sid);

            context.CurrentSession = entity;
        }
    }

    public static void Delete(ossContext model, string sid)
    {
      lock (LockMe)
      {
        var entity = SessionDal.Get(model, sid);
        SessionDal.Delete(model, entity);
      }
    }

    private static List<Models.Session> Purge(ossContext model)
    {
      var entities = SessionDal.Read(model);
      var most = DateTime.Now;
      var result = new List<Models.Session>();

      foreach (var entity in entities)
      {
        if (entity.Ervenyes <= most)
          SessionDal.Delete(model, entity);
        else
          result.Add(entity);
      }

      return result;
    }
  }
}