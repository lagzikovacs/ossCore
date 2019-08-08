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
        public static async Task<List<Models.Kifizetes>> ReadAsync(ossContext context, int bizonylatKod)
        {
            return await context.Kifizetes.AsNoTracking()
              .Include(r => r.PenznemkodNavigation)
              .Include(r => r.FizetesimodkodNavigation)
              .Where(s => s.Bizonylatkod == bizonylatKod)
              .OrderByDescending(s => s.Datum)
              .ToListAsync();
        }

        public static async Task<Models.Kifizetes> GetAsync(ossContext context, int pKey)
        {
            var result = await context.Kifizetes
              .Include(r => r.PenznemkodNavigation)
              .Include(r => r.FizetesimodkodNavigation)
              .Where(s => s.Kifizeteskod == pKey)
              .ToListAsync();

            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato,
                  $"{nameof(Models.Kifizetes.Kifizeteskod)}={pKey}"));

            return result.First();
        }

        public async static Task Lock(ossContext context, int pKey, DateTime utoljaraModositva)
        {
            await context.ExecuteLockFunction("lockkifizetes", "kifizeteskod", pKey, utoljaraModositva);
        }

        public static async Task DeleteAsync(ossContext context, Models.Kifizetes entity)
        {
            context.Kifizetes.Remove(entity);
            await context.SaveChangesAsync();
        }

        public static async Task<int> AddAsync(ossContext context, Models.Kifizetes entity)
        {
            Register.Creation(context, entity);
            await context.Kifizetes.AddAsync(entity);
            await context.SaveChangesAsync();

            return entity.Kifizeteskod;
        }

        public static async Task<int> UpdateAsync(ossContext context, Models.Kifizetes entity)
        {
            Register.Modification(context, entity);
            await context.SaveChangesAsync();

            return entity.Kifizeteskod;
        }
    }
}
