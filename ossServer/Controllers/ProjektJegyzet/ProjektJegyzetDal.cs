using Microsoft.EntityFrameworkCore;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.ProjektJegyzet
{
    public class ProjektJegyzetDal
    {
        public static async Task<int> AddAsync(ossContext context, Models.Projektjegyzet entity)
        {
            Register.Creation(context, entity);
            await context.Projektjegyzet.AddAsync(entity);
            await context.SaveChangesAsync();

            return entity.Projektjegyzetkod;
        }

        public async static Task Lock(ossContext context, int pKey, DateTime utoljaraModositva)
        {
            await context.ExecuteLockFunction("lockprojektjegyzet", "projektjegyzetkod", pKey, utoljaraModositva);
        }

        public static async Task<Projektjegyzet> GetAsync(ossContext context, int pKey)
        {
            var result = await context.Projektjegyzet
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Projektjegyzetkod == pKey).ToListAsync();

            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato,
                  $"{nameof(Projektjegyzet.Projektjegyzetkod)}={pKey}"));

            return result.First();
        }

        public static async Task DeleteAsync(ossContext context, Models.Projektjegyzet entity)
        {
            context.Projektjegyzet.Remove(entity);
            await context.SaveChangesAsync();
        }

        public static async Task<int> UpdateAsync(ossContext context, Models.Projektjegyzet entity)
        {
            Register.Modification(context, entity);
            await context.SaveChangesAsync();

            return entity.Projektjegyzetkod;
        }

        public static async Task<List<Projektjegyzet>> SelectAsync(ossContext context, int projektKod)
        {
            return await context.Projektjegyzet.AsNoTracking()
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Projektkod == projektKod)
              .OrderBy(s => s.Projektjegyzetkod)
              .ToListAsync();
        }
    }
}
