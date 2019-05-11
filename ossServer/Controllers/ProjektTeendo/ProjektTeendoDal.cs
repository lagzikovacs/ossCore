using Microsoft.EntityFrameworkCore;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ossServer.Controllers.ProjektTeendo
{
    public class ProjektTeendoDal
    {
        public static int Add(ossContext context, Models.Projektteendo entity)
        {
            Register.Creation(context, entity);
            context.Projektteendo.Add(entity);
            context.SaveChanges();

            return entity.Projektteendokod;
        }

        public async static void Lock(ossContext context, int pKey, DateTime utoljaraModositva)
        {
            await context.ExecuteLockFunction("lockprojektteendo", "projektteendokod", pKey, utoljaraModositva);
        }

        public static Models.Projektteendo Get(ossContext context, int pKey)
        {
            var result = context.Projektteendo
              .Include(r => r.TeendokodNavigation)
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Projektteendokod == pKey).ToList();
            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato,
                  $"{nameof(Projektteendo.Projektteendokod)}={pKey}"));
            return result.First();
        }

        public static void Delete(ossContext context, Models.Projektteendo entity)
        {
            context.Projektteendo.Remove(entity);
            context.SaveChanges();
        }

        public static int Update(ossContext context, Models.Projektteendo entity)
        {
            Register.Modification(context, entity);
            context.SaveChanges();

            return entity.Projektteendokod;
        }

        public static List<Models.Projektteendo> Select(ossContext context, int projektKod)
        {
            return context.Projektteendo.AsNoTracking()
              .Include(r => r.TeendokodNavigation)
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Projektkod == projektKod)
              .OrderBy(s => s.Projektteendokod)
              .ToList();
        }
    }
}
