using ossServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ossServer.Controllers.Session
{
  public class SessionDal
  {
    public static Models.Session Get(ossContext context, string id)
    {
      var result = context.Session.Where(s => s.Sessionid == id).ToList();
      if (result.Count != 1)
        throw new Exception(string.Format(Messages.AdatNemTalalhato, $"{nameof(Models.Session.Sessionid)}={id}"));
      return result.First();
    }

    public static List<Models.Session> Read(ossContext context)
    {
      return context.Session //nincs .AsNoTracking(), használja a Purge
        .OrderBy(s => s.Letrehozva)
        .ToList();
    }

    public static string Update(ossContext context, Models.Session entity)
    {
      context.SaveChanges();

      return entity.Sessionid;
    }

    public static void Delete(ossContext context, Models.Session entity)
    {
      context.Session.Remove(entity);
      context.SaveChanges();
    }

    public static string Add(ossContext context, Models.Session entity)
    {
      context.Session.Add(entity);
      context.SaveChanges();

      return entity.Sessionid;
    }
  }
}