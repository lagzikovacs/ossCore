using Microsoft.EntityFrameworkCore;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Kifizetes
{
    public class KifizetesDal
    {
        public static List<Models.Kifizetes> Read(ossContext context, int bizonylatKod)
        {
            return context.Kifizetes.AsNoTracking()
              .Include(r => r.PenznemkodNavigation)
              .Include(r => r.FizetesimodkodNavigation)
              .Where(s => s.Bizonylatkod == bizonylatKod)
              .OrderByDescending(s => s.Datum)
              .ToList();
        }

        public static Models.Kifizetes Get(ossContext context, int pKey)
        {
            var result = context.Kifizetes
              .Include(r => r.PenznemkodNavigation)
              .Include(r => r.FizetesimodkodNavigation)
              .Where(s => s.Kifizeteskod == pKey)
              .ToList();
            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato,
                  $"{nameof(Models.Kifizetes.Kifizeteskod)}={pKey}"));
            return result.First();
        }

        public async static Task Lock(ossContext context, int pKey, DateTime utoljaraModositva)
        {
            await context.ExecuteLockFunction("lockkifizetes", "kifizeteskod", pKey, utoljaraModositva);
        }

        public static void Delete(ossContext context, Models.Kifizetes entity)
        {
            context.Kifizetes.Remove(entity);
            context.SaveChanges();
        }

        public static int Add(ossContext context, Models.Kifizetes entity)
        {
            Register.Creation(context, entity);
            context.Kifizetes.Add(entity);
            context.SaveChanges();

            return entity.Kifizeteskod;
        }

        public static int Update(ossContext context, Models.Kifizetes entity)
        {
            Register.Modification(context, entity);
            context.SaveChanges();

            return entity.Kifizeteskod;
        }
    }
}
