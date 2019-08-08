using Microsoft.EntityFrameworkCore;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.ProjektTeendo
{
    public class ProjektTeendoDal
    {
        public static async Task<int> AddAsync(ossContext context, Models.Projektteendo entity)
        {
            Register.Creation(context, entity);
            await context.Projektteendo.AddAsync(entity);
            await context.SaveChangesAsync();

            return entity.Projektteendokod;
        }

        public async static Task Lock(ossContext context, int pKey, DateTime utoljaraModositva)
        {
            await context.ExecuteLockFunction("lockprojektteendo", "projektteendokod", pKey, utoljaraModositva);
        }

        public static async Task<Projektteendo> GetAsync(ossContext context, int pKey)
        {
            var result = await context.Projektteendo
              .Include(r => r.TeendokodNavigation)
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Projektteendokod == pKey).ToListAsync();

            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato,
                  $"{nameof(Projektteendo.Projektteendokod)}={pKey}"));

            return result.First();
        }

        public static async Task DeleteAsync(ossContext context, Models.Projektteendo entity)
        {
            context.Projektteendo.Remove(entity);
            await context.SaveChangesAsync();
        }

        public static async Task<int> UpdateAsync(ossContext context, Models.Projektteendo entity)
        {
            Register.Modification(context, entity);
            await context.SaveChangesAsync();

            return entity.Projektteendokod;
        }

        public static async Task<List<Projektteendo>> SelectAsync(ossContext context, int projektKod)
        {
            return await context.Projektteendo.AsNoTracking()
              .Include(r => r.TeendokodNavigation)
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Projektkod == projektKod)
              .OrderBy(s => s.Projektteendokod)
              .ToListAsync();
        }
    }
}
