using Microsoft.EntityFrameworkCore;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.SzamlazasiRend
{
    public class SzamlazasiRendDal
    {
        public static int Add(ossContext context, Models.Szamlazasirend entity)
        {
            Register.Creation(context, entity);
            context.Szamlazasirend.Add(entity);
            context.SaveChanges();

            return entity.Szamlazasirendkod;
        }

        public async static Task Lock(ossContext context, int pKey, DateTime utoljaraModositva)
        {
            await context.ExecuteLockFunction("lockszamlazasirend", "szamlazasirendkod", pKey, utoljaraModositva);
        }

        public static Models.Szamlazasirend Get(ossContext context, int pKey)
        {
            var result = context.Szamlazasirend
              .Include(r => r.PenznemkodNavigation)
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Szamlazasirendkod == pKey)
              .ToList();
            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato,
                  $"{nameof(Models.Szamlazasirend.Szamlazasirendkod)}={pKey}"));
            return result.First();
        }

        public static void Delete(ossContext context, Models.Szamlazasirend entity)
        {
            context.Szamlazasirend.Remove(entity);
            context.SaveChanges();
        }

        public static int Update(ossContext context, Models.Szamlazasirend entity)
        {
            Register.Modification(context, entity);
            context.SaveChanges();

            return entity.Szamlazasirendkod;
        }

        public static List<Models.Szamlazasirend> Select(ossContext context, int projektKod)
        {
            return context.Szamlazasirend.AsNoTracking()
              .Include(r => r.PenznemkodNavigation)
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Projektkod == projektKod)
              .OrderBy(s => s.Szamlazasirendkod)
              .ToList();
        }
    }
}
