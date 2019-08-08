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
        public static async Task<int> AddAsync(ossContext context, Models.Szamlazasirend entity)
        {
            Register.Creation(context, entity);
            await context.Szamlazasirend.AddAsync(entity);
            await context.SaveChangesAsync();

            return entity.Szamlazasirendkod;
        }

        public async static Task Lock(ossContext context, int pKey, DateTime utoljaraModositva)
        {
            await context.ExecuteLockFunction("lockszamlazasirend", "szamlazasirendkod", pKey, utoljaraModositva);
        }

        public static async Task<Szamlazasirend> GetAsync(ossContext context, int pKey)
        {
            var result = await context.Szamlazasirend
              .Include(r => r.PenznemkodNavigation)
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Szamlazasirendkod == pKey)
              .ToListAsync();

            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato,
                  $"{nameof(Models.Szamlazasirend.Szamlazasirendkod)}={pKey}"));

            return result.First();
        }

        public static async Task DeleteAsync(ossContext context, Models.Szamlazasirend entity)
        {
            context.Szamlazasirend.Remove(entity);
            await context.SaveChangesAsync();
        }

        public static async Task<int> UpdateAsync(ossContext context, Models.Szamlazasirend entity)
        {
            Register.Modification(context, entity);
            await context.SaveChangesAsync();

            return entity.Szamlazasirendkod;
        }

        public static async Task<List<Szamlazasirend>> SelectAsync(ossContext context, int projektKod)
        {
            return await context.Szamlazasirend.AsNoTracking()
              .Include(r => r.PenznemkodNavigation)
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Projektkod == projektKod)
              .OrderBy(s => s.Szamlazasirendkod)
              .ToListAsync();
        }
    }
}
