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

    internal static string CreateNew(OSSContext model, string ip, string host, string osUser,
      int felhasznaloKod, string felhasznalo, string azonosito, bool logol)
    {
      string result;

      lock (LockMe)
      {
        Purge(model);

        var entity = new SESSION
        {
          SESSIONID = Guid.NewGuid().ToString(),

          FELHASZNALOKOD = felhasznaloKod,
          FELHASZNALO = felhasznalo,
          AZONOSITO = azonosito,
          LOGOL = logol,

          IP = ip,
          HOST = host,
          OSUSER = osUser,

          ERVENYES = DateTime.Now.AddHours(8), //hűha
          LETREHOZVA = DateUtils.SqlNow,
        };

        result = SessionDal.Add(model, entity);

        entity = SessionDal.Get(model, result);
        model.RefreshModelFromSession(entity);
      }

      return result;
    }

    internal static void Check(OSSContext model, string sid, bool roleMustBeChosen)
    {
      SESSION entity;

      lock (LockMe)
      {
        var entities = Purge(model).Where(s => s.SESSIONID == sid);
        if (entities.Count() == 0)
          throw new Exception("Ismeretlen Sid vagy lejárt munkamenet!");
        entity = entities.First();

        if (roleMustBeChosen)
          if (entity.PARTICIOKOD == null || entity.CSOPORTKOD == null)
            throw new Exception("Bejelentkezés után kötelező szerepkört választani!");

        // entity.ERVENYES = DateTime.Now.AddHours(1);
        entity.ERVENYES = entity.ERVENYES.AddMinutes(1);

        SessionDal.Update(model, entity);
      }

      model.RefreshModelFromSession(entity);
    }

    internal static void UpdateRole(OSSContext model, string sid, int particioKod, int csoportKod)
    {
      CsoportDal.CheckSzerepkor(model, particioKod, csoportKod);

      var entityParticio = ParticioDal.Get(model, particioKod);
      var entityCsoport = CsoportDal.Get(model, csoportKod);

      SESSION entity;

      lock (LockMe)
      {
        entity = SessionDal.Get(model, sid);
        entity.PARTICIOKOD = particioKod;
        entity.PARTICIO = entityParticio.MEGNEVEZES;
        entity.CSOPORTKOD = csoportKod;
        entity.CSOPORT = entityCsoport.CSOPORT1;
        SessionDal.Update(model, entity);
      }

      model.RefreshModelFromSession(entity);
    }

    internal static void Delete(OSSContext model, string sid)
    {
      lock (LockMe)
      {
        var entity = SessionDal.Get(model, sid);
        SessionDal.Delete(model, entity);
      }
    }

    private static List<SESSION> Purge(OSSContext model)
    {
      var entities = SessionDal.Read(model);
      var most = DateTime.Now;
      var result = new List<SESSION>();

      foreach (var entity in entities)
      {
        if (entity.ERVENYES <= most)
          SessionDal.Delete(model, entity);
        else
          result.Add(entity);
      }

      return result;
    }
  }
}